using CongestionTaxCalculator.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CongestionTaxCalculator.Persistence.EF.TaxFreeVehicles;

public class TaxFreeVehicleEntityMap : IEntityTypeConfiguration<TaxFreeVehicle>
{
    public void Configure(EntityTypeBuilder<TaxFreeVehicle> builder)
    {
        builder.ToTable("TaxFreeVehicles");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.VehicleType).IsRequired();

        builder.HasOne(x => x.TaxSetting)
            .WithMany(x => x.TaxFreeVehicles)
            .HasForeignKey(x => x.TaxSettingId)
            .IsRequired();
    }
}