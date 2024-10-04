using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Services.TaxFreeVehicles.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Persistence.EF.TaxFreeVehicles;

public class EFTaxFreeVehicleRepository : TaxFreeVehicleRepository
{
    private readonly DbSet<TaxFreeVehicle> _taxFreeVehicles;

    public EFTaxFreeVehicleRepository(EFDataContext context)
    {
        _taxFreeVehicles = context.Set<TaxFreeVehicle>();
    }

    public async Task<bool> IsVehicleTaxFree(long taxSettingId, VehicleType vehicleType)
    {
        return await _taxFreeVehicles.AnyAsync(_ => _.TaxSettingId == taxSettingId && _.VehicleType == vehicleType);
    }
}