using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soat.AntiGaspi.Api.Models;

namespace Soat.AntiGaspi.Api.Repository.Configurations
{
    public class AnnoncesTypeConfiguration : IEntityTypeConfiguration<Annonce>
    {
        public void Configure(EntityTypeBuilder<Annonce> builder)
        {
            builder.ToTable("Annonces", AntiGaspiContext.DefaultSchema);
            builder.HasKey(x => x.Id);
        }
    }
}
