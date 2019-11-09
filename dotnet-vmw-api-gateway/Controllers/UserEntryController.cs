using dotnet_vmw_api_gateway.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace dotnet_vmw_api_gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserEntryController : ControllerBase
    {
        private static readonly HttpClient _client;

        static UserEntryController()
        {
            _client = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(2)
            };
        }

        private readonly ILogger<UserEntryController> _logger;
        private readonly IConfiguration _configuration;

        public UserEntryController(ILogger<UserEntryController> logger, IConfiguration configuration)
        {
            this._logger = logger;
            this._configuration = configuration;
        }

        [HttpGet]
        public async Task<UserEntry[]> Get()
        {
            string url = getUrl();
            var getUserEntries = _client.GetStringAsync(url);
            return JsonSerializer.Deserialize<UserEntry[]>(await getUserEntries);
        }

        [HttpPost]
        private async Task postUserEntry(UserEntry userEntry)
        {
            string url = getUrl();
            string json = JsonSerializer.Serialize<UserEntry>(userEntry);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var postUserEntry = await _client.PostAsync(url, content);
            postUserEntry.EnsureSuccessStatusCode();
        }

        private string getUrl()
        {
            string key = "API_GATEWAY_RANKING_API_URL";
            string url = _configuration.GetValue<string>(key);
            if (string.IsNullOrEmpty(url))
            {
                _logger.LogError($"Environment Variable {key} is emptry.");
                return string.Empty;
            }
            return url;
        }
    }
}