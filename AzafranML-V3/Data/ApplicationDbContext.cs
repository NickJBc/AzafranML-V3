using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AzafranML_V3.Models;

namespace AzafranML_V3.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AzafranML_V3.Models.Cattle>? Cattle { get; set; }
        public DbSet<AzafranML_V3.Models.CattleDailyProduction>? CattleDailyProduction { get; set; }
        public DbSet<AzafranML_V3.Models.DailyProduction>? DailyProduction { get; set; }
    }
}