using NHibernate;
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

        public List<T> GetAll()
        {
            return _session.Query<T>().ToList();
        }

        public T? GetById(TKey id)
        {
            return _session.Get<T>(id);
        }

        public void Add(T entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(entity);
                transaction.Commit();
            }
        }

        public void Update(T entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Merge(entity);
                transaction.Commit();
            }
        }

        public void Delete(T entity)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Delete(entity);
                transaction.Commit();
            }
        }
    }
}
