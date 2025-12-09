namespace EmployeeManagementSystem.Models;

/// <summary>
/// Represents an employee in the system.
/// </summary>
public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Department { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public decimal Salary { get; set; }
    public DateTime JoinDate { get; set; } = DateTime.Today;
    public int VacationDays { get; set; } = 20;
    public EmployeeStatus Status { get; set; } = EmployeeStatus.Active;
    public List<AttendanceRecord> Attendance { get; set; } = new();

    /// <summary>Calculates the monthly salary (Salary / 12).</summary>
    public decimal MonthlySalary => Math.Round(Salary / 12, 2);

    /// <summary>Calculates approximate years of service.</summary>
    public int YearsOfService => (int)((DateTime.Today - JoinDate).TotalDays / 365);

    public override string ToString() => $"{Id}: {Name} ({Position})";
}

public enum EmployeeStatus
{
    Active,
    OnLeave,
    Resigned,
    Terminated
}
