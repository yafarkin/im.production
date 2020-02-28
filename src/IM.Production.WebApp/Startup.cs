using AutoMapper;
using CalculationEngine;
using IM.Production.Services;
using IM.Production.WebApp.Helpers;
using IM.Production.WebApp.Dtos;
using Epam.ImitationGames.Production.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using IM.Production.CalculationEngine;

namespace IM.Production.WebApp
{
    public class Startup
    {
        public IConfiguration AppConfiguration { get; set; }

        public Startup(IConfiguration config)
        {
            AppConfiguration = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            /// <summary>
            /// Test data for cheking displaying information about contracts.
            /// </summary>>
            var result = new GameConfigDto();
            AppConfiguration.GetSection("Game").Bind(result);
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

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
