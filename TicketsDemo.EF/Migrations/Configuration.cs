namespace TicketsDemo.EF.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TicketsDemo.Data.Entities;
    using TicketsDemo.Data.Entities.HolidayAggregate;

    internal sealed class Configuration : DbMigrationsConfiguration<TicketsDemo.EF.TicketsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TicketsDemo.EF.TicketsContext context)
        {
            //This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            
            Func<List<Place>> placeGenerator = () =>
            {
                var retIt = new List<Place>();
                Random random = new Random();
                 

                for (int i = 0; i < 100; i++)
                {
                    decimal randomNumber = random.Next(80, 120);
                    var newPlace = new Place() { Number = i, PriceMultiplier = randomNumber / 100 };
                    retIt.Add(newPlace);
                }
                return retIt;
            };

            context.Trains.AddOrUpdate(
              t => t.Number,
              new Train
              {
                  Number = 90,
                  StartLocation = "Kiev",
                  EndLocation = "Odessa",
                  Carriages = new List<Carriage>() { 
                      new Carriage() { 
                          Places = placeGenerator(),
                          Type = CarriageType.SecondClassSleeping,
                          DefaultPrice = 100m,
                          Number = 1,
                      },new Carriage() { 
                          Places = placeGenerator(),
                          Type = CarriageType.SecondClassSleeping,
                          DefaultPrice = 100m,
                          Number = 2,
                      },new Carriage() { 
                          Places = placeGenerator(),
                          Type = CarriageType.FirstClassSleeping,
                          DefaultPrice = 120m,
                          Number = 3,
                      },new Carriage() { 
                          Places = placeGenerator(),
                          Type = CarriageType.FirstClassSleeping,
                          DefaultPrice = 130m,
                          Number = 4,
                      } 
                  }
              },
              new Train
              {
                  Number = 720,
                  StartLocation = "Kiev",
                  EndLocation = "Vinnitsa",
                  Carriages = new List<Carriage>() { 
                      new Carriage() { 
                          Places = placeGenerator(),
                          Type = CarriageType.Sedentary,
                          DefaultPrice = 40m,
                          Number = 1,
                      },new Carriage() { 
                          Places = placeGenerator(),
                          Type = CarriageType.Sedentary,
                          DefaultPrice = 40m,
                          Number = 2,
                      },new Carriage() { 
                          Places = placeGenerator(),
                          Type = CarriageType.Sedentary,
                          DefaultPrice = 40m,
                          Number = 3,
                      } 
                  }
              }

            );

            context.Holidays.AddOrUpdate(
                h=>h.Id,
                new Holiday 
                { 
                    Date = new DateTime(2020, 1, 1)
                },
                new Holiday
                {
                    Date = new DateTime(2020, 1, 7)
                },
                new Holiday
                {
                    Date = new DateTime(2020, 1, 14)
                },
                new Holiday
                {
                    Date = new DateTime(2020, 3, 8)
                },
                new Holiday
                {
                    Date = new DateTime(2020, 4, 19)
                },
                new Holiday
                {
                    Date = new DateTime(2020, 5, 1)
                }
                );
            
        }
    }
}
