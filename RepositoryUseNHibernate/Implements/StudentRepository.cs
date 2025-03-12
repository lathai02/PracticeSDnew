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

        public List<Student> GetStudentList()
        {
            return _session.Query<Student>().Fetch(s => s.Class).ToList();
        }

        public List<Student> GetStudentListSortByName()
        {
            return _session.Query<Student>().Fetch(s => s.Class).OrderBy(s => s.Name).ToList();
        }
    }
}
