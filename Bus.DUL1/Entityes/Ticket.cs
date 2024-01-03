using System.Linq;
using Bus.DAL.Entitayes.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Bus.DAL.Entitayes
{
    public class Ticket : Entity
    {
        public string? UserEmail => User?.Email;

        public string? BusNumber => Trip?.BusMashine?.NumberBus;

        public virtual User? User { get; set; }
      
        public virtual Trip? Trip { get; set; }

        public string? CityStart => Trip?.StartCity;
        public string? CityFinish => Trip?.FinishCity;
        public int? TripPrice => Trip?.Price;

        public string? DateDeparture => Trip?.DepartureDate;
       



    }
}
