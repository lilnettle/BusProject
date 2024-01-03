using Bus.DAL.Context;
using Bus.DAL.Entitayes;
using Microsoft.EntityFrameworkCore;
using WebBusServer.Services.Base;

namespace WebBusServer.Services
{
    public class SeedDataBuses : ISeedDataService
    {
        private readonly BusDB _dbContext;

        public SeedDataBuses(BusDB dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            // Додавання нових автобусів тільки, якщо база даних не містить жодного запису про автобус
            if (!_dbContext.Buses.Any())
            {
                var bus1 = new BusMashine { NumberBus = "A1" };
                var bus2 = new BusMashine { NumberBus = "B1" };

                _dbContext.Buses.AddRange(bus1, bus2);
                _dbContext.SaveChanges();
            }
        }
    }

}


