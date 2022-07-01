namespace Soat.AntiGaspi.Api.Repository.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soat.AntiGaspi.Api.Models;

public class OffersTypeConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.ToTable("Offers", AntiGaspiContext.DefaultSchema);
        builder.HasKey(x => x.Id);
    }
}
