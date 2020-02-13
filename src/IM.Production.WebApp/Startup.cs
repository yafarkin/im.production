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
using IM.Production.CalculationEngine;

namespace IM.Production.WebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            /// <summary>
            /// Test data for cheking displaying information about contracts.
            /// </summary>>
            var game = FakeGameInitializer.CreateGame(30);
            var logic = new Logic(game);
            services.AddSingleton<Game>(game);
            services.AddSingleton<Logic>(logic);
            services.AddTransient<IContractsService, ContractsService>();
            services.AddTransient<ITeamsService, TeamsService>();            
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
