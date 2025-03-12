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
    public class StudentRepository : GenericRepository<Student, string>, IStudentRepository
    {
        private readonly ISession _session;

        public StudentRepository(ISession session) : base(session)
        {
            _session = session;
        }

        public async Task<List<Student>> GetStudentListWithClassAsync()
        {
            return await _session.Query<Student>().Fetch(s => s.Class).ToListAsync();
        }

        public async Task<List<Student>> GetStudentListSortByNameAsync()
        {
            return await _session.Query<Student>().Fetch(s => s.Class).OrderBy(s => s.Name).ToListAsync();
        }
    }
}
