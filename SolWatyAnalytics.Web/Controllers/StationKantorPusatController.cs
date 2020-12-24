using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text;
using SolWatyAnalytics.Web.Services;
using System.Data;
using Npgsql;
using SolWatyAnalytics.BLL.Models;
using Dapper;
using System.Threading.Tasks;

namespace SolWatyAnalytics.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationKantorPusatController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly StationXService _stationXService;

        public StationKantorPusatController(IWebHostEnvironment webHostEnvironment, StationXService stationXService)
        {
            _webHostEnvironment = webHostEnvironment;
            _stationXService = stationXService;            
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [HttpGet]
        [Route("DailyReport")]
        public IActionResult DailyReport()
        {
            var dt = new DataTable();
            dt = _stationXService.GetStationInfoList();

            string mimetype = "";
            int extension = 1;
            var path = $@"{this._webHostEnvironment.WebRootPath}\Reports\DailyReport.rdlc";

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("header", "Station Pusat Daily Reports");
            param.Add("selectDate", "");

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("dsStationX", dt);

            var result = localReport.Execute(RenderType.Pdf, extension, param, mimetype); 
            return File(result.MainStream, "application/pdf");
        }

        [HttpGet("{tanggal:int}")]
        public IActionResult DailyReport(string tanggal)
        {
            var dt = new DataTable();
            dt = _stationXService.GetStationInfoListbyDate(tanggal);

            string mimetype = "";
            int extension = 1;
            var path = $@"{this._webHostEnvironment.WebRootPath}\Reports\DailyReport.rdlc";

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("header", "Station Pusat Daily Reports");
            param.Add("selectDate", tanggal);

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("dsStationX", dt);

            var result = localReport.Execute(RenderType.Pdf, extension, param, mimetype);
            return File(result.MainStream, "application/pdf");
        }
    }
}
