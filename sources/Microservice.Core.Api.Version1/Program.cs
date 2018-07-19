using System.IO;
using Microsoft.AspNetCore.Hosting;
using NLog.Extensions.Logging;

namespace Microservice.Core.Api.Version1
{
    public class Program
    {
        public static IWebHostBuilder WebHostBuilder
        {
            get
            {
                return new WebHostBuilder()
                    .UseKestrel()
                    .ConfigureLogging(factory => { factory.AddNLog(); })
                    .CaptureStartupErrors(true)
                    .UseSetting("detailedErrors", "true")
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIISIntegration()
                    .UseStartup<Startup>();
            }
        }

        public static void Main(string[] args)
        {
            var host = WebHostBuilder.Build();

            host.Run();
        }
    }
}
