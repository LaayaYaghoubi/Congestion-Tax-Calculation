using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Persistence.EF.SeedData.Contracts;

namespace CongestionTaxCalculator.Persistence.EF.SeedData;

public class SeedDataAppService : SeedDataService
{
    private readonly EFDataContext _dataContext;

    public SeedDataAppService(EFDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task Execute()
    {
        if (!_dataContext.Set<Holiday>().Any() && !_dataContext.Set<TaxRate>().Any() &&
            !_dataContext.Set<TaxSetting>().Any())
        {
            var taxSetting = new TaxSetting
            {
                CityName = "Gothenburg",
                MaxTaxPerDay = 60,
                IsHolidayTaxFree = true,
                IsWeekendTaxFree = true,
                SingleChargeIntervalMinutes = 60,
                ActiveMonths = [..new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }],
                IsDayBeforeHolidayTaxFree = true,
                IsDayAfterHolidayTaxFree = true
            };
            _dataContext.Set<TaxSetting>().Add(taxSetting);


            var holidays = new List<Holiday>
            {
                new()
                {
                    Date = new DateOnly(2013, 1, 1), TaxSetting = taxSetting
                },
                new() { Date = new DateOnly(2013, 3, 29), TaxSetting = taxSetting },
                new() { Date = new DateOnly(2013, 4, 1), TaxSetting = taxSetting },
                new() { Date = new DateOnly(2013, 5, 1), TaxSetting = taxSetting },
                new() { Date = new DateOnly(2013, 5, 9), TaxSetting = taxSetting },
                new() { Date = new DateOnly(2013, 6, 6), TaxSetting = taxSetting },
                new()
                {
                    Date = new DateOnly(2013, 12, 25), TaxSetting = taxSetting
                },
                new() { Date = new DateOnly(2013, 12, 26), TaxSetting = taxSetting }
            };
            _dataContext.Set<Holiday>().AddRange(holidays);


            var taxRates = new List<TaxRate>
            {
                new TaxRate
                {
                    Start = new TimeOnly(6, 0), End = new TimeOnly(6, 29), Amount = 8, TaxSetting = taxSetting
                },
                new TaxRate
                {
                    Start = new TimeOnly(6, 30), End = new TimeOnly(6, 59), Amount = 13, TaxSetting = taxSetting
                },
                new TaxRate
                {
                    Start = new TimeOnly(7, 0), End = new TimeOnly(7, 59), Amount = 18, TaxSetting = taxSetting
                },
                new TaxRate
                {
                    Start = new TimeOnly(8, 0), End = new TimeOnly(8, 29), Amount = 13, TaxSetting = taxSetting
                },
                new TaxRate
                {
                    Start = new TimeOnly(8, 30), End = new TimeOnly(14, 59), Amount = 8, TaxSetting = taxSetting
                },
                new TaxRate
                {
                    Start = new TimeOnly(15, 0), End = new TimeOnly(15, 29), Amount = 13, TaxSetting = taxSetting
                },
                new TaxRate
                {
                    Start = new TimeOnly(15, 30), End = new TimeOnly(16, 59), Amount = 18, TaxSetting = taxSetting
                },
                new TaxRate
                {
                    Start = new TimeOnly(17, 0), End = new TimeOnly(17, 59), Amount = 13, TaxSetting = taxSetting
                },
                new TaxRate
                {
                    Start = new TimeOnly(18, 0), End = new TimeOnly(18, 29), Amount = 8, TaxSetting = taxSetting
                },
                new TaxRate
                {
                    Start = new TimeOnly(18, 30), End = new TimeOnly(23, 59), Amount = 0, TaxSetting = taxSetting
                }
            };
            _dataContext.Set<TaxRate>().AddRange(taxRates);

            await _dataContext.SaveChangesAsync();
        }
    }
}