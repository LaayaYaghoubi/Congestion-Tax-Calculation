namespace CongestionTaxCalculator.Domain.Entities;

public class Holiday
{
    public long Id { get; set; }
    public long TaxSettingId { get; set; }

    public TaxSetting TaxSetting { get; set; } = null!;
    public DateOnly Date { get; set; }
}