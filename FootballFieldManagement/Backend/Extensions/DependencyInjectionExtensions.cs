using Backend.Helpers;
using Backend.Repository.Account;
using Backend.Repository.Pitch;
using Backend.Repository.Staff;
using Backend.Service.Account;
using Backend.Service.Pitch;
using Backend.Service.Staff;
using Backend.UnitOfWork;

namespace Backend.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencyInjectionExtensions(this IServiceCollection services,
        IConfiguration config)
    {
        services.AddAutoMapper(typeof(Program));
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IStaffRepository, StaffRepository>();
        services.AddScoped<IPitchRepository, PitchRepository>();
        services.AddScoped<IPitchService, PitchService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IStaffService, StaffService>();
        
        return services;
    }
}