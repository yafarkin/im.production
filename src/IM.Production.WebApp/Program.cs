using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace IM.Production.WebApp
{
    /// <summary>
    /// Настройка и запуск веб-хоста.
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
