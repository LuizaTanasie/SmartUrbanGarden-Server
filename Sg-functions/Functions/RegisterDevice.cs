using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using DataAccess.DbContexts;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using Sg_functions.Models;
using Microsoft.EntityFrameworkCore;

namespace Sg_functions.Functions
{

    public class RegisterDevice
    {
        private SGContext context;
        public RegisterDevice(SGContext context)
        {
            this.context = context;
        }

        [FunctionName("RegisterDevice")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req)
        {

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var deviceModel = JsonConvert.DeserializeObject<DeviceModel>(requestBody);

            var device = context.Devices.FirstOrDefault(d => d.SerialNumber == deviceModel.SerialNumber);
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
            context.SaveChanges();
            return new OkObjectResult(device.Id);
        }
    }
}
