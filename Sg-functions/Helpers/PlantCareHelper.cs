using Core.DataObjects.EFEnums;
using Core.DataObjects.EFObjects;
using DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;
using Sg_functions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sg_functions.Helpers
{
    public class PlantCareHelper
    {
        private readonly SGContext context;
        public PlantCareHelper(SGContext context)
        {
            this.context = context;
        }
        public List<PlantCareWarningModel> GetWarnings(Guid deviceId)
        {
            var device = context.Devices.Include(d => d.Measurements).FirstOrDefault(d => d.Id == deviceId);
            var latestMeasurement = device.Measurements
                .OrderByDescending(m => m.MeasuredAtTime)
                .FirstOrDefault();
            var warnings = new List<PlantCareWarningModel>();

            GetTemperatureWarning(latestMeasurement.Temperature, device.IdealTemperatureId, warnings);
            GetHumidityWarning(latestMeasurement.Humidity, device.HowMuchHumidityId, warnings);
            GetSoilMoistureWarning(latestMeasurement.SoilMoisturePercentage, device.HowMuchWaterId, warnings);
            GetLightWarning(device, device.HowMuchLightId, warnings);

            return warnings;
            
        }

        private void GetTemperatureWarning(decimal? temperature, int? idealTemperatureId, List<PlantCareWarningModel> warnings)
        {
            var tooColdMessage = "I'm too cold.";
            var tooHotMessage = "I'm too warm.";
            var warning = string.Empty;
            if (temperature < 10)
            {
                warning = tooColdMessage;
            }
            switch (idealTemperatureId)
            {
                case (int)MeasurementIdealAmounts.Low:
                    {
                        if (temperature > 20)
                        {
                            warning = tooHotMessage;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.Moderate:
                    {
                        if (temperature < 18)
                        {
                            warning = tooColdMessage;
                        }
                        if (temperature > 30)
                        {
                            warning = tooHotMessage;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.High:
                    {
                        if (temperature < 28)
                        {
                            warning = tooColdMessage;
                        }
                        break;
                    }
            }
            if (!string.IsNullOrEmpty(warning))
            {
                warnings.Add(new PlantCareWarningModel { Message = warning , WarningTypeId = (int)WarningTypes.TemperatureWarning});
            }
        }

        private void GetHumidityWarning(decimal? humidityPercentage, int? howMuchHumidity, List<PlantCareWarningModel> warnings)
        {
            var humidityTooHigh = "The humidity is too high.";
            var humidityTooLow = "The humidity is too low.";
            var warning = string.Empty;
            if (humidityPercentage < 10)
            {
                warning = humidityTooLow;
            }
            switch (howMuchHumidity)
            {
                case (int)MeasurementIdealAmounts.Low:
                    {
                        if (humidityPercentage > 40)
                        {
                            warning = humidityTooHigh;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.Moderate:
                    {
                        if (humidityPercentage < 40)
                        {
                            warning = humidityTooLow;
                        }
                        if (humidityPercentage > 70)
                        {
                            warning = humidityTooHigh;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.High:
                    {
                        if (humidityPercentage < 65)
                        {
                            warning = humidityTooLow;
                        }
                        break;
                    }
            }
            if (!string.IsNullOrEmpty(warning))
            {
                warnings.Add(new PlantCareWarningModel { Message = warning, WarningTypeId = (int)WarningTypes.HumidityWarning });
            }
        }

        private void GetSoilMoistureWarning(decimal? soilMoisture, int? howMuchWater, List<PlantCareWarningModel> warnings)
        {
            var soilMoistureTooLow = "I need water.";
            var soilMoistureTooHigh = "I received too much water.";
            var warning = string.Empty;
            switch (howMuchWater)
            {
                case (int)MeasurementIdealAmounts.Low:
                    {
                        if (soilMoisture > 70)
                        {
                            warning = soilMoistureTooHigh;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.Moderate:
                    {
                        if (soilMoisture < 60)
                        {
                            warning = soilMoistureTooLow;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.High:
                    {
                        if (soilMoisture < 70)
                        {
                            warning = soilMoistureTooLow;
                        }
                        break;
                    }
            }
            if (!string.IsNullOrEmpty(warning))
            {
                warnings.Add(new PlantCareWarningModel { Message = warning, WarningTypeId = (int)WarningTypes.SoilWarning });
            }
        }

        private void GetLightWarning(Device device, int? howMuchLight, List<PlantCareWarningModel> warnings)
        {
            var lightAverage = device.Measurements
                .Where(m => m.MeasuredAtTime < DateTime.Now.AddDays(-1)
                            && m.MeasuredAtTime.TimeOfDay > new TimeSpan(6, 00, 00)
                            && m.MeasuredAtTime.TimeOfDay < new TimeSpan(20, 00, 00)).Select(m => m.LightPercentage).Average();
            var lightTooLow = "The amount of light I received in the past day wasn't enough. I need more light.";
            var lightTooStrong = "The amount of light I received in the past day was too strong. Make sure I can handle direct sunlight.";
            var warning = string.Empty;
            switch (howMuchLight)
            {
                case (int)MeasurementIdealAmounts.Low:
                    {
                        if (lightAverage > 70)
                        {
                            warning = lightTooStrong;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.Moderate:
                    {
                        if (lightAverage < 50)
                        {
                            warning = lightTooLow;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.High:
                    {
                        if (lightAverage < 80)
                        {
                            warning = lightTooLow;
                        }
                        break;
                    }
            }
            if (!string.IsNullOrEmpty(warning))
            {
                warnings.Add(new PlantCareWarningModel { Message = warning, WarningTypeId = (int)WarningTypes.LightWarning });
            }
        }
    }
}
