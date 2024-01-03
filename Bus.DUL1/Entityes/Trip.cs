using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Bus.DAL.Entitayes.Base;
using Bus.DAL.Entitayes;

namespace Bus.DAL.Entitayes
{
    public class Trip : Entity
    {
        public virtual BusMashine? BusMashine { get; set; }
        public string? StartCity { get; set; }

        public string? FinishCity { get; set; }

       
        public string? DepartureDate { get; set; }

        
        public string? DepartureTime { get; set; }

     
        public string? ArrivalTime { get; set; }

        public int AvailableSeats { get; set; }

        public int Price { get; set; }

        public virtual ICollection<Ticket>? Tickets { get; set; }
    }
}
