using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Sg_functions.Models;
using Core.DataObjects.EFObjects;
using DataAccess.DbContexts;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using Sg_functions.Helpers;
using WebApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Core.DataObjects.EFEnums;

namespace Sg_functions.Functions
{
    public class SaveMeasurement
    {
        private SGContext context;
        private readonly PlantCareHelper plantCareHelper;
        private readonly EmailHelper emailHelper;

        public SaveMeasurement(SGContext context, EmailHelper emailHelper)
        {
            this.context = context;
            this.plantCareHelper = new PlantCareHelper(context);
            this.emailHelper = emailHelper;
        }

        [FunctionName("SaveMeasurement")]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req
        )
        {
            string requestBody = await new StreamReader(req.Body, Encoding.UTF8).ReadToEndAsync();

            var smartPot = JsonConvert.DeserializeObject<SmartPotModel>(requestBody);
            var dataStr = $"Registered: {smartPot.PiId} {smartPot.Temperature} {smartPot.Humidity} {smartPot.SoilMoisture:0.##}% {smartPot.Light:0.##}%";

            var device = context.Devices
                .Include(d => d.DeviceUsers)
                .FirstOrDefault(d => d.SerialNumber == smartPot.PiId);
            if (device == null)
            {
                device = new Device { Id = Guid.NewGuid(), SerialNumber = smartPot.PiId, DateRegistered = DateTime.UtcNow };
                context.Devices.Add(device);
            }
            var newMeasurement = new Measurement
            {
                Id = Guid.NewGuid(),
                DeviceId = device.Id,
                Humidity = smartPot.Humidity,
                Temperature = smartPot.Temperature,
                SoilMoisturePercentage = smartPot.SoilMoisture,
                ReceivedAtTime = DateTime.Now,
                MeasuredAtTime = smartPot.MeasuredAtTime,
                LightPercentage = smartPot.Light
            };
            context.Measurements.Add(newMeasurement);
            context.SaveChanges();

            var warnings = plantCareHelper.GetWarnings(device.Id);
            if (warnings.Any())
            {
                SendEmail(newMeasurement, device, warnings);
            }

            return new OkObjectResult(dataStr);
        }

        private List<PlantCareWarningModel> FilterOutRecentWarnings(List<PlantCareWarningModel> warnings, DeviceUser deviceUser)
        {
            const int hours = 24;
            var warningsDictionary = warnings.ToDictionary(w => w.WarningTypeId);
            var hasRecentlySentTemperatureEmail = deviceUser.TemperatureWarningEmailSentAt.HasValue ? (DateTime.Now - deviceUser.TemperatureWarningEmailSentAt.Value).TotalHours < hours : false;
            var hasRecentlySentHumidityEmail = deviceUser.HumidityWarningEmailSentAt.HasValue ? (DateTime.Now - deviceUser.HumidityWarningEmailSentAt.Value).TotalHours < hours : false;
            var hasRecentlySentSoilEmail = deviceUser.SoilWarningEmailSentAt.HasValue ? (DateTime.Now - deviceUser.SoilWarningEmailSentAt.Value).TotalHours < hours : false;
            var hasRecentlySentLightEmail = deviceUser.LightWarningEmailSentAt.HasValue ? (DateTime.Now - deviceUser.LightWarningEmailSentAt.Value).TotalHours < hours : false;

            if (hasRecentlySentHumidityEmail)
                warningsDictionary.Remove((int)WarningTypes.HumidityWarning);
            if (hasRecentlySentLightEmail)
                warningsDictionary.Remove((int)WarningTypes.LightWarning);
            if (hasRecentlySentSoilEmail)
                warningsDictionary.Remove((int)WarningTypes.SoilWarning);
            if (hasRecentlySentTemperatureEmail)
                warningsDictionary.Remove((int)WarningTypes.TemperatureWarning);
           
            return warningsDictionary.Select(pair => pair.Value).ToList();
        }

        private void SendEmail(Measurement lastMeasurement, Device device, List<PlantCareWarningModel> allWwarnings)
        {
            var url = System.Environment.GetEnvironmentVariable("Url", EnvironmentVariableTarget.Process);

            foreach (var du in device.DeviceUsers)
            {
                var warnings = FilterOutRecentWarnings(allWwarnings, du);
                if (!warnings.Any())
                {
                    return;
                }
                SetEmailTimes(warnings, du);
                StringBuilder sb = new StringBuilder();
                sb.Append("Hi,<br><br>");
                sb.Append($"<div>Your plant needs attention!</div>");
                foreach (var warning in warnings)
                {
                    sb.Append($"<h3>\"{warning.Message}\"</h3>");
                }
                sb.Append($"<div style=\"text-align:right\">- {device.PlantName} ({device.PlantSpecies})</div>");
                sb.Append($"<br><div>Latest measurement from {lastMeasurement.MeasuredAtTime.ToString("dd/MM/yyyy HH:mm")}:</div>");
                sb.Append($"<div>Temperature: {Math.Round(lastMeasurement.Temperature.Value)}°C</div>");
                sb.Append($"<div>Soil moisture: {Math.Round(lastMeasurement.SoilMoisturePercentage.Value)}%</div>");
                sb.Append($"<div>Humidity: {Math.Round(lastMeasurement.Humidity.Value)}%</div>");
                sb.Append($"<div>Light intensity: {Math.Round(lastMeasurement.LightPercentage.Value)}%</div>");
                sb.Append($"<br><br><div>HappyPlant</div><br>");
                sb.Append($"<div>Don't want to receive emails anymore? <a href=\"{url}/api/Unsubscribe?deviceId={device.Id}&email={du.Email}&key={emailHelper.GetUnsubscribeHash(du.Email)}\">Unsubscribe</a></div>");

                emailHelper.Send(du.Email, $"Your plant {device.PlantName} needs care!", sb.ToString());
            }
            context.SaveChanges();
        }

        private void SetEmailTimes(List<PlantCareWarningModel> warnings, DeviceUser deviceUser)
        {
            foreach (var warning in warnings)
            {
                switch (warning.WarningTypeId)
                {
                    case (int)WarningTypes.HumidityWarning:
                        {
                            deviceUser.HumidityWarningEmailSentAt = DateTime.Now;
                            break;
                        }
                    case (int)WarningTypes.TemperatureWarning:
                        {
                            deviceUser.TemperatureWarningEmailSentAt = DateTime.Now;
                            break;
                        }
                    case (int)WarningTypes.SoilWarning:
                        {
                            deviceUser.SoilWarningEmailSentAt = DateTime.Now;
                            break;
                        }
                    case (int)WarningTypes.LightWarning:
                        {
                            deviceUser.LightWarningEmailSentAt = DateTime.Now;
                            break;
                        }
                }
            }
        }
    }
}
