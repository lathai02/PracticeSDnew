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
    public class StudentMappingProfile : Profile
    {
        public StudentMappingProfile()
        {
            // Map từ Student sang StudentResponse
            CreateMap<Student, StudentResponse>()
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.StudentDateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.StudentAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class != null ? src.Class.Name : "No Class"));

            // Map từ RequestStudentAdd sang Student
            CreateMap<RequestStudentAdd, Student>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Bỏ qua nếu Id được tự động tạo
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.Class));

            // Map từ List<Student> sang StudentListResponse
            CreateMap<List<Student>, StudentListResponse>()
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src));
        }
    }
}
