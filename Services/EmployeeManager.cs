using System.Text.Json;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services;

/// <summary>
/// Manages the employee list and persistence.
/// </summary>
public class EmployeeManager
{
    private readonly List<Employee> _employees = new();
    private readonly string _filePath;

    public EmployeeManager(string filePath = "employees.json")
    {
        _filePath = filePath;
        Load();
    }

    // ═══════════════════════════════════════════════════════════════
    // CRUD Operations
    // ═══════════════════════════════════════════════════════════════

    public IReadOnlyList<Employee> GetAll() => _employees.AsReadOnly();

    public Employee? FindById(int id) => _employees.FirstOrDefault(e => e.Id == id);

    public List<Employee> Search(string? name = null, string? department = null, string? position = null)
    {
        var query = _employees.AsEnumerable();
        
        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(e => e.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        
        if (!string.IsNullOrWhiteSpace(department))
            query = query.Where(e => e.Department.Equals(department, StringComparison.OrdinalIgnoreCase));
        
        if (!string.IsNullOrWhiteSpace(position))
            query = query.Where(e => e.Position.Contains(position, StringComparison.OrdinalIgnoreCase));
        
        return query.ToList();
    }

    public bool IdExists(int id) => _employees.Any(e => e.Id == id);

    public int GetNextId() => _employees.Count == 0 ? 1 : _employees.Max(e => e.Id) + 1;

    public void Add(Employee employee)
    {
        if (IdExists(employee.Id))
            throw new InvalidOperationException($"Employee ID {employee.Id} already exists.");
        
        ValidateEmployee(employee);
        _employees.Add(employee);
        Save();
    }

    public void Update(Employee updated)
    {
        var existing = FindById(updated.Id)
            ?? throw new InvalidOperationException($"Employee ID {updated.Id} not found.");
        
        ValidateEmployee(updated);
        
        existing.Name = updated.Name;
        existing.Age = updated.Age;
        existing.Department = updated.Department;
        existing.Position = updated.Position;
        existing.Email = updated.Email;
        existing.Phone = updated.Phone;
        existing.Salary = updated.Salary;
        existing.JoinDate = updated.JoinDate;
        existing.VacationDays = updated.VacationDays;
        existing.Status = updated.Status;
        Save();
    }

    public void Delete(int id)
    {
        var emp = FindById(id)
            ?? throw new InvalidOperationException($"Employee ID {id} not found.");
        _employees.Remove(emp);
        Save();
    }

    private void ValidateEmployee(Employee emp)
    {
        if (string.IsNullOrWhiteSpace(emp.Name))
            throw new InvalidOperationException("Employee name is required.");
        if (emp.Age < 18 || emp.Age > 100)
            throw new InvalidOperationException("Age must be between 18 and 100.");
        if (emp.Salary < 0)
            throw new InvalidOperationException("Salary cannot be negative.");
        if (!string.IsNullOrWhiteSpace(emp.Email) && !emp.Email.Contains('@'))
            throw new InvalidOperationException("Invalid email format.");
    }

    // ═══════════════════════════════════════════════════════════════
    // Statistics
    // ═══════════════════════════════════════════════════════════════

    public int TotalEmployees => _employees.Count;
    public int ActiveEmployees => _employees.Count(e => e.Status == EmployeeStatus.Active);
    public decimal AverageSalary => _employees.Count > 0 ? Math.Round(_employees.Average(e => e.Salary), 2) : 0;
    public double AverageAge => _employees.Count > 0 ? Math.Round(_employees.Average(e => e.Age), 1) : 0;
    public double AverageYearsOfService => _employees.Count > 0 ? Math.Round(_employees.Average(e => e.YearsOfService), 1) : 0;
    public decimal HighestSalary => _employees.Count > 0 ? _employees.Max(e => e.Salary) : 0;
    public decimal LowestSalary => _employees.Count > 0 ? _employees.Min(e => e.Salary) : 0;

    public List<string> GetAllDepartments() => 
        _employees.Select(e => e.Department).Distinct().Where(d => !string.IsNullOrEmpty(d)).OrderBy(d => d).ToList();
    
    public List<string> GetAllPositions() => 
        _employees.Select(e => e.Position).Distinct().Where(p => !string.IsNullOrEmpty(p)).OrderBy(p => p).ToList();

    public Dictionary<string, int> GetDepartmentCounts() =>
        _employees.GroupBy(e => string.IsNullOrEmpty(e.Department) ? "Unassigned" : e.Department)
                  .ToDictionary(g => g.Key, g => g.Count());

    public Dictionary<string, decimal> GetDepartmentPayroll() =>
        _employees.GroupBy(e => string.IsNullOrEmpty(e.Department) ? "Unassigned" : e.Department)
                  .ToDictionary(g => g.Key, g => g.Sum(e => e.Salary));

    // ═══════════════════════════════════════════════════════════════
    // Payroll Operations
    // ═══════════════════════════════════════════════════════════════

    public decimal TotalAnnualPayroll() => _employees.Sum(e => e.Salary);
    public decimal TotalMonthlyPayroll() => _employees.Sum(e => e.MonthlySalary);

    public void ApplySalaryRaise(int id, decimal percentage)
    {
        var emp = FindById(id) ?? throw new InvalidOperationException($"Employee ID {id} not found.");
        if (percentage < -50 || percentage > 100)
            throw new InvalidOperationException("Raise percentage must be between -50% and 100%.");
        
        emp.Salary = Math.Round(emp.Salary * (1 + percentage / 100), 2);
        Save();
    }

    public void ApplyRaiseToAll(decimal percentage)
    {
        if (percentage < -50 || percentage > 100)
            throw new InvalidOperationException("Raise percentage must be between -50% and 100%.");
        
        foreach (var emp in _employees)
            emp.Salary = Math.Round(emp.Salary * (1 + percentage / 100), 2);
        Save();
    }

    // ═══════════════════════════════════════════════════════════════
    // Vacation Operations
    // ═══════════════════════════════════════════════════════════════

    public void AddVacationDays(int id, int days)
    {
        var emp = FindById(id) ?? throw new InvalidOperationException($"Employee ID {id} not found.");
        if (days <= 0) throw new InvalidOperationException("Days must be positive.");
        emp.VacationDays += days;
        Save();
    }

    public void DeductVacationDays(int id, int days)
    {
        var emp = FindById(id) ?? throw new InvalidOperationException($"Employee ID {id} not found.");
        if (days <= 0) throw new InvalidOperationException("Days must be positive.");
        if (emp.VacationDays < days)
            throw new InvalidOperationException($"Not enough vacation days. Available: {emp.VacationDays}");
        emp.VacationDays -= days;
        Save();
    }

    // ═══════════════════════════════════════════════════════════════
    // Attendance
    // ═══════════════════════════════════════════════════════════════

    public void MarkAttendance(int id, DateTime date, AttendanceStatus status, TimeSpan? checkIn = null, TimeSpan? checkOut = null, string? notes = null)
    {
        var emp = FindById(id) ?? throw new InvalidOperationException($"Employee ID {id} not found.");
        var targetDate = date.Date;

        var record = emp.Attendance.FirstOrDefault(a => a.Date.Date == targetDate);
        if (record is null)
        {
            record = new AttendanceRecord { Date = targetDate };
            emp.Attendance.Add(record);
        }

        record.Status = status;
        record.CheckIn = checkIn;
        record.CheckOut = checkOut;
        record.Notes = notes?.Trim() ?? string.Empty;

        Save();
    }

    public List<AttendanceRecord> GetAttendanceForMonth(int id, int year, int month)
    {
        var emp = FindById(id) ?? throw new InvalidOperationException($"Employee ID {id} not found.");
        return emp.Attendance
            .Where(a => a.Date.Year == year && a.Date.Month == month)
            .OrderByDescending(a => a.Date)
            .ToList();
    }

    public AttendanceSummary GetAttendanceSummaryForMonth(int id, int year, int month)
    {
        var records = GetAttendanceForMonth(id, year, month);

        var summary = new AttendanceSummary
        {
            Present = records.Count(a => a.Status == AttendanceStatus.Present),
            Absent = records.Count(a => a.Status == AttendanceStatus.Absent),
            Leave = records.Count(a => a.Status == AttendanceStatus.Leave),
            Late = records.Count(a => a.Status == AttendanceStatus.Late),
            EarlyLeave = records.Count(a => a.Status == AttendanceStatus.EarlyLeave),
            RecordedDays = records.Count,
            WorkingDays = CountWeekdaysInMonth(year, month)
        };

        int effectivePresence = summary.Present + summary.Late + summary.EarlyLeave;
        summary.AttendancePercentage = summary.WorkingDays > 0
            ? Math.Round((double)effectivePresence / summary.WorkingDays * 100, 1)
            : 0;

        return summary;
    }

    private int CountWeekdaysInMonth(int year, int month)
    {
        int days = DateTime.DaysInMonth(year, month);
        int weekdays = 0;
        for (int day = 1; day <= days; day++)
        {
            var dow = new DateTime(year, month, day).DayOfWeek;
            if (dow != DayOfWeek.Saturday && dow != DayOfWeek.Sunday)
                weekdays++;
        }
        return weekdays;
    }

    // ═══════════════════════════════════════════════════════════════
    // Export
    // ═══════════════════════════════════════════════════════════════

    public string ExportToCsv()
    {
        var lines = new List<string>
        {
            "ID,Name,Age,Department,Position,Email,Phone,Salary,JoinDate,VacationDays,Status"
        };
        
        foreach (var e in _employees)
        {
            lines.Add($"{e.Id},\"{e.Name}\",{e.Age},\"{e.Department}\",\"{e.Position}\",\"{e.Email}\",\"{e.Phone}\",{e.Salary},{e.JoinDate:yyyy-MM-dd},{e.VacationDays},{e.Status}");
        }
        
        return string.Join(Environment.NewLine, lines);
    }

    // ═══════════════════════════════════════════════════════════════
    // Persistence (JSON)
    // ═══════════════════════════════════════════════════════════════

    private void Load()
    {
        if (!File.Exists(_filePath)) return;
        try
        {
            var json = File.ReadAllText(_filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var list = JsonSerializer.Deserialize<List<Employee>>(json, options);
            if (list is not null)
                _employees.AddRange(list);
        }
        catch
        {
            // If file is corrupt, start fresh
        }
    }

    private void Save()
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var json = JsonSerializer.Serialize(_employees, options);
        File.WriteAllText(_filePath, json);
    }
}
