using Microsoft.AspNetCore.Mvc;
using Bus.DAL;
using Bus.DAL.Context;
using Bus.DAL.Entitayes;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebBusServer.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Http;

namespace WebBusServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly string _jwtSecretKey = "MySuperSecretKey123!IncreaseTheKeySize"; // Замініть на справжній секретний ключ

        private readonly BusDB _dbContext;
       
       

      


        public UserController(BusDB dbContext)
        {
            _dbContext = dbContext;
            
        }


        public class UserLoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }



        [HttpPost("register")]
        public async Task<IActionResult> RegisterUserAsync([FromBody] User newUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Перевірка на унікальність електронної адреси
                    if (_dbContext.Users.Any(u => u.Email == newUser.Email))
                    {
                        ModelState.AddModelError("Email", "Користувач з такою електронною адресою вже існує.");
                        return BadRequest(ModelState);
                    }

                    // Перевірка на унікальність номера телефону
                    if (_dbContext.Users.Any(u => u.PhoneNumber == newUser.PhoneNumber))
                    {
                        ModelState.AddModelError("PhoneNumber", "Користувач з таким номером телефону вже існує.");
                        return BadRequest(ModelState);
                    }

                    // Додавання користувача
                    _dbContext.Users.Add(newUser);
                    await _dbContext.SaveChangesAsync();
                    HttpContext.Session.SetInt32("LoggedInUserId", newUser.Id);
                    return CreatedAtAction("GetUserById", new { id = newUser.Id }, newUser);
                   

                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                // Логування помилки
                Console.Error.WriteLine($"Помилка при реєстрації користувача: {ex.Message}");
                return StatusCode(500, "Виникла помилка під час реєстрації користувача.");
            }
        }

        [HttpGet("getUser/{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(id);
               

                if (user == null)
                {
                    return NotFound($"Користувача з ID {id} не знайдено");
                }

                return Ok(user);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Помилка при отриманні користувача: {ex.Message}");
                return StatusCode(500, "Виникла помилка під час отримання користувача.");
            }

            
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("nameidentifier", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                // Додайте додаткові клейми, які вам потрібні
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1), // Тут можна налаштувати термін дії токена
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        [HttpPost("login")]
        public async Task<ActionResult<bool>> LoginAsync([FromBody] UserLoginModel loginModel)
        {
            try
            {
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Email == loginModel.Email && u.Password == loginModel.Password);

                if (user != null)
                {
                    
                    var token = GenerateJwtToken(user);
                    return Ok(token);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                // Логування помилки
                Console.Error.WriteLine($"Помилка при авторизації: {ex.Message}");
                return StatusCode(500, "Виникла помилка під час авторизації.");
            }
        }


        [HttpGet("getUserEmail")]
        public async Task<ActionResult<string>> GetUserEmail()
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

                if (user == null)
                {
                    return NotFound($"Користувача з ID {userId} не знайдено");
                }

                return Ok(user.Email);
            }

            catch (Exception ex)
            {
                // Логування помилки
                Console.Error.WriteLine($"Помилка при отриманні Email користувача за ID: {ex.Message}");
                return StatusCode(500, "Виникла помилка під час отримання Email користувача за ID.");
            }
        }

       
    }

}



