using AutoMapper;
using Shares.Constants;
using Shares.Models;
using Shares.ServiceContracts;
using Shares.Utils;

namespace StudentManagement.Services
{
    public class StudentService
    {
        private readonly IStudentProto _studentProto;
        private readonly IClassProto _classProto;
        private readonly IMapper _mapper;

        public StudentService(IStudentProto studentProto, IClassProto classProto, IMapper mapper)
        {
            _studentProto = studentProto;
            _classProto = classProto;
            _mapper = mapper;
        }

        public async Task<List<Student>> GetStudentList(bool sorted = false)
        {
            var response = sorted
                ? await _studentProto.SortStudentListByNameAsync(new Empty())
                : await _studentProto.GetListStudentAsync(new Empty());

            var students = _mapper.Map<List<Student>>(response.Data);

            return students;
        }

        public async Task<string> AddOrUpdateStudent(Student student, bool isUpdate = false)
        {

            var res = await GetStudentByIdAsync(student.Id);
            if (isUpdate && res == null)
            {
                return "Student ID not found. Cannot update.";
            }
            else if (!isUpdate && res != null)
            {
                return "Student ID already exists. Cannot add a new student with this ID.";
            }

            var request = _mapper.Map<RequestStudentAdd>(student);
            if (isUpdate)
            {
                await _studentProto.UpdateStudentAsync(request);
                return "Student updated successfully.";
            }
            else
            {
                await _studentProto.AddStudentAsync(request);
                return "Student added successfully.";
            }
        }

        public async Task<string> DeleteStudent(string studentId)
        {
            var res = await _studentProto.DeleteStudentAsync(new RequestStudent { StudentId = studentId });

            return res.Message;
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

        public async Task<List<Class>> GetAllClass()
        {
            var response = await _classProto.GetAllClassWithTeacherAsync(new Empty());
            return _mapper.Map<List<Class>>(response.Data);
        }


    }
}
