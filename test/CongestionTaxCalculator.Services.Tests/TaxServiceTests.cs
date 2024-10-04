using CongestionTaxCalculator.Domain.Entities;
using CongestionTaxCalculator.Services.Taxes.Contracts;
using CongestionTaxCalculator.Services.Taxes.Contracts.Dto;
using CongestionTaxCalculator.TestTools.Holidays;
using CongestionTaxCalculator.TestTools.Infrastructure.DataBaseConfig.Unit;
using CongestionTaxCalculator.TestTools.Taxes;
using CongestionTaxCalculator.TestTools.TaxFreeVehicles;
using CongestionTaxCalculator.TestTools.TaxRates;
using CongestionTaxCalculator.TestTools.TaxSettings;
using FluentAssertions;

namespace CongestionTaxCalculator.Services.Tests;

public class TaxServiceTests : BusinessUnitTest
{
    private readonly TaxService _taxService;

    public TaxServiceTests()
    {
        _taxService = TaxServiceFactory.Create(SetupContext);
    }

    [Fact]
    public async Task CalculateTax_calculates_tax_properly_for_a_normal_vehicle_in_a_normal_day()
    {
        var taxSetting = new TaxSettingBuilder()
            .Build();
        Save(taxSetting);

        var taxRate = new TaxRateBuilder()
            .WithTaxSettingId(taxSetting.Id)
            .WithStart(new TimeOnly(00, 00, 00))
            .WithEnd(new TimeOnly(03, 00, 00))
            .Build();
        Save(taxRate);

        var dto = new CalculateTaxDto
        {
            VehicleType = VehicleType.Normal,
            PassedDateTimes = [new DateTime(2013, 1, 4, 00, 00, 00)],
            CityName = taxSetting.CityName
        };


        var tax = await _taxService.CalculateTax(dto);

        tax.Should().Be(taxRate.Amount);
    }

    [Theory]
    [InlineData(VehicleType.Emergency)]
    [InlineData(VehicleType.Buss)]
    [InlineData(VehicleType.Diplomat)]
    [InlineData(VehicleType.Foreign)]
    [InlineData(VehicleType.Military)]
    [InlineData(VehicleType.Motorcycle)]
    public async Task CalculateTax_returns_zero_for_tax_free_vehicle(VehicleType vehicleType)
    {
        var taxSetting = new TaxSettingBuilder()
            .Build();
        Save(taxSetting);

        var taxFreeVehicle = new TaxFreeVehicleBuilder()
            .WithTaxSettingId(taxSetting.Id)
            .WithVehicleType(vehicleType)
            .Build();
        Save(taxFreeVehicle);

        var dto = new CalculateTaxDto
        {
            VehicleType = VehicleType.Emergency,
            PassedDateTimes = [new DateTime(2013, 1, 4, 00, 00, 00)],
            CityName = taxSetting.CityName
        };

        var tax = await _taxService.CalculateTax(dto);

        tax.Should().Be(0);
    }

    [Fact]
    public async Task CalculateTax_applies_max_tax_per_day()
    {
        var taxSetting = new TaxSettingBuilder()
            .WithMaxTaxPerDay(60)
            .Build();
        Save(taxSetting);

        var taxRate = new TaxRateBuilder()
            .WithTaxSettingId(taxSetting.Id)
            .WithStart(new TimeOnly(00, 00, 00))
            .WithEnd(new TimeOnly(23, 59, 59))
            .WithAmount(30)
            .Build();
        Save(taxRate);

        var dto = new CalculateTaxDto
        {
            VehicleType = VehicleType.Normal,
            PassedDateTimes =
            [
                new DateTime(2013, 1, 4, 00, 00, 00),
                new DateTime(2013, 1, 4, 01, 00, 00),
                new DateTime(2013, 1, 4, 02, 00, 00)
            ],
            CityName = taxSetting.CityName
        };

        var tax = await _taxService.CalculateTax(dto);

        tax.Should().Be(60);
    }

