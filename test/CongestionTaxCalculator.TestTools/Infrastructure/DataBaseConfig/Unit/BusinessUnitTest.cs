using CongestionTaxCalculator.Persistence.EF;

namespace CongestionTaxCalculator.TestTools.Infrastructure.DataBaseConfig.Unit;

public class BusinessUnitTest
{
    protected EFDataContext DbContext { get; set; }
    protected EFDataContext SetupContext { get; set; }
    protected EFDataContext ReadContext { get; set; }
    protected string TenantId { get; } = "TenantId";


    protected BusinessUnitTest(string? tenantId = null)
    {
        var db = CreateDatabase();

      

        DbContext = db.CreateDataContext<EFDataContext>();
        SetupContext = db.CreateDataContext
            <EFDataContext>();
        ReadContext = db.CreateDataContext<EFDataContext>();
    }

    protected EFInMemoryDatabase CreateDatabase()
    {
        return new EFInMemoryDatabase();
    }
    protected void Save<T>(T entity)
    {
        if (entity != null)
        {
            DbContext.Manipulate(_ => _.Add(entity));
        }
    }

    public void Save<T>(params T[] entities)
    {
        foreach (var item in entities)
            DbContext.Manipulate(_ => _.Add(item!));
    }

    public void Save()
    {
        DbContext.SaveChanges();
    }
}