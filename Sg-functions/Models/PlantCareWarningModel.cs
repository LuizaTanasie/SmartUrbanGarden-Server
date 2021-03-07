using System;
using System.Collections.Generic;
using System.Text;

namespace Sg_functions.Models
{
    public class PlantCareWarningModel
    {
        public string TemperatureWarning { get; set; }
        public string HumidityWarning { get; set; }
        public string SoilMoistureWarning { get; set; }
        public string LightWarning { get; set; }
    }
}
