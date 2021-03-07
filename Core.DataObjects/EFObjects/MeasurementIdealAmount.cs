using System;
using System.Collections.Generic;

namespace Core.DataObjects.EFObjects
{
    public partial class MeasurementIdealAmount
    {
        public MeasurementIdealAmount()
        {
            DeviceHowMuchHumidities = new HashSet<Device>();
            DeviceHowMuchLights = new HashSet<Device>();
            DeviceHowMuchWaters = new HashSet<Device>();
            DeviceIdealTemperatures = new HashSet<Device>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Device> DeviceHowMuchHumidities { get; set; }
        public virtual ICollection<Device> DeviceHowMuchLights { get; set; }
        public virtual ICollection<Device> DeviceHowMuchWaters { get; set; }
        public virtual ICollection<Device> DeviceIdealTemperatures { get; set; }
    }
}
