using Bus.DAL.Context;
using Bus.DAL.Entitayes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace WebBusServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController:Controller
    {
        private readonly BusDB _dbContext;

        public TicketController(BusDB dbContext)
        {
            _dbContext = dbContext;
        }

        public class TicketRequestModel
        {
            public int TripId { get; set; }
            public string UserEmail { get; set; }
        }

        public class TicketsModel
        {

            public int ID { get; set; }
            public string? UserEmail { get; set; }


        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateTicket([FromBody] TicketRequestModel requestModel)
        {
            try
            {
                var trip = await _dbContext.Trips.Include(t => t.BusMashine).FirstOrDefaultAsync(t => t.Id == requestModel.TripId);
                if (trip == null)
                    return NotFound("Trip not found");

                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == requestModel.UserEmail);
                if (user == null)
                    return NotFound("User not found");

                if (trip.AvailableSeats <= 0)
                    return BadRequest("Немає вільних місць");

                var ticket = new Ticket
                {
                    User = user,
                    Trip = trip,
                    
                    // Додайте інші поля за необхідності
                };

                // Оновлення кількості вільних місць
                var tripsWithSameDepartureTime = await _dbContext.Trips
                    .Where(t => t.DepartureTime == trip.DepartureTime)
                    .ToListAsync();

                foreach (var tripWithSameDepartureTime in tripsWithSameDepartureTime)
                {
                    tripWithSameDepartureTime.AvailableSeats -= 1;
                }

                // Додайте квиток до колекції квитків поїздки
                if (trip.Tickets == null)
                {
                    trip.Tickets = new List<Ticket>();
                }
                trip.Tickets.Add(ticket);

                // Додайте квиток до колекції квитків користувача
                if (user.Tickets == null)
                {
                    user.Tickets = new List<Ticket>();
                }
                user.Tickets.Add(ticket);

                _dbContext.Tickets.Add(ticket);
                await _dbContext.SaveChangesAsync();

                return Ok("Квиток замовлено!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating ticket: {ex.Message}");
            }
        }

        [HttpGet("getUserTickets")]
        public async Task<ActionResult<IEnumerable<TicketsModel>>> GetUserTickets()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

                if (jsonToken == null)
                    return BadRequest("Користувач не авторизований");

                var userIdClaim = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "nameidentifier");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))

                    return BadRequest("Невірний або відсутній 'nameid' у токені");
                var user = await _dbContext.Users.FindAsync(userId);

                // Знайдіть квитки для користувача
                var userTickets = await _dbContext.Tickets
                    .Where(t => t.User.Id == userId)
                    .ToListAsync();

                // Створіть ViewModel для квитків
                var ticketModel = userTickets
                    .Where(t => t.User != null)
    .Select(t => new TicketsModel
    {
        ID = t.Id,
        UserEmail = t.UserEmail






        // Додайте інші поля за необхідності
    }) ;

                return Ok(ticketModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting user tickets: {ex.Message}");
            }
        }


        [HttpDelete("delete/{ticketId}")]
        public async Task<IActionResult> DeleteTicket(int ticketId, [FromQuery] string userEmail)
        {
            try
            {
                var ticket = await _dbContext.Tickets
                    .Include(t => t.User)
                    .Include(t => t.Trip)  // Включаємо дані поїздки для квитка
                    .FirstOrDefaultAsync(t => t.Id == ticketId);

                if (ticket == null)
                {
                    return NotFound($"Ticket with ID {ticketId} not found.");
                }

                // Перевіряємо, чи користувач з указаним email є власником квитка
                if (ticket.User?.Email != userEmail)
                {
                    return BadRequest("You do not have permission to delete this ticket.");
                }

                // Зменшуємо кількість вільних місць в подорожі
                if (ticket.Trip != null)
                {
                    var tripsWithSameDepartureTime = await _dbContext.Trips
                        .Where(t => t.DepartureTime == ticket.Trip.DepartureTime)
                        .ToListAsync();

                    foreach (var tripWithSameDepartureTime in tripsWithSameDepartureTime)
                    {
                        // Перевірте за ідентифікатором, а не об'єктом Trip
                        if (tripWithSameDepartureTime.Id == ticket.Trip.Id)
                        {
                            tripWithSameDepartureTime.AvailableSeats += 1;
                        }
                    }
                }

                _dbContext.Tickets.Remove(ticket);
                await _dbContext.SaveChangesAsync();

                
            




                return Ok("Скасування квитка успішне!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error deleting ticket: {ex.Message}");
            }
        }
    }
}

