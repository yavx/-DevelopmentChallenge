using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Petroineos.PowerPosition.Config;
using Petroineos.PowerPosition.Services;
using Petroineos.PowerPosition.Services.Intrefaces;
using Services;

namespace Petroineos.PowerPosition
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureLogging((context, builder) =>
                 {
                     builder.AddFile(context.Configuration.GetLoggerConfiguration());
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    int schedulingInterval = hostContext.Configuration.Get<int>("SchedulingInterval");
                    string outputPath = hostContext.Configuration.Get<string>("OutputPath");

                    services.AddTransient<ITradeAggregator, TradeAggregator>();
                    services.AddTransient<IFileWriter, FileWriter>();
                    services.AddTransient<IPowerService, PowerService>();

                    services.AddTransient<ITradeProcessor, TradeProcessor>(x =>
                       new TradeProcessor(
                           x.GetRequiredService<ILogger<TradeProcessor>>(),
                           x.GetRequiredService<IPowerService>(),
                           x.GetRequiredService<ITradeAggregator>(),
                           x.GetRequiredService<IFileWriter>(),
                           outputPath)
                       );

                    services.AddHostedService(x =>
                        new ServiceWorker(
                            x.GetRequiredService<ILogger<ServiceWorker>>(),
                            x.GetRequiredService<ITradeProcessor>(),
                            schedulingInterval));

                })  
#if RELEASE
            //Run as a service if in Release mode 
                       .UseWindowsService()  
#endif
            ;

    }
}
