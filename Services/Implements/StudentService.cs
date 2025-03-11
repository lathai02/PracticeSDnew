using Microsoft.IdentityModel.Tokens;
using Repositories.Interfaces;
using Services.Interfaces;
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
        private readonly IClassRepository _classRepository;
        public StudentService(IStudentRepository studentRepository, IClassRepository classRepository)
        {
            _studentRepository = studentRepository;
            _classRepository = classRepository;
        }

        public void AddStudent()
        {
            var studentId = StringUtils.InputString("Enter student id:", "^[A-Za-z]{2}\\d{6}$");

            var studentName = StringUtils.InputString("Enter student name:");
            var studentAddress = StringUtils.InputString("Enter student address:");
            var studentDob = DateTimeUtils.InputDateOnly("Enter student dob (dd/MM/yyyy): ", "dd/MM/yyyy");

            var classes = _classRepository.GetAllClass();
            StringUtils.PrintList(classes, "Class List");
            var classId = NumberUtils.InputIntegerNumber("Enter class id: ", 1, classes.Count - 1);
            var student = new Student
            {
                Id = studentId,
                Name = studentName,
                Address = studentAddress,
                DateOfBirth = studentDob,
                ClassId = classId
            };

            _studentRepository.AddStudent(student);
        }

        public void DeleteStudent()
        {
            var studentId = StringUtils.InputString("Enter student id to delete:", "^[A-Za-z]{2}\\d{6}$");

            var student = _studentRepository.GetStudentById(studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found!");
                return;
            }

            _studentRepository.DeleteStudent(student);
        }

        public void PrintStudentList()
        {
            _studentRepository.GetStudentList();
            StringUtils.PrintList(_studentRepository.GetStudentList(), "Student List");
        }

        public void SearchByStudentId()
        {
            var studentId = StringUtils.InputString("Enter student id you want to find:", "^[A-Za-z]{2}\\d{6}$");

            var student = _studentRepository.GetStudentById(studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found!");
                return;
            }
            Console.WriteLine(student.ToString());
        }

        public void SortStudentListByName()
        {
            StringUtils.PrintList(_studentRepository.GetStudentListSortByName(), "Student List");
        }

        public void UpdateStudent()
        {
            var studentId = StringUtils.InputString("Enter student id to update:", "^[A-Za-z]{2}\\d{6}$");
            var student = _studentRepository.GetStudentById(studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found!");
                return;
            }

            var studentName = StringUtils.InputString("Enter student name:");
            var studentAddress = StringUtils.InputString("Enter student address:");
            var studentDob = DateTimeUtils.InputDateOnly("Enter student dob (dd/MM/yyyy): ", "dd/MM/yyyy");

            var classes = _classRepository.GetAllClass();
            StringUtils.PrintList(classes, "Class List");
            var classId = NumberUtils.InputIntegerNumber("Enter class id: ", 1, classes.Count);
            var studentUpdated = new Student
            {
                Id = studentId,
                Name = studentName,
                Address = studentAddress,
                DateOfBirth = studentDob,
                ClassId = classId
            };

            _studentRepository.UpdateStudent(studentUpdated, student);
        }
    }
}
