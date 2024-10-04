namespace CongestionTaxCalculator.Domain.Entities;

public class TaxRate
{
    public long Id { get; set; }

    public long TaxSettingId { get; set; }

    public TaxSetting TaxSetting { get; set; } = null!;

    public decimal Amount { get; set; }
    public TimeOnly Start { get; set; }
    public TimeOnly End { get; set; }
}