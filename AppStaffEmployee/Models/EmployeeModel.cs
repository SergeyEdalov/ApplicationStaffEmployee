﻿using System.ComponentModel.DataAnnotations;

namespace AppStaffEmployee.Models;

public class EmployeeModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public DateTime Birthday { get; set; }
    public string Department { get; set; }
    public string JobTitle { get; set; }
    public DateTime WorkStart { get; set; }
    public decimal Salary { get; set; }
}

