using AutoMapper;
using Shares.Models;
using Shares.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Globalization;
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

            // Map từ List<Student> sang StudentListResponse
            CreateMap<List<Student>, StudentListResponse>()
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src));

            CreateMap<StudentResponse, Student>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.StudentName))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateTime.ParseExact(src.StudentDateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.StudentAddress))
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => new Class { Name = src.ClassName }))
                .ForMember(dest => dest.ClassId, opt => opt.Ignore());

            CreateMap<StudentListResponse, List<Student>>()
                .ConvertUsing((src, dest, context) =>
                     src.Students != null ? context.Mapper.Map<List<Student>>(src.Students) : new List<Student>()
                 );

            CreateMap<Class, ClassResponse>();

            CreateMap<Student, RequestStudentAdd>()
                .ForMember(dest => dest.ClassResponse, opt => opt.MapFrom(src => src.Class ?? new Class()));

            CreateMap<RequestStudentAdd, Student>()
               .ForMember(dest => dest.ClassId, opt => opt.MapFrom(src => src.ClassResponse.Id))
               .ForMember(dest => dest.Class, opt => opt.MapFrom(src => new Class
               {
                   Id = src.ClassResponse.Id,
                   Name = src.ClassResponse.Name,
                   Subject = src.ClassResponse.Subject,
               }));

            CreateMap<ClassResponse, Class>();

        }
    }
}
