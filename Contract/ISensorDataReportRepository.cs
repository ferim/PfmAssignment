using Data;

namespace Contract
{
    public interface ISensorDataReportRepository
    {
        IEnumerable<SensorData> GetSensorData(string fullPath);
    }
}
