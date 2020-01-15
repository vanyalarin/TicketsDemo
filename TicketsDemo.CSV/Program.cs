using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using TicketsDemo.Data.Entities;
using TicketsDemo.CSV;
using CsvHelper.Configuration;

namespace TicketsDemo.CSV
{
    

    class Program
    {
        static Func<List<Place>> placeGenerator = () =>
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

        static List<Place> generatePlaces(List<Carriage> carriages)
        {
            List<Place> places = new List<Place>();
            int placeCount = 0;
            int id = 0;
            int carriageCount = carriages.Count;
            Random random = new Random();

            for (int carriageCounter = 0; carriageCounter < carriageCount; carriageCounter++)
            {
                Carriage thisCarriage = carriages[carriageCounter];

                switch (thisCarriage.Type)
                {
                    case CarriageType.Sedentary:
                        placeCount = random.Next(100, 150);
                        break;
                    case CarriageType.SecondClassSleeping:
                        placeCount = random.Next(20, 25);
                        break;
                    case CarriageType.FirstClassSleeping:
                        placeCount = random.Next(10, 15);
                        break;
                }

                for (int placeCounter = 0; placeCounter <= placeCount; id++, placeCounter++)
                {
                    places.Add(new Place(){
                        Id = id,
                        Number = placeCounter,
                        PriceMultiplier = (random.Next(80, 120) / 100),
                        CarriageId = thisCarriage.Id
                    });
                }
            }

            return places;
        }



        static void Main(string[] args)
        {
            string pathTrainRepositoryCSV = "D:\\progs\\TicketsDemo\\TicketsDemoCSV\\TrainRepository.csv";
            string pathCarriageRepositoryCSV = "D:\\progs\\TicketsDemo\\TicketsDemoCSV\\CarriageRepository.csv";
            string pathPlaceRepositoryCSV = "D:\\progs\\TicketsDemo\\TicketsDemoCSV\\PlaceRepository.csv";

            List<Carriage> carriages;
            Random random = new Random();

            using (CsvReader csvReader = new CsvReader(new StreamReader(pathCarriageRepositoryCSV))) {
                csvReader.Configuration.HasHeaderRecord = false;
                csvReader.Configuration.RegisterClassMap<CSV.Repositories.CarriageMap>();
                carriages = csvReader.GetRecords<Carriage>().ToList<Carriage>(); 
            }

            foreach(Carriage carriage in carriages)
            {
                carriage.DefaultPrice = (decimal)random.Next(40, 100);
            }

            using (CsvWriter csvWriter = new CsvWriter(new StreamWriter(pathCarriageRepositoryCSV)))
            {
                csvWriter.Configuration.HasHeaderRecord = false;
                csvWriter.Configuration.Delimiter = "\t;";
                csvWriter.Configuration.RegisterClassMap<CSV.Repositories.CarriageMap>();
                csvWriter.WriteRecords(carriages);
            }

            //List<Carriage> carriages;
            //List<Place> places;

            //using (CsvReader csvReader = new CsvReader(new StreamReader(pathCarriageRepositoryCSV)))
            //{
            //    csvReader.Configuration.HasHeaderRecord = false;
            //    csvReader.Configuration.RegisterClassMap<Repositories.CarriageMap>();
            //    carriages = csvReader.GetRecords<Carriage>().ToList<Carriage>();
            //}

            //places = generatePlaces(carriages);

            //using (CsvWriter csvWriter = new CsvWriter(new StreamWriter(pathPlaceRepositoryCSV)))
            //{
            //    csvWriter.Configuration.HasHeaderRecord = false;
            //    csvWriter.Configuration.RegisterClassMap<Repositories.PlaceMap>();
            //    csvWriter.Configuration.Delimiter = ";\t";
            //    csvWriter.WriteRecords(places);
            //}

            //List<Train> trainList = new List<Train>();
            //Train _train = new Train
            //{
            //    Number = 90,
            //    StartLocation = "Kiev",
            //    EndLocation = "Odessa",
            //    Carriages = new List<Carriage>() {
            //          new Carriage() {
            //              Places = placeGenerator(),
            //              Type = CarriageType.SecondClassSleeping,
            //              DefaultPrice = 100m,
            //              Number = 1,
            //          },new Carriage() {
            //              Places = placeGenerator(),
            //              Type = CarriageType.SecondClassSleeping,
            //              DefaultPrice = 100m,
            //              Number = 2,
            //          },new Carriage() {
            //              Places = placeGenerator(),
            //              Type = CarriageType.FirstClassSleeping,
            //              DefaultPrice = 120m,
            //              Number = 3,
            //          },new Carriage() {
            //              Places = placeGenerator(),
            //              Type = CarriageType.FirstClassSleeping,
            //              DefaultPrice = 130m,
            //              Number = 4,
            //          }
            //      }
            //};
            //List<Carriage> carriages = new List<Carriage>() {
            //          new Carriage() {
            //              Id = 0,
            //              Places = placeGenerator(),
            //              Type = CarriageType.SecondClassSleeping,
            //              DefaultPrice = 100m,
            //              Number = 1,
            //          },new Carriage() {
            //              Id = 1,
            //              Places = placeGenerator(),
            //              Type = CarriageType.SecondClassSleeping,
            //              DefaultPrice = 100m,
            //              Number = 2,
            //          },new Carriage() {
            //              Id = 2,
            //              Places = placeGenerator(),
            //              Type = CarriageType.FirstClassSleeping,
            //              DefaultPrice = 120m,
            //              Number = 3,
            //          },new Carriage() {
            //              Id = 3,
            //              Places = placeGenerator(),
            //              Type = CarriageType.FirstClassSleeping,
            //              DefaultPrice = 130m,
            //              Number = 4,
            //          }
            //      };

            //using (CsvReader csvReader = new CsvReader(new StreamReader(pathCarriageRepositoryCSV)))
            //{
            //    csvReader.Configuration.RegisterClassMap<CarriageMap>();
            //    csvReader.Configuration.HasHeaderRecord = false;
            //    carriages = csvReader.GetRecords<Carriage>().ToList<Carriage>();
            //}
        }
    }
}
