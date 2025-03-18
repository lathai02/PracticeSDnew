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
    public class ClassService : BaseService, IClassProto
    {
        private readonly IClassRepository _classRepository;
        private readonly IMapper _mapper;

        public ClassService(IClassRepository classRepository, IMapper mapper)
        {
            _classRepository = classRepository;
            _mapper = mapper;
        }

        public async Task<ResponseObj<ClassListResponse>> GetAllClassWithTeacherAsync(Empty request)
        {
            try
            {
                var classes = await _classRepository.GetAllClassWithTeacherAsync();
                var listClassRes = _mapper.Map<ClassListResponse>(classes);

                return CreateResponse(listClassRes, "Get all class with teacher successfully.");
            }
            catch (Exception)
            {
                return CreateResponse<ClassListResponse>(null, "Error when getting all classes with teacher!");
            }
        }

        public async Task<ResponseObj<ClassResponse>> GetClassByIdAsync(ClassRequest request)
        {
            try
            {
                var resClass = await _classRepository.GetByIdAsync(request.ClassId);
                if (resClass == null)
                {
                    return CreateResponse<ClassResponse>(null, "Class not found!");
                }
                var classRes = _mapper.Map<ClassResponse>(resClass);

                return CreateResponse(classRes, "Get class by ID successfully.");
            }
            catch (Exception)
            {
                return CreateResponse<ClassResponse>(null, "Error when getting class by ID!");
            }
        }
    }
}
