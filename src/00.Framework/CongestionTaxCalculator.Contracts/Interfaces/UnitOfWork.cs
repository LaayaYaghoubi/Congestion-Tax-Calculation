namespace CongestionTaxCalculator.Contracts.Interfaces;

public interface UnitOfWork : Service
{
    Task Begin();
    Task Commit();
    Task Rollback();
    Task CommitPartial();
    Task Complete();
}