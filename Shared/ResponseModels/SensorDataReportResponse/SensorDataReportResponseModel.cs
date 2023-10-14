
using System.Text.Json.Serialization;

namespace Shared.ResponseModels.SensorDataReportResponse
{
    public class SensorDataReportResponseModel
    {
        [JsonPropertyOrder(2)]
        public IEnumerable<SensorDataResponseHourlyModel> HourlyReport { get; set; }
        [JsonPropertyOrder(1)]
        public IEnumerable<SensorDataResponseDailyModel>  DailyReport { get; set; }
        [JsonPropertyOrder(0)]
        public IEnumerable<SensorDataResponseWeeklyModel> WeeklyReport { get; set; }
    }
}
