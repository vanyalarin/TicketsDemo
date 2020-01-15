using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsDemo.CSV.Interfaces;

namespace TicketsDemo.CSV
{
    public class Configuration : IConfiguration
    {
        public string CsvRootPath { 
            get 
            { 
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                appDirectory = appDirectory.Remove(appDirectory.Length - 1);
                var solution = Directory.GetParent(appDirectory);
                return solution + @"\TicketsDemo.CSV"; 
            }
        }
    }
}
