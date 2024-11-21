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
                        //.WithOrigins("http:localhost:3000");
                        .SetIsOriginAllowed(hosts => true);
                });
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
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
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                    RoleClaimType = "typ"
                };
            });

        // AUTHORIZATION
        services.AddAuthorization(options =>
        {
            // Policy for Admin or Staff
            options.AddPolicy("AdminOrStaffPolicy", policy =>
                policy.RequireRole("Admin", "Staff"));

            // Policy for Admin only
            options.AddPolicy("AdminPolicy", policy =>
                policy.RequireRole("Admin"));
        });

        return services;
    }
}