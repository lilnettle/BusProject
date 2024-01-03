using ClientBus.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace ClientBus.Services
{
    internal class ServiceTrip
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;




        public ServiceTrip(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
        }

        public async Task<IEnumerable<TripModel>> SearchTripsAsync(TripSearchModel criteria)
        {
            try
            {
                var apiUrl = $"{_baseUrl}/api/TripConroller/search";
                var content = new StringContent(JsonConvert.SerializeObject(criteria), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var trips = JsonConvert.DeserializeObject<IEnumerable<TripModel>>(responseData);
                    return trips;
                }
                else
                {
                    // Обробка помилок
                    throw new Exception($"Error during trip search: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Обробка помилок
                throw new Exception($"Error during trip search: {ex.Message}");
            }
        }
    }

}

