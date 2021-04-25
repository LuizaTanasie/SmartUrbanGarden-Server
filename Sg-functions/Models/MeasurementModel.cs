using Core.DataObjects.EFObjects;
using System;
using System.Collections.Generic;

namespace Sg_functions.Models
{
    public class MeasurementModel
    {
        public Guid DeviceId { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }
        public decimal? SoilMoisture { get; set; }
        public decimal? Light { get; set; }
        public DateTime MeasuredAtTime { get; set; }
        public string PlantName { get; set; }
        public string PlantSpecies { get; set; }
        public List<PlantCareWarningModel> Warnings { get; set; }

        public static MeasurementModel FromMeasurement(Measurement measurement)
        {
            return new MeasurementModel
            {
                Humidity = measurement.Humidity != null ? Math.Round(measurement.Humidity.Value) : (decimal?)null,
                Light = measurement.LightPercentage != null ? Math.Round(measurement.LightPercentage.Value) : (decimal?)null,
                MeasuredAtTime = measurement.MeasuredAtTime,
                SoilMoisture = measurement.SoilMoisturePercentage != null ? Math.Round(measurement.SoilMoisturePercentage.Value) : (decimal?)null,
                Temperature = measurement.Temperature != null ? Math.Round(measurement.Temperature.Value) : (decimal?)null,
                DeviceId = measurement.DeviceId,
                PlantName = measurement.Device.PlantName,
                PlantSpecies = measurement.Device.PlantSpecies
            };
        }
    }
}
