using Contract;
using Service.Contract;
using Shared.ResponseModels.SensorDataReportResponse;
using System.Globalization;

namespace Service
{
    public class SensorDataReportService : ISensorDataReportService
    {
        private readonly ISensorDataReportRepository _sensorDataReportRepository;
        public SensorDataReportService(ISensorDataReportRepository sensorDataReportRepository)
        {
            _sensorDataReportRepository = sensorDataReportRepository;
        }
        public SensorDataReportResponseModel GetSensorDataReport(string fullPath)
        {
            var data = _sensorDataReportRepository.GetSensorData(fullPath);

            var hourlyReport = data.Select(x => new
            {
                UniversalTime = x.DateFrom.ToUniversalTime(),
                Count = x.Count,
            })
                .GroupBy(c => c.UniversalTime.Date.AddHours(c.UniversalTime.Hour))
                .Select(g => new SensorDataResponseHourlyModel
            {
                HourlyTime = TimeZoneInfo.ConvertTimeFromUtc(g.Key, TimeZoneInfo.Local),
                TotalCount = g.Sum(s => s.Count)
            });

            var dailyReport = data.Select(x => new
            {
                SelectedTime = x.DateFrom.ToUniversalTime(),
                Count = x.Count,
            })
                .GroupBy(c => c.SelectedTime.Date)
                .Select(g => new SensorDataResponseDailyModel
            {
                DailyTime = g.Key,
                TotalCount = g.Sum(s => s.Count)
            });

            var weeklyReport = new List<SensorDataResponseWeeklyModel>();  //TODO




            return new SensorDataReportResponseModel()
            {
                HourlyReport = hourlyReport,
                DailyReport = dailyReport,
                WeeklyReport = weeklyReport
            };
        }
       
    }
}
