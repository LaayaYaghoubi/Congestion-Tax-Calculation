using CongestionTaxCalculator.Domain.Entities;

namespace CongestionTaxCalculator.Services.Taxes.Contracts.Dto;

public class CalculateTaxDto
{
    public string CityName { get; set; } = null!;
    public List<DateTime> PassedDateTimes { get; set; } = [];
    public VehicleType VehicleType { get; set; }
}