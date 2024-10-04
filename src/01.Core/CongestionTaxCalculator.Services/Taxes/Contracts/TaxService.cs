using CongestionTaxCalculator.Contracts.Interfaces;
using CongestionTaxCalculator.Services.Taxes.Contracts.Dto;

namespace CongestionTaxCalculator.Services.Taxes.Contracts;

public interface TaxService : Service
{
    Task<decimal> CalculateTax(CalculateTaxDto dto);
}