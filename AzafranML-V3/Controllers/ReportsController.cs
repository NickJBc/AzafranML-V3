using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AzafranML_V3.Data;
using AzafranML_V3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;

namespace AzafranML_V3.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = await GetReportViewModel();
            return View(viewModel);
        }

        private async Task<ReportViewModel> GetReportViewModel()
        {
            var viewModel = new ReportViewModel
            {
                DailyProductions = await _context.DailyProduction
                    .Select(dp => new DailyMilkProduction
                    {
                        Date = dp.Date.ToString("yyyy-MM-dd"),
                        TotalMilkProduced = dp.CattleDailyProductions.Sum(cdp => cdp.AmountProduced)
                    })
                    .ToListAsync(),

                AvgWeightByFeedType = await _context.Cattle
                    .GroupBy(c => c.FeedType)
                    .Select(g => new FeedTypeWeightAverage
                    {
                        FeedType = g.Key,
                        AverageWeight = g.Average(c => c.WeightInKg)
                    })
                    .ToListAsync()
            };

            var weightGroups = new List<(double, double)>
            {
            (0, 100),
            (100, 200),
            (200, 300),
            (300, 400),
            (400, 500),
            (500, 600),
            (600, 700),
            (700, 800),
                // Add more ranges as needed
            };

            viewModel.WeightDistributions = weightGroups.Select(group => new WeightDistribution
            {
                WeightRange = $"{group.Item1}-{group.Item2}kg",
                Count = _context.Cattle.Count(c => c.WeightInKg >= group.Item1 && c.WeightInKg < group.Item2)
            }).ToList();

            return viewModel;
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportToExcel()
        {
            var viewModel = await GetReportViewModel();

            var stream = new MemoryStream();
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");

                // Daily Milk Production data
                worksheet.Cells[1, 1].Value = "Date";
                worksheet.Cells[1, 2].Value = "Total Milk Produced";
                int rowIndex = 2;
                foreach (var item in viewModel.DailyProductions)
                {
                    worksheet.Cells[rowIndex, 1].Value = item.Date;
                    worksheet.Cells[rowIndex, 2].Value = item.TotalMilkProduced;
                    rowIndex++;
                }

                // Daily Milk Production graph data
                var dailyMilkChart = worksheet.Drawings.AddChart("DailyMilkChart", eChartType.Line) as ExcelLineChart;
                dailyMilkChart.Title.Text = "Daily Milk Production";
                dailyMilkChart.Series.Add($"B2:B{rowIndex - 1}", $"A2:A{rowIndex - 1}");
                dailyMilkChart.Border.LineStyle = OfficeOpenXml.Drawing.eLineStyle.Solid;
                dailyMilkChart.SetPosition(0, 0, 4, 0);
                dailyMilkChart.SetSize(800, 400);

                // Calculate the starting position for the second chart
                int secondChartRowStart = dailyMilkChart.To.Row + 5;  // Add some buffer rows between graphs

                // Average Weight by Feed Type data
                worksheet.Cells[secondChartRowStart, 1].Value = "Feed Type";
                worksheet.Cells[secondChartRowStart, 2].Value = "Average Weight";
                rowIndex = secondChartRowStart + 1;
                foreach (var item in viewModel.AvgWeightByFeedType)
                {
                    worksheet.Cells[rowIndex, 1].Value = item.FeedType;
                    worksheet.Cells[rowIndex, 2].Value = item.AverageWeight;
                    rowIndex++;
                }

                // Average Weight by Feed Type graph data
                var avgWeightChart = worksheet.Drawings.AddChart("AvgWeightByFeedTypeChart", eChartType.ColumnClustered) as ExcelBarChart;
                avgWeightChart.Title.Text = "Average Weight by Feed Type";
                avgWeightChart.Series.Add($"B{secondChartRowStart + 1}:B{rowIndex - 1}", $"A{secondChartRowStart + 1}:A{rowIndex - 1}");
                avgWeightChart.Border.LineStyle = OfficeOpenXml.Drawing.eLineStyle.Solid;
                avgWeightChart.SetPosition(secondChartRowStart, 0, 4, 0);
                avgWeightChart.SetSize(800, 400);

                // Calculate the starting position for the third chart/data
                int thirdChartRowStart = avgWeightChart.To.Row + 5;  // Add some buffer rows between graphs

                // Weight Distribution data
                worksheet.Cells[thirdChartRowStart, 1].Value = "Weight Range";
                worksheet.Cells[thirdChartRowStart, 2].Value = "Count";
                rowIndex = thirdChartRowStart + 1;
                foreach (var item in viewModel.WeightDistributions)
                {
                    worksheet.Cells[rowIndex, 1].Value = item.WeightRange;
                    worksheet.Cells[rowIndex, 2].Value = item.Count;
                    rowIndex++;
                }

                // Weight Distribution graph data
                var weightDistChart = worksheet.Drawings.AddChart("WeightDistributionChart", eChartType.ColumnClustered) as ExcelBarChart;
                weightDistChart.Title.Text = "Weight Distribution";
                weightDistChart.Series.Add($"B{thirdChartRowStart + 1}:B{rowIndex - 1}", $"A{thirdChartRowStart + 1}:A{rowIndex - 1}");
                weightDistChart.Border.LineStyle = OfficeOpenXml.Drawing.eLineStyle.Solid;
                weightDistChart.SetPosition(thirdChartRowStart, 0, 4, 0);
                weightDistChart.SetSize(800, 400);

                package.Save();
            }
            stream.Position = 0;
            string excelName = $"Report-{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}.xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

    }
}
