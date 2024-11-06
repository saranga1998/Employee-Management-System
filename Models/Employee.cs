using System;
using System.Collections.Generic;

namespace EMS_Project.Models;

public partial class Employee
{
    public string EmployeeId { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string? EmployeeEmail { get; set; }

    public string? EmployeeJob { get; set; }
}
