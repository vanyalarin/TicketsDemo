using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsDemo.CSV.Interfaces
{
    public interface IConfiguration
    {
        string CsvRootPath { get; }
    }
}
