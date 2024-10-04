namespace CongestionTaxCalculator.Domain.Entities;

public class TaxFreeVehicle
{
    public long Id { get; set; }
    public long TaxSettingId { get; set; }

    public TaxSetting TaxSetting { get; set; } = null!;
    public VehicleType VehicleType { get; set; }
}