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
    public class StudentRepository : IStudentRepository
    {
        private readonly ISession _session;

        public StudentRepository(ISession session)
        {
            _session = session;
        }
        public void AddStudent(Student student)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(student);
                transaction.Commit();
            }
        }

        public void DeleteStudent(Student student)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Delete(student);
                transaction.Commit();
            }
        }

        public Student? GetStudentById(string studentId)
        {
            return _session.Query<Student>().FirstOrDefault(s => s.Id == studentId);
        }

        public List<Student> GetStudentList()
        {
            return _session.Query<Student>().Fetch(s => s.Class).ToList();
        }

        public List<Student> GetStudentListSortByName()
        {
            return _session.Query<Student>().Fetch(s => s.Class).OrderBy(s => s.Name).ToList();
        }

        public void UpdateStudent(Student studentUpdated, Student oldStudent)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Merge(studentUpdated); 
                transaction.Commit();
            }
        }
    }
}
