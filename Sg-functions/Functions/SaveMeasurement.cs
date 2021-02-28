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

namespace Sg_functions.Functions
{
    public class SaveMeasurement
    {
        private SGContext context;
        public SaveMeasurement(SGContext context)
        {
            this.context = context;
        }

        [FunctionName("SaveMeasurement")]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req
        )
        {
            string requestBody = await new StreamReader(req.Body, Encoding.UTF8).ReadToEndAsync();

            var smartPot = JsonConvert.DeserializeObject<SmartPotModel>(requestBody);
            var dataStr = $"Registered: {smartPot.PiId} {smartPot.Temperature} {smartPot.Humidity} {smartPot.SoilMoisture:0.##}% {smartPot.Light:0.##}% {smartPot.IsRaining}";

            var device = context.Devices.FirstOrDefault(d => d.SerialNumber == smartPot.PiId);
            if (device == null)
            {
                device = new Device { Id = Guid.NewGuid(), SerialNumber = smartPot.PiId, DateRegistered = DateTime.UtcNow };
                context.Devices.Add(device);
            }
            context.Measurements.Add(new Measurement
            {
                Id = Guid.NewGuid(),
                DeviceId = device.Id,
                Humidity = smartPot.Humidity,
                Temperature = smartPot.Temperature,
                IsRaining = smartPot.IsRaining,
                SoilMoisturePercentage = smartPot.SoilMoisture,
                ReceivedAtTime = DateTime.UtcNow,
                MeasuredAtTime = smartPot.MeasuredAtTime,
                LightPercentage = smartPot.Light
            });
            context.SaveChanges();

            return new OkObjectResult(dataStr);
        }
    }
}
