namespace Soat.AntiGaspi.Api.Repository.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Soat.AntiGaspi.Api.Models;

public class ContactOffersTypeConfiguration : IEntityTypeConfiguration<ContactOffer>
{
    public void Configure(EntityTypeBuilder<ContactOffer> builder)
    {
        builder.ToTable("ContactOffers", AntiGaspiContext.DefaultSchema);
        builder.HasKey(x => x.Id);
        builder
            .HasOne<Offer>()
            .WithMany()
            .HasForeignKey(x => x.OfferId);
    }
}
