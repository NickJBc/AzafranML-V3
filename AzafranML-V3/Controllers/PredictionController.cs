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
                var result = MLModel.Predict(horizon: model.PredictionHorizon);
                ViewBag.Result = new
                {
                    Prediction = result.Monthly_milk_production__litters_per_cow_,
                    LowerBound = result.Monthly_milk_production__litters_per_cow__LB,
                    UpperBound = result.Monthly_milk_production__litters_per_cow__UB
                };

                return View(model);
            }

            return View();
        }
    }
}