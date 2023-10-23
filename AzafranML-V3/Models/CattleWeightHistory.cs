using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AzafranML_V3.Models
{
    public class CattleWeightHistory
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int CattleId { get; set; }  // Foreign Key to Cattle

        [ForeignKey("CattleId")]
        public Cattle? Cattle { get; set; }  // Navigation property

        [Required]
        public DateTime RecordedDate { get; set; }  // Date when the weight was recorded

        [Required]
        public double WeightInKg { get; set; }  // Weight of the cattle on the recorded date
        public CattleWeightHistory()
        {
            
        }
    }
}
