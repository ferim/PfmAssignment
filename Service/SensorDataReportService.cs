using Contract;
using Service.Contract;
using Shared.ResponseModels.SensorDataReportResponse;

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
                }).Select(x => new
                {
                    Time = new DateTime(x.Key.Year, x.Key.Month, x.Key.Day, x.Key.Hour, 0, 0),
                    TotalCount = x.Sum(v => v.Count)
                });

            var dailyReport = data.GroupBy(c =>
                new
                {
                    Year = c.DateFrom.Year,
                    Month = c.DateFrom.Month,
                    Day = c.DateFrom.Day
                }).Select(x => new
                {
                    Time = new DateTime(x.Key.Year, x.Key.Month, x.Key.Day, 0, 0, 0),
                    TotalCount = x.Sum(v => v.Count)
                });

            var weeklyReport = "";//Todo
            return new SensorDataReportResponseModel();
        }
       
    }
}
