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
        private readonly ITeacherProto _teacherProto;
        private readonly IMapper _mapper;

        public StudentService(IStudentProto studentProto, IClassProto classProto, IMapper mapper, ITeacherProto teacherProto)
        {
            _studentProto = studentProto;
            _classProto = classProto;
            _mapper = mapper;
            _teacherProto = teacherProto;
        }

        public async Task<TeacherResponseChart> GetTeacherByNameAsync(string teacherName)
        {
            var response = await _teacherProto.GetListTeacherWithClassStudentAsync(new RequestTeacherChart { TeacherName = teacherName });
            return response.Data!;
        }

        public async Task<List<Student>> GetStudentListAsync(int pageNumber, int pageSize, string? sortBy)
        {
            var response = sortBy != null
                ? await _studentProto.SortStudentListByOptAsync(new PagingRequestSort { PageNumber = pageNumber, PageSize = pageSize, SortBy = sortBy })
                : await _studentProto.GetListStudentAsync(new PagingRequest { PageNumber = pageNumber, PageSize = pageSize });

            return _mapper.Map<List<Student>>(response.Data);
        }

        public async Task<int> GetTotalCountAsync()
        {
            var response = await _studentProto.GetTotalCountAsync(new Empty());
            return response.Data!.TotalItem;
        }

        public async Task<string> AddOrUpdateStudentAsync(Student student, bool isUpdate = false)
        {
            var existingStudent = await GetStudentByIdAsync(student.Id);

            if (isUpdate)
            {
                if (existingStudent == null) return "Student ID not found. Cannot update.";
                await _studentProto.UpdateStudentAsync(_mapper.Map<RequestStudentAdd>(student));
                return "Student updated successfully.";
            }
            else
            {
                if (existingStudent != null) return "Student ID already exists. Cannot add a new student with this ID.";
                await _studentProto.AddStudentAsync(_mapper.Map<RequestStudentAdd>(student));
                return "Student added successfully.";
            }
        }

        public async Task<string> DeleteStudentAsync(string studentId)
        {
            var response = await _studentProto.DeleteStudentAsync(new RequestStudent { StudentId = studentId });
            return response.Message;
        }

        public async Task<Student?> GetStudentByIdAsync(string studentId)
        {
            var response = await _studentProto.SearchByStudentIdAsync(new RequestStudent { StudentId = studentId });
            return response.Data == null ? null : _mapper.Map<Student>(response.Data);
        }

        public async Task<List<Class>> GetAllClassesAsync()
        {
            var response = await _classProto.GetAllClassWithTeacherAsync(new Empty());
            return _mapper.Map<List<Class>>(response.Data);
        }

        public async Task<List<TeacherResponseChart>> GetAllTeacherAsync()
        {
            var response = await _teacherProto.GetAllTeacher(new Empty());
            return response.Data!;
        }
    }
}
