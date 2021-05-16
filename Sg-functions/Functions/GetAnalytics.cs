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
    public class GetAnalytics
    {
        private readonly SGContext context;
        private readonly AnalyticsHelper analyticsHelper;
        public GetAnalytics(SGContext context, AnalyticsHelper analyticsHelper)
        {
            this.context = context;
            this.analyticsHelper = analyticsHelper;
        }

        [FunctionName("GetAnalytics")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req)
        {
            var success = Guid.TryParse(req.Query["deviceId"], out var deviceId);
            int settingId = 0;
            success = success && int.TryParse(req.Query["settingId"], out settingId);
            if (!success)
            {
                return new NotFoundObjectResult("Invalid data.");
            };

            var analytics = analyticsHelper.GetAnalytics(deviceId, (AnalyticsSettings)settingId);

            return new OkObjectResult(analytics);
        }
    }
}
