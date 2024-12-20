﻿using Backend.Helpers;
using Backend.Repository.Account;
using Backend.Repository.Booking;
using Backend.Repository.Pitch;
using Backend.Repository.PitchType;
using Backend.Repository.Staff;
using Backend.Service.Account;
using Backend.Service.Booking;
using Backend.Service.Pitch;
using Backend.Service.PitchType;
using Backend.Service.Staff;
using Backend.UnitOfWork;

namespace Backend.Extensions;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencyInjectionExtensions(this IServiceCollection services,
        IConfiguration config)
    {
        // Đăng ký AutoMapper
        services.AddAutoMapper(typeof(Program)); // Hoặc bạn có thể chỉ định Assembly chứa Profile nếu có

        // Đăng ký các dịch vụ
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IStaffRepository, StaffRepository>();
        services.AddScoped<IPitchRepository, PitchRepository>();
        services.AddScoped<IPitchTypesRepository, PitchTypesRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IPitchService, PitchService>();
        services.AddScoped<IPitchTypesService, PitchTypesService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IStaffService, StaffService>();

        return services;
    }
}
