using AutoMapper;
using Shares.Models;
using Shares.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shares.MappingProfiles
{
    public class ClassMappingProfile : Profile
    {
        public ClassMappingProfile()
        {
            // Mapping từ Class sang ClassResponse
            CreateMap<Class, ClassResponse>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher != null ? src.Teacher.Name : null))
                .ReverseMap();

            // Mapping từ danh sách Class sang ClassListResponse
            CreateMap<List<Class>, ClassListResponse>()
                .ForMember(dest => dest.Classes, opt => opt.MapFrom(src => src));

            // Map từ ClassResponse sang Class
            CreateMap<ClassResponse, Class>()
                .ForMember(dest => dest.Teacher, opt => opt.Ignore()) // Teacher là một đối tượng, không thể map trực tiếp từ TeacherName
                .ForMember(dest => dest.Students, opt => opt.Ignore()) // Bỏ qua Students vì không có trong response
                .ForMember(dest => dest.TeacherId, opt => opt.Ignore()); // TeacherId không có trong response

            // Map từ ClassListResponse sang List<Class>
            CreateMap<ClassListResponse, List<Class>>()
                .ConvertUsing((src, dest, context) =>
                    src.Classes.Select(classResponse => context.Mapper.Map<Class>(classResponse)).ToList()
                );
        }
    }
}
