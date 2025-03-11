using Microsoft.EntityFrameworkCore;
using Repositories.Interfaces;
using Shares.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Implements
{
    public class StudentRepository : IStudentRepository
    {
        private readonly SchoolDbContext _schoolDbContext;

        public StudentRepository(SchoolDbContext schoolDbContext)
        {
            _schoolDbContext = schoolDbContext;
        }

        public void AddStudent(Student student)
        {
            _schoolDbContext.Students.Add(student);
            _schoolDbContext.SaveChanges();
        }

        public void DeleteStudent(Student student)
        {
            _schoolDbContext.Students.Remove(student);
            _schoolDbContext.SaveChanges();
        }

        public Student? GetStudentById(string studentId)
        {
            return _schoolDbContext.Students.FirstOrDefault(s => s.Id == studentId);
        }

        public List<Student> GetStudentList()
        {
            return _schoolDbContext.Students.Include(s => s.Class).ToList();
        }

        public List<Student> GetStudentListSortByName()
        {
            return _schoolDbContext.Students.Include(s => s.Class).OrderBy(s => s.Name).ToList();
        }

        public void UpdateStudent(Student studentUpdated, Student oldStudent)
        {
            _schoolDbContext.Entry(oldStudent).State = EntityState.Detached;
            _schoolDbContext.Students.Update(studentUpdated);
            _schoolDbContext.SaveChanges();
        }
    }
}
