using Microsoft.Extensions.Logging;
using Petroineos.PowerPosition.Services.Intrefaces;
using Services;
using System;
using System.Threading.Tasks;

namespace Petroineos.PowerPosition.Services
{
    public class TradeProcessor  : ITradeProcessor
    {
        private readonly IPowerService powerService;
        private readonly ITradeAggregator tradeAggregator;
        private readonly IFileWriter fileWriter;
        private readonly ILogger logger;
        private readonly string outputPath;
 
        public TradeProcessor(ILogger<TradeProcessor> logger, 
                          IPowerService powerService,
                          ITradeAggregator tradeAggregator,
                          IFileWriter fileHandler,
                          string outputPath)
        {
            this.logger = logger;  
            this.powerService = powerService;
            this.tradeAggregator = tradeAggregator;
            this.fileWriter = fileHandler;
            this.outputPath = outputPath;  
        }
         
        public async Task Run(DateTime startTime)
        {
            this.logger.LogDebug("Starting Aggregator at: {0}", startTime);
            try
            {
                this.logger.LogInformation($"Getting Trades for Time: {startTime}");
                var trades = await this.powerService.GetTradesAsync(startTime);

                this.logger.LogDebug("Aggregating volumes");
                var aggregatedVolumes = await this.tradeAggregator.AggregateVolumesAsync(trades);

                this.logger.LogInformation("Writing data to file");
                await this.fileWriter.WriteCsvAsync(this.outputPath, startTime, aggregatedVolumes);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Exception occurred"); 
                throw ex;
            }
        }
          
    }
}
