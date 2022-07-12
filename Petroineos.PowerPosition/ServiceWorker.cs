using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Petroineos.PowerPosition.Services.Intrefaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Petroineos.PowerPosition
{
    public class ServiceWorker : BackgroundService
    {
        private readonly ILogger<ServiceWorker> logger;
        private readonly ITradeProcessor tradeProcessor;
        private readonly int schedulingInterval;
        public ServiceWorker(ILogger<ServiceWorker> logger, ITradeProcessor aggregator, int pollInterval)
        {
            this.logger = logger;
            this.tradeProcessor = aggregator;
            this.schedulingInterval = pollInterval; 
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Service started");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time"));

                    logger.LogInformation($"Processing started at: {currentTime}"); 
                    await this.tradeProcessor.Run(currentTime); 
                    await Task.Delay(schedulingInterval * 60 * 1000, stoppingToken); 
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, "ServiceWorker exception");
                    throw ex;
                }

            }
        }
    }
}
