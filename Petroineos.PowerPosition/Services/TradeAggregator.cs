using Microsoft.Extensions.Logging;
using Petroineos.PowerPosition.Services.Intrefaces;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Petroineos.PowerPosition.Services
{
    public class TradeAggregator : ITradeAggregator
    { 
        private readonly ILogger logger;

        public TradeAggregator(ILogger<TradeAggregator> logger)
        {
            this.logger = logger;
        }

        public async Task<List<double>> AggregateVolumesAsync(IEnumerable<PowerTrade> trades)
        {
            this.logger.LogDebug("Calculating aggregated volumes");

            return await Task.Run(() =>
            { 
                var volumes = new List<double>();

                for (int period = 1; period <= 24; period++)
                {
                    var total = trades.Sum(a => a.Periods.Where(c => c.Period == period).First().Volume);
                    volumes.Add(total);
                }
                return volumes; 
            }); 
        }
    }
}
