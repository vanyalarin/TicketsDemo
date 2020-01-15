using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsDemo.Data.Entities;
using TicketsDemo.Data.Repositories;
using TicketsDemo.Domain.Interfaces;

namespace TicketsDemo.Domain.DefaultImplementations
{
    public class HolidaysPriceCalculationDecorator : IPriceCalculationStrategy
    {
        IPriceCalculationStrategy _priceCalculationStrategy;
        IHolidayRepository _holidayRepository;
        public HolidaysPriceCalculationDecorator(IPriceCalculationStrategy priceCalculationStrategy, IHolidayRepository holidayRepository)
        {
            _priceCalculationStrategy = priceCalculationStrategy;
            _holidayRepository = holidayRepository;
        }

        public List<PriceComponent> CalculatePrice(PlaceInRun placeInRun)
        {
            var components = new List<PriceComponent>();
            components.AddRange(_priceCalculationStrategy.CalculatePrice(placeInRun));
            
            if(IsHoliday(placeInRun.Run.Date))
            {
                var sum = 0m;

                foreach (var component in components)
                {
                    sum += component.Value;
                }

                components.Add(new PriceComponent { Name = "Holidays fee", Value = sum * Constants.HolidayPriceMultiplier });
            }
       
            return components;
        }

        private bool IsHoliday(DateTime date)
        {
            var holidays = _holidayRepository.GetHolidays().Select(h => h.Date);
            return holidays.Any(h => h.Date == date.Date);
        }
    }
}
