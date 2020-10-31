using DataAccess.DbContexts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartGarden.Controllers.Base
{
    public class BaseController : ControllerBase
    {
        public BaseController(ControllerInitializer controllerInitializer)
        {
            Context = controllerInitializer.Context;
        }

        public SGContext Context { get; set; }

        protected BadRequestObjectResult Error(params string[] errors) {
            return BadRequest(new { Errors = errors });
        }

    }
}
