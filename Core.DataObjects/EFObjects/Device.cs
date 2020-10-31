using System;
using System.Collections.Generic;

namespace Core.DataObjects.EFObjects
{
    public partial class Device
    {
        public Guid? Id { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? DateRegistered { get; set; }
    }
}
