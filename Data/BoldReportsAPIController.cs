using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BoldReports.Web.ReportViewer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Cors;
using System.IO;
using System.Threading.Tasks;

namespace BlazorReportingTools.Data
{
    [EnableCors("AllowCors")]
    [Route("api/{controller}/{action}/{id?}")]    
    public class BoldReportsAPIController : ControllerBase, IReportController
    {
        // Report viewer requires a memory cache to store the information of consecutive client request and
        // have the rendered report viewer information in server.
        private IMemoryCache _cache;

        // IHostingEnvironment used with sample to get the application data from wwwroot.
        private IWebHostEnvironment _hostingEnvironment;

        public BoldReportsAPIController(IMemoryCache memoryCache, IWebHostEnvironment hostingEnvironment)
        {
            _cache = memoryCache;
            _hostingEnvironment = hostingEnvironment;
        }
        //Get action for getting resources from the report
        [ActionName("GetResource")]
        [AcceptVerbs("GET")]
        // Method will be called from Report Viewer client to get the image src for Image report item.
        public object GetResource(ReportResource resource)
        {
            return ReportHelper.GetResource(resource, this, _cache);
        }

        // RDL: Method will be called to initialize the report information to load the report with ReportHelper for processing.
        // public void OnInitReportOptions(ReportViewerOptions reportOption)
        // {   
        //     string basePath = _hostingEnvironment.WebRootPath;
        //     // Here, we have loaded the sales-order-detail.rdl report from the application folder wwwroot\Resources. sales-order-detail.rdl should be in the wwwroot\Resources application folder.
        //     System.IO.FileStream inputStream = new System.IO.FileStream(basePath + @"\resources\" + reportOption.ReportModel.ReportPath + ".rdl", System.IO.FileMode.Open, System.IO.FileAccess.Read);
        //     MemoryStream reportStream = new MemoryStream();
        //     inputStream.CopyTo(reportStream);
        //     reportStream.Position = 0;
        //     inputStream.Close();
        //     reportOption.ReportModel.Stream = reportStream;
        // }

       //RDLC: Method will be called to initialize the report information to load the report with ReportHelper for processing.
       //Utilizzato per caricare report RDLC
        public void OnInitReportOptions(ReportViewerOptions reportOption)
        {
            string basePath = _hostingEnvironment.WebRootPath;
            reportOption.ReportModel.ProcessingMode = ProcessingMode.Local;
            FileStream inputStream = new FileStream(basePath + @"\resources\" + reportOption.ReportModel.ReportPath + ".rdlc", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            MemoryStream reportStream = new MemoryStream();
            inputStream.CopyTo(reportStream);
            reportStream.Position = 0;
            inputStream.Close();
            reportOption.ReportModel.Stream = reportStream;
           
            //Instanzio la classe GitHubApi e ottengo una lista di valori attraverso una chiamata a HTTPClient ad una WEB API          
            List<Contributor> contributors;
            //Utilizzo GetAwaiter().GetResult() per chiamare un metodo asincrono in modalità sincrona
            contributors = GitHubApi.GetContributors().GetAwaiter().GetResult();
            reportOption.ReportModel.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "DataSetContributors", Value = contributors });

            //Instanzio la classe WeatherForecastService e ottengo un array di valori
            // var WeatherService = new WeatherForecastService();
            // WeatherForecast[] forecasts;
            // forecasts = await WeatherService.GetForecastAsync(System.DateTime.Now);
            // //Nota: il report è stato creato con BoldBi Report Designer RDLC
            // //Il dataset è stato chiamato DataSetForecasts e contiene le colonne che corrispondono come nome e tipo di dati alla classe WeatherForecast
            // reportOption.ReportModel.DataSources.Add(new BoldReports.Web.ReportDataSource { Name = "DataSetForecasts", Value = forecasts });
        }


        // Method will be called when reported is loaded with internally to start to layout process with ReportHelper.
        public void OnReportLoaded(ReportViewerOptions reportOption)
        {
        }

        [HttpPost]
        public object PostFormReportAction()
        {
            return ReportHelper.ProcessReport(null, this, _cache);
        }

        // Post action to process the report from server based json parameters and send the result back to the client.
        [HttpPost]
        public object PostReportAction([FromBody] Dictionary<string, object> jsonArray)
        {
            return ReportHelper.ProcessReport(jsonArray, this, this._cache);
        }
    }

}
