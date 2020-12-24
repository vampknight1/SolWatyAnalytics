using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SolWatyAnalytics.Api.Repo;
using SolWatyAnalytics.BLL.DAL;
using SolWatyAnalytics.BLL.Models;

namespace SolWatyAnalytics.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StationXController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly StationXRepo _stationXRepo;

        public StationXController(StationXRepo stationXRepo, IWebHostEnvironment webHostEnvironment)
        {
            _stationXRepo = stationXRepo;
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [HttpGet]
        public async Task<ActionResult> GetStationXAllInfo()
        {
            try
            {
                return Ok(await _stationXRepo.FindAll());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the DB !");
            }
            
        }

        [HttpGet]
        [Route("DailyReport")]
        public IActionResult DailyReport()
        {
            var dt = _stationXRepo.FindAll();

            string mimetype = "";
            int extension = 1;
            var path = $@"{this._webHostEnvironment.WebRootPath}\Reports\DailyReport.rdlc";

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("param", "Station Pusat Daily Reports");

            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("dsStationX", dt);
            var result = localReport.Execute(RenderType.Pdf, extension, param, mimetype);
            return File(result.MainStream, "application/pdf");
        }
    }
}
