using Newtonsoft.Json;
using System;

namespace Sg_functions.Models
{
    public class SmartPotModel
    {
        [JsonProperty(PropertyName = "pi_id")]
        public string PiId { get; set; }
        [JsonProperty(PropertyName = "temperature")]
        public decimal? Temperature { get; set; }
        [JsonProperty(PropertyName = "humidity")]
        public decimal? Humidity { get; set; }
        [JsonProperty(PropertyName = "soil_moisture")]
        public decimal? SoilMoisture { get; set; }
        [JsonProperty(PropertyName = "light")]
        public decimal? Light { get; set; }
        [JsonProperty(PropertyName = "measured_at_time")]
        public DateTime MeasuredAtTime { get; set; }
    }
}
