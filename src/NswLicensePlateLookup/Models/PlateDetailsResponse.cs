using Newtonsoft.Json;

namespace NswLicensePlateLookup.Models
{
    public class PlateDetailsResponse
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
        public Result Result { get; set; }
    }

    public class Result
    {
        [JsonProperty("statusCode")]
        public long StatusCode { get; set; }

        [JsonProperty("statusMessage")]
        public string StatusMessage { get; set; }

        [JsonProperty("statusObject")]
        public StatusObject StatusObject { get; set; }
    }

    public class StatusObject
    {
        [JsonProperty("vehicle")]
        public Vehicle Vehicle { get; set; }
    }

    public class Vehicle
    {
        [JsonProperty("bodyShape")]
        public string BodyShape { get; set; }

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }

        [JsonProperty("manufactureYear")]
        public long ManufactureYear { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("NSWPlateNumber")]
        public string NswPlateNumber { get; set; }

        [JsonProperty("plateType")]
        public string PlateType { get; set; }

        [JsonProperty("tareWeight")]
        public long TareWeight { get; set; }

        [JsonProperty("variant")]
        public string Variant { get; set; }

        [JsonProperty("vehicleColour")]
        public string VehicleColour { get; set; }

        [JsonProperty("vehicleType")]
        public string VehicleType { get; set; }

        [JsonProperty("vinNumber")]
        public string VinNumber { get; set; }
    }
}
