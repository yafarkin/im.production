<<<<<<< HEAD
using AutoMapper;
using CalculationEngine;
using IM.Production.Services;
using IM.Production.WebApp.Helpers;
=======
using CalculationEngine;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.Services;
>>>>>>> d2f996465694edb0ccf1f595191be1d3e642e6e7
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
<<<<<<< HEAD
using Epam.ImitationGames.Production.Domain.Services;
=======
using AutoMapper;
>>>>>>> d2f996465694edb0ccf1f595191be1d3e642e6e7
using IM.Production.WebApp.Dtos;

namespace IM.Production.WebApp
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
<<<<<<< HEAD
            /// <summary>
            /// Test data for cheking displaying information about contracts.
            /// </summary>>
            var game = FakeGameInitializer.CreateGame(15);
            services.AddSingleton<Game>(game);
            services.AddTransient<IContractsService, ContractsService>();
            services.AddAutoMapper(c => c.AddProfile<FactoryProfile>(), typeof(Startup));
=======
            services.AddTransient<ITeamsService, TeamsService>();            
            services.AddAutoMapper(c => c.AddProfile<OrganizationProfile>(), typeof(Startup));
>>>>>>> d2f996465694edb0ccf1f595191be1d3e642e6e7
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
