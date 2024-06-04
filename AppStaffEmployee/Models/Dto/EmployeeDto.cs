﻿using System.ComponentModel.DataAnnotations;

namespace AppStaffEmployee.Models.Dto;

public class EmployeeDto
{
    public Guid? Id { get; set; }
    public string FullName { get; set; } = null!;

    //[RegularExpression("^(0[1-9]|[12][0-9]|3[01])\\.(0[1-9]|1[0-2])\\.(19|20)\\d{2}$\r\n")]
    public DateTime Birthday { get; set; }
    public string Department { get; set; }
    public string JobTitle { get; set; }

    //[RegularExpression("^(0[1-9]|[12][0-9]|3[01])\\.(0[1-9]|1[0-2])\\.(19|20)\\d{2}$\r\n")]
    public DateTime WorkStart { get; set; }
    public decimal Salary { get; set; }
}
