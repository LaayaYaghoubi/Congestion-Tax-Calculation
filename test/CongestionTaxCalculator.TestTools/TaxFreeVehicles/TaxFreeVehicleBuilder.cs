using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.TestTools.TaxFreeVehicles;

public class TaxFreeVehicleBuilder
{
    private readonly TaxFreeVehicle _taxFreeVehicle;

    public TaxFreeVehicleBuilder()
    {
        _taxFreeVehicle = new TaxFreeVehicle
        {
            VehicleType = VehicleType.Emergency
        };
    }

    public TaxFreeVehicleBuilder WithTaxSettingId(long taxSettingId)
    {
        _taxFreeVehicle.TaxSettingId = taxSettingId;
        return this;
    }
    
    public TaxFreeVehicleBuilder WithVehicleType(VehicleType vehicleType)
    {
        _taxFreeVehicle.VehicleType = vehicleType;
        return this;
    }

    public TaxFreeVehicle Build()
    {
        return _taxFreeVehicle;
    }

 
}