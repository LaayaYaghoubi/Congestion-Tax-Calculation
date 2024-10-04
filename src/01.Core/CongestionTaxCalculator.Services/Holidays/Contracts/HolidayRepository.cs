using CongestionTaxCalculator.Contracts.Interfaces;

namespace CongestionTaxCalculator.Services.Holidays.Contracts;

public interface HolidayRepository : Repository
{
    Task<List<DateOnly>> GetHolidaysForDates(long taxSettingId, DateOnly first, DateOnly last);
}