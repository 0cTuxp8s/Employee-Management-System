namespace EmployeeManagementSystem.Models;

/// <summary>
/// Represents a single attendance entry for an employee on a given date.
/// </summary>
public class AttendanceRecord
{
    public DateTime Date { get; set; } = DateTime.Today;
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Present;
    public TimeSpan? CheckIn { get; set; }
    public TimeSpan? CheckOut { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public class AttendanceSummary
{
    public int Present { get; set; }
    public int Absent { get; set; }
    public int Leave { get; set; }
    public int Late { get; set; }
    public int EarlyLeave { get; set; }
    public int RecordedDays { get; set; }
    public int WorkingDays { get; set; }
    public double AttendancePercentage { get; set; }
}

public enum AttendanceStatus
{
    Present,
    Absent,
    Leave,
    Late,
    EarlyLeave
}
