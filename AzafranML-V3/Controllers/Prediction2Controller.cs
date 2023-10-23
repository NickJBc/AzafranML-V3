using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AzafranML_V3.Controllers
{
    public class Prediction2Controller : Controller
    {
        public IActionResult Index()
        {
            var predictions = GetPredictionsFromPython();
            if (predictions == null)
            {
                throw new Exception("Predictions are null.");
            }
            return View(predictions);
        }


        private List<double> GetPredictionsFromPython()
        {
            string pythonPath = @"C:\Python311\python.exe"; // e.g., C:\Python39\python.exe
            string scriptPath = @"C:\Users\njbed\source\repos\PythonApplication1\PythonApplication1\PythonApplication1.py"; // e.g., C:\path_to_your_project\your_script.py

            ProcessStartInfo start = new ProcessStartInfo
            {
                FileName = pythonPath,
                Arguments = $"\"{scriptPath}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    // Convert the result (string) to a list of doubles and return
                    List<double> predictions = JsonConvert.DeserializeObject<List<double>>(result);
                    return predictions;
                }
            }
        }
    }
}
