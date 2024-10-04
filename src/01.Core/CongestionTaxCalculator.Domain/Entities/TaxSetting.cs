namespace CongestionTaxCalculator.Domain.Entities;

public class TaxSetting
{
    public long Id { get; set; }
    public string CityName { get; set; } = null!;

    public decimal MaxTaxPerDay { get; set; }

    public int SingleChargeIntervalMinutes { get; set; }

    public bool IsHolidayTaxFree { get; set; }

    public bool IsDayBeforeHolidayTaxFree { get; set; }
    public bool IsDayAfterHolidayTaxFree { get; set; }

    public bool IsWeekendTaxFree { get; set; }

    public HashSet<int> ActiveMonths { get; set; } = [];

    public HashSet<TaxRate> TaxRates { get; } = [];

    public ICollection<Holiday> Holidays { get; } = [];

    public ICollection<TaxFreeVehicle> TaxFreeVehicles { get; } = [];
}