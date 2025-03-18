﻿using AutoMapper;
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

        public async Task<List<Student>> GetStudentList(int pageNumber, int pageSize, bool sorted = false)
        {
            var response = sorted
                ? await _studentProto.SortStudentListByNameAsync(new PagingRequest { PageNumber = pageNumber, PageSize = pageSize })
                : await _studentProto.GetListStudentAsync(new PagingRequest { PageNumber = pageNumber, PageSize = pageSize });

            var students = _mapper.Map<List<Student>>(response.Data);

            return students;
        }

        public async Task<int> GetTotalCountAsync()
        {
            var res = await _studentProto.GetTotalCountAsync(new Empty());

            return res.Data.TotalItem;
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

        public async Task<Student> GetStudentByIdAsync(string studentId)
        {
            var response = await _studentProto.SearchByStudentIdAsync(new RequestStudent { StudentId = studentId });
            return response.Data == null ? null : _mapper.Map<Student>(response.Data);
        }

        public async Task<List<Class>> GetAllClass()
        {
            var response = await _classProto.GetAllClassWithTeacherAsync(new Empty());
            return _mapper.Map<List<Class>>(response.Data);
        }
    }
}
