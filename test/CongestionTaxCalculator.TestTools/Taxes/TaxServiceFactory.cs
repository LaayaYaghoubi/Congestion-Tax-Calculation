
using CongestionTaxCalculator.Persistence.EF;
using CongestionTaxCalculator.Persistence.EF.Holidays;
using CongestionTaxCalculator.Persistence.EF.TaxFreeVehicles;
using CongestionTaxCalculator.Persistence.EF.TaxRates;
using CongestionTaxCalculator.Persistence.EF.TaxSettings;
using CongestionTaxCalculator.Services.Taxes;
using CongestionTaxCalculator.Services.Taxes.Contracts;

namespace CongestionTaxCalculator.TestTools.Taxes;

public static class TaxServiceFactory
{
    public static TaxService Create(EFDataContext context)
    {
        return new TaxAppService(
            new EFTaxSettingRepository(context),
            new EFTaxFreeVehicleRepository(context),
            new EFHolidayRepository(context),
            new EFTaxRateRepository(context)
            );
    }
}