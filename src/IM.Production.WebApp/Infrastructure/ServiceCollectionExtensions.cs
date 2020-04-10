using IM.Production.Services;
using IM.Production.Services.Options;
using IM.Production.WebApp.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace IM.Production.WebApp.Infrastructure
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ConfigureOptions(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
            services.Configure<GameOptions>(configuration.GetSection("Game"));
            services.Configure<CredentialsOptions>(configuration.GetSection("Credentials"));

            return services;
        }

        internal static IServiceCollection AddAuthenticationWithJwt(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var options = services.BuildServiceProvider().GetRequiredService<IOptions<AuthenticationOptions>>();

            services
                .AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = true;

                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Value.Secret)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }
    }
}
