using AutoMapper;
using RepositoriesUseNHibernate.Interfaces;
using Shares.ServiceContracts;

namespace GrpcService.Services.Implements
{
    public class TeacherService : BaseService, ITeacherProto
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IMapper _mapper;

        public TeacherService(ITeacherRepository teacherRepository, IMapper mapper)
        {
            _teacherRepository = teacherRepository;
            _mapper = mapper;
        }

        public async Task<ResponseObj<TeacherResponseChart>> GetListTeacherWithClassStudentAsync(RequestTeacherChart request)
        {
            var teacher = await _teacherRepository.GetStudentAndClassByTeacherName(request.TeacherName);
            var teacherResponse = _mapper.Map<TeacherResponseChart>(teacher);

            return CreateResponse(teacherResponse, "Get success");
        }
    }
}