    [Fact]
    public async Task CalculateTax_returns_zero_for_holiday()
    {
        var taxSetting = new TaxSettingBuilder()
            .WithHolidayTaxFree(true)
            .Build();
        Save(taxSetting);

        var holiday = new HolidayBuilder()
            .WithTaxSettingId(taxSetting.Id)
            .WithDate(new DateOnly(2013, 1, 1))
            .Build();
        Save(holiday);
        
        var taxRate = new TaxRateBuilder()
            .WithTaxSettingId(taxSetting.Id)
            .WithStart(new TimeOnly(00, 00, 00))
            .WithEnd(new TimeOnly(23, 59, 59))
            .WithAmount(30)
            .Build();
        Save(taxRate);

        var dto = new CalculateTaxDto
        {
            VehicleType = VehicleType.Normal,
            PassedDateTimes = [new DateTime(2013, 1, 1, 00, 00, 00)],
            CityName = taxSetting.CityName
        };

        var tax = await _taxService.CalculateTax(dto);

        tax.Should().Be(0);
    }

    [Fact]
    public async Task CalculateTax_returns_zero_for_weekend()
    {
        var taxSetting = new TaxSettingBuilder()
            .WithWeekendTaxFree(true)
            .Build();
        Save(taxSetting);
        
        var taxRate = new TaxRateBuilder()
            .WithTaxSettingId(taxSetting.Id)
            .WithStart(new TimeOnly(00, 00, 00))
            .WithEnd(new TimeOnly(23, 59, 59))
            .WithAmount(30)
            .Build();
        Save(taxRate);

        var dto = new CalculateTaxDto
        {
            VehicleType = VehicleType.Normal,
            PassedDateTimes = [new DateTime(2013, 1, 5, 00, 00, 00)],
            CityName = taxSetting.CityName
        };

        var tax = await _taxService.CalculateTax(dto);

        tax.Should().Be(0);
    }
    

    [Fact]
    public async Task CalculateTax_returns_zero_for_day_before_holiday()
    {
        var taxSetting = new TaxSettingBuilder()
            .WithDayBeforeHolidayTaxFree(true)
            .Build();
        Save(taxSetting);

        var holiday = new HolidayBuilder()
            .WithTaxSettingId(taxSetting.Id)
            .WithDate(new DateOnly(2013, 1, 1))
            .Build();
        Save(holiday);

        var dto = new CalculateTaxDto
        {
            VehicleType = VehicleType.Normal,
            PassedDateTimes = [new DateTime(2012, 12, 31, 10, 00, 00)],
            CityName = taxSetting.CityName
        };

        var tax = await _taxService.CalculateTax(dto);

        tax.Should().Be(0);
    }

    [Fact]
    public async Task CalculateTax_returns_zero_for_day_after_holiday()
    {
        var taxSetting = new TaxSettingBuilder()
            .WithDayAfterHolidayTaxFree(true)
            .Build();
        Save(taxSetting);

        var holiday = new HolidayBuilder()
            .WithTaxSettingId(taxSetting.Id)
            .WithDate(new DateOnly(2013, 1, 1))
            .Build();
        Save(holiday);

        var dto = new CalculateTaxDto
        {
            VehicleType = VehicleType.Normal,
            PassedDateTimes = [new DateTime(2013, 1, 2, 10, 00, 00)],
            CityName = taxSetting.CityName
        };

        var tax = await _taxService.CalculateTax(dto);

        tax.Should().Be(0);
    }

    [Fact]
    public async Task CalculateTax_calculates_tax_properly_across_multiple_days()
    {
        var taxSetting = new TaxSettingBuilder()
            .Build();
        Save(taxSetting);

        var taxRate = new TaxRateBuilder()
            .WithTaxSettingId(taxSetting.Id)
            .WithStart(new TimeOnly(00, 00, 00))
            .WithEnd(new TimeOnly(23, 59, 59))
            .WithAmount(20)
            .Build();
        Save(taxRate);

        var dto = new CalculateTaxDto
        {
            VehicleType = VehicleType.Normal,
            PassedDateTimes =
            [
                new DateTime(2013, 1, 4, 10, 00, 00),
                new DateTime(2013, 1, 7, 10, 00, 00),
                new DateTime(2013, 1, 8, 10, 00, 00)
            ],
            CityName = taxSetting.CityName
        };

        var tax = await _taxService.CalculateTax(dto);

        tax.Should().Be(60); 
    }

