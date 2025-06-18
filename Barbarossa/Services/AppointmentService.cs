using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using Barbarossa.Models;

namespace Barbarossa.Services
{
    public class AppointmentService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://id-barbarossa.ru/";
        private const int FixedFilialId = 53; // Фиксированный ID филиала

        public AppointmentService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl),
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public async Task<bool> CreateAppointment(
            int employeeId,
            string servicesIds,
            string date,
            int timeId,
            User currentUser,
            string comment = "")
        {
            try
            {
                var requestData = new Dictionary<string, object>
                {
                    ["fil_id"] = FixedFilialId, // Используем константу
                    ["status"] = 1,
                    ["staf_id"] = employeeId,
                    ["services"] = servicesIds,
                    ["seans"] = $"{date},{timeId}",
                    ["visite_date"] = timeId,
                    ["fio"] = currentUser.Name,
                    ["phone"] = currentUser.Phone,
                    ["email"] = currentUser.Email,
                    ["comment"] = comment
                };

                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("save", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Appointment error: {ex.Message}");
                return false;
            }
        }
    }
}