namespace AzafranML_V3.Models
{
    public class ReportViewModel
    {
        public List<DailyMilkProduction> DailyProductions { get; set; }
        public List<FeedTypeWeightAverage> AvgWeightByFeedType { get; set; }
        public List<WeightDistribution> WeightDistributions { get; set; }
    }

    public class DailyMilkProduction
    {
        public string Date { get; set; }
        public double TotalMilkProduced { get; set; }
    }

    public class FeedTypeWeightAverage
    {
        public string FeedType { get; set; }
        public double AverageWeight { get; set; }
    }

    public class WeightDistribution
    {
        public string WeightRange { get; set; }
        public int Count { get; set; }
    }
}
