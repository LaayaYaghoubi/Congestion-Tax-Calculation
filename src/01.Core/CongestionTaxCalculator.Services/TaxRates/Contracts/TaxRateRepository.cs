using CongestionTaxCalculator.Contracts.Interfaces;
using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Services.TaxRates.Contracts;

public interface TaxRateRepository : Repository
{
    Task<List<TaxRate>> GetRates(long taxSettingId);
}