using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace dotnet_vmw_api_gateway.Models
{
    public class AggregationStatus
    {
        [JsonPropertyName("userEntries")]
        public UserEntry[] UserEntries { get; set; }

        [JsonPropertyName("licenseStatus")]
        public LicenseStatus LicenseStatus { get; set; }
    }
}
