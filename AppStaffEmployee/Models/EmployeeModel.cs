using System.ComponentModel.DataAnnotations;

namespace AppStaffEmployee.Models;

public class EmployeeModel
{
    public Guid Id { get; set; }
    public string FullName { get; set; }

    [RegularExpression("^(0[1-9]|[12][0-9]|3[01])\\.(0[1-9]|1[0-2])\\.(19|20)\\d{2}$\r\n")]
    public DateTime Birthday { get; set; }
    public string Department { get; set; }
    public string JobTitle { get; set; }

    [RegularExpression("^(0[1-9]|[12][0-9]|3[01])\\.(0[1-9]|1[0-2])\\.(19|20)\\d{2}$\r\n")]
    public DateTime WorkStart { get; set; }
    public decimal Salary { get; set; }
}

