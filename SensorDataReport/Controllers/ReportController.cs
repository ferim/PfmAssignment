using Microsoft.AspNetCore.Http;
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
        public SensorDataReportResponseModel Get() 
        {
            return _sensorDataReportService.GetSensorDataReport("C:\\git\\PfmAssignment\\SensorDataReport\\counts.csv");
        }
        [Route("SensorDataReportV2")]
        [HttpGet]
        public SensorDataResponseOverviewModel GetV2()
        {
            return _sensorDataReportService.GetSensorDataReportV2("C:\\git\\PfmAssignment\\SensorDataReport\\counts.csv");
            
        }
        //
    }

}
