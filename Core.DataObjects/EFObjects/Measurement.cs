using System;
using System.Collections.Generic;

namespace Core.DataObjects.EFObjects
{
    public partial class Measurement
    {
        public Guid Id { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }
        public decimal? SoilMoisturePercentage { get; set; }
        public bool? IsRaining { get; set; }
        public Guid DeviceId { get; set; }
        public DateTime MeasuredAtTime { get; set; }
        public DateTime ReceivedAtTime { get; set; }
        public decimal? LightPercentage { get; set; }

        public virtual Device Device { get; set; }
    }
}
