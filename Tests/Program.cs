using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    class Program
    {
        static async Task Main()
        {
            try
            {
                // Ваша базова адреса API
                string baseUrl = "http://localhost:7283";

                // ID авторизованого користувача
                int loggedInUserId = 1; // Припустимо, що у вас є цей ID

                using (HttpClient httpClient = new HttpClient())
                {
                    // Додаємо заголовок авторизації (якщо необхідно)
                    // httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer YourAccessToken");

                    // Викликаємо GET-запит
                    HttpResponseMessage response = await httpClient.GetAsync($"{baseUrl}/api/User/getUserEmail");

                    if (response.IsSuccessStatusCode)
                    {
                        // Читаємо та виводимо Email користувача
                        string userEmail = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Email користувача: {userEmail}");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Console.WriteLine("Користувача не знайдено");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        Console.WriteLine("Користувач не авторизований");
                    }
                    else
                    {
                        Console.WriteLine($"Помилка: {response.StatusCode}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Помилка в консольній програмі: {ex.Message}");
            }
            Console.ReadLine();
        }
    }
}




    
    

