using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ClientBus.Models;
using System.Net.Http;
using Microsoft.Extensions.Http;
using System.Windows;
using System.Net.Http.Headers;


namespace ClientBus.Services
{
    internal class ServiceUser
    {
        private static string _accessToken;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;




        public ServiceUser(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
        }

        public async Task<bool> RegisterUserAsync(User newUser)
        {
            var jsonContent = JsonConvert.SerializeObject(newUser);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/User/register", httpContent);

            if (response.IsSuccessStatusCode)
            {
                // Запит був успішним
                return true;
            }
            else
            {
                // Вивести код статусу
                Console.WriteLine($"HTTP Status Code: {response.StatusCode}");

                // Отримати та вивести вміст відповіді
                var content = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Response Content: {content}");

                // Запит завершився невдало
                return false;
            }
        }

        public async Task<bool> LoginAsync(string email, string password)
        {
            var loginModel = new { Email = email,Password = password };
            var jsonContent = JsonConvert.SerializeObject(loginModel);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Add("Origin", $"{_baseUrl}");

            var response = await _httpClient.PostAsync($"{_baseUrl}/api/User/login", httpContent);

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();

                // Зберегти токен локально (наприклад, у пам'яті)
                SaveAccessToken(token);
                return true;
            }
            else
            {
                // Вивести код статусу
                Console.WriteLine($"HTTP Status Code: {response.StatusCode}");

                // Отримати та вивести вміст відповіді
                var content = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Response Content: {content}");

                // Запит завершився невдало
                return false;
            }


        }

        public async Task<string> GetUserEmailAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(_accessToken))
                {
                    // Користувач не авторизований
                    return null;
                }

                // Додайте токен до запиту
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken);

                // Виклик API для отримання Email користувача
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/User/getUserEmail");

                if (response.IsSuccessStatusCode)
                {
                    string userEmail = await response.Content.ReadAsStringAsync();
                    return userEmail;
                }
                else
                {
                    Console.WriteLine($"HTTP Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Логування помилки
                Console.Error.WriteLine($"Помилка при отриманні Email користувача: {ex.Message}");
                return null;
            }

        }

        public async Task<IEnumerable<TicketsModel>> GetUserTicketsAsync()
        {
            try
            {
                // Отримайте токен (якщо необхідно)
                var accessToken = GetAccessToken();

                // Додайте токен до запиту (якщо використовується авторизація)
                if (!string.IsNullOrEmpty(accessToken))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                }

                // Виклик API для отримання квитків користувача
                HttpResponseMessage response = await _httpClient.GetAsync($"{_baseUrl}/api/Ticket/getUserTickets");

                if (response.IsSuccessStatusCode)
                {
                    // Парсинг вмісту відповіді в ViewModel квитків
                    var content = await response.Content.ReadAsStringAsync();
                    var userTickets = JsonConvert.DeserializeObject<IEnumerable<TicketsModel>>(content);
                    return userTickets;
                }
                else
                {
                    Console.WriteLine($"HTTP Status Code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Логування помилки
                Console.Error.WriteLine($"Error getting user tickets: {ex.Message}");
                return null;
            }
        }

        public string GetAccessToken()
        {
            // Реалізуйте логіку отримання токену (якщо необхідно)
            return _accessToken;
        }
        private void SaveAccessToken(string token)
        {
            // Зберегти токен в пам'яті
            _accessToken = token;
        }

        public async Task<bool> IsServerAvailableAsync()
        {
            try
            {
                _httpClient.Timeout = TimeSpan.FromSeconds(5);
                HttpResponseMessage response = await _httpClient.GetAsync("https://localhost:7283/");

                // Перевірка коду відповіді. 200 означає успіх
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                // Помилка при виконанні запиту (наприклад, відсутній з'єднання з сервером)
                return false;
            }
        }

    }
}
