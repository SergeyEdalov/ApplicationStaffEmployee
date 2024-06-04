using AppStaffEmployee.Models.Dto;
using AutoMapper;

namespace AppStaffEmployee.Models.Mapper
{
    public class EmployeeMapper : Profile
    {
        public EmployeeMapper()
        {
            CreateMap<EmployeeModel, EmployeeDto>(MemberList.Destination).ReverseMap();
        }
    }
}
