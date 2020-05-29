using Autofac;
using RequestSpeedTest.Domain.Abstractions;
using RequestSpeedTest.Domain.Entities;
using WebsiteSpeedTest.DataAccess.Factories.Interfaces;

namespace WebsiteSpeedTest.DataAccess.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly ILifetimeScope _lifeTimeScope;

        public RepositoryFactory(ILifetimeScope lifeTimeScope)
        {
            _lifeTimeScope = lifeTimeScope;
        }

        public IRepository<TEntity> Create<TEntity>() where TEntity : BaseEntity
        {
            var repository = _lifeTimeScope.Resolve<IRepository<TEntity>>();

            return repository;
        }
    }
}
