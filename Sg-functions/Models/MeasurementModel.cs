using Core.DataObjects.EFObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sg_functions.Models
{
    public class MeasurementModel
    {
        public Guid DeviceId { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }
        public decimal? SoilMoisture { get; set; }
        public decimal? Light { get; set; }
        public bool? IsRaining { get; set; }
        public DateTime MeasuredAtTime { get; set; }
        public PlantCareWarningModel Warnings { get; set; }

        public static MeasurementModel FromMeasurement(Measurement measurement)
        {
            return new MeasurementModel
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
