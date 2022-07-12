using System;
using System.Threading.Tasks;

namespace Petroineos.PowerPosition.Services.Intrefaces
{
    public interface ITradeProcessor
    {
        Task Run(DateTime startTime);
    }
}
