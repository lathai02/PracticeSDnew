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
                .ReverseMap(); // Nếu bạn muốn hỗ trợ map ngược từ ClassResponse về Class

            // Mapping từ danh sách Class sang ClassListResponse
            CreateMap<List<Class>, ClassListResponse>()
                .ForMember(dest => dest.Classes, opt => opt.MapFrom(src => src));
        }
    }
}
