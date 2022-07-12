using FluentAssertions;
using Petroineos.PowerPosition.Services;
using Petroineos.PowerPosition.Tests.Mocks;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Petroineos.PowerPosition.Tests.Services
{
    public class TradeAggregatorTests
    {  
        [Fact]
        public async Task Test_Aggregator_Aggregation_Totals()
        { 
            List<PowerTrade> trades = new List<PowerTrade>();
                      
            var pt = PowerTrade.Create(DateTime.Now, 24);
            for (int i = 0; i < 24; i++)
            {
                pt.Periods[i].Period = i + 1;
                pt.Periods[i].Volume = i * 50;
            }
            trades.Add(pt);

            pt = PowerTrade.Create(DateTime.Now, 24);
            for (int i = 0; i < 24; i++)
            {
                pt.Periods[i].Period = i + 1;
                pt.Periods[i].Volume = i * 40;
            }
            trades.Add(pt);

            var loggerMock = new LoggerMock<TradeAggregator>(); 

            var tradeAggregator = new TradeAggregator(loggerMock);

            var volumes = await tradeAggregator.AggregateVolumesAsync(trades);

            volumes.Count().Should().Be(24);
            for (int period = 1; period <= 24; period++)
            {
                volumes[period - 1].Should().Be((period - 1) * 90);
            } 
        }

    }
     
}
