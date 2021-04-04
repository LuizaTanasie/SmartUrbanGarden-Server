using DataAccess.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Sg_functions.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace Sg_functions.Functions
{
    public class GetDeviceIdBySerialNumber
    {
        private readonly SGContext context;
        public GetDeviceIdBySerialNumber(SGContext context)
        {
            this.context = context;
        }

        [FunctionName("GetDeviceIdBySerialNumber")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            string serialNumber = req.Query["serialNumber"];

            var device = context.Devices
                .FirstOrDefault(m => m.SerialNumber == serialNumber);
            if (device == null || string.IsNullOrEmpty(serialNumber))
            {
                return new NotFoundObjectResult("Device not found");
            }
            else if (device != null)
            {
                return new OkObjectResult(device.Id);
            }
            else
            {
                return new BadRequestObjectResult("Something went wrong");
            }
        }
    }
}
