using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Services.Holidays.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Persistence.EF.Holidays;

public class EFHolidayRepository : HolidayRepository
{
    private readonly DbSet<Holiday> _holidays;

    public EFHolidayRepository(EFDataContext context)
    {
        _holidays = context.Set<Holiday>();
    }

    public async Task<List<DateOnly>> GetHolidaysForDates(long taxSettingId, DateOnly first, DateOnly last)
    {
        return await _holidays
            .Where(holiday =>
                holiday.TaxSettingId == taxSettingId
                && holiday.Date >= first.AddDays(-1)
                && holiday.Date <= last.AddDays(1))
            .Select(holiday => holiday.Date)
            .ToListAsync();
    }
}