using Core.DataObjects.EFObjects;
using DataAccess.DbContexts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Services;

namespace Sg_functions.Functions
{

    public class Unsubscribe
    {
        private readonly EmailHelper emailHelper;
        private SGContext context;

        public Unsubscribe(EmailHelper emailHelper, SGContext context)
        {
            this.emailHelper = emailHelper;
            this.context = context;
        }

        [FunctionName("Unsubscribe")]
        public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req
        )
        {
            var email = req.Query["email"];
            var key = req.Query["key"];
            Guid.TryParse(req.Query["deviceId"], out var deviceId);
            if (key == emailHelper.GetUnsubscribeHash(email))
            {
                context.DeviceUsers.Remove(new DeviceUser
                {
                    Email = email,
                    DeviceId = deviceId
                });
                context.SaveChanges();
                return new OkObjectResult("Unsubscribe sucessful");
            }
            else
            {
                return new BadRequestObjectResult("Error. Unsubscribe information is wrong.");
            }
        }
    }
}
