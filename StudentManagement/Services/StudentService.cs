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

        public async Task<List<Student>> GetStudentListAsync(int pageNumber, int pageSize, bool sorted = false)
        {
            var request = new PagingRequest { PageNumber = pageNumber, PageSize = pageSize };
            var response = sorted
                ? await _studentProto.SortStudentListByNameAsync(request)
                : await _studentProto.GetListStudentAsync(request);

            return _mapper.Map<List<Student>>(response.Data);
        }

        public async Task<int> GetTotalCountAsync()
        {
            var response = await _studentProto.GetTotalCountAsync(new Empty());
            return response.Data.TotalItem;
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
    }
}
