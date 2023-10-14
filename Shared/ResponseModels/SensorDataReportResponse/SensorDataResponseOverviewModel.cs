
namespace Shared.ResponseModels.SensorDataReportResponse
{
    public class SensorDataResponseOverviewModel
    {
        public IEnumerable<Week> WeeklyReport { get; set; }
        public IEnumerable<Day> DailyReport { get; set; }
        public IEnumerable<Hour> HourlyReport { get; set; }
    }
    public class Week
    {
        public DateTime WeekStart { get; set; }
        public int WeeklyCount { get; set; }
        public IEnumerable<Day> Days { get; set; }
    }
    public class Day
    {
        public DateTime DayTime { get; set; }
        public int DailyCount { get; set; }
        public IEnumerable<Hour> Hours { get; set; }

    }
    public class Hour
    {
        public string HourTime { get; set; }
        public int HourlyCount { get; set; }
    }
}
