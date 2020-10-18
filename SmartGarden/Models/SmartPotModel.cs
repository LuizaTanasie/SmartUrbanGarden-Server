using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartGarden.Models
{
    public class SmartPotModel
    {
        [JsonProperty(PropertyName = "temperature")]
        public decimal Temperature { get; set; }
        [JsonProperty(PropertyName = "humidity")]
        public decimal Humidity { get; set; }
        [JsonProperty(PropertyName = "soil_moisture")]
        public decimal SoilMoisture { get; set; }
        [JsonProperty(PropertyName = "is_raining")]
        public bool IsRaining { get; set; }
    }
}
