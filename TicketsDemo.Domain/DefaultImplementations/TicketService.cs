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
    public class TicketService : ITicketService
    {
        private ITicketRepository _tickRepo;
        private IReservationRepository _resRepo;
        private IRunRepository _runRepository;
        private IPriceCalculationStrategy _priceCalculationStrategy;

        public TicketService(ITicketRepository tickRepo, IReservationRepository resRepo,
            IRunRepository runRepository, IPriceCalculationStrategy priceCalculationStrategy)
        {
            _tickRepo = tickRepo;
            _resRepo = resRepo;
            _runRepository = runRepository;
            _priceCalculationStrategy = priceCalculationStrategy;
        }

        public Ticket CreateTicket(int reservationId, string fName, string lName)
        {
            var res = _resRepo.Get(reservationId);

            if (res.TicketId != null) {
                throw new InvalidOperationException("ticket has been already issued to this reservation, unable to create another one");
            }

            var placeInRun = _runRepository.GetPlaceInRun(res.PlaceInRunId);

            var newTicket = new Ticket()
            {
                ReservationId = res.Id,
                CreatedDate = DateTime.Now,
                FirstName = fName,
                LastName = lName,
                Status = TicketStatusEnum.Active,
                PriceComponents = new List<PriceComponent>()
            };
            newTicket.PriceComponents = _priceCalculationStrategy.CalculatePrice(placeInRun);

            _tickRepo.Create(newTicket);
            return newTicket;
        }

        public void SellTicket(Ticket ticket)
        {
            if (ticket.Status == TicketStatusEnum.Sold)
            {
                throw new ArgumentException("ticket is already sold");
            }

            ticket.Status = TicketStatusEnum.Sold;
            _tickRepo.Update(ticket);
        }
    }
}
