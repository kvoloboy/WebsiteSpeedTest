using Autofac;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using RequestSpeedTest.Domain.Abstractions;
using RequestSpeedTest.Domain.Entities;
using WebsiteSpeedTest.DataAccess.Factories;
using WebsiteSpeedTest.DataAccess.Factories.Interfaces;
using WebsiteSpeedTest.DataAccess.Repositories;

namespace WebsiteSpeedTest.DataAccess
{
    public class DataAccessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                {
                    const string configSegmentName = "DatabaseOptions";
                    const string connectionStringSegment = "ConnectionString";

                    var configuration = c.Resolve<IConfiguration>();
                    var connectionString = configuration[$"{configSegmentName}:{connectionStringSegment}"];
                    var client = new MongoClient(connectionString);

                    return client;
                }).As<IMongoClient>()
                .SingleInstance();

            builder.RegisterType<DatabaseOptions>()
                .As<IDatabaseOptions>()
                .SingleInstance();

            builder.RegisterType<RepositoryFactory>()
                .As<IRepositoryFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterType<UnitOfWork>()
                .As<IUnitOfWork>()
                .InstancePerLifetimeScope();

            builder.RegisterType<RequestBenchmarkEntryRepository>()
                .As<IRepository<RequestBenchmarkEntry>>()
                .InstancePerLifetimeScope();
        }
    }
}
