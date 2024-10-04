using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.TestTools.Holidays;

public class HolidayBuilder
{
    private readonly Holiday _holiday;

    public HolidayBuilder()
    {
        _holiday = new Holiday();
    }

    public HolidayBuilder WithDate(DateOnly date)
    {
        _holiday.Date = date;
        return this;
    }

    public HolidayBuilder WithTaxSettingId(long id)
    {
        _holiday.TaxSettingId = id;
        return this;
    }

    public Holiday Build()
    {
        return _holiday;
    }
}