using DataAccess.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
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
        private SGContext context;
        public GetLatestMeasurement(SGContext context)
        {
            this.context = context;
        }

        [FunctionName("GetLatestMeasurement")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            Guid deviceId = Guid.Parse(req.Query["DeviceId"]);

            var measurement = context.Measurements.OrderByDescending(m => m.MeasuredAtTime).FirstOrDefault(m => m.DeviceId == deviceId);
            if (measurement != null)
            {
                return new OkObjectResult(MeasurementWithWarningsModel.FromMeasurement(measurement));
            }
            else
            {
                return new BadRequestObjectResult("No measurements.");
            }
        }
    }
}