    [Fact]
    public async Task CalculateTax_calculates_single_interval_properly_within_window()
    {
        var taxSetting = new TaxSettingBuilder()
            .WithMaxTaxPerDay(60)
            .WithSingleChargeIntervalMinutes(60)
            .Build();
        Save(taxSetting);

        var taxRate = new TaxRateBuilder()
            .WithTaxSettingId(taxSetting.Id)
            .WithStart(new TimeOnly(00, 00, 00))
            .WithEnd(new TimeOnly(23, 59, 59))
            .WithAmount(20)
            .Build();
        Save(taxRate);

        var dto = new CalculateTaxDto
        {
            VehicleType = VehicleType.Normal,
            PassedDateTimes =
            [
                new DateTime(2013, 1, 4, 10, 00, 00),
                new DateTime(2013, 1, 4, 10, 30, 00),
                new DateTime(2013, 1, 4, 11, 00, 00)
            ],
            CityName = taxSetting.CityName
        };

        var tax = await _taxService.CalculateTax(dto);

        tax.Should().Be(20); 
    }
    
    [Fact]
public async Task CalculateTax_calculates_tax_for_multiple_days_with_single_interval()
{
    
    var taxRates = new List<TaxRate>
    {
        new TaxRateBuilder().WithStart(new TimeOnly(06, 00)).WithEnd(new TimeOnly(06, 29)).WithAmount(8).Build(),
        new TaxRateBuilder().WithStart(new TimeOnly(06, 30)).WithEnd(new TimeOnly(06, 59)).WithAmount(13).Build(),
        new TaxRateBuilder().WithStart(new TimeOnly(07, 00)).WithEnd(new TimeOnly(07, 59)).WithAmount(18).Build(),
        new TaxRateBuilder().WithStart(new TimeOnly(08, 00)).WithEnd(new TimeOnly(08, 29)).WithAmount(13).Build(),
        new TaxRateBuilder().WithStart(new TimeOnly(08, 30)).WithEnd(new TimeOnly(14, 59)).WithAmount(8).Build(),
        new TaxRateBuilder().WithStart(new TimeOnly(15, 00)).WithEnd(new TimeOnly(15, 29)).WithAmount(13).Build(),
        new TaxRateBuilder().WithStart(new TimeOnly(15, 30)).WithEnd(new TimeOnly(16, 59)).WithAmount(18).Build(),
        new TaxRateBuilder().WithStart(new TimeOnly(17, 00)).WithEnd(new TimeOnly(17, 59)).WithAmount(13).Build(),
        new TaxRateBuilder().WithStart(new TimeOnly(18, 00)).WithEnd(new TimeOnly(18, 29)).WithAmount(8).Build(),
        new TaxRateBuilder().WithStart(new TimeOnly(18, 30)).WithEnd(new TimeOnly(05, 59)).WithAmount(0).Build(),
    };
    var taxSetting = new TaxSettingBuilder()
        .WithMaxTaxPerDay(60)
        .WithSingleChargeIntervalMinutes(60)
        .WithTaxRates(taxRates)
        .Build();
    Save(taxSetting);

    var holiday = new HolidayBuilder()
        .WithTaxSettingId(taxSetting.Id)
        .WithDate(new DateOnly(2013, 3, 29))
        .Build();
    Save(holiday);
 

    var dto = new CalculateTaxDto
    {
        VehicleType = VehicleType.Normal,
        PassedDateTimes =
        [
            new DateTime(2013, 1, 14, 21, 00, 00),
            new DateTime(2013, 1, 15, 21, 00, 00),
            new DateTime(2013, 2, 7, 06, 23, 27),
            new DateTime(2013, 2, 7, 15, 27, 00),
            new DateTime(2013, 2, 8, 06, 27, 00),
            new DateTime(2013, 2, 8, 06, 20, 27),
            new DateTime(2013, 2, 8, 14, 35, 00),
            new DateTime(2013, 2, 8, 15, 29, 00),
            new DateTime(2013, 2, 8, 15, 47, 00),
            new DateTime(2013, 2, 8, 16, 01, 00),
            new DateTime(2013, 2, 8, 16, 48, 00),
            new DateTime(2013, 2, 8, 17, 49, 00),
            new DateTime(2013, 2, 8, 18, 29, 00),
            new DateTime(2013, 2, 8, 18, 35, 00),
            new DateTime(2013, 3, 26, 14, 25, 00),
            new DateTime(2013, 3, 28, 14, 07, 27)
        ],
        CityName = taxSetting.CityName
    };

    var tax = await _taxService.CalculateTax(dto);

    
    tax.Should().Be(89); 
}

}
