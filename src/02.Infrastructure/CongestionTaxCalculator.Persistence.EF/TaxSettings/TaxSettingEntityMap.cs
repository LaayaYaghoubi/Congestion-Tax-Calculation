using System.Text.Json;
using CongestionTaxCalculator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Persistence.EF.TaxSettings;

public class TaxSettingEntityMap : IEntityTypeConfiguration<TaxSetting>
{
    public void Configure(EntityTypeBuilder<TaxSetting> builder)
    {
        builder.ToTable("TaxSettings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CityName).HasMaxLength(100).IsRequired();
        builder.Property(e => e.ActiveMonths)
            .HasConversion(
                v => 
                    JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                v => JsonSerializer.Deserialize<HashSet<int>>(v,
                    (JsonSerializerOptions)null!)!);
    }
}