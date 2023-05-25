using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantHomework.Authorization.Bll.Options;
using RestaurantHomework.Authorization.Bll.Services;
using RestaurantHomework.Authorization.Bll.Services.Interfaces;

namespace RestaurantHomework.Authorization.Bll.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBll(this IServiceCollection services, IConfiguration config)
    {
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
        services.Configure<JwtOptions>(config.GetSection(nameof(JwtOptions)));
        services.AddTransient<ISessionService, SessionService>();
        return services;
    }
}