using CongestionTaxCalculator.Contracts.Interfaces;
using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Services.TaxSettings.Contracts;

public interface TaxSettingRepository : Repository
{
    Task<TaxSetting?> GetByCityName(string cityName);
}