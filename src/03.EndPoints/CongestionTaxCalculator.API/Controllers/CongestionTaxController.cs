using CongestionTaxCalculator.Services.Taxes.Contracts;
using CongestionTaxCalculator.Services.Taxes.Contracts.Dto;
using Microsoft.AspNetCore.Mvc;

namespace CongestionTaxCalculator.API.Controllers;

[ApiController]
[Route("api/v1/congestion-tax")]
public class CongestionTaxController : ControllerBase
{
    private readonly TaxService _taxService;

    public CongestionTaxController(TaxService taxService)
    {
        _taxService = taxService;
    }

    [HttpPost("calculate")]
    public async Task<decimal> CalculateTax([FromBody] CalculateTaxDto taxCalculationDto)
    {
        return await _taxService.CalculateTax(taxCalculationDto);
    }
}