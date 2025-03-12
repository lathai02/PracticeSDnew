using Shares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesUseNHibernate.Interfaces
{
    public interface IStudentRepository
    {
        List<Student> GetStudentList();
        void AddStudent(Student student);
        Student? GetStudentById(string studentId);
        void UpdateStudent(Student studentUpdated, Student oldStudent);
        void DeleteStudent(Student student);
        List<Student> GetStudentListSortByName();
    }
}
