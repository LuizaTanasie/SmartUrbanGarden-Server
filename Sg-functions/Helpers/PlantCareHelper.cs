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
        public PlantCareWarningModel GetWarnings(Guid deviceId)
        {
            var device = context.Devices.Include(d => d.Measurements).FirstOrDefault(d => d.Id == deviceId);
            var latestMeasurement = device.Measurements
                .OrderByDescending(m => m.MeasuredAtTime)
                .FirstOrDefault();

            return new PlantCareWarningModel
            {
                TemperatureWarning = GetTemperatureWarning(latestMeasurement.Temperature, device.IdealTemperatureId),
                HumidityWarning = GetHumidityWarning(latestMeasurement.Humidity, device.HowMuchHumidityId),
                SoilMoistureWarning = GetSoilMoistureWarning(latestMeasurement.SoilMoisturePercentage, device.HowMuchWaterId),
                LightWarning = GetLightWarning(device, device.HowMuchLightId),
            };
        }

        private string GetTemperatureWarning(decimal? temperature, int? idealTemperatureId)
        {
            var tooColdMessage = "I'm too cold.";
            var tooHotMessage = "I'm too warm.";
            if (temperature < 10)
            {
                return tooColdMessage;
            }
            switch (idealTemperatureId)
            {
                case (int)MeasurementIdealAmounts.Low: {
                        if (temperature > 20)
                        {
                            return tooHotMessage;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.Moderate:
                    {
                        if (temperature < 18)
                        {
                            return tooColdMessage;
                        }
                        if (temperature > 30)
                        {
                            return tooHotMessage;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.High:
                    {
                        if (temperature < 28)
                        {
                            return tooColdMessage;
                        }
                        break;
                    }
            }
            return null;
        }

        private string GetHumidityWarning(decimal? humidityPercentage, int? howMuchHumidity)
        {
            var humidityTooHigh = "The humidity is too high.";
            var humidityTooLow = "The humidity is too low.";
            if (humidityPercentage < 10)
            {
                return humidityTooLow;
            }
            switch (howMuchHumidity)
            {
                case (int)MeasurementIdealAmounts.Low:
                    {
                        if (humidityPercentage > 40)
                        {
                            return humidityTooHigh;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.Moderate:
                    {
                        if (humidityPercentage < 40)
                        {
                            return humidityTooLow;
                        }
                        if (humidityPercentage > 70)
                        {
                            return humidityTooHigh;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.High:
                    {
                        if (humidityPercentage < 65)
                        {
                            return humidityTooLow;
                        }
                        break;
                    }
            }
            return null;
        }

        private string GetSoilMoistureWarning(decimal? soilMoisture, int? howMuchWater)
        {
            var soilMoistureTooLow = "I need water.";
            var soilMoistureTooHigh = "I received too much water.";
            switch (howMuchWater)
            {
                case (int)MeasurementIdealAmounts.Low:
                    {
                        if (soilMoisture > 70)
                        {
                            return soilMoistureTooHigh;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.Moderate:
                    {
                        if (soilMoisture < 30)
                        {
                            return soilMoistureTooLow;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.High:
                    {
                        if (soilMoisture < 50)
                        {
                            return soilMoistureTooLow;
                        }
                        break;
                    }
            }
            return null;
        }

        private string GetLightWarning(Device device, int? howMuchLight)
        {
            var lightAverage = device.Measurements
                .Where(m => m.MeasuredAtTime < DateTime.Now.AddDays(-3)
                            && m.MeasuredAtTime.TimeOfDay > new TimeSpan(6, 00, 00)
                            && m.MeasuredAtTime.TimeOfDay < new TimeSpan(20, 00, 00)).Select(m => m.LightPercentage).Average();
            var lightTooLow = "The amount of light I received in the past 3 days wasn't enough. I need more light.";
            var lightTooStrong = "The amount of light I received in the past 3 days was too strong. Make sure I can handle direct sunlight.";
            switch (howMuchLight)
            {
                case (int)MeasurementIdealAmounts.Low:
                    {
                        if (lightAverage > 70)
                        {
                            return lightTooStrong;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.Moderate:
                    {
                        if (lightAverage < 50)
                        {
                            return lightTooLow;
                        }
                        break;
                    }
                case (int)MeasurementIdealAmounts.High:
                    {
                        if (lightAverage < 80)
                        {
                            return lightTooLow;
                        }
                        break;
                    }
            }
            return null;
        }
    }
}
