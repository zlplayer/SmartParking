using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SmartParking.Application.Extensions;
using SmartParking.Application.Interfaces;
using Xunit;

namespace SmartParking.Application.Tests.Extensions;

[TestSubject(typeof(ServiceCollectionExtension))]
public class ServiceCollectionExtensionTest
{
    private readonly IServiceCollection _services;
    private readonly Mock<IConfiguration> _configurationMock;

    public ServiceCollectionExtensionTest()
    {
        _services = new ServiceCollection();
        _configurationMock = new Mock<IConfiguration>();

        // Setup JWT configuration
        _configurationMock.Setup(x => x["Jwt:Key"]).Returns("SuperSecretKeyForTestingPurposes123456789!");
        _configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");
    }

    [Fact]
    public void AddApplication_ShouldRegisterJwtAuthService()
    {
        // Act
        _services.AddApplication(_configurationMock.Object);

        // Assert - check that IJwtAuth is registered
        var jwtAuthDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IJwtAuth));
        Assert.NotNull(jwtAuthDescriptor);
    }

    [Fact]
    public void AddApplication_ShouldRegisterAutoMapperServices()
    {
        // Act
        _services.AddApplication(_configurationMock.Object);

        // Assert - check that AutoMapper IMapper is registered
        var mapperDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(AutoMapper.IMapper));
        Assert.NotNull(mapperDescriptor);
    }

    [Fact]
    public void AddApplication_ShouldRegisterMediatRServices()
    {
        // Act
        _services.AddApplication(_configurationMock.Object);

        // Assert - check that MediatR IMediator is registered
        var mediatorDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(MediatR.IMediator));
        Assert.NotNull(mediatorDescriptor);
    }

    [Fact]
    public void AddApplication_ShouldRegisterAuthenticationServices()
    {
        // Act
        _services.AddApplication(_configurationMock.Object);

        // Assert - check that authentication services are registered
        var authenticationSchemeProvider = _services.Any(s =>
            s.ServiceType.Name.Contains("AuthenticationSchemeProvider"));
        Assert.True(authenticationSchemeProvider);
    }

    [Fact]
    public void AddApplication_JwtAuthShouldBeScoped()
    {
        // Act
        _services.AddApplication(_configurationMock.Object);

        // Assert
        var jwtAuthDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(IJwtAuth));
        Assert.NotNull(jwtAuthDescriptor);
        Assert.Equal(ServiceLifetime.Scoped, jwtAuthDescriptor.Lifetime);
    }

    [Fact]
    public void AddApplication_ShouldNotThrowException()
    {
        // Act & Assert
        var exception = Record.Exception(() => _services.AddApplication(_configurationMock.Object));
        Assert.Null(exception);
    }

    [Fact]
    public void AddApplication_ShouldRegisterJwtBearerAuthentication()
    {
        // Act
        _services.AddApplication(_configurationMock.Object);

        // Assert - check that JWT Bearer authentication handler is registered
        var jwtBearerHandler = _services.Any(s =>
            s.ImplementationType?.Name.Contains("JwtBearer") == true ||
            s.ServiceType.Name.Contains("JwtBearer"));
        Assert.True(jwtBearerHandler);
    }
}
