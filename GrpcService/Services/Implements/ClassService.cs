using AutoMapper;
using Azure.Core;
using ProtoBuf.Grpc;
using RepositoriesUseNHibernate.Implements;
using RepositoriesUseNHibernate.Interfaces;
using Shares.Models;
using Shares.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcService.Services.Implements
{
    public class ClassService : IClassProto
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public ClassService(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<ClassListResponse> GetAllClassWithTeacherAsync(Empty request, CallContext? context = null)
        {
            var classes = await _classRepository.GetAllClassWithTeacherAsync();
            return _mapper.Map<ClassListResponse>(classes);
        }

        public async Task<ClassResponse?> GetClassByIdAsync(ClassRequest request, CallContext? context = null)
        {
            var resClass = await _classRepository.GetByIdAsync(request.ClassId);
            return _mapper.Map<ClassResponse>(resClass);
        }
    }
}
