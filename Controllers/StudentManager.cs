using AutoMapper;
using Shares.Constants;
using Shares.Models;
using Shares.ServiceContracts;
using Shares.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controllers
{
    public class StudentManager
    {
        private readonly IStudentProto _studentProto;
        private readonly IClassProto _classProto;
        private readonly IMapper _mapper;

        public StudentManager(IStudentProto studentProto, IClassProto classProto, IMapper mapper)
        {
            _studentProto = studentProto;
            _classProto = classProto;
            _mapper = mapper;
        }

        public async Task DisplayStudentList(bool sorted = false)
        {
            var response = sorted
                ? await _studentProto.SortStudentListByNameAsync(new Empty())
                : await _studentProto.GetListStudentAsync(new Empty());

            var students = _mapper.Map<List<Student>>(response.Data);
            Console.WriteLine(response.Message);
            Console.WriteLine($"Total students: {students.Count}");
            StringUtils.PrintList(students, sorted ? "Sorted Student List" : "Student List");
        }

        public async Task AddOrUpdateStudent(bool isUpdate = false)
        {
            var studentId = StringUtils.InputString($"Enter student ID{(isUpdate ? " to update" : "")}:", AppConstants.STUDENT_ID_PARTERN);

            if (isUpdate && await GetStudentByIdAsync(studentId) == null)
            {
                Console.WriteLine("Student ID not found. Cannot update.");
                return;
            }
            else if (!isUpdate && await GetStudentByIdAsync(studentId) != null)
            {
                Console.WriteLine("Student ID already exists. Cannot add a new student with this ID.");
                return;
            }

            var student = GetStudentInput(studentId);
            student.Class = await SelectClass();

            var request = _mapper.Map<RequestStudentAdd>(student);
            if (isUpdate)
            {
                await _studentProto.UpdateStudentAsync(request);
                Console.WriteLine("Student updated successfully.");
            }
            else
            {
                await _studentProto.AddStudentAsync(request);
                Console.WriteLine("Student added successfully.");
            }
        }

        public async Task DeleteStudent()
        {
            var studentId = StringUtils.InputString("Enter student ID to delete:", AppConstants.STUDENT_ID_PARTERN);
            await _studentProto.DeleteStudentAsync(new RequestStudent { StudentId = studentId });
            Console.WriteLine("Student deleted successfully.");
        }

        public async Task SearchStudentById()
        {
            var studentId = StringUtils.InputString("Enter student ID to search:", AppConstants.STUDENT_ID_PARTERN);
            var student = await GetStudentByIdAsync(studentId);
            if (student != null)
            {
                Console.WriteLine(student);
            }
        }

        private async Task<Student?> GetStudentByIdAsync(string studentId)
        {
            var response = await _studentProto.SearchByStudentIdAsync(new RequestStudent { StudentId = studentId });
            return response.Data == null ? null : _mapper.Map<Student>(response.Data);
        }

        private Student GetStudentInput(string studentId)
        {
            return new Student
            {
                Id = studentId,
                Name = StringUtils.InputString("Enter student name:"),
                Address = StringUtils.InputString("Enter student address:"),
                DateOfBirth = DateTimeUtils.InputDateTime($"Enter student DOB ({AppConstants.DATE_FORMAT}): ")
            };
        }

        private async Task<Class> SelectClass()
        {
            var response = await _classProto.GetAllClassWithTeacherAsync(new Empty());
            var classes = _mapper.Map<List<Class>>(response.Data);
            StringUtils.PrintList(classes, "Class List");

            var classId = NumberUtils.InputIntegerNumber("Enter class ID:", 1, classes.Count);
            var selectedClass = await _classProto.GetClassByIdAsync(new ClassRequest { ClassId = classId });
            return _mapper.Map<Class>(selectedClass.Data);
        }
    }
}
