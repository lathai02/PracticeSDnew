using AutoMapper;
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
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<Empty> AddStudentAsync(RequestStudentAdd request, CallContext? context = null)
        {
            await _studentRepository.AddAsync(_mapper.Map<Student>(request));

            return new Empty();
        }

        public async Task<Empty> UpdateStudentAsync(RequestStudentAdd request, CallContext? context = null)
        {

            await _studentRepository.UpdateAsync(_mapper.Map<Student>(request));
            Console.WriteLine("Student updated successfully.");

            return new Empty();
        }

        public async Task<Empty> DeleteStudentAsync(RequestStudent request, CallContext? context = null)
        {
            var student = await _studentRepository.GetByIdAsync(request.StudentId);
            if (student == null)
            {
                throw new Exception("Student not found!");
            }
            await _studentRepository.DeleteAsync(student);

            return new Empty();
        }

        public async Task<StudentResponse?> SearchByStudentIdAsync(RequestStudent request, CallContext? context = null)
        {
            var student = await _studentRepository.GetByIdAsync(request.StudentId);
            return _mapper.Map<StudentResponse>(student);
        }

        public async Task<StudentListResponse> PrintStudentListAsync(Empty request, CallContext? context = null)
        {
            var students = await _studentRepository.GetStudentListWithClassAsync();
            return _mapper.Map<StudentListResponse>(students);
        }

        public async Task<StudentListResponse> SortStudentListByNameAsync(Empty request, CallContext? context = null)
        {
            var students = await _studentRepository.GetStudentListSortByNameAsync();
            return _mapper.Map<StudentListResponse>(students);
        }
    }
}
