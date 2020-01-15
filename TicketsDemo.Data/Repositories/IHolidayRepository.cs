using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsDemo.Data.Entities.HolidayAggregate;

namespace TicketsDemo.Data.Repositories
{
    public interface IHolidayRepository
    {
        List<Holiday> GetHolidays();
    }
}
