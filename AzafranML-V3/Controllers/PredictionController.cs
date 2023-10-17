using Microsoft.AspNetCore.Mvc;
using AzafranML_V3.Models;
using AzafranML_V3;

namespace AzafranML_V3.Controllers
{
    public class PredictionController : Controller
    {
        [HttpGet]
        public IActionResult Predict()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Predict(PredictionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var milkResult = MLModel.Predict(horizon: model.PredictionHorizon);
                var fatResult = FatPercModel.Predict(horizon: model.PredictionHorizon);

                ViewBag.MilkResult = new
                {
                    Prediction = milkResult.Monthly_milk_production__litters_per_cow_,
                    LowerBound = milkResult.Monthly_milk_production__litters_per_cow__LB,
                    UpperBound = milkResult.Monthly_milk_production__litters_per_cow__UB
                };

                ViewBag.FatResult = new
                {
                    Prediction = fatResult.Fat_Percentage,
                    LowerBound = fatResult.Fat_Percentage_LB,
                    UpperBound = fatResult.Fat_Percentage_UB
                };

                return View(model);
            }

            return View();
        }
    }
}