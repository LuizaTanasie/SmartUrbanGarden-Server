using System;
using System.Collections.Generic;

namespace Core.DataObjects.EFObjects
{
    public partial class Device
    {
        public Device()
        {
            Measurements = new HashSet<Measurement>();
        }

        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? DateRegistered { get; set; }
        public int? HowMuchWaterId { get; set; }
        public int? HowMuchLightId { get; set; }
        public int? HowMuchHumidityId { get; set; }
        public int? IdealTemperatureId { get; set; }
        public string PlantName { get; set; }
        public string PlantSpecies { get; set; }

        public virtual MeasurementIdealAmount HowMuchHumidity { get; set; }
        public virtual MeasurementIdealAmount HowMuchLight { get; set; }
        public virtual MeasurementIdealAmount HowMuchWater { get; set; }
        public virtual MeasurementIdealAmount IdealTemperature { get; set; }
        public virtual ICollection<Measurement> Measurements { get; set; }
    }
}
