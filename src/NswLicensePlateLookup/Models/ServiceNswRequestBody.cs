using System.Collections.Generic;
using Newtonsoft.Json;

namespace NswLicensePlateLookup.Models
{
    public class ServiceNswRequestBody
    {
        public ServiceNswRequestBody()
        {
            Action = "RMSWrapperCtrl";
            Type = "rpc";
            Tid = 1;
            Ctx = new Ctx
            {
                Csrf = "VmpFPSxNakF4T1Mwd09DMHpNVlF4TWpveE5qb3dNaTQxTmpOYSxULWtRbkpmSG5SNm4zS19mMWlPSzVLLE0yTmtaV1V6",
                Vid = "06690000005lUNp",
                Ns = "",
                Ver = 34
            };
        }

        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }

        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; }

        [JsonProperty(PropertyName = "data")]
        public List<string> Data { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "tid")]
        public int Tid { get; set; }

        [JsonProperty(PropertyName = "ctx")]
        public Ctx Ctx { get; set; }
    }

    public class Ctx
    {
        [JsonProperty(PropertyName = "csrf")]
        public string Csrf { get; set; }

        [JsonProperty(PropertyName = "vid")]
        public string Vid { get; set; }

        [JsonProperty(PropertyName = "ns")]
        public string Ns { get; set; }

        [JsonProperty(PropertyName = "ver")]
        public int Ver { get; set; }
    }
}