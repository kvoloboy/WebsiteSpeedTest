using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RequestSpeedTest.Domain.Entities;

namespace RequestSpeedTest.Domain.Abstractions
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> predicate);
        Task<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task AddAsync(TEntity entity);
    }
}
