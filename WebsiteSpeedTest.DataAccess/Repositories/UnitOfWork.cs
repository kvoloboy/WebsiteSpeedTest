using System.Threading.Tasks;
using RequestSpeedTest.Domain.Abstractions;
using RequestSpeedTest.Domain.Entities;
using WebsiteSpeedTest.DataAccess.Context;
using WebsiteSpeedTest.DataAccess.Factories.Interfaces;

namespace WebsiteSpeedTest.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private readonly IRepositoryFactory _repositoryFactory;

        public UnitOfWork(AppDbContext dbContext, IRepositoryFactory repositoryFactory)
        {
            _dbContext = dbContext;
            _repositoryFactory = repositoryFactory;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity
        {
            var repository = _repositoryFactory.Create<TEntity>();

            return repository;
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
