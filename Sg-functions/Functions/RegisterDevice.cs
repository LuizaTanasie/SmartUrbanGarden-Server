using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using DataAccess.DbContexts;
using System.Linq;

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
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            string serialNumber = req.Query["SerialNumber"];

            var device = context.Devices.FirstOrDefault(d => d.SerialNumber == serialNumber);
            if (device == null)
            {
                return new BadRequestObjectResult("No device found.");
            }
            return new OkObjectResult(device.Id);
        }
    }
}
