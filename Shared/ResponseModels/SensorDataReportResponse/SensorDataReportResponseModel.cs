using System.Text.Json.Serialization;

namespace Shared.ResponseModels.SensorDataReportResponse
{
    public class SensorDataReportResponseModel
    {
        [JsonPropertyOrder(2)]
        public IEnumerable<SensorDataReportResponseHourlyModel> HourlyReport { get; set; }
        [JsonPropertyOrder(1)]
        public IEnumerable<SensorDataReportResponseDailyModel>  DailyReport { get; set; }
        [JsonPropertyOrder(0)]
        public IEnumerable<SensorDataReportResponseWeeklyModel> WeeklyReport { get; set; }
    }
}
