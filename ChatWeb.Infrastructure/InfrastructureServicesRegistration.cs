using ChatWeb.Application.Contracts.Identity;
using ChatWeb.Application.Contracts.Infrastructure;
using ChatWeb.Domain.Identity;
using ChatWeb.Infrastructure.Identity;
using ChatWeb.Infrastructure.JwtToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWeb.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection ConfigureInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IJwtTokenService, JwtTokenService>();
            services.AddTransient<IAuthService, AuthService>();

            return services;
        }
    }
}
