using System.Reflection;

namespace CongestionTaxCalculator.Persistence.EF.Infrastructure;

public interface IPersistenceRegister
{
    IPersistenceRegister WithEntityMapsAssembly(Assembly assembly);
    void Register();
}
