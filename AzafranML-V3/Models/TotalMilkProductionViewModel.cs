namespace AzafranML_V3.Models
{
    public class TotalMilkProductionViewModel
    {
        public Dictionary<DateTime, double> DailyProductions { get; set; } = new Dictionary<DateTime, double>();
        public Dictionary<DateTime, Dictionary<int, double>> DetailedProductions { get; set; } = new Dictionary<DateTime, Dictionary<int, double>>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? CattleId { get; set; }  
    }
}
