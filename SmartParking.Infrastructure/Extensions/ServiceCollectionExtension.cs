using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartParking.Domain.Interfaces;
using SmartParking.Infrastructure.Repositories;
using SmartParking.Infrastructure.Seeders;

namespace SmartParking.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSqlServer<SmartParkingDbContext>(configuration.GetConnectionString("SmartParkingDbContext"));

        services.AddScoped<IUserRepositories, UserRepositories>();
        services.AddScoped<IVehicleRepositories, VehicleRepositories>();
        services.AddScoped<IParkingZoneRepositories, ParkingZoneRepositories>();
        services.AddScoped<IParkingSpaceRepositories, ParkingSpaceRepositories>();
        
        services.AddScoped<SmartParkingSeeder>();
    }
}