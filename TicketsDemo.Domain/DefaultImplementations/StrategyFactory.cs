using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsDemo.Data.Repositories;
using TicketsDemo.Domain.DefaultImplementations.PriceCalculationStrategy;
using TicketsDemo.Domain.Interfaces;

namespace TicketsDemo.Domain.DefaultImplementations
{
    public class StrategyFactory : IStrategyFactory
    {
        private IRunRepository _runRepository;
        private ITrainRepository _trainRepository;
        public StrategyFactory(IRunRepository runRepository, ITrainRepository trainRepository)
        {
            _runRepository = runRepository;
            _trainRepository = trainRepository;
        }
        public IPriceCalculationStrategy GetStrategy(DateTime runDate)
        {
            var defStrategy = new DefaultPriceCalculationStrategy(_runRepository, _trainRepository);
            if (IsHoliday(runDate))
            {
                return new HolidaysPriceCalculationDecorator(defStrategy);
            }
            else
            {
                return defStrategy;
            }
        }

        private bool IsHoliday(DateTime date)
        {
            var holidays = GetHolidays();
            return holidays.Any(d => d.Date == date.Date);
        }

        private List<DateTime> GetHolidays()
        {
            return new List<DateTime>() {
                new DateTime(2020, 1, 1),
                new DateTime(2020, 1, 7),
                new DateTime(2020, 1, 14),
                new DateTime(2020, 3, 8),
                new DateTime(2020, 4, 19),
                new DateTime(2020, 5, 1) };
        }
    }
}
