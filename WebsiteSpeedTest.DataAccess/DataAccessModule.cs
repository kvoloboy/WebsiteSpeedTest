using Autofac;
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
