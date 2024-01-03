using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientBus.Models
{
    internal class BusMashineModel
    {
        public int Id { get; set; }
        public string NumberBus { get; set; }
        public virtual ICollection<TripModel> Trips { get; set; }
    }
}


