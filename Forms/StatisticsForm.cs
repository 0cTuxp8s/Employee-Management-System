using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem;

public class StatisticsForm : Form
{
    private readonly EmployeeManager _manager;

    public StatisticsForm(EmployeeManager manager)
    {
        _manager = manager;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Statistics Dashboard";
        Size = new Size(700, 550);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(248, 249, 250);

        // Title Panel
        var panelTitle = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = Color.FromArgb(75, 0, 130)
        };
        var lblTitle = new Label
        {
            Text = "Employee Statistics Dashboard",
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 14),
            Location = new Point(20, 18),
            AutoSize = true
        };
        panelTitle.Controls.Add(lblTitle);

        // Content Panel
        var panelContent = new Panel
        {
            Location = new Point(0, 60),
            Size = new Size(650, 380),
            AutoScroll = true
        };

        int x = 20, y = 20;

        // ── EMPLOYEE OVERVIEW ──
        var cardOverview = CreateStatCard("EMPLOYEE OVERVIEW", x, y, 290, 160);
        int cy = 40;
        AddStatRow(cardOverview, "Total Employees:", _manager.TotalEmployees.ToString(), cy); cy += 28;
        AddStatRow(cardOverview, "Active:", _manager.ActiveEmployees.ToString(), cy); cy += 28;
        AddStatRow(cardOverview, "Average Age:", $"{_manager.AverageAge:F1} years", cy); cy += 28;
        AddStatRow(cardOverview, "Avg. Service:", $"{_manager.AverageYearsOfService:F1} years", cy);
        panelContent.Controls.Add(cardOverview);

        // ── SALARY STATS ──
        var cardSalary = CreateStatCard("SALARY STATISTICS", x + 310, y, 290, 160);
        cy = 40;
        AddStatRow(cardSalary, "Total Monthly:", _manager.TotalMonthlyPayroll().ToString("C0"), cy); cy += 28;
        AddStatRow(cardSalary, "Total Annual:", _manager.TotalAnnualPayroll().ToString("C0"), cy); cy += 28;
        AddStatRow(cardSalary, "Average:", _manager.AverageSalary.ToString("C0"), cy); cy += 28;
        AddStatRow(cardSalary, "Range:", $"{_manager.LowestSalary:C0} - {_manager.HighestSalary:C0}", cy);
        panelContent.Controls.Add(cardSalary);

        y += 180;

        // ── DEPARTMENT BREAKDOWN ──
        var deptCounts = _manager.GetDepartmentCounts();
        int deptCardHeight = Math.Max(120, 40 + deptCounts.Count * 26);
        var cardDept = CreateStatCard("EMPLOYEES BY DEPARTMENT", x, y, 290, deptCardHeight);
        cy = 40;
        foreach (var kvp in deptCounts.OrderByDescending(k => k.Value))
        {
            AddStatRow(cardDept, kvp.Key + ":", kvp.Value.ToString(), cy);
            cy += 26;
        }
        if (deptCounts.Count == 0)
        {
            AddStatRow(cardDept, "No departments", "", cy);
        }
        panelContent.Controls.Add(cardDept);

        // ── DEPARTMENT PAYROLL ──
        var deptPayroll = _manager.GetDepartmentPayroll();
        int payrollCardHeight = Math.Max(120, 40 + deptPayroll.Count * 26);
        var cardPayroll = CreateStatCard("PAYROLL BY DEPARTMENT", x + 310, y, 290, payrollCardHeight);
        cy = 40;
        foreach (var kvp in deptPayroll.OrderByDescending(k => k.Value))
        {
            AddStatRow(cardPayroll, kvp.Key + ":", kvp.Value.ToString("C0"), cy);
            cy += 26;
        }
        if (deptPayroll.Count == 0)
        {
            AddStatRow(cardPayroll, "No data", "", cy);
        }
        panelContent.Controls.Add(cardPayroll);

        // Button Panel
        var panelButtons = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            BackColor = Color.FromArgb(240, 242, 245)
        };

        var btnClose = new Button
        {
            Text = "Close",
            Location = new Point(530, 12),
            Size = new Size(90, 38),
            BackColor = Color.FromArgb(108, 117, 125),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.Click += (s, e) => Close();
        panelButtons.Controls.Add(btnClose);

        Controls.AddRange(new Control[] { panelTitle, panelContent, panelButtons });
    }

    private Panel CreateStatCard(string title, int x, int y, int width, int height)
    {
        var card = new Panel
        {
            Location = new Point(x, y),
            Size = new Size(width, height),
            BackColor = Color.White,
            BorderStyle = BorderStyle.None
        };

        // Add shadow effect
        card.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(220, 220, 225), 1);
            e.Graphics.DrawRectangle(pen, 0, 0, card.Width - 1, card.Height - 1);
        };

        var lblHeader = new Label
        {
            Text = title,
            Location = new Point(15, 12),
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 9),
            ForeColor = Color.FromArgb(100, 100, 120)
        };
        card.Controls.Add(lblHeader);

        return card;
    }

    private void AddStatRow(Panel parent, string label, string value, int y)
    {
        parent.Controls.Add(new Label
        {
            Text = label,
            Location = new Point(15, y),
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5f),
            ForeColor = Color.FromArgb(60, 60, 70)
        });

        parent.Controls.Add(new Label
        {
            Text = value,
            Location = new Point(160, y),
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 9.5f),
            ForeColor = Color.FromArgb(0, 120, 212)
        });
    }
}
