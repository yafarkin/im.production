using AutoMapper;
using Epam.ImitationGames.Production.Domain.Authentication;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.Authentication;
using IM.Production.CalculationEngine;
using IM.Production.Services;
using IM.Production.WebApp.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.ConfigureOptions();

            services.AddControllers();
            services.AddAuthenticationWithJwt();
            services.AddAutoMapper(typeof(Startup));

            services.AddSingleton(FakeGameFactory.CreateGame);
            services.AddSingleton<Logic>();
            services.AddSingleton<CalculationEngine.CalculationEngine>();

            services.AddTransient<IContractsService, ContractsService>();
            services.AddTransient<ITeamsService, TeamsService>();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IFactoriesService, FactoriesService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            services.AddTransient<ITokenGenerator, TokenGenerator>();
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