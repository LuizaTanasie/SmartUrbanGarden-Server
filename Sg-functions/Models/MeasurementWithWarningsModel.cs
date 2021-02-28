using Core.DataObjects.EFObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sg_functions.Models
{
    public class MeasurementWithWarningsModel
    {
        public string DeviceId { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }
        public decimal? SoilMoisture { get; set; }
        public decimal? Light { get; set; }
        public bool? IsRaining { get; set; }
        public DateTime MeasuredAtTime { get; set; }
        public string TemperatureWarning { get; set; }
        public string HumidityWarning { get; set; }
        public string SoilMoistureWarning { get; set; }
        public string LightWarning { get; set; }


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
