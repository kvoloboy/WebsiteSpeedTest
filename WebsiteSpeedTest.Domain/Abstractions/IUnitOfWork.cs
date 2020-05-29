using System.Threading.Tasks;
using RequestSpeedTest.Domain.Entities;

namespace RequestSpeedTest.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity;
        Task CommitAsync();
    }
}
