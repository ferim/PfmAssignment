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

            var hourlyReport = data.GroupBy(c =>
                new 
                {
                    Year = c.DateFrom.Year,
                    Month = c.DateFrom.Month,
                    Day = c.DateFrom.Day,
                    Hour = c.DateFrom.Hour,
                }).Select(x => new SensorDataResponseHourlyModel
                {
                    HourlyTime = new DateTime(x.Key.Year, x.Key.Month, x.Key.Day, x.Key.Hour, 0, 0),
                    TotalCount = x.Sum(v => v.Count)
                });

            var dailyReport = data.GroupBy(c =>
                new 
                {
                    Year = c.DateFrom.Year,
                    Month = c.DateFrom.Month,
                    Day = c.DateFrom.Day
                }).Select(x => new SensorDataResponseDailyModel
                {
                    DailyTime = new DateTime(x.Key.Year, x.Key.Month, x.Key.Day, 0, 0, 0),
                    TotalCount = x.Sum(v => v.Count)
                });

            var weeklyReport = data.GroupBy(c =>
            CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(c.DateFrom, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
            ).Select(x => new SensorDataResponseWeeklyModel
            {
                    WeekNumber = x.Key,
                    TotalCount = x.Sum(v => v.Count)
                });

            return new SensorDataReportResponseModel()
            {
                HourlyReport = hourlyReport,
                DailyReport = dailyReport,
                WeeklyReport = weeklyReport
            };
        }
       
    }
}
