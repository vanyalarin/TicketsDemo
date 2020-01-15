using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsDemo.Data.Entities.HolidayAggregate;
using TicketsDemo.Data.Repositories;

namespace TicketsDemo.EF.Repositories
{
    public class HolidayRepository : IHolidayRepository
    {
        public List<Holiday> GetHolidays()
        {
            using(var context = new TicketsContext())
            {
                return context.Holidays.ToList();
            }
        }
    }
}
