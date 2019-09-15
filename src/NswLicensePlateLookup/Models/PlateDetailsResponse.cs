using Newtonsoft.Json;

namespace NswLicensePlateLookup.Models
{    
    public class PlateDetailsResult
    {
        [JsonProperty("statusCode")]
        public long StatusCode { get; set; }

        [JsonProperty("statusMessage")]
        public string StatusMessage { get; set; }

        [JsonProperty("statusObject")]
        public PlateDetails PlateDetails { get; set; }
    }

    public class PlateDetails
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
