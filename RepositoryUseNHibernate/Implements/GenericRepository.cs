using NHibernate;
using NHibernate.Linq;
using RepositoriesUseNHibernate.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesUseNHibernate.Implements
{
    public class GenericRepository<T, TKey> : IGenericRepository<T, TKey> where T : class
    {
        private readonly ISession _session;

        public GenericRepository(ISession session)
        {
            _session = session;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _session.Query<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(TKey id)
        {
            return await _session.GetAsync<T>(id);
        }

        public async Task AddAsync(T entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                await _session.SaveAsync(entity);
                await transaction.CommitAsync();
            }

        }

        public async Task UpdateAsync(T entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                await _session.MergeAsync(entity);
                await transaction.CommitAsync();
            }

        }

        public async Task DeleteAsync(T entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                await _session.DeleteAsync(entity);
                await transaction.CommitAsync();
            }
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _session.Query<T>().CountAsync();
        }
    }
}
