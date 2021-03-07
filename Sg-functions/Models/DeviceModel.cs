using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sg_functions.Models
{
    public class DeviceModel
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        [Required]
        public int HowMuchWater { get; set; }
        [Required]
        public int HowMuchLight { get; set; }
        [Required]
        public int HowMuchHumidity { get; set; }
        [Required]
        public int IdealTemperature { get; set; }
        public string PlantName { get; set; }
        [Required]
        public string PlantSpecies { get; set; }
    }
}
