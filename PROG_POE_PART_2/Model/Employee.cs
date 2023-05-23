using System;
using System.Collections.Generic;

namespace PROG_POE_PART_2.Model;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string EmployeeName { get; set; } = null!;

    public string EmployeeSurname { get; set; } = null!;

    public string EmployeeUsername { get; set; } = null!;

    public string EmployeePassword { get; set; } = null!;
}
