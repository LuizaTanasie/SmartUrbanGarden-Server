using System;
using System.Collections.Generic;

namespace Core.DataObjects.EFObjects
{
    public partial class DeviceUser
    {
        public Guid DeviceId { get; set; }
        public string Email { get; set; }
        public DateTime? TemperatureWarningEmailSentAt { get; set; }
        public DateTime? HumidityWarningEmailSentAt { get; set; }
        public DateTime? SoilWarningEmailSentAt { get; set; }
        public DateTime? LightWarningEmailSentAt { get; set; }

        public virtual Device Device { get; set; }
    }
}
