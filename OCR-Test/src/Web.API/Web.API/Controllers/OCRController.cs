using IronOcr;
using Microsoft.AspNetCore.Mvc;
using Regula.DocumentReader.WebClient.Api;
using Regula.DocumentReader.WebClient.Model;
using Regula.DocumentReader.WebClient.Model.Ext;
using System.IO;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OCRController : ControllerBase
    {
        private readonly ILogger<OCRController> _logger;
        private const string API_BASE_PATH = "API_BASE_PATH";
        private const string TEST_LICENSE = "TEST_LICENSE";
        private const string LICENSE_FILE_NAME = "regula.license";
        public OCRController(ILogger<OCRController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Get")]
        public IActionResult Get()
        {
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();

            var OCR = new IronTesseract();
            OCR.Language = OcrLanguage.Azerbaijani;
            string text;
            using (var Input = new OcrInput("C:\\Users\\labusers\\Downloads\\yess.png"))
            {
                Input.DeNoise();
                Input.Deskew();
                var response = OCR.Read(Input);
                text= response.Text;
            }

            var result = new IronOcr.IronTesseract().Read("C:\\Users\\labusers\\Downloads\\said2.png").Text;
           return Ok(text);

        }

        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var apiBaseUrl = Environment.GetEnvironmentVariable(API_BASE_PATH) ?? "https://api.regulaforensics.com";

            var licenseFromEnv =
                Environment.GetEnvironmentVariable(TEST_LICENSE); // optional, used here only for smoke test purposes
            var licenseFromFile = System.IO.File.Exists(LICENSE_FILE_NAME)
                ? System.IO.File.ReadAllBytes(LICENSE_FILE_NAME)
                : null;

            var whitePage0 = System.IO.File.ReadAllBytes("C:\\Users\\labusers\\Downloads\\test3.png");
            //var irPage0 = System.IO.File.ReadAllBytes("C:\\Users\\labusers\\Downloads\\test3.png");
            //var uvPage0 = System.IO.File.ReadAllBytes("C:\\Users\\labusers\\Downloads\\test3.png");

            var requestParams = new RecognitionParams()
                .WithScenario(Scenario.FULL_AUTH)
                .WithResultTypeOutput(new List<int>
                {
                    // actual results
                    Result.STATUS, Result.AUTHENTICITY, Result.TEXT, Result.IMAGES,
                    Result.DOCUMENT_TYPE, Result.DOCUMENT_TYPE_CANDIDATES, Result.DOCUMENT_POSITION,
                    // legacy results
                    Result.MRZ_TEXT, Result.VISUAL_TEXT, Result.BARCODE_TEXT, Result.RFID_TEXT,
                    Result.VISUAL_GRAPHICS, Result.BARCODE_GRAPHICS, Result.RFID_GRAPHICS,
                    Result.LEXICAL_ANALYSIS, Result.IMAGE_QUALITY
                });

            var request = new RecognitionRequest(requestParams, new List<ProcessRequestImage>
            {
                new ProcessRequestImage(new ImageData(whitePage0), Light.WHITE),
                //new ProcessRequestImage(new ImageData(irPage0), Light.IR),
                //new ProcessRequestImage(new ImageData(uvPage0), Light.UV)
            });
            var api = licenseFromEnv != null
                ? new DocumentReaderApi(apiBaseUrl).WithLicense(licenseFromEnv)
                : new DocumentReaderApi(apiBaseUrl).WithLicense(licenseFromFile);

            var response = api.Process(request);

            // overall status results 
            var status = response.Status();

            return Ok(response);

        }

    }
}