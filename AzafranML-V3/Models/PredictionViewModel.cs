using System.ComponentModel.DataAnnotations;

namespace AzafranML_V3.Models
{
    public class PredictionViewModel
    {
        [Required]
        [Display(Name = "Monthly Milk Production")]
        public float MonthlyMilkProduction { get; set; }

        [Required]
        [Display(Name = "Prediction Horizon")]
        [Range(1, int.MaxValue, ErrorMessage = "Prediction Horizon must be a positive number.")]
        public int PredictionHorizon { get; set; }

        [Required]
        [Display(Name = "Fat Percentage")]
        public float FatPercentage { get; set; } // Added this for the new model
    }
}