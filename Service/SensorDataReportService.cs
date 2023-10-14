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
                .Select(g => new SensorDataReportResponseHourlyModel
            {
                HourlyTime = g.Key,
                TotalCount = g.Sum(s => s.Count)
            });

            var dailyReport = data.Select(x => new
            {
                SelectedTime = x.DateFrom.ToUniversalTime(),
                Count = x.Count,
            })
                .GroupBy(c => c.SelectedTime.Date)
                .Select(g => new SensorDataReportResponseDailyModel
            {
                DailyTime = g.Key,
                TotalCount = g.Sum(s => s.Count)
            });

            var weeklyReport = data.Select(x=> new 
            {
                WeekStartTime = GetStartOfWeek(x.DateFrom),
                Count = x.Count,
            })
                .GroupBy(c => c.WeekStartTime)
                .Select(g => new SensorDataReportResponseWeeklyModel
            {
                WeekStartTime = g.Key,
                TotalCount = g.Sum(s => s.Count)
            });
            sw.Stop();
            var elml = sw.ElapsedMilliseconds;

            return new SensorDataReportResponseModel()
            {
                HourlyReport = hourlyReport,
                DailyReport = dailyReport,
                WeeklyReport = weeklyReport
            };
        }

        public SensorDataReportResponseOverviewModel GetSensorDataReportV2(string fullPath)
        {
            var sw = new Stopwatch();
            sw.Start();
            var data = _sensorDataReportRepository.GetSensorData(fullPath);
            var groupedData = data
                   .GroupBy(c => new
                   {
                       WeekStart = GetStartOfWeek(c.DateFrom)
                   })
                   .Select(weekGroup => new Week
                   {
                       WeekStart = weekGroup.Key.WeekStart,
                       WeeklyCount = weekGroup.Sum(s => s.Count),
                       Days = weekGroup
                           .GroupBy(c => new
                           {
                               c.DateFrom.Date
                           })
                           .Select(dayGroup => new Day
                           {
                               DayTime = dayGroup.Key.Date.Date,
                               DailyCount = dayGroup.Sum(s => s.Count),
                               Hours = dayGroup
                                   .GroupBy(c => new
                                   {
                                       c.DateFrom.Hour
                                   })
                                   .Select(hourGroup => new Hour
                                   {
                                       HourTime = hourGroup.FirstOrDefault().DateFrom.ToString("yyyy-MM-ddTHH:mm"),
                                       HourlyCount = hourGroup.Sum(s => s.Count),                                    
                                   })
                                   .ToList()
                           })
                           .ToList()
                   })
                   .ToList();
           
            var weeklySummaries = groupedData.Select(week => new Week
            {
                WeekStart = week.WeekStart,
                WeeklyCount = week.WeeklyCount
            }).ToList();

            var dailySummaries = groupedData.SelectMany(week => week.Days.Select(day => new Day
            {              
                DayTime = day.DayTime,
                DailyCount = day.DailyCount
            })).ToList();

            var hourlySummaries = groupedData.SelectMany(week => week.Days.SelectMany(day => day.Hours.Select(hour => new Hour
            {               
                HourTime = hour.HourTime,
                HourlyCount = hour.HourlyCount            
            }))).ToList();
            
            sw.Stop();
            var elml = sw.ElapsedMilliseconds;
            
            return new SensorDataReportResponseOverviewModel()
            { 
                        HourlyReport = hourlySummaries,
                        DailyReport = dailySummaries,
                        WeeklyReport= weeklySummaries
            };
            
        }

        static DateTime GetStartOfWeek(DateTime date)
        {
            int diff = date.DayOfWeek - DayOfWeek.Monday;
            if (diff < 0)
            {
                diff += 7;
            }
            return date.Date.AddDays(-diff);
        }
    }
}
