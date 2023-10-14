using Microsoft.AspNetCore.Mvc;
using Service.Contract;
using Shared.ResponseModels.SensorDataReportResponse;

namespace SensorDataReport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ISensorDataReportService _sensorDataReportService;
        public ReportController(ISensorDataReportService sensorDataReportService) 
        {
            _sensorDataReportService = sensorDataReportService;
        }

        [Route("SensorDataReport")]
        [HttpGet]
        public SensorDataReportResponseModel Get(string csvFullPath) 
        {
            return _sensorDataReportService.GetSensorDataReport(csvFullPath);
        }

        [Route("SensorDataReportV2")]
        [HttpGet]
        public SensorDataReportResponseOverviewModel GetV2(string csvFullPath)
        {
            return _sensorDataReportService.GetSensorDataReportV2(csvFullPath); 


        }
      
    }

}
