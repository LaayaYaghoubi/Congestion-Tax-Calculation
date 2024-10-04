using CongestionTaxCalculator.Contracts.Interfaces;
using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Services.TaxFreeVehicles.Contracts;

public interface TaxFreeVehicleRepository : Repository
{
    Task<bool> IsVehicleTaxFree(long taxSettingId, VehicleType vehicleType);
}