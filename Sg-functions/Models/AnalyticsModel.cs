using System;
using System.Collections.Generic;
using System.Text;

namespace Sg_functions.Models
{
    public class AnalyticsModel
    {
        public List<string> Labels { get; set; }
        public List<AnalyticsLineModel> Line { get; set; }
        public int MeasurementTypeId { get; set; }
    }
}
