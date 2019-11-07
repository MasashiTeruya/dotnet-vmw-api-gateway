using dotnet_vmw_api_gateway.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace dotnet_vmw_api_gateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AggregationStatusController
    {
        private static readonly HttpClient _client;

        static AggregationStatusController()
        {
            _client = new HttpClient();
            _client.Timeout = TimeSpan.FromSeconds(2);
        }

        private readonly ILogger<AggregationStatusController> _logger;
        private readonly IConfiguration _configuration;

        public AggregationStatusController(ILogger<AggregationStatusController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<AggregationStatus> Get()
        {
            string rankingApiUrl = _configuration.GetValue<string>("API_GATEWAY_RANKING_API_URL");
            var getUserEntries = _client.GetStringAsync(rankingApiUrl);
            string licensingStatusUrl = _configuration.GetValue<string>("API_GATEWAY_LICENSING_STATUS_URL");
            var getLicenseStatus = _client.GetStringAsync(licensingStatusUrl);
            UserEntry[] userEntries = new UserEntry[] { };
            try
            {
                userEntries = JsonSerializer.Deserialize<UserEntry[]>(await getUserEntries);
            }
            catch (TaskCanceledException)
            {
            }
            LicenseStatus licenseStatus = null;
            try
            {
                licenseStatus = JsonSerializer.Deserialize<LicenseStatus>(await getLicenseStatus);
            }
            catch (TaskCanceledException)
            {
            }
            var aggregationStatus = new AggregationStatus()
            {
                UserEntries = userEntries,
                LicenseStatus = licenseStatus,
            };
            return aggregationStatus;
        }

    }
}
