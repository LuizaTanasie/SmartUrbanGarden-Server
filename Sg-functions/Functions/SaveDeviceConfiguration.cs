using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using DataAccess.DbContexts;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Sg_functions.Models;
using Microsoft.EntityFrameworkCore;
using Core.DataObjects.EFObjects;

namespace Sg_functions.Functions
{

    public class SaveDeviceConfiguration
    {
        private SGContext context;
        public SaveDeviceConfiguration(SGContext context)
        {
            this.context = context;
        }

        [FunctionName("SaveDeviceConfiguration")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var deviceModel = JsonConvert.DeserializeObject<DeviceModel>(requestBody);

            var device = context.Devices.Include(d => d.DeviceUsers).FirstOrDefault(d => d.Id == deviceModel.Id);
            if (device == null)
            {
                return new BadRequestObjectResult("No device found.");
            }
            device.PlantName = deviceModel.PlantName;
            device.PlantSpecies = deviceModel.PlantSpecies;
            device.HowMuchHumidityId = deviceModel.HowMuchHumidity;
            device.HowMuchLightId = deviceModel.HowMuchLight;
            device.HowMuchWaterId = deviceModel.HowMuchWater;
            device.IdealTemperatureId = deviceModel.IdealTemperature;
            if (!string.IsNullOrEmpty(deviceModel.Email))
            {
                if (!device.DeviceUsers.Any(du => du.Email == deviceModel.Email))
                {
                    device.DeviceUsers.Add(new DeviceUser { Email = deviceModel.Email });
                }
            }
            context.SaveChanges();
            return new OkObjectResult(device.Id);
        }
    }
}
