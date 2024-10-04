using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.TestTools.TaxSettings;

public class TaxSettingBuilder
{
    private readonly TaxSetting _taxSetting;

    public TaxSettingBuilder()
    {
        _taxSetting = new TaxSetting
        {
            CityName = "Gothenburg",
            ActiveMonths = [1, 2, 3, 4, 5, 6, 8, 9, 10, 11, 12],
            IsWeekendTaxFree = true,
            IsHolidayTaxFree = true,
            IsDayAfterHolidayTaxFree = true,
            IsDayBeforeHolidayTaxFree = true,
            SingleChargeIntervalMinutes = 60,
            MaxTaxPerDay = 60
        };
    }

    public TaxSettingBuilder WithCityName(string cityName)
    {
        _taxSetting.CityName = cityName;
        return this;
    }

    public TaxSettingBuilder WithActiveMonths(int[] activeMonths)
    {
        _taxSetting.ActiveMonths = [..activeMonths];
        return this;
    }

    public TaxSettingBuilder WithWeekendTaxFree(bool isWeekendTaxFree)
    {
        _taxSetting.IsWeekendTaxFree = isWeekendTaxFree;
        return this;
    }

    public TaxSettingBuilder WithHolidayTaxFree(bool isHolidayTaxFree)
    {
        _taxSetting.IsHolidayTaxFree = isHolidayTaxFree;
        return this;
    }

    public TaxSettingBuilder WithDayAfterHolidayTaxFree(bool isDayAfterHolidayTaxFree)
    {
        _taxSetting.IsDayAfterHolidayTaxFree = isDayAfterHolidayTaxFree;
        return this;
    }

    public TaxSettingBuilder WithDayBeforeHolidayTaxFree(bool isDayBeforeHolidayTaxFree)
    {
        _taxSetting.IsDayBeforeHolidayTaxFree = isDayBeforeHolidayTaxFree;
        return this;
    }

    public TaxSettingBuilder WithSingleChargeIntervalMinutes(int singleChargeIntervalMinutes)
    {
        _taxSetting.SingleChargeIntervalMinutes = singleChargeIntervalMinutes;
        return this;
    }

    public TaxSettingBuilder WithMaxTaxPerDay(int maxTaxPerDay)
    {
        _taxSetting.MaxTaxPerDay = maxTaxPerDay;
        return this;
    }
    
    public TaxSettingBuilder WithTaxRates(List<TaxRate> taxRates)
    {
        _taxSetting.TaxRates = [..taxRates];
        return this;
    }

    public TaxSetting Build()
    {
        return _taxSetting;
    }


}