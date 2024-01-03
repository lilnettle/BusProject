using Bus.DAL.Context;
using Bus.DAL.Entitayes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebBusServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TripConroller : Controller
    {
        private readonly BusDB _dbContext;

        public TripConroller(BusDB dbContext)
        {
            _dbContext = dbContext;
        }

        public class TripSearchModel
        {
            public string StartCity { get; set; }
            public string FinishCity { get; set; }
            public string DepartureDate { get; set; }
        }

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

           
        }

        [HttpPost("search")]
        public ActionResult<IEnumerable<Trip>> SearchTrips([FromBody] TripSearchModel criteria)
        {
            try
            {
                var trips = _dbContext.Trips
                    .Include(t => t.BusMashine)
                    .Where(t =>
                        t.StartCity == criteria.StartCity &&
                        t.FinishCity == criteria.FinishCity &&
                        t.DepartureDate == criteria.DepartureDate)
                    .Select(t => new TripModel
                    {
                        Id = t.Id,
                        NumberBus = t.BusMashine.NumberBus ?? string.Empty,
                        StartCity = t.StartCity,
                        FinishCity = t.FinishCity,
                        DepartureDate = t.DepartureDate,
                        DepartureTime = t.DepartureTime,
                        ArrivalTime = t.ArrivalTime,
                        AvailableSeats = t.AvailableSeats,
                        Price = t.Price,

                    })
                    .ToList();

                return Ok(trips);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during trip search: {ex.Message}");
            }
          
        }
    }

}
