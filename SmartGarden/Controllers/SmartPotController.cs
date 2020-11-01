using System;
using System.Collections.Generic;
using System.Linq;
using Core.DataObjects.EFObjects;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartGarden.Controllers.Base;
using SmartGarden.Models;

namespace SmartGarden.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SmartPotController : BaseController
    {
        public SmartPotController(ControllerInitializer initializer) : base(initializer) { }

        [HttpPost]
        public IActionResult SaveSensorsData([FromBody]string model)
        {
            var smartPot = JsonConvert.DeserializeObject<SmartPotModel>(model);
            var dataStr = $"Server: {smartPot.PiId} {smartPot.Temperature} {smartPot.Humidity} {smartPot.SoilMoisture:0.##}% {smartPot.Light:0.##}% {smartPot.IsRaining}";

            var device = Context.Devices.FirstOrDefault(d => d.SerialNumber == smartPot.PiId);
            if (device == null)
            {
                device = new Device { Id = Guid.NewGuid(), SerialNumber = smartPot.PiId, DateRegistered = DateTime.UtcNow };
                Context.Devices.Add(device);
            }
            Context.Measurements.Add(new Measurement
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
            Context.SaveChanges();
            
            return Ok(dataStr);
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("test");
        }
    }
}
