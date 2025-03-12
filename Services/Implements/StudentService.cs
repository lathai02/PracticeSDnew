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

        public async Task AddStudentAsync()
        {
            var studentId = StringUtils.InputString("Enter student id:", AppConstants.StudentIdPattern);

            var student = await GetStudentInfo(studentId);
            await _studentRepository.AddAsync(student);
            Console.WriteLine("Student added successfully.");
        }

        public async Task UpdateStudentAsync()
        {
            var studentId = StringUtils.InputString("Enter student id to update:", AppConstants.StudentIdPattern);
            var student = await _studentRepository.GetByIdAsync(studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found!");
                return;
            }

            var studentUpdated = await GetStudentInfo(studentId);
            await _studentRepository.UpdateAsync(studentUpdated);
            Console.WriteLine("Student updated successfully.");
        }

        public async Task DeleteStudentAsync()
        {
            var student = await FindStudentById();
            if (student == null)
            {
                return;
            }

            await _studentRepository.DeleteAsync(student);
            Console.WriteLine("Student deleted successfully.");
        }

        public async Task SearchByStudentIdAsync()
        {
            var student = await FindStudentById();
            if (student == null)
            {
                return;
            }

            Console.WriteLine(student.ToString());
        }

        public async Task PrintStudentListAsync()
        {
            var students = await _studentRepository.GetStudentListWithClassAsync();
            StringUtils.PrintList(students, "Student List");
        }

        public async Task SortStudentListByNameAsync()
        {
            StringUtils.PrintList(await _studentRepository.GetStudentListSortByNameAsync(), "Student List");
        }

        private async Task<Student> GetStudentInfo(string studentId = "")
        {
            var studentName = StringUtils.InputString("Enter student name:");
            var studentAddress = StringUtils.InputString("Enter student address:");
            var studentDob = DateTimeUtils.InputDateTime($"Enter student dob ({AppConstants.DateFormat}): ");

            var classes = await _classService.GetAllClassWithTeacherAsync();
            StringUtils.PrintList(classes, "Class List");
            var classId = NumberUtils.InputIntegerNumber("Enter class id: ", 1, classes.Count);

            return new Student
            {
                Id = studentId,
                Name = studentName,
                Address = studentAddress,
                DateOfBirth = studentDob,
                Class = await _classService.GetClassByIdAsync(classId)
            };
        }

        private async Task<Student?> FindStudentById()
        {
            var studentId = StringUtils.InputString("Enter student id:", AppConstants.StudentIdPattern);
            var student = await _studentRepository.GetByIdAsync(studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found!");
            }

            return student;
        }
    }
}
