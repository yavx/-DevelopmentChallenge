using Microsoft.Extensions.Logging;
using Petroineos.PowerPosition.Services.Intrefaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Petroineos.PowerPosition.Services
{
    public class FileWriter : IFileWriter
    { 
        private readonly ILogger Logger;

        public FileWriter(ILogger<FileWriter> logger)
        {
            this.Logger = logger;
        }
         
        public async Task WriteCsvAsync(string outputPath, DateTime startTime, List<double> tradeInfo)
        { 
            string fileName = $"{outputPath}\\{startTime:yyyyMMdd_HHm}.csv";
                    
            this.Logger.LogInformation($"Writing to file: {fileName}");
             
            using var writer = new StreamWriter(fileName);

            await writer.WriteLineAsync("Local Time, Volume");
             
            for (int period = 0; period < 24; period++)
            {
                var localTimeString = period == 0 ? "23:00" : $"{period - 1:00}:00"; 
                var data = $"{localTimeString},{tradeInfo[period]}"; 
                await writer.WriteLineAsync(data);
            } 
        }  
    }
}

