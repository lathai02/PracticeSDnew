using NHibernate;
using RepositoriesUseNHibernate.Interfaces;
using Shares.Models;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesUseNHibernate.Implements
{
    public class ClassRepository : IClassRepository
    {
        private readonly ISession _session;

        public ClassRepository(ISession session)
        {
            _session = session;
        }

        public List<Class> GetAllClass()
        {
            return _session
                 .Query<Class>()
                 .Fetch(c => c.Teacher)
                 .ToList();
        }

        public Class? GetClassById(int id)
        {
            return _session
                .Query<Class>()
                .FirstOrDefault(c => c.Id == id);
        }
    }
}
