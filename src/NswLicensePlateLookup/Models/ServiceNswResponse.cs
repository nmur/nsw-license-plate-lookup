using Newtonsoft.Json;

namespace NswLicensePlateLookup.Models
{
    public class ServiceNswResponse<ResultType>
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
        public ResultType Result { get; set; }
    }
}
