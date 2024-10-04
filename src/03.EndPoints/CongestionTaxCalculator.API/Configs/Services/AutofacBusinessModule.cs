using Autofac;
using CongestionTaxCalculator.Contracts.Interfaces;
using CongestionTaxCalculator.Persistence.EF;
using CongestionTaxCalculator.Persistence.EF.Infrastructure;
using CongestionTaxCalculator.Services.Taxes.Contracts;

namespace CongestionTaxCalculator.API.Configs.Services;

public class AutofacBusinessModule : Module
{
    private readonly IPersistenceConfig _persistenceConfig;
    private const string ConnectionStringKey = "connectionString";

    public AutofacBusinessModule(IConfiguration configuration)
    {
        _persistenceConfig =
            TaxPersistence.BuildPersistenceConfig(configuration);
    }

    protected override void Load(ContainerBuilder container)
    {
        var serviceAssembly = typeof(TaxService).Assembly;
        var persistentAssembly = typeof(EFUnitOfWork).Assembly;


        container.RegisterAssemblyTypes(
                serviceAssembly,
                persistentAssembly)
            .AssignableTo<Service>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        container.RegisterAssemblyTypes(persistentAssembly, serviceAssembly)
            .AssignableTo<Repository>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();


        container.RegisterType<EFDataContext>()
            .WithParameter(
                ConnectionStringKey, _persistenceConfig.ConnectionString)
            .AsSelf()
            .InstancePerLifetimeScope();

        base.Load(container);
    }
}