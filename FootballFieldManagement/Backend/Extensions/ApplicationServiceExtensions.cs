using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Backend.Data;

namespace Backend.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServiceExtensions(this IServiceCollection services,
        IConfiguration config)
    {
        // Kết nối cơ sở dữ liệu
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            opt.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder =>
                {
                    builder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowed(hosts => true);
                });
        });

        // AUTHORIZATION
        services.AddAuthorization(options =>
        {
            // Chính sách chỉ dành cho Admin
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin")); // "Admin" phải trùng với giá trị role trong token

            // Chính sách cho cả Admin và Staff
            options.AddPolicy("AdminOrStaffPolicy", policy =>
                policy.RequireRole("Admin", "Staff"));
        });

        // SYSTEM
        services.AddSingleton(config);
        services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

        // AUTHENTICATION
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, // Xác thực issuer
                    ValidateAudience = true, // Xác thực audience
                    ValidateLifetime = true, // Xác thực thời gian sống của token
                    ValidateIssuerSigningKey = true, // Xác thực khóa ký
                    ValidIssuer = config["Jwt:Issuer"], // Issuer hợp lệ
                    ValidAudience = config["Jwt:Issuer"], // Audience hợp lệ
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                    RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" // Xác định nơi chứa role
                };
            });

        return services;
    }
}
