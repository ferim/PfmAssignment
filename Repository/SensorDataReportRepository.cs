using Contract;
using Data;

namespace Repository
{
    public class SensorDataReportRepository : ISensorDataReportRepository
    {
        public IEnumerable<SensorData> GetSensorData(string fullPath)
        {
            var sensorDatas = File.ReadLines(fullPath)
           .Skip(1)
           .Select(line => line.Split(','))
           .Select(parts => new SensorData
           {
               SensorId = int.Parse(parts[0]),
               DateFrom = DateTime.Parse(parts[1]),
               DateTo = DateTime.Parse(parts[2]),
               Count = int.Parse(parts[3])
           })
           .ToList();

            return sensorDatas;
        }
    }
}
