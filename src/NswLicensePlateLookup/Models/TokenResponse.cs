using System;
using Newtonsoft.Json;

namespace NswLicensePlateLookup.Models
{
    public class TokenResponse
    {
        [JsonProperty("statusCode")]
        public long StatusCode { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("tid")]
        public long Tid { get; set; }

        [JsonProperty("ref")]
        public bool Ref { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("result")]
        public TokenResult Result { get; set; }
    }

    public class TokenResult
    {
        [JsonProperty("statusCode")]
        public long StatusCode { get; set; }

        [JsonProperty("statusMessage")]
        public string StatusMessage { get; set; }

        [JsonProperty("statusObject")]
        public Guid StatusObject { get; set; }
    }
}
