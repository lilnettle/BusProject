using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bus.DAL.Entitayes;


namespace Bus.DAL.Context
{
    public class BusDB : DbContext
    {


        public virtual DbSet<User> Users { get; set; }


        public virtual DbSet<BusMashine> Buses { get; set; }

        public virtual DbSet<Trip> Trips { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }

        public BusDB() { }

        public BusDB(DbContextOptions<BusDB> options) : base(options)
        {

        }


    }
}
