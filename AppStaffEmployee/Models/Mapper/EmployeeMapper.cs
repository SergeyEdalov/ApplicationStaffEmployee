using AppStaffEmployee.Models.Dto;
using AppStaffEmployee.ViewModels;
using AutoMapper;

namespace AppStaffEmployee.Models.Mapper;

public class EmployeeMapper : Profile
{
    public EmployeeMapper()
    {
        CreateMap<EmployeeModel, EmployeeDto>(MemberList.Destination).ReverseMap();
        CreateMap<EmployeeViewModel, EmployeeDto>(MemberList.Destination).ReverseMap();
    }
}
