using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace dotnet_vmw_api_gateway.Models
{
    public class LicenseStatus
    {
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }
}
