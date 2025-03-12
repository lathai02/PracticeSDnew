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
            try
            {
                Console.WriteLine($"Session open: {_session.IsOpen}");
                var res = _session.Query<Student>().Fetch(s => s.Class);
                var res2 = await res.ToListAsync();
                Console.WriteLine($"After query, session open: {_session.IsOpen}");
                return res2;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return new List<Student>();
        }

        public async Task<List<Student>> GetStudentListSortByNameAsync()
        {
            return await _session.Query<Student>().Fetch(s => s.Class).OrderBy(s => s.Name).ToListAsync();
        }
    }
}
