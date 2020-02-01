using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using Epam.ImitationGames.Production.Domain.Services;
using IM.Production.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using Epam.ImitationGames.Production.Domain.Production;
using AutoMapper;
using IM.Production.WebApp.Dtos;
using Epam.ImitationGames.Production.Domain.Base;

namespace IM.Production.WebApp
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            /// <summary>
            /// Test data for cheking displaying information about teams.
            /// </summary>>
            services.AddSingleton<Game>(new Game()
            {
                Customers = new List<Customer> {
                new Customer{
                    Login = "TestTeam1",
                    ProductionType = new ProductionType{ DisplayName = "Metall" },
                    Factories = new List<Factory>{
                        new Factory{ FactoryDefinition = new FactoryDefinition{
                            Name = "Тестовая первая фабрика",
                            GenerationLevel = 2,
                            BaseWorkers = 20,
                            ProductionType = new ProductionType{ DisplayName = "Тестовая область производства " }
                        } },
                        new Factory{ FactoryDefinition = new FactoryDefinition{
                            Name = "Тестовая вторая фабрика",
                            GenerationLevel = 4,
                            BaseWorkers = 30,
                            ProductionType = new ProductionType{ DisplayName = "Тестовая область производства " }
                        } }
                    },
                    Sum = 18000,
                    Contracts = new List<Contract>{
                        new Contract(new GameTime(), new MaterialWithPrice(),"Тестовый контракт 1" ),
                        new Contract(new GameTime(), new MaterialWithPrice(),"Тестовый контракт 2" ),
                    }
                },
                new Customer{
                    Login = "TestTeam2",
                    ProductionType = new ProductionType{ DisplayName = "Chemical" },
                    Factories = new List<Factory>{
                        new Factory{ FactoryDefinition = new FactoryDefinition{
                            Name = "Тестовая первая фабрика",
                            GenerationLevel = 2,
                            BaseWorkers = 20,
                            ProductionType = new ProductionType{ DisplayName = "Тестовая область производства " }
                        } },
                        new Factory{ FactoryDefinition = new FactoryDefinition{
                            Name = "Тестовая вторая фабрика",
                            GenerationLevel = 4,
                            BaseWorkers = 30,
                            ProductionType = new ProductionType{ DisplayName = "Тестовая область производства " }
                        } }
                    },
                    Sum = 21000,
                    Contracts = new List<Contract>{
                        new Contract(new GameTime(), new MaterialWithPrice(),"Тестовый контракт 1" ),
                        new Contract(new GameTime(), new MaterialWithPrice(),"Тестовый контракт 2" ),
                    }
                }
            }
            });
            services.AddTransient<ITeamsService, TeamsService>();            
            services.AddAutoMapper(c => c.AddProfile<OrganizationProfile>(), typeof(Startup));
        }

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
