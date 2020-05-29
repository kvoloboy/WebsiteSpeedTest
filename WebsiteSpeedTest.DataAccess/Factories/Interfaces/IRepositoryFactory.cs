using RequestSpeedTest.Domain.Abstractions;
using RequestSpeedTest.Domain.Entities;

namespace WebsiteSpeedTest.DataAccess.Factories.Interfaces
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> Create<TEntity>() where TEntity : BaseEntity;
    }
}
