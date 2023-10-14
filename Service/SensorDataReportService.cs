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
            return new SensorDataReportResponseModel();
        }
    }
}
