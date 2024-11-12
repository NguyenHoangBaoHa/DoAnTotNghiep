using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuanLySanBong.Data;
using QuanLySanBong.Helpers;
using QuanLySanBong.Repository.Account;
using QuanLySanBong.Repository.Staff;
using QuanLySanBong.Service.Account;
using QuanLySanBong.Service.Staff;
using QuanLySanBong.Token;
using QuanLySanBong.UnitOfWork;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình JwtSettings và đăng ký DI cho JwtSettings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtSettings>>().Value);

// Cấu hình JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options =>
  {
      options.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = jwtSettings.Issuer,
          ValidAudience = jwtSettings.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
          RoleClaimType = "role"
      };
  });

// Cấu hình DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowCredentials(); // Bật AllowCredentials để chấp nhận các yêu cầu có credentials;
    });
});

// Đăng ký AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Đăng ký các dịch vụ và repository
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IStaffService, StaffService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Sử dụng CORS
app.UseCors("AllowSpecificOrigin");

// Cấu hình middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Phải đặt trước UseAuthorization và MapControllers
app.UseAuthorization();

app.Use(async (context, next) =>
{
    var token = context.Request.Headers["Authorization"].ToString();
    if (!string.IsNullOrEmpty(token))
    {
        Console.WriteLine($"Token: {token}");  // In token ra để kiểm tra
    }
    await next();
});

app.MapControllers();

app.Run();