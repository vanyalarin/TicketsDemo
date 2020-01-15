using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsDemo.Data.Entities;
using TicketsDemo.Data.Repositories;
using CsvHelper;
using System.IO;
using CsvHelper.Configuration;
using TicketsDemo.CSV.Interfaces;

namespace TicketsDemo.CSV.Repositories
{
    public class TrainRepository : ITrainRepository
    { 
        string pathTrainRepositoryCSV;
        string pathCarriageRepositoryCSV ;
        string pathPlaceRepositoryCSV;

        public TrainRepository(IConfiguration configuration)
        {
            pathTrainRepositoryCSV = configuration.CsvRootPath + @"\TrainRepository.csv";
            pathCarriageRepositoryCSV = configuration.CsvRootPath + @"\CarriageRepository.csv";
            pathPlaceRepositoryCSV = configuration.CsvRootPath + @"\PlaceRepository.csv";
        }

        public void CreateTrain(Train train)
        {
            List<Train> allTrains = GetAllTrains();

            allTrains.Add(train);
            WriteTrainsToScvFile(allTrains);
        }

        public void DeleteTrain(Train train)
        {
            List<Train> allTrains = GetAllTrains();
            Train trainToRemove;

            trainToRemove = allTrains.Single(t => t.Id == train.Id);
            allTrains.Remove(trainToRemove);
            WriteTrainsToScvFile(allTrains);
        }

        public List<Train> GetAllTrains()
        {
            List<Place> places;
            List<Carriage> carriages;
            List<Train> trains;

            using (CsvReader csvReader = new CsvReader(new StreamReader(pathTrainRepositoryCSV)))
            {
                csvReader.Configuration.HasHeaderRecord = false;
                csvReader.Configuration.RegisterClassMap<TrainMap>();
                trains = csvReader.GetRecords<Train>().ToList();
            }
            using (CsvReader csvReader = new CsvReader(new StreamReader(pathCarriageRepositoryCSV)))
            {
                csvReader.Configuration.RegisterClassMap<CarriageMap>();
                csvReader.Configuration.HasHeaderRecord = false;
                carriages = csvReader.GetRecords<Carriage>().ToList();
            }
            using (CsvReader csvReader = new CsvReader(new StreamReader(pathPlaceRepositoryCSV)))
            {
                csvReader.Configuration.RegisterClassMap<PlaceMap>();
                csvReader.Configuration.HasHeaderRecord = false;
                places = csvReader.GetRecords<Place>().ToList();
            }

            foreach(Carriage carriage in carriages)
            {
                int carriageId = carriage.Id;
                carriage.Places = new List<Place>();

                foreach(Place place in places)
                {
                    if(carriageId == place.CarriageId)
                    {
                        place.CarriageId = carriageId;
                        place.Carriage = carriage;
                        carriage.Places.Add(place);
                    }
                }
            }

            foreach(Train train in trains)
            {
                int trainId = train.Id;
                train.Carriages = new List<Carriage>();

                foreach(Carriage carriage in carriages)
                {
                    if(trainId == carriage.TrainId)
                    {
                        carriage.TrainId = trainId;
                        carriage.Train = train;
                        train.Carriages.Add(carriage);
                    }
                }
            }

            return trains;
        }

        public Train GetTrainDetails(int trainId)
        {
            List<Train> allTrains = GetAllTrains();
            return allTrains.Find(t => t.Id == trainId);
        }

        public void UpdateTrain(Train train)
        {
            List<Train> allTrains = GetAllTrains();
            Train trainToRemove = allTrains.Single(x => x.Id == train.Id);

            allTrains.Remove(trainToRemove);
            allTrains.Add(train);

            using (CsvWriter writer = new CsvWriter(new StreamWriter(pathTrainRepositoryCSV))) {
                writer.Configuration.Delimiter = "\t;";
                writer.Configuration.HasHeaderRecord = false;
                writer.Configuration.RegisterClassMap<TrainMap>();
                writer.WriteRecords(allTrains);
            }
        }

        private void WriteTrainsToScvFile(List<Train> trains)
        {
            using (CsvWriter csvWriter = new CsvWriter(new StreamWriter(pathTrainRepositoryCSV)))
            {
                csvWriter.Configuration.HasHeaderRecord = false;
                csvWriter.Configuration.RegisterClassMap<TrainMap>();
                csvWriter.Configuration.Delimiter = "\t;";
                csvWriter.WriteRecords(trains);
            }
        }
    }

    public sealed class TrainMap : ClassMap<Train> {
        public TrainMap()
        {
            Map(t => t.Id);
            Map(t => t.Number);
            Map(t => t.StartLocation);
            Map(t => t.EndLocation);
        }
    }

    public sealed class CarriageMap : ClassMap<Carriage>
    {
        public CarriageMap()
        {
            Map(c => c.Id);
            Map(c => c.Type);
            Map(c => c.DefaultPrice);
            Map(c => c.TrainId);
            Map(c => c.Number);
        }
    }

    public sealed class PlaceMap : ClassMap<Place>
    {
        public PlaceMap()
        {
            Map(p => p.Id);
            Map(p => p.Number);
            Map(p => p.PriceMultiplier);
            Map(p => p.CarriageId);
        }
    }
}
