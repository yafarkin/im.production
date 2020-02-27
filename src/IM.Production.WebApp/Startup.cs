using AutoMapper;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.CalculationEngine;
using IM.Production.Services;
using IM.Production.WebApp.Dtos;
using IM.Production.WebApp.Helpers;
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
            services.Configure<Credentials>(Configuration.GetSection(nameof(Credentials)));

            services.AddControllers();

            var result = new GameConfigDto();
            Configuration.GetSection("Game").Bind(result);
            var game = FakeGameInitializer.CreateGame(30);
            game.TotalGameDays = result.TotalDays;

            services.AddSingleton(game);
            services.AddSingleton<Logic>();
            services.AddSingleton<CalculationEngine.CalculationEngine>();
            services.AddTransient<IContractsService, ContractsService>();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<ITeamsService, TeamsService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();

            services.AddAutoMapper(c => c.AddProfile<BaseProfile>(), typeof(Startup));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
