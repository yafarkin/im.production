using AutoMapper;
using System.Text;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.CalculationEngine;
using IM.Production.Services;
using IM.Production.WebApp.Dtos;
using IM.Production.WebApp.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using CalculationEngine;

namespace IM.Production.WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<Authentication>(Configuration.GetSection(nameof(Authentication)));

            services.AddControllers();

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetValue<string>($"{nameof(Authentication)}:{nameof(Authentication.Secret)}"))),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            /// <summary>
            /// Test data for cheking displaying information about contracts.
            /// </summary>>
            var result = new GameConfigDto();
            Configuration.GetSection("Game").Bind(result);
            var game = FakeGameInitializer.CreateGame(30);
            game.TotalGameDays = result.TotalDays; 

            services.AddSingleton<Game>(game);
            services.AddSingleton<Logic>(new Logic(game));
            services.AddSingleton<CalculationEngine.CalculationEngine>();
            services.AddTransient<IContractsService, ContractsService>();
            services.AddTransient<ITeamsService, TeamsService>();            
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IFactoriesService, FactoriesService>();
            services.AddAutoMapper(c => c.AddProfile<BaseProfile>(), typeof(Startup));
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            services.AddAutoMapper(typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}