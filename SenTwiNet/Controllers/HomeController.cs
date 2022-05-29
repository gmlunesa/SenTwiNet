using Microsoft.AspNetCore.Mvc;
using SenTwiNet.Models;
using System.Diagnostics;

namespace SenTwiNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index([Bind("SentimentText")] SentimentDataModel sentimentData)
        {
            if (ModelState.IsValid)
            {
                SentimentPredictionModel prediction = Predict(sentimentData);

                return View(prediction);
            }

            return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }

        public SentimentPredictionModel Predict(SentimentDataModel sentimentData)
        {
            var modelInput = new MLModel.ModelInput() { Col0 = sentimentData.SentimentText, };

            //Load model and predict output
            var output = MLModel.Predict(modelInput);

            SentimentPredictionModel prediction = new SentimentPredictionModel
            {
                SentimentText = sentimentData.SentimentText,
                PredictedLabel = output.PredictedLabel,
            };

            return prediction;
        }
    }
}
