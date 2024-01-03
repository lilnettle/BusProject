using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBus.Models
{
    internal class TicketRequestModel
    {
        public int TripId { get; set; }
        public string UserEmail { get; set; }
    }
}
