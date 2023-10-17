namespace AzafranML_V3.Models
{
    public class CattleDailyProduction
    {
        public int Id { get; set; }
        public int CattleId { get; set; }
        public Cattle? Cattle { get; set; }
        public int DailyProductionId { get; set; }
        public DailyProduction? DailyProduction { get; set; }

        public double AmountProduced { get; set; } // daily
    }
}
