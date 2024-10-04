

using CongestionTaxCalculator.Contracts.Interfaces;

namespace CongestionTaxCalculator.Persistence.EF.SeedData.Contracts;

public interface SeedDataService : Service
{
    Task Execute();
}