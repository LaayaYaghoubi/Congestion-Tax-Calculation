using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.TestTools.TaxRates;

public class TaxRateBuilder
{
    private readonly TaxRate _taxRate;

    public TaxRateBuilder()
    {
        _taxRate = new TaxRate
        {
            Amount = 10,
            Start = new TimeOnly(00, 00, 00),
            End = new TimeOnly(03, 00, 00)
        };
    }

    public TaxRateBuilder WithTaxSettingId(long taxSettingId)
    {
        _taxRate.TaxSettingId = taxSettingId;
        return this;
    }

    public TaxRateBuilder WithAmount(long amount)
    {
        _taxRate.Amount = amount;
        return this;
    }

    public TaxRateBuilder WithStart(TimeOnly start)
    {
        _taxRate.Start = start;
        return this;
    }

    public TaxRateBuilder WithEnd(TimeOnly end)
    {
        _taxRate.End = end;
        return this;
    }

    public TaxRate Build()
    {
        return _taxRate;
    }
}