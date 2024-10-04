using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Services.Holidays.Contracts;
using CongestionTaxCalculator.Services.Taxes.Contracts;
using CongestionTaxCalculator.Services.Taxes.Contracts.Dto;
using CongestionTaxCalculator.Services.Taxes.Exceptions;
using CongestionTaxCalculator.Services.TaxFreeVehicles.Contracts;
using CongestionTaxCalculator.Services.TaxRates.Contracts;
using CongestionTaxCalculator.Services.TaxSettings.Contracts;

namespace CongestionTaxCalculator.Services.Taxes;

public class TaxAppService : TaxService
{
    private readonly TaxSettingRepository _taxSettingRepository;
    private readonly TaxFreeVehicleRepository _taxFreeVehicleRepository;
    private readonly HolidayRepository _holidayRepository;
    private readonly TaxRateRepository _taxRateRepository;

    public TaxAppService(
        TaxSettingRepository taxSettingRepository,
        TaxFreeVehicleRepository taxFreeVehicleRepository,
        HolidayRepository holidayRepository,
        TaxRateRepository taxRateRepository)
    {
        _taxSettingRepository = taxSettingRepository;
        _taxFreeVehicleRepository = taxFreeVehicleRepository;
        _holidayRepository = holidayRepository;
        _taxRateRepository = taxRateRepository;
    }

    public async Task<decimal> CalculateTax(CalculateTaxDto dto)
    {
        decimal totalTax = 0;

        var taxSetting = await _taxSettingRepository.GetByCityName(dto.CityName);
        ThrowIfTaxSettingDoesNotExist(taxSetting);

        if (await IsVehicleTaxFree(dto, taxSetting))
        {
            return totalTax;
        }

        var holidaysWithinDates = await HolidaysWithinDates(dto, taxSetting);

        var groupedPassedDates = dto.PassedDateTimes
            .OrderBy(date => date)
            .GroupBy(date => date.Date)
            .ToList();

        var taxRates = await _taxRateRepository.GetRates(taxSetting.Id);

        foreach (var group in groupedPassedDates)
        {
            decimal currentDayTax = 0;
            var passedDate = DateOnly.FromDateTime(group.Key);

            var isDayTaxFree = IsDayTaxFree(
                holidaysWithinDates,
                passedDate,
                taxSetting);

            if (isDayTaxFree)
            {
                continue;
            }

            if (IsSingleChargeIntervalMinutesDefined(taxSetting))
            {
                var datesList = group.ToList();
                decimal maxFeeInWindow = 0;
                DateTime windowStartTime = datesList.First();

                for (var i = 0; i < datesList.Count; i++)
                {
                    var currentPassedTime = datesList[i];
                    var currentFee = GetCurrentFee(taxRates, TimeOnly.FromDateTime(currentPassedTime));

                    if ((currentPassedTime - windowStartTime).TotalMinutes > taxSetting.SingleChargeIntervalMinutes)
                    {
                        currentDayTax += maxFeeInWindow;

                        windowStartTime = currentPassedTime;
                        maxFeeInWindow = currentFee;
                    }
                    else
                    {
                        maxFeeInWindow = decimal.Max(maxFeeInWindow, currentFee);
                    }

                    if (i == datesList.Count - 1)
                    {
                        currentDayTax += maxFeeInWindow;
                    }
                }

                totalTax += currentDayTax < taxSetting.MaxTaxPerDay
                    ? currentDayTax
                    : taxSetting.MaxTaxPerDay;
            }
            else
            {
                foreach (var time in group)
                {
                    currentDayTax += GetCurrentFee(taxRates, TimeOnly.FromDateTime(time));
                }
            }
        }


        return totalTax;
    }

    private static decimal GetCurrentFee(IEnumerable<TaxRate> taxRates, TimeOnly passedTime)
    {
        return taxRates
            .Where(taxRate => taxRate.Start <= passedTime && taxRate.End >= passedTime)
            .Select(taxRate => taxRate.Amount)
            .FirstOrDefault();
    }

    private static bool IsSingleChargeIntervalMinutesDefined(TaxSetting? cityTaxSetting)
    {
        return cityTaxSetting.SingleChargeIntervalMinutes > 0;
    }

    private static void ThrowIfTaxSettingDoesNotExist(TaxSetting? cityTaxSetting)
    {
        if (cityTaxSetting == null)
        {
            throw new CityToCalculateTaxDoesNotExistException();
        }
    }

    private async Task<List<DateOnly>> HolidaysWithinDates(CalculateTaxDto dto, TaxSetting? cityTaxSetting)
    {
        var holidaysWithinDates = new List<DateOnly>();
        var dates = dto.PassedDateTimes.OrderBy(dateTime => dateTime.Date).ToList().ConvertAll(DateOnly.FromDateTime);
        if (cityTaxSetting.IsHolidayTaxFree)
        {
            holidaysWithinDates =
                await _holidayRepository.GetHolidaysForDates(
                    cityTaxSetting.Id,
                    dates.First(),
                    dates.Last());
        }

        return holidaysWithinDates;
    }

    private async Task<bool> IsVehicleTaxFree(CalculateTaxDto dto, TaxSetting? cityTaxSetting)
    {
        return await _taxFreeVehicleRepository.IsVehicleTaxFree(cityTaxSetting.Id, dto.VehicleType);
    }

    private static bool IsDayTaxFree(
        List<DateOnly> holidaysWithinDates,
        DateOnly passedDate,
        TaxSetting? cityTaxSetting)
    {
        var isHoliday = holidaysWithinDates.Contains(passedDate) && cityTaxSetting.IsHolidayTaxFree;

        var isWeekend = cityTaxSetting.IsHolidayTaxFree &&
                        passedDate.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;

        var isDayAfterHoliday = cityTaxSetting.IsDayAfterHolidayTaxFree &&
                                holidaysWithinDates.Contains(passedDate.AddDays(-1));

        var isDayBeforeHoliday = cityTaxSetting.IsDayBeforeHolidayTaxFree &&
                                 holidaysWithinDates.Contains(passedDate.AddDays(1));

        var isMonthActive = cityTaxSetting.ActiveMonths.Contains(passedDate.Month);

        return isHoliday || isWeekend || isDayAfterHoliday || isDayBeforeHoliday || (!isMonthActive);
    }
}