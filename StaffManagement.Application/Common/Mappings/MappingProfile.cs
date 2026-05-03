using AutoMapper;
using StaffManagement.Application.Departments.DTOs;
using StaffManagement.Application.Employees.DTOs;
using StaffManagement.Application.Positions.DTOs;
using StaffManagement.Domain.Entities;

namespace StaffManagement.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.PositionsCount, opt => opt.MapFrom(src => src.Positions.Count));

            CreateMap<Position, PositionDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.PositionTitle, opt => opt.MapFrom(src => src.Position.Title));
        }
    }
}
