using System.Reflection;
using Autofac;
using Autofac.Builder;
using CongestionTaxCalculator.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.Persistence.EF.Infrastructure;

public class PersistenceRegister : IPersistenceRegister
{
    private readonly ContainerBuilder _container;
    private IRegistrationBuilder<
        EFDataContext,
        ConcreteReflectionActivatorData,
        SingleRegistrationStyle> _efDataContextRegisterer;

    public PersistenceRegister(
        IPersistenceConfig config,
        ContainerBuilder container)
    {
        _container = container;
        _efDataContextRegisterer = _container.RegisterType<EFDataContext>()
            .WithParameter(
                "options",
                new DbContextOptionsBuilder<EFDataContext>()
                .UseSqlServer(config.ConnectionString)
                .Options);
    }

    public IPersistenceRegister WithEntityMapsAssembly(Assembly assembly)
    {
        _efDataContextRegisterer = _efDataContextRegisterer.WithParameter(
            "entityMapsAssembly", assembly);
        return this;
    }

    public void Register()
    {
        _efDataContextRegisterer
            .AsSelf().InstancePerLifetimeScope();

        _container.RegisterType<EFUnitOfWork>()
            .As<UnitOfWork>()
            .InstancePerLifetimeScope();
    }
}