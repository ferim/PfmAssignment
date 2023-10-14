using Contract;
using Data;
using System.Globalization;

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
               DateFrom = ParseDateTime(parts[1]),
               DateTo = ParseDateTime(parts[2]),
               Count = int.Parse(parts[3])
           })
           .ToList();

            return sensorDatas;
        }
        static DateTime ParseDateTime(string dateTimeString)
        {

            if (DateTime.TryParseExact(dateTimeString, "yyyy-MM-ddTHH:mm:sszzz", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime result))
            {
                return result;
            }


            if (DateTime.TryParseExact(dateTimeString, "yyyy-MM-ddTHH:mm:sszz", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out result))
            {
                return result.ToUniversalTime();
            }

            throw new Exception($"Failed to parse DateTime: {dateTimeString}");
        }
    }
}
