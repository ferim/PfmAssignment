using Contract;
using Service.Contract;
using Shared.ResponseModels.SensorDataReportResponse;
using System.Diagnostics;

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
            var sw = new Stopwatch();
            sw.Start();
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

            var weeklyReport = data.Select(x=> new 
            {
                WeekStartTime = TimeZoneInfo.ConvertTimeToUtc(x.DateFrom).Date.AddDays(-(int)TimeZoneInfo.ConvertTimeToUtc(x.DateFrom).DayOfWeek),
                Count = x.Count,
            })
                .GroupBy(c => c.WeekStartTime)
                .Select(g => new SensorDataResponseWeeklyModel
            {
                WeekStartTime = g.Key,
                TotalCount = g.Sum(s => s.Count)
            });
            sw.Stop();
           
          
            return new SensorDataReportResponseModel()
            {
                HourlyReport = hourlyReport,
                DailyReport = dailyReport,
                WeeklyReport = weeklyReport
            };
        }
       
    }
}
