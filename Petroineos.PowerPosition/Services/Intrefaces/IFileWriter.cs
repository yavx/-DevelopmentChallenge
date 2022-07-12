using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Petroineos.PowerPosition.Services.Intrefaces
{
    public interface IFileWriter
    {
        Task WriteCsvAsync(string outputPath, DateTime startTime, List<double> tradeInfo);
    }
}

