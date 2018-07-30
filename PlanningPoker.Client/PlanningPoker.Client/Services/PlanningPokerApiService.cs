using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PlanningPoker.Client.Exceptions;
using PlanningPoker.Client.Model;

namespace PlanningPoker.Client.Services
{
    internal sealed class PlanningPokerApiService : IPlanningPokerService
    {
        HttpClient _httpClient;
        ConnectionSettings _connectionSettings;
        public PlanningPokerApiService(HttpClient httpClient, IOptions<ConnectionSettings> connectionSettings)
        {
            _httpClient = httpClient;
            _connectionSettings = connectionSettings.Value;
            _httpClient.DefaultRequestHeaders.Add("user-key", _connectionSettings.ApiKey);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<PokerSession> GetSessionDetails(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentNullException(nameof(sessionId));
            }
            var response = await _httpClient.GetAsync($"{_connectionSettings.PlanningApiUri}Sessions/" + sessionId);
            if (!response.IsSuccessStatusCode)
            {
                throw new NotFoundException($"Session with the id {sessionId} was not found");
            }
            return JsonConvert.DeserializeObject<PokerSession>(await response.Content.ReadAsStringAsync());
        }
    }
}