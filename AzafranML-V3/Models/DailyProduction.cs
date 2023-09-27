namespace AzafranML_V3.Models
{
    public class DailyProduction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public ICollection<CattleDailyProduction> CattleDailyProductions { get; set; } = new List<CattleDailyProduction>();
    }
}
