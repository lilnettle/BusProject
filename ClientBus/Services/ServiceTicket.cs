using ClientBus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace ClientBus.Services
{
    internal class ServiceTicket
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;




        public ServiceTicket(string baseUrl)
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
        }

        public async Task<bool> CreateTicketAsync(int tripId, string userEmail)
        {
            try
            {
                var ticketRequest = new TicketRequestModel
                {
                    TripId = tripId,
                    UserEmail = userEmail
                    // Інші поля, якщо необхідно
                };

                var json = System.Text.Json.JsonSerializer.Serialize(ticketRequest);

                _httpClient.DefaultRequestHeaders.Clear(); // Clear existing headers
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.PostAsync($"{_baseUrl}/api/Ticket/create", new StringContent(json, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error creating ticket: {errorMessage}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteTicketAsync(int ticketId, string userEmail)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"{_baseUrl}/api/Ticket/delete/{ticketId}?userEmail={userEmail}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting ticket: {ex.Message}");
                return false;
            }
        }
    }
 }