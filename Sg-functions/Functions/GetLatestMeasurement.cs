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
    public class GetLatestMeasurement
    {
        private readonly SGContext context;
        private readonly PlantCareHelper plantCareHelper;
        public GetLatestMeasurement(SGContext context)
        {
            this.context = context;
            this.plantCareHelper = new PlantCareHelper(context);
        }

        [FunctionName("GetLatestMeasurement")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            var success = Guid.TryParse(req.Query["DeviceId"], out Guid deviceId);
            if (!success)
            {
                return new BadRequestObjectResult("Invalid device ID.");
            }
            var measurement = context.Measurements
                .Include(m => m.Device)
                .OrderByDescending(m => m.MeasuredAtTime)
                .FirstOrDefault(m => m.DeviceId == deviceId);
            if (measurement != null)
            {
                var measurementModel = MeasurementModel.FromMeasurement(measurement);
                measurementModel.Warnings = plantCareHelper.GetWarnings(deviceId);
                return new OkObjectResult(measurementModel);
            }
            else
            {
                return new BadRequestObjectResult("No measurements yet.");
            }
        }
    }
}
