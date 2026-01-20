using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SmartParking.Application.Auth;
using SmartParking.Application.Interfaces;
using SmartParking.Application.Mappings;
using SmartParking.Application.SmartParking.Commands.LoginCommand;

namespace SmartParking.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(cfg => cfg.AddProfile<SmartParkingMappingProfile>());
        
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining<LoginCommand>()
        );

        services.AddScoped<IJwtAuth, JwtAuth>();
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
    }
}