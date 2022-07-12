using Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Petroineos.PowerPosition.Services.Intrefaces
{
    public interface ITradeAggregator
    {
        Task<List<double>> AggregateVolumesAsync(IEnumerable<PowerTrade> trades);
    }
}
