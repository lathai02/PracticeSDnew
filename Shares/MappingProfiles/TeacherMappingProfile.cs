using AutoMapper;
using Shares.Dtos;
using Shares.Enums;
using Shares.Models;
using Shares.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shares.MappingProfiles
{
    public class TeacherMappingProfile : Profile
    {
        public TeacherMappingProfile() {
            // Map từ Teacher sang TeacherResponseChart
            CreateMap<Teacher, TeacherResponseChart>()
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")));

            // Map từ Class sang ClassResponseChart
            CreateMap<Class, ClassResponseChart>()
                .ForMember(dest => dest.TeacherName, opt => opt.MapFrom(src => src.Teacher.Name));

            // Map từ Student sang StudentResponseChart
            CreateMap<Student, StudentResponseChart>()
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.StudentDateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.StudentAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class.Name));

            CreateMap<TeacherResponseChart, List<ClassWithStudentsResponse>>()
                      .ConvertUsing(src =>
                          src.Classes
                              .GroupBy(c => c.Subject)
                              .Select(g => new ClassWithStudentsResponse
                              {
                                  SubjectName = g.Key ?? string.Empty,
                                  Classes = g.ToList(),
                                  Students = g.SelectMany(c => c.Students).ToList()
                              })
                              .ToList());

            CreateMap<ClassResponseChart, ClassWithStudentsResponse>()
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject ?? string.Empty))
                .ForMember(dest => dest.Classes, opt => opt.Ignore())
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.Students));
        }
    }
}
