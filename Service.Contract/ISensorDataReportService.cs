
using Shared.ResponseModels.SensorDataReportResponse;

namespace Service.Contract
{
    public interface ISensorDataReportService
    {
        SensorDataReportResponseModel GetSensorDataReport(string fullPath);
    }
}
