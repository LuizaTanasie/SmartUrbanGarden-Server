using Core.DataObjects.EFEnums;
using DataAccess.DbContexts;
using Sg_functions.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Sg_functions.Helpers
{
    public class AnalyticsHelper
    {
        private readonly SGContext context;
        public AnalyticsHelper(SGContext context)
        {
            this.context = context;
        }

        public List<AnalyticsModel> GetAnalytics(Guid deviceId, AnalyticsSettings settings)
        {
            var noOfDays = settings == AnalyticsSettings.PerMonth ? -30 : -7;
            var measurements = context.Measurements
                .Where(m => m.DeviceId == deviceId
                            && m.MeasuredAtTime >= DateTime.Now.Date.AddDays(noOfDays))
                .OrderBy(m => m.MeasuredAtTime).ToList();

            var analyticsList = new List<AnalyticsModel>();
            var labels = measurements
                .Select(m => m.MeasuredAtTime.ToString("dd MMM", new CultureInfo("en-GB"))).Distinct().ToList();

            var measurementGroup = measurements.GroupBy(m => m.MeasuredAtTime.Date);
            var humidityAvgs = measurementGroup.Select(g => (int)Math.Floor(g.Average(mm => mm.Humidity).Value)).ToList();
            var temperatureAvgs = measurementGroup.Select(g => (int)Math.Floor(g.Average(mm => mm.Temperature).Value)).ToList();
            var soilAvgs = measurementGroup.Select(g => (int)Math.Floor(g.Average(mm => mm.SoilMoisturePercentage).Value)).ToList();
            var lightAvgs = measurementGroup.Select(g => (int)Math.Floor(g.Average(mm => mm.LightPercentage).Value)).ToList();

            analyticsList.Add(new AnalyticsModel { 
                MeasurementTypeId = (int)MeasurementTypes.Humidity, 
                Labels = labels, 
                Line = new List<AnalyticsLineModel>() { new AnalyticsLineModel { Label = MeasurementTypes.Humidity.ToString(), Data = humidityAvgs } } });
            analyticsList.Add(new AnalyticsModel {
                MeasurementTypeId = (int)MeasurementTypes.Light,
                Labels = labels, 
                Line = new List<AnalyticsLineModel>() { new AnalyticsLineModel { Label = MeasurementTypes.Soil.ToString(), Data = soilAvgs } } });
            analyticsList.Add(new AnalyticsModel {
                MeasurementTypeId = (int)MeasurementTypes.Soil,
                Labels = labels, 
                Line = new List<AnalyticsLineModel>() { new AnalyticsLineModel { Label = MeasurementTypes.Light.ToString(), Data = lightAvgs } } });
            analyticsList.Add(new AnalyticsModel {
                MeasurementTypeId = (int)MeasurementTypes.Temperature,
                Labels = labels, 
                Line = new List<AnalyticsLineModel>() { new AnalyticsLineModel { Label = MeasurementTypes.Temperature.ToString(), Data = temperatureAvgs } } });
            return analyticsList;
        }

    }
}
