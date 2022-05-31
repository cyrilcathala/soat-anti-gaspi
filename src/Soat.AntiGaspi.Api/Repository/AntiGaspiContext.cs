using Microsoft.EntityFrameworkCore;
using Soat.AntiGaspi.Api.Models;
using Soat.AntiGaspi.Api.Repository.Configurations;

namespace Soat.AntiGaspi.Api.Repository
{
    public class AntiGaspiContext : DbContext
    {
        public const string DefaultSchema = "antigaspi";
        public DbSet<Annonce> Annonces { get; set; }

        public AntiGaspiContext()
        {
        }

        public AntiGaspiContext(DbContextOptions<AntiGaspiContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AnnoncesTypeConfiguration());
        }
    }
}
