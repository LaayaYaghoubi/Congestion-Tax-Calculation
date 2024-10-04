using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Services.TaxSettings.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Persistence.EF.TaxSettings;

public class EFTaxSettingRepository : TaxSettingRepository
{
    private readonly DbSet<TaxSetting> _taxSettings;

    public EFTaxSettingRepository(EFDataContext context)
    {
        _taxSettings = context.Set<TaxSetting>();
    }

    public async Task<TaxSetting?> GetByCityName(string cityName)
    {
        return await _taxSettings.FirstOrDefaultAsync(setting => setting.CityName.ToLower() == cityName.ToLower());
    }
}