using Newtonsoft.Json;

namespace NswLicensePlateLookup.Models
{
    public class TokenResult
    {
        [JsonProperty("statusCode")]
        public long StatusCode { get; set; }

        [JsonProperty("statusMessage")]
        public string StatusMessage { get; set; }

        [JsonProperty("statusObject")]
        public string Token { get; set; }
    }
}