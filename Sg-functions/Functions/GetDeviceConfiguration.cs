using DataAccess.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Sg_functions.Helpers;
using Sg_functions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sg_functions.Functions
{
    public class GetDeviceConfiguration
    {
        private readonly SGContext context;
        public GetDeviceConfiguration(SGContext context)
        {
            this.context = context;
        }

        [FunctionName("GetDeviceConfiguration")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            Guid.TryParse(req.Query["deviceId"], out var deviceId);

            var device = context.Devices
                .Include(d => d.DeviceUsers)
                .FirstOrDefault(d => d.Id == deviceId);
            if (device == null)
            {
                return new NotFoundObjectResult("Device not found");
            }
            else if (device != null)
            {
                return new OkObjectResult(new DeviceModel
                {
                    Id = deviceId,
                    PlantName = device.PlantName,
                    HowMuchHumidity = device.HowMuchHumidityId,
                    HowMuchLight = device.HowMuchLightId,
                    HowMuchWater = device.HowMuchWaterId,
                    IdealTemperature = device.IdealTemperatureId,
                    PlantSpecies = device.PlantSpecies,
                    ConfiguredEmails = device.DeviceUsers.Select(du => StringHelper.MaskEmailAddress(du.Email)).ToList()
                });
            }
            else
            {
                return new BadRequestObjectResult("Something went wrong");
            }
        }
    }
}
