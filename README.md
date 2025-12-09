# Employee-Management-System
 A C# (.NET 7) Windows Forms application to manage employees: CRUD, search/filter, vacation tracking, salary raises, payroll summary, statistics, and attendance logging with monthly summaries.

## Table of Contents

1. [Introduction](#1-introduction)
2. [Objectives](#2-objectives)
3. [System Requirements](#3-system-requirements)
4. [System Architecture](#4-system-architecture)
5. [Features & Functionality](#5-features--functionality)
6. [User Interface Design](#6-user-interface-design)
7. [Database / Data Storage](#7-database--data-storage)
8. [Key Code Implementation](#8-key-code-implementation)
9. [Screenshots](#9-screenshots)
10. [How to Run](#10-how-to-run)
11. [Testing & Validation](#11-testing--validation)
12. [Limitations & Future Enhancements](#12-limitations--future-enhancements)
13. [Conclusion](#13-conclusion)
14. [References](#14-references)
15. [Appendix: Source Code Links](#15-appendix-source-code-links)

---

## 1. Introduction

Employee management is a critical function for any organization. This project presents a **desktop application** built using **C# and Windows Forms** on the **.NET 7** platform. The system provides a comprehensive solution for managing employee records, tracking attendance, handling vacation requests, processing salary adjustments, and generating reports.

The application demonstrates key programming concepts including:
- Object-Oriented Programming (OOP)
- Event-driven GUI development
- Data persistence using JSON
- LINQ for querying and filtering
- Clean separation of concerns (Models, Services, Forms)

---

## 2. Objectives

The primary objectives of this project are:

1. **Develop a user-friendly desktop application** for employee management.
2. **Implement full CRUD operations** (Create, Read, Update, Delete) for employee records.
3. **Enable search and filtering** by name, department, and employment status.
4. **Track employee attendance** with daily status logging and monthly summaries.
5. **Manage vacation days** with add/deduct functionality.
6. **Process salary adjustments** with percentage-based raises.
7. **Generate reports** including payroll summaries and statistical dashboards.
8. **Export data** to CSV format for external use.
9. **Persist all data** using JSON file storage.

---

## 3. System Requirements

### Hardware Requirements
- Processor: 1 GHz or faster
- RAM: 2 GB minimum
- Storage: 50 MB free space
- Display: 1024×768 resolution or higher

### Software Requirements
- Operating System: Windows 10/11
- Runtime: .NET 7 SDK
- IDE (for development): Visual Studio 2022 or VS Code

---

## 4. System Architecture

The application follows a **layered architecture** pattern:
```

┌─────────────────────────────────────────────────────┐
│                  Presentation Layer                 │
│         (Windows Forms - UI Components)             │
│   MainForm, AddEditEmployeeForm, AttendanceForm,    │
│   VacationForm, RaiseForm, PayrollForm, etc.        │
├─────────────────────────────────────────────────────┤
│                   Service Layer                     │
│              (Business Logic)                       │
│              EmployeeManager.cs                     │
├─────────────────────────────────────────────────────┤
│                    Model Layer                      │
│                (Data Structures)                    │
│   Employee.cs, AttendanceRecord.cs,                 │
│   AttendanceSummary.cs                              │
├─────────────────────────────────────────────────────┤
│                  Data Layer                         │
│            (JSON File Persistence)                  │
│               employees.json                        │
└─────────────────────────────────────────────────────┘

```
### Project Structure

```
EmployeeManagementSystem/
├── Program.cs                    # Application entry point
├── EmployeeManagementSystem.csproj
├── employees.json                # Data storage file
├── Forms/
│   ├── MainForm.cs               # Main application window
│   ├── AddEditEmployeeForm.cs    # Add/Edit employee dialog
│   ├── AttendanceForm.cs         # Attendance management
│   ├── VacationForm.cs           # Vacation tracking
│   ├── RaiseForm.cs              # Salary adjustment
│   ├── PayrollForm.cs            # Payroll summary report
│   ├── StatisticsForm.cs         # Statistics dashboard
│   └── AboutForm.cs              # About dialog
├── Models/
│   ├── Employee.cs               # Employee data model
│   └── AttendanceRecord.cs       # Attendance data model
└──── Services/
    └── EmployeeManager.cs        # Core business logic

```

## 5. Features & Functionality

### 5.1 Employee Management (CRUD)

| Feature | Description |
|---------|-------------|
| **Add Employee** | Create new employee with name, age, department, position, email, phone, salary, join date, vacation days, and status |
| **Edit Employee** | Modify existing employee details |
| **Delete Employee** | Remove employee with confirmation dialog |
| **View All** | Display employees in a sortable data grid |

### 5.2 Search & Filter

- **Text Search:** Filter employees by name (partial match)
- **Department Filter:** Dropdown to filter by department
- **Status Filter:** Filter by Active, On Leave, Resigned, or Terminated

### 5.3 Attendance Tracking

| Component | Description |
|-----------|-------------|
| **Daily Entry** | Mark attendance status for any date |
| **Status Options** | Present, Absent, Leave, Late, Early Leave |
| **Check-in/Out** | Optional time recording |
| **Notes** | Add remarks for each entry |
| **Monthly View** | Grid showing all records for selected month |
| **Summary Stats** | Present/Absent/Leave/Late/Early counts and attendance percentage |

**Attendance Percentage Calculation:**
```
##Attendance % = (Present + Late + EarlyLeave) / Working Weekdays × 100##
```
- Weekends (Saturday, Sunday) are excluded
- Only recorded entries affect the numerator; unrecorded days reduce percentage

### 5.4 Vacation Management

- View current vacation balance
- Add vacation days (accrual)
- Deduct vacation days (usage)
- Real-time balance update

### 5.5 Salary Adjustment

- View current salary
- Apply percentage-based raise (-50% to +100%)
- Preview new salary before confirmation
- Supports both increases and decreases

### 5.6 Reports

| Report | Description |
|--------|-------------|
| **Payroll Summary** | Total monthly/annual payroll, average salary, salary range |
| **Statistics Dashboard** | Employee count, department breakdown, payroll by department |

### 5.7 Data Export
```
- Export employee roster to CSV format
- Includes all fields: ID, Name, Age, Department, Position, Email, Phone, Salary, Join Date, Vacation Days, Status

```

## 6. User Interface Design
```
The application features a **modern, professional UI** with:

- **Menu Bar:** File, Employee, Reports, Help menus
- **Sidebar Navigation:** Quick access buttons for common actions
- **Top Filter Bar:** Search box, department dropdown, status dropdown
- **Data Grid:** Sortable table with alternating row colors
- **Status Bar:** Shows employee count, active count, monthly payroll
```
### Color Scheme

| Element | Color | Purpose |
|---------|-------|---------|
| Primary | #0078D4 | Headers, buttons, accents |
| Sidebar | #2D343C | Navigation background |
| Success | #28A745 | Add, Save actions |
| Danger | #DC3545 | Delete actions |
| Warning | #FFA500 | Salary raise |
| Background | #F0F2F5 | Form backgrounds |

### Design Principles
```
1. **Consistency:** Uniform styling across all forms
2. **Accessibility:** Clear labels, sufficient contrast, keyboard navigation
3. **Feedback:** Status messages, confirmation dialogs, validation errors
4. **Simplicity:** Clean layout without clutter

```

## 7. Database / Data Storage
```
The application uses **JSON file storage** (`employees.json`) for simplicity and portability.
```
### Employee Data Structure

```json
{
  "Id": 1,
  "Name": "Ali Hassan",
  "Age": 30,
  "Department": "IT",
  "Position": "Software Engineer",
  "Email": "ali.hassan@company.com",
  "Phone": "+92-300-1234567",
  "Salary": 72000,
  "JoinDate": "2020-03-15",
  "VacationDays": 18,
  "Status": "Active",
  "Attendance": [
    {
      "Date": "2025-12-10",
      "Status": "Present",
      "CheckIn": "09:00",
      "CheckOut": "18:00",
      "Notes": ""
    }
  ]
}
```

### Why JSON?
```
- No database server required
- Human-readable format
- Easy to backup and transfer
- Sufficient for single-user desktop application
- Native .NET support via `System.Text.Json`

```

## 8. Key Code Implementation

### 8.1 Employee Model (`Models/Employee.cs`)

```csharp
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

    public decimal MonthlySalary => Math.Round(Salary / 12, 2);
    public int YearsOfService => (int)((DateTime.Today - JoinDate).TotalDays / 365);
}
```

### 8.2 Search Implementation (`Services/EmployeeManager.cs`)

```csharp
public List<Employee> Search(string? name = null, string? department = null)
{
    var query = _employees.AsEnumerable();
    
    if (!string.IsNullOrWhiteSpace(name))
        query = query.Where(e => e.Name.Contains(name, 
            StringComparison.OrdinalIgnoreCase));
    
    if (!string.IsNullOrWhiteSpace(department))
        query = query.Where(e => e.Department.Equals(department, 
            StringComparison.OrdinalIgnoreCase));
    
    return query.ToList();
}
```

### 8.3 Attendance Summary Calculation

```csharp
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
        WorkingDays = CountWeekdaysInMonth(year, month)
    };

    int effectivePresence = summary.Present + summary.Late + summary.EarlyLeave;
    summary.AttendancePercentage = summary.WorkingDays > 0
        ? Math.Round((double)effectivePresence / summary.WorkingDays * 100, 1)
        : 0;

    return summary;
}
```

### 8.4 JSON Persistence

```csharp
private void Save()
{
    var options = new JsonSerializerOptions { WriteIndented = true };
    var json = JsonSerializer.Serialize(_employees, options);
    File.WriteAllText(_filePath, json);
}

private void Load()
{
    if (!File.Exists(_filePath)) return;
    var json = File.ReadAllText(_filePath);
    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    var list = JsonSerializer.Deserialize<List<Employee>>(json, options);
    if (list is not null) _employees.AddRange(list);
}
```

## 9. Screenshots

> <img width="975" height="520" alt="image" src="https://github.com/user-attachments/assets/0092931f-d6e7-4cfc-b591-fc8ec120a60c" />
>  <img width="975" height="519" alt="image" src="https://github.com/user-attachments/assets/366dfebc-2bab-4891-8bc4-b73f34e1f63e" />
>  <img width="975" height="517" alt="image" src="https://github.com/user-attachments/assets/024aabfd-abc8-4350-8842-5c4a41896754" />
>  <img width="975" height="518" alt="image" src="https://github.com/user-attachments/assets/306ec985-f7ad-4b5e-96c1-45eb9d5d9ec8" />
>  <img width="975" height="520" alt="image" src="https://github.com/user-attachments/assets/b83169d1-4a04-4e11-a081-71259d073bc5" />
>  <img width="975" height="517" alt="image" src="https://github.com/user-attachments/assets/dbf3ac67-862b-4548-89f0-88d104e5d9a2" />
>  <img width="975" height="517" alt="image" src="https://github.com/user-attachments/assets/44809758-4b8f-469f-916d-15d1571d3b65" />
>  <img width="975" height="516" alt="image" src="https://github.com/user-attachments/assets/3ce46f2c-1e3e-48bd-ab67-3fdb150371c0" />
>  <img width="975" height="518" alt="image" src="https://github.com/user-attachments/assets/d04863de-c5c9-4265-938f-62e74af579b4" />

| Screen | Description |
|--------|-------------|
| **Main Window** | Shows employee grid, sidebar, search/filter bar |
| **Add/Edit Form** | Employee data entry with validation |
| **Attendance Form** | Daily entry, monthly grid, summary statistics |
| **Vacation Form** | View/add/deduct vacation days |
| **Raise Form** | Salary adjustment with preview |
| **Statistics Dashboard** | Charts and department breakdown |

---

## 10. How to Run

### Prerequisites

- Windows 10 or 11
- .NET 7 SDK installed ([Download](https://dotnet.microsoft.com/download/dotnet/7.0))

### Steps

1. Clone or download the project:
   ```bash
   git clone https://github.com/0cTuxp8s/Employee-Management-System.git
   ```

2. Navigate to the project folder:
   ```bash
   cd EmployeeManagementSystem
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run
   ```

5. The application launches with sample data from `employees.json`.

---

## 11. Testing & Validation

### Build Verification

- `dotnet build` completes with **0 errors, 0 warnings**
- No external NuGet packages required (uses .NET built-in libraries)

### Manual Testing Checklist

| Feature | Test Case | Result |
|---------|-----------|--------|
| Add Employee | Fill all fields, save | ✓ Saved to grid and JSON |
| Edit Employee | Modify name, save | ✓ Updated correctly |
| Delete Employee | Confirm delete | ✓ Removed from grid and JSON |
| Search | Type partial name | ✓ Filters correctly |
| Department Filter | Select "IT" | ✓ Shows only IT employees |
| Mark Attendance | Select Present, save | ✓ Recorded in grid |
| Attendance % | Mark 1 present in December | ✓ Shows ~4.3% (1/23 weekdays) |
| Add Vacation | Add 5 days | ✓ Balance increases |
| Deduct Vacation | Deduct 3 days | ✓ Balance decreases |
| Salary Raise | Apply 10% | ✓ New salary calculated |
| CSV Export | Export file | ✓ Valid CSV generated |

---

## 12. Limitations & Future Enhancements

### Current Limitations

| Limitation | Explanation |
|------------|-------------|
| No holiday calendar | All weekdays count as working days |
| Single-user only | No multi-user or network support |
| No authentication | No login/password protection |
| Basic CSV export | No Excel formatting |
| No database | JSON storage limits scalability |

### Future Enhancements

1. **SQLite/SQL Server integration** for larger datasets
2. **User authentication** with role-based access (Admin, Manager, Employee)
3. **Holiday calendar** for accurate attendance calculation
4. **Email notifications** for leave requests
5. **Graphical reports** with charts (using LiveCharts or similar)
6. **Backup/restore** functionality
7. **Print reports** feature

---

## 13. Conclusion

This project successfully demonstrates a functional **Employee Management System** built with **C# and Windows Forms**. The application provides essential HR functionalities including employee CRUD operations, attendance tracking, vacation management, salary adjustments, and reporting.

Key achievements:
- Clean, maintainable code structure with separation of concerns
- Professional, user-friendly interface
- Reliable data persistence with JSON
- Comprehensive feature set suitable for small organizations

The project showcases proficiency in:
- C# programming language
- Windows Forms GUI development
- Object-oriented design principles
- Data manipulation with LINQ
- File I/O operations

---

## 14. References

1. Microsoft .NET Documentation: https://docs.microsoft.com/dotnet/
2. C# Programming Guide: https://docs.microsoft.com/dotnet/csharp/
3. Windows Forms Documentation: https://docs.microsoft.com/dotnet/desktop/winforms/
4. System.Text.Json: https://docs.microsoft.com/dotnet/standard/serialization/system-text-json/

---

## 15. Appendix: Source Code Links

All source code is available on GitHub:

| File | Link |
|------|------|
| Program.cs | [View](https://github.com/0cTuxp8s/Employee-Management-System/blob/main/Program.cs) |
| MainForm.cs | [View](https://github.com/0cTuxp8s/Employee-Management-System/blob/main/Forms/MainForm.cs) |
| AddEditEmployeeForm.cs | [View](https://github.com/0cTuxp8s/Employee-Management-System/blob/main/Forms/AddEditEmployeeForm.cs) |
| AttendanceForm.cs | [View](https://github.com/0cTuxp8s/Employee-Management-System/blob/main/Forms/AttendanceForm.cs) |
| VacationForm.cs | [View](https://github.com/0cTuxp8s/Employee-Management-System/blob/main/Forms/VacationForm.cs) |
| RaiseForm.cs | [View](https://github.com/0cTuxp8s/Employee-Management-System/blob/main/Forms/RaiseForm.cs) |
| PayrollForm.cs | [View](https://github.com/0cTuxp8s/Employee-Management-System/blob/main/Forms/PayrollForm.cs) |
| StatisticsForm.cs | [View](https://github.com/0cTuxp8s/Employee-Management-System/blob/main/Forms/StatisticsForm.cs) |
| Employee.cs | [View](https://github.com/0cTuxp8s/Employee-Management-System/blob/main/Models/Employee.cs) |
| AttendanceRecord.cs | [View](https://github.com/0cTuxp8s/Employee-Management-System/blob/main/Models/AttendanceRecord.cs) |
| EmployeeManager.cs | [View](https://github.com/0cTuxp8s/Employee-Management-System/blob/main/Services/EmployeeManager.cs) |

---

*End of Report*
