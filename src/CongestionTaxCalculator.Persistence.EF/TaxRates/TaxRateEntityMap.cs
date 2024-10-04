using CongestionTaxCalculator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Persistence.EF.TaxRates;

public class TaxRateEntityMap : IEntityTypeConfiguration<TaxRate>
{
    public void Configure(EntityTypeBuilder<TaxRate> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Amount).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.Start).IsRequired();
        builder.Property(x => x.End).IsRequired();

        builder.HasOne(x => x.TaxSetting)
            .WithMany(x => x.TaxRates)
            .HasForeignKey(x => x.TaxSettingId)
            .IsRequired();
    }
}