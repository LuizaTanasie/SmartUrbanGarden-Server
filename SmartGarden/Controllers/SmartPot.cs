using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmartGarden.Models;

namespace SmartGarden.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SmartPot : ControllerBase
    {

        [HttpPost]
        public IActionResult SaveSensorsData([FromBody]string model)
        {
            var data = JsonConvert.DeserializeObject<SmartPotModel>(model);
            var dataStr = $"Server: {data.Id} {data.Temperature} {data.Humidity} {data.SoilMoisture:0.##}% {data.IsRaining}";
            return Ok(dataStr);
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok("test");
        }
    }
}
