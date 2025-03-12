using Microsoft.IdentityModel.Tokens;
using RepositoriesUseNHibernate.Interfaces;
using Services.Interfaces;
using Shares.Constants;
using Shares.Models;
using Shares.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implements
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IClassService _classService;

        public StudentService(IStudentRepository studentRepository, IClassService classService)
        {
            _studentRepository = studentRepository;
            _classService = classService;
        }

        public void AddStudent()
        {
            var studentId = StringUtils.InputString("Enter student id:", AppConstants.StudentIdPattern);

            var student = GetStudentInfo(studentId);
            _studentRepository.Add(student);
            Console.WriteLine("Student added successfully.");
        }

        public void UpdateStudent()
        {
            var studentId = StringUtils.InputString("Enter student id to update:", AppConstants.StudentIdPattern);
            var student = _studentRepository.GetById(studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found!");
                return;
            }

            var studentUpdated = GetStudentInfo(studentId);
            _studentRepository.Update(studentUpdated);
            Console.WriteLine("Student updated successfully.");
        }

        public void DeleteStudent()
        {
            var student = FindStudentById();
            if (student == null)
            {
                return;
            }

            _studentRepository.Delete(student);
            Console.WriteLine("Student deleted successfully.");
        }

        public void SearchByStudentId()
        {
            var student = FindStudentById();
            if (student == null)
            {
                return;
            }

            Console.WriteLine(student.ToString());
        }

        public void PrintStudentList()
        {
            var students = _studentRepository.GetStudentListWithClass();
            StringUtils.PrintList(students, "Student List");
        }

        public void SortStudentListByName()
        {
            StringUtils.PrintList(_studentRepository.GetStudentListSortByName(), "Student List");
        }

        private Student GetStudentInfo(string studentId = "")
        {
            var studentName = StringUtils.InputString("Enter student name:");
            var studentAddress = StringUtils.InputString("Enter student address:");
            var studentDob = DateTimeUtils.InputDateTime($"Enter student dob ({AppConstants.DateFormat}): ");

            var classes = _classService.GetAllClassWithTeacher();
            StringUtils.PrintList(classes, "Class List");
            var classId = NumberUtils.InputIntegerNumber("Enter class id: ", 1, classes.Count);

            return new Student
            {
                Id = studentId,
                Name = studentName,
                Address = studentAddress,
                DateOfBirth = studentDob,
                Class = _classService.GetClassById(classId)
            };
        }

        private Student? FindStudentById()
        {
            var studentId = StringUtils.InputString("Enter student id:", AppConstants.StudentIdPattern);
            var student = _studentRepository.GetById(studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found!");
            }

            return student;
        }
    }
}
