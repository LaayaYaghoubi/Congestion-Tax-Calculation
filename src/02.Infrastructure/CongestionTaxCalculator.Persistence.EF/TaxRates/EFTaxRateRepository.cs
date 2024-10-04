using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Services.TaxRates.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Persistence.EF.TaxRates;

public class EFTaxRateRepository : TaxRateRepository
{
    private readonly DbSet<TaxRate> _taxRates;

    public EFTaxRateRepository(EFDataContext context)
    {
        _taxRates = context.Set<TaxRate>();
    }

    public async Task<List<TaxRate>> GetRates(long taxSettingId)
    {
        return await _taxRates
            .Where(rate => rate.TaxSettingId == taxSettingId)
            .ToListAsync();
    }
}