using Barbarossa.Models;
using System.Text.Json;

namespace Barbarossa.Services
{
    public interface IApiService
    {
        Task<IEnumerable<Master>> GetMastersAsync();
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://id-barbarossa.ru/staf?id=53";

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<IEnumerable<Master>> GetMastersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(ApiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var masters = JsonSerializer.Deserialize<List<MasterDto>>(json);

                return masters.Select(dto => new Master
                {
                    Id = dto.id,
                    Name = dto.fio,
                    JobTitle = dto.job,
                    AvatarUrl = !string.IsNullOrEmpty(dto.avatar)
                        ? $"https://id-barbarossa.ru/img/{dto.avatar}"
                        : null,
                    Services = dto.services.Select(s => new Service
                    {
                        Id = s.id,
                        Name = s.name,
                        Duration = s.time,
                        Price = decimal.Parse(s.price)
                    }).ToList(),
                    AvailableSlots = dto.seans?.ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Select(t => t.time).Distinct().ToList()
                    ) ?? []
                });
            }
            catch (Exception ex)
            {
                // В случае ошибки можно вернуть пустой список или пробросить исключение
                return new List<Master>();
            }
        }

        private class MasterDto
        {
            public int id { get; set; }
            public string fio { get; set; }
            public string avatar { get; set; }
            public string job { get; set; }
            public List<ServiceDto> services { get; set; }
            public Dictionary<string, List<TimeSlotDto>> seans { get; set; }
        }

        private class ServiceDto
        {
            public int id { get; set; }
            public string name { get; set; }
            public string time { get; set; }
            public string price { get; set; }
        }

        private class TimeSlotDto
        {
            public string time { get; set; }
            public string date { get; set; }
            public int id { get; set; }
        }
    }
}