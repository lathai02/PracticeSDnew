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
    public class ClassRepository : GenericRepository<Class, int>, IClassRepository
    {
        private readonly ISession _session;

        public ClassRepository(ISession session) : base(session)
        {
            _session = session;
        }

        public async Task<List<Class>> GetAllClassWithTeacherAsync()
        {
            return await _session
                 .Query<Class>()
                 .Fetch(c => c.Teacher)
                 .ToListAsync();
        }
    }
}
