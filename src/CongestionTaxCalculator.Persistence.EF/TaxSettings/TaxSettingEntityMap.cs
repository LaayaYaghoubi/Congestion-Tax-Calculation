using CongestionTaxCalculator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Persistence.EF.TaxSettings;

public class TaxSettingEntityMap : IEntityTypeConfiguration<TaxSetting>
{
    public void Configure(EntityTypeBuilder<TaxSetting> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CityName).HasMaxLength(100).IsRequired();
    }
}