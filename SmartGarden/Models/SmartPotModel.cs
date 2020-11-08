using Core.DataObjects.EFObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartGarden.Models
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
        [JsonProperty(PropertyName = "is_raining")]
        public bool? IsRaining { get; set; }
        [JsonProperty(PropertyName = "measured_at_time")]
        public DateTime MeasuredAtTime { get; set; }

        public static SmartPotModel FromMeasurement(Measurement measurement)
        {
            return new SmartPotModel
            {
                Humidity = measurement.Humidity,
                IsRaining = measurement.IsRaining,
                Light = measurement.LightPercentage,
                MeasuredAtTime = measurement.MeasuredAtTime,
                SoilMoisture = measurement.SoilMoisturePercentage,
                Temperature = measurement.Temperature
            };
        }
    }
}
