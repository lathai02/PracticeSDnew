using AutoMapper;
using Azure;
using Azure.Core;
using Grpc.Core;
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
    public class StudentService : BaseService, IStudentProto
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<ResponseObj<EmptyResponse>> AddStudentAsync(RequestStudentAdd request)
        {
            try
            {
                var student = _mapper.Map<Student>(request);
                await _studentRepository.AddAsync(student);
                return CreateResponse<EmptyResponse>(null, "Student added successfully.");
            }
            catch (Exception)
            {
                return CreateResponse<EmptyResponse>(null, "Error when adding student!");
            }
        }

        public async Task<ResponseObj<EmptyResponse>> UpdateStudentAsync(RequestStudentAdd request)
        {
            try
            {
                var student = _mapper.Map<Student>(request);
                await _studentRepository.UpdateAsync(student);
                return CreateResponse<EmptyResponse>(null, "Student updated successfully.");
            }
            catch (Exception)
            {
                return CreateResponse<EmptyResponse>(null, "Error when updating student!");
            }
        }

        public async Task<ResponseObj<EmptyResponse>> DeleteStudentAsync(RequestStudent request)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(request.StudentId);
                if (student == null)
                {
                    return CreateResponse<EmptyResponse>(null, "Student not found!");
                }

                await _studentRepository.DeleteAsync(student);
                return CreateResponse<EmptyResponse>(null, "Student deleted successfully.");
            }
            catch (Exception)
            {
                return CreateResponse<EmptyResponse>(null, "Error when deleting student!");
            }
        }

        public async Task<ResponseObj<StudentResponse>> SearchByStudentIdAsync(RequestStudent request)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(request.StudentId);
                if (student == null)
                {
                    return CreateResponse<StudentResponse>(null, "Student not found!");
                }

                var studentResponse = _mapper.Map<StudentResponse>(student);
                return CreateResponse(studentResponse, "Student found!");
            }
            catch (Exception)
            {
                return CreateResponse<StudentResponse>(null, "Error when searching student by ID!");
            }
        }

        public async Task<ResponseObj<StudentListResponse>> GetListStudentAsync(PagingRequest request)
        {
            try
            {
                var students = await _studentRepository.GetStudentListWithClassAsync(request.PageNumber, request.PageSize);
                var studentListResponse = _mapper.Map<StudentListResponse>(students);

                var message = students.Count > 0 ? "Print student list successfully." : "No student found!";
                return CreateResponse(studentListResponse, message);
            }
            catch (Exception)
            {
                return CreateResponse<StudentListResponse>(null, "Error when getting student list!");
            }
        }

        public async Task<ResponseObj<StudentListResponse>> SortStudentListByNameAsync(PagingRequest request)
        {
            try
            {
                var students = await _studentRepository.GetStudentListSortByNameAsync(request.PageNumber, request.PageSize);
                var studentListResponse = _mapper.Map<StudentListResponse>(students);

                var message = students.Count > 0 ? "Sort student list by name successfully." : "No student found!";
                return CreateResponse(studentListResponse, message);
            }
            catch (Exception)
            {
                return CreateResponse<StudentListResponse>(null, "Error when sorting student list by name!");
            }
        }

        public async Task<ResponseObj<ResponseNumber>> GetTotalCountAsync(Empty empty)
        {
            try
            {
                return CreateResponse(new ResponseNumber { TotalItem = await _studentRepository.GetTotalCountAsync() }, "Getting total count of student successfully.");
            }
            catch (Exception)
            {
                return CreateResponse<ResponseNumber>(null, "Error when getting total count of student!");
            }
        }
    }
}
