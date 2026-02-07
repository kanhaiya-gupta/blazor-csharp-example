using ExampleProject.Core.Interfaces;
using ExampleProject.Infrastructure.Persistence;
using ExampleProject.Infrastructure.Persistence.Mongo;
using ExampleProject.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExampleProject.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(connectionString));
                services.AddScoped<IFlexibilityOfferRepository, FlexibilityOfferRepository>();
                services.AddScoped<IPlantRepository, PlantRepository>();
                services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();
                services.AddScoped<IAlertRepository, AlertRepository>();
            }
            else
            {
                services.AddScoped<IFlexibilityOfferRepository, NullFlexibilityOfferRepository>();
                services.AddScoped<IPlantRepository, NullPlantRepository>();
                services.AddScoped<IMeterReadingRepository, NullMeterReadingRepository>();
                services.AddScoped<IAlertRepository, NullAlertRepository>();
            }

            var mongoConnectionString = configuration[$"{MongoOptions.SectionName}:ConnectionString"];
            if (!string.IsNullOrWhiteSpace(mongoConnectionString))
            {
                services.Configure<MongoOptions>(options =>
                {
                    options.ConnectionString = mongoConnectionString;
                    options.DatabaseName = configuration[$"{MongoOptions.SectionName}:DatabaseName"] ?? "exampleproject";
                    options.AuditCollectionName = configuration[$"{MongoOptions.SectionName}:AuditCollectionName"] ?? "audit";
                    options.MarketSignalsCollectionName = configuration[$"{MongoOptions.SectionName}:MarketSignalsCollectionName"] ?? "market_signals";
                    options.DispatchLogCollectionName = configuration[$"{MongoOptions.SectionName}:DispatchLogCollectionName"] ?? "dispatch_log";
                });
                services.AddSingleton<IAuditLogStore, MongoAuditLogStore>();
                services.AddSingleton<IMarketSignalStore, MongoMarketSignalStore>();
                services.AddSingleton<IDispatchLogStore, MongoDispatchLogStore>();
            }
            else
            {
                services.AddSingleton<IAuditLogStore, NullAuditLogStore>();
                services.AddSingleton<IMarketSignalStore, NullMarketSignalStore>();
                services.AddSingleton<IDispatchLogStore, NullDispatchLogStore>();
            }

            return services;
        }
    }
}
