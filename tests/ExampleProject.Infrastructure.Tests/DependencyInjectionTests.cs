using ExampleProject.Core.Interfaces;
using ExampleProject.Infrastructure.Persistence.Mongo;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ExampleProject.Infrastructure.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void AddInfrastructure_Returns_Same_ServiceCollection()
    {
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder().Build();
        var result = ExampleProject.Infrastructure.DependencyInjection.AddInfrastructure(services, config);
        Assert.Same(services, result);
    }

    [Fact]
    public void AddInfrastructure_WithPostgres_Registers_MeterReadingRepository_And_AlertRepository()
    {
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = "Host=localhost;Database=test",
                ["Mongo:ConnectionString"] = ""
            })
            .Build();
        ExampleProject.Infrastructure.DependencyInjection.AddInfrastructure(services, config);
        var provider = services.BuildServiceProvider();

        var meterRepo = provider.GetService<IMeterReadingRepository>();
        var alertRepo = provider.GetService<IAlertRepository>();

        Assert.NotNull(meterRepo);
        Assert.NotNull(alertRepo);
        Assert.IsType<ExampleProject.Infrastructure.Persistence.Repositories.MeterReadingRepository>(meterRepo);
        Assert.IsType<ExampleProject.Infrastructure.Persistence.Repositories.AlertRepository>(alertRepo);
    }

    [Fact]
    public void AddInfrastructure_WithoutPostgres_Registers_NullMeterReadingRepository_And_NullAlertRepository()
    {
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = "",
                ["Mongo:ConnectionString"] = ""
            })
            .Build();
        ExampleProject.Infrastructure.DependencyInjection.AddInfrastructure(services, config);
        var provider = services.BuildServiceProvider();

        var meterRepo = provider.GetService<IMeterReadingRepository>();
        var alertRepo = provider.GetService<IAlertRepository>();

        Assert.NotNull(meterRepo);
        Assert.NotNull(alertRepo);
        Assert.IsType<ExampleProject.Infrastructure.Persistence.Repositories.NullMeterReadingRepository>(meterRepo);
        Assert.IsType<ExampleProject.Infrastructure.Persistence.Repositories.NullAlertRepository>(alertRepo);
    }

    [Fact]
    public void AddInfrastructure_WithMongo_Registers_MarketSignalStore_And_DispatchLogStore()
    {
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = "",
                ["Mongo:ConnectionString"] = "mongodb://localhost:27017"
            })
            .Build();
        ExampleProject.Infrastructure.DependencyInjection.AddInfrastructure(services, config);
        var provider = services.BuildServiceProvider();

        var marketStore = provider.GetService<IMarketSignalStore>();
        var dispatchStore = provider.GetService<IDispatchLogStore>();

        Assert.NotNull(marketStore);
        Assert.NotNull(dispatchStore);
        Assert.IsType<MongoMarketSignalStore>(marketStore);
        Assert.IsType<MongoDispatchLogStore>(dispatchStore);
    }

    [Fact]
    public void AddInfrastructure_WithoutMongo_Registers_NullMarketSignalStore_And_NullDispatchLogStore()
    {
        var services = new ServiceCollection();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = "",
                ["Mongo:ConnectionString"] = ""
            })
            .Build();
        ExampleProject.Infrastructure.DependencyInjection.AddInfrastructure(services, config);
        var provider = services.BuildServiceProvider();

        var marketStore = provider.GetService<IMarketSignalStore>();
        var dispatchStore = provider.GetService<IDispatchLogStore>();

        Assert.NotNull(marketStore);
        Assert.NotNull(dispatchStore);
        Assert.IsType<NullMarketSignalStore>(marketStore);
        Assert.IsType<NullDispatchLogStore>(dispatchStore);
    }
}
