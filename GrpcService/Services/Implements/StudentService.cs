using GrpcService.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using RepositoriesUseNHibernate.Interfaces;
using Shares.Constants;
using Shares.Models;
using Shares.ServiceContracts;
using Shares.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcService.Services.Implements
{
    public class StudentService : IStudentProto
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IClassService _classService;

        public StudentService(IStudentRepository studentRepository, IClassService classService)
        {
            _studentRepository = studentRepository;
            _classService = classService;
        }

        public async Task<Empty> AddStudentAsync(Empty request, CallContext? context = null)
        {
            var studentId = StringUtils.InputString("Enter student id:", AppConstants.STUDENT_ID_PARTERN);

            var student = await GetStudentInfo(studentId);
            await _studentRepository.AddAsync(student);
            Console.WriteLine("Student added successfully.");

            return new Empty();
        }

        public async Task<Empty> UpdateStudentAsync(Empty request, CallContext? context = null)
        {
            var studentId = StringUtils.InputString("Enter student id to update:", AppConstants.STUDENT_ID_PARTERN);
            var student = await _studentRepository.GetByIdAsync(studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found!");
                return new Empty();
            }

            var studentUpdated = await GetStudentInfo(studentId);
            await _studentRepository.UpdateAsync(studentUpdated);
            Console.WriteLine("Student updated successfully.");

            return new Empty();
        }

        public async Task<Empty> DeleteStudentAsync(Empty request, CallContext? context = null)
        {
            var student = await FindStudentById();
            if (student == null)
            {
                return new Empty();
            }

            await _studentRepository.DeleteAsync(student);
            Console.WriteLine("Student deleted successfully.");

            return new Empty();

        }

        public async Task<Empty> SearchByStudentIdAsync(Empty request, CallContext? context = null)
        {
            var student = await FindStudentById();
            if (student == null)
            {
                return new Empty();
            }

            Console.WriteLine(student.ToString());

            return new Empty();
        }

        public async Task<Empty> PrintStudentListAsync(Empty request, CallContext? context = null)
        {
            var students = await _studentRepository.GetStudentListWithClassAsync();
            StringUtils.PrintList(students, "Student List");

            return new Empty();
        }

        public async Task<Empty> SortStudentListByNameAsync(Empty request, CallContext? context = null)
        {
            StringUtils.PrintList(await _studentRepository.GetStudentListSortByNameAsync(), "Student List");

            return new Empty();
        }

        private async Task<Student> GetStudentInfo(string studentId = "")
        {
            var studentName = StringUtils.InputString("Enter student name:");
            var studentAddress = StringUtils.InputString("Enter student address:");
            var studentDob = DateTimeUtils.InputDateTime($"Enter student dob ({AppConstants.DATE_FORMAT}): ");

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
            var studentId = StringUtils.InputString("Enter student id:", AppConstants.STUDENT_ID_PARTERN);
            var student = await _studentRepository.GetByIdAsync(studentId);

            if (student == null)
            {
                Console.WriteLine("Student not found!");
            }

            return student;
        }
    }
}
