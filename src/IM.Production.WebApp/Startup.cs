using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CalculationEngine;
using Epam.ImitationGames.Production.Domain;
using IM.Production.CalculationEngine;
using Epam.ImitationGames.Production.Domain.Production;
using System.Collections.Generic;

namespace IM.Production.WebApp
{
    public class Startup
    {
        public static Game GameInstance { get; private set; }

        public Game CreateGame(int customersCount)
        {
            var game = new Game();
            var logic = new Logic(game);
            var rand = new System.Random();
            var list = new List<MaterialWithPrice>();

            for (int i = 0; i < 100; i++)
            {
                var material = new MaterialWithPrice();
                material.Amount = rand.Next(10, 100_000);
                material.SellPrice = rand.Next(250, 50_000);
                list.Add(material);
            }

            for (int c = 0; c < customersCount; c++)
            {
                var productionType = new ProductionType();
                productionType.Key = "key";
                productionType.DisplayName = "DisplayName";
                var customer = logic.AddCustomer("CustomerLogin" + c, "CustomerPassword" + c, "CustomerName" + c, productionType);

                int contractsNumber = rand.Next(3, 10);
                for (int r = 0; r < contractsNumber; r++)
                {
                    int materialNumber = rand.Next(1, 99);
                    if (materialNumber > 0 && materialNumber < list.Count)
                    {
                        var material = list[materialNumber];
                        var contract = new Contract(customer, material);
                        contract.SourceFactory = new Factory();
                        contract.DestinationFactory = new Factory();
                        logic.AddContract(contract);
                    }
                }
                

                //Customer customer = new Customer();
                //Contract contract = new Contract(customer, materialWithPrice);
            }

            return game;
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            GameInstance = CreateGame(15);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
