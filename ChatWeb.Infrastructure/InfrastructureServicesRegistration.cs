using ChatWeb.Application.Contracts.Identity;
using ChatWeb.Application.Contracts.Infrastructure;
using ChatWeb.Infrastructure.Identity;
using ChatWeb.Infrastructure.ImageWorker;
using ChatWeb.Infrastructure.JwtToken;
using Microsoft.Extensions.DependencyInjection;

namespace ChatWeb.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services)
        {
            services.AddTransient<IJwtTokenService, JwtTokenService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IImageService, ImageService>();

            return services;
        }
    }
}
