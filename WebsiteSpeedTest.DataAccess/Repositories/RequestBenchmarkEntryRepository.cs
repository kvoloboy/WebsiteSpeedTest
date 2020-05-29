using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RequestSpeedTest.Domain.Abstractions;
using RequestSpeedTest.Domain.Entities;
using WebsiteSpeedTest.DataAccess.Context;

namespace WebsiteSpeedTest.DataAccess.Repositories
{
    public class RequestBenchmarkEntryRepository : IRepository<RequestBenchmarkEntry>
    {
        private readonly DbSet<RequestBenchmarkEntry> _requestDbSet;

        public RequestBenchmarkEntryRepository(AppDbContext dbContext)
        {
            _requestDbSet = dbContext.Set<RequestBenchmarkEntry>();
        }

        public Task<RequestBenchmarkEntry> FindSingleAsync(Expression<Func<RequestBenchmarkEntry, bool>> predicate)
        {
            var request = _requestDbSet
                .Include(r => r.Endpoints)
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate);

            return request;
        }

        public async Task<List<RequestBenchmarkEntry>> FindAllAsync(Expression<Func<RequestBenchmarkEntry, bool>> predicate = null)
        {
            var requests = _requestDbSet
                .Include(request => request.Endpoints)
                .AsNoTracking();

            if (predicate != null)
            {
                requests = requests.Where(predicate);
            }

            return await requests.ToListAsync();
        }

        public async Task<int> AddAsync(RequestBenchmarkEntry entity)
        {
            _requestDbSet.Attach(entity);
            await _requestDbSet.AddAsync(entity);

            return entity.Id;
        }
    }
}
