using CongestionTaxCalculator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Persistence.EF.Holidays;

public class HolidayEntityMap : IEntityTypeConfiguration<Holiday>
{
    public void Configure(EntityTypeBuilder<Holiday> builder)
    {
        builder.ToTable("Holidays");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Date).IsRequired();

        builder.HasOne(x => x.TaxSetting)
            .WithMany(x => x.Holidays)
            .HasForeignKey(x => x.TaxSettingId)
            .IsRequired();
    }
}