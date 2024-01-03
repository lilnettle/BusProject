using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBus.Models
{
    internal class TripModel
    {
        public int Id { get; set; }

        public string NumberBus { get; set; }
        public string StartCity { get; set; }

       
        public string FinishCity { get; set; }

        public string DepartureDate { get; set; }

        public string DepartureTime { get; set; }

        public string ArrivalTime { get; set; }

        public int AvailableSeats { get; set; }

        public int Price { get; set; }

        public virtual ICollection<TicketsModel> Tickets { get; set; }
    }
}
