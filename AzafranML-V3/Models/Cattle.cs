﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace AzafranML_V3.Models
{
    public class Cattle
    {
        [Required]
        public int Id { get; set; }
        public string Tag { get; set; }
        public int Age { get; set; }
        public string Race { get; set; }
        public double WeightInKg { get; set; }
        public string FeedType { get; set; }
        public ICollection<CattleDailyProduction> CattleDailyProductions { get; set; } = new List<CattleDailyProduction>();
        public ICollection<CattleWeightHistory> WeightHistories { get; set; } = new List<CattleWeightHistory>();

        public Cattle()
        {
            Tag = string.Empty;
            FeedType = string.Empty;  //Might not be necessary
            Race = string.Empty;
        }
    }
}
