using Bus.DAL.Context;
using Bus.DAL.Entitayes;
using System.Globalization;
using WebBusServer.Services.Base;

namespace WebBusServer.Services
{

    public class SeedDataTrips : ISeedDataService
    {
        private readonly BusDB _dbContext;

        public SeedDataTrips(BusDB dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
              // Перевірка чи база даних містить записи про поїздки
              if (!_dbContext.Trips.Any())
              {
                  // Отримання автобусів
                  var bus1 = _dbContext.Buses.FirstOrDefault(b => b.NumberBus == "A1");
                  var bus2 = _dbContext.Buses.FirstOrDefault(b => b.NumberBus == "B1");

                  // Якщо автобуси існують, додаємо поїздки
                  if (bus1 != null && bus2 != null)
                  {
                      var trip1 = new Trip
                      {
                          BusMashine = bus1,
                          StartCity = "Львів",
                          FinishCity = "Київ",
                          DepartureDate ="19.12.2023",
                          DepartureTime ="19.12.2023:13.00",
                          ArrivalTime = "19.12.2023:20.00",
                          AvailableSeats = 15,
                          Price = 400
                      };

                      var trip2 = new Trip
                      {
                          BusMashine = bus1,
                          StartCity = "Львів",
                          FinishCity = "Житомир",
                          DepartureDate = "19.12.2023",
                          DepartureTime = "19.12.2023:13.00",
                          ArrivalTime = "19.12.2023:18.00",
                          AvailableSeats = 15,
                          Price = 270
                      };

                      var trip3 = new Trip
                      {
                          BusMashine = bus1,
                          StartCity = "Львів",
                          FinishCity = "Рівне",
                          DepartureDate = "19.12.2023",
                          DepartureTime = "19.12.2023:13.00",
                          ArrivalTime = "19.12.2023:15.00",
                          AvailableSeats = 15,
                          Price = 200
                      };

                      var trip4 = new Trip
                      {
                          BusMashine = bus2,
                          StartCity = "Львів",
                          FinishCity = "Київ",
                          DepartureDate = "18.12.2023",
                          DepartureTime = "18.12.2023:08.00",
                          ArrivalTime = "18.12.2023:16.00",
                          AvailableSeats = 15,
                          Price = 500
                      };

                    var trip6 = new Trip
                    {
                        BusMashine = bus2,
                        StartCity = "Львів",
                        FinishCity = "Київ",
                        DepartureDate = "19.12.2023",
                        DepartureTime = "19.12.2023:08.00",
                        ArrivalTime = "19.12.2023:16.00",
                          AvailableSeats = 15,
                          Price = 500
                      };

                      var trip5 = new Trip
                      {
                          BusMashine = bus2,
                          StartCity = "Львів",
                          FinishCity = "Тернопіль",
                          DepartureDate = "18.12.2023",
                          DepartureTime = "18.12.2023:08.00",
                          ArrivalTime = "18.12.2023:10.00",
                          AvailableSeats = 15,
                          Price = 200
                      };

                    var trip7 = new Trip
                    {
                        BusMashine = bus2,
                        StartCity = "Львів",
                        FinishCity = "Хмельницький",
                        DepartureDate = "18.12.2023",
                        DepartureTime = "18.12.2023:08.00",
                          ArrivalTime = "18.12.2023:08.00",
                          AvailableSeats = 15,
                          Price = 200
                      };

                      bus1.Trips ??= new List<Trip>();
                      bus1.Trips.Add(trip1);
                      bus1.Trips.Add(trip2);
                      bus1.Trips.Add(trip3);

                      bus2.Trips ??= new List<Trip>();
                      bus2.Trips.Add(trip4);
                      bus2.Trips.Add(trip5);
                      bus2.Trips.Add(trip6);
                      bus2.Trips.Add(trip7);

                      // Перевірка чи база даних не містить записів про поїздки перед додаванням нових
                      if (!_dbContext.Trips.Any())
                      {
                          // Додаємо нові поїздки
                          _dbContext.Trips.AddRange(trip1, trip2, trip3, trip4, trip5, trip6, trip7);
                          // Збереження змін в базі даних
                          _dbContext.SaveChanges();
                      }
                  }
              }
          }
      }
        
    
}
    
