using System.Collections.Generic;
using Bus.DAL.Entitayes.Base;
using System;

namespace Bus.DAL.Entitayes
{
    public class BusMashine : Entity
    {
        public string NumberBus { get; set; }
        public virtual ICollection<Trip>? Trips { get; set; }
    }
}
