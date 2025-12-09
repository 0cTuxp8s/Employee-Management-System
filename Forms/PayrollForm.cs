using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem;

public class PayrollForm : Form
{
    private readonly EmployeeManager _manager;

    public PayrollForm(EmployeeManager manager)
    {
        _manager = manager;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Payroll Summary";
        Size = new Size(500, 500);
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
            BackColor = Color.FromArgb(138, 43, 226)
        };
        var lblTitle = new Label
        {
            Text = "Payroll Summary Report",
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
            Size = new Size(500, 320),
            Padding = new Padding(30)
        };

        int y = 30;
        
        // Overview Section
        AddSectionHeader(panelContent, "OVERVIEW", 30, y);
        y += 35;
        AddInfoRow(panelContent, "Total Employees:", _manager.TotalEmployees.ToString(), 30, y, Color.FromArgb(0, 123, 255));
        y += 35;
        AddInfoRow(panelContent, "Active Employees:", _manager.ActiveEmployees.ToString(), 30, y, Color.FromArgb(40, 167, 69));
        y += 50;

        // Salary Section
        AddSectionHeader(panelContent, "SALARY INFORMATION", 30, y);
        y += 35;
        AddInfoRow(panelContent, "Monthly Payroll:", _manager.TotalMonthlyPayroll().ToString("C0"), 30, y, Color.FromArgb(220, 53, 69));
        y += 35;
        AddInfoRow(panelContent, "Annual Payroll:", _manager.TotalAnnualPayroll().ToString("C0"), 30, y, Color.FromArgb(220, 53, 69));
        y += 35;
        AddInfoRow(panelContent, "Average Salary:", _manager.AverageSalary.ToString("C0"), 30, y, Color.FromArgb(255, 193, 7));
        y += 35;
        AddInfoRow(panelContent, "Highest Salary:", _manager.HighestSalary.ToString("C0"), 30, y, Color.FromArgb(40, 167, 69));
        y += 35;
        AddInfoRow(panelContent, "Lowest Salary:", _manager.LowestSalary.ToString("C0"), 30, y, Color.FromArgb(108, 117, 125));

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
            Location = new Point(380, 12),
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

    private void AddSectionHeader(Panel parent, string text, int x, int y)
    {
        parent.Controls.Add(new Label
        {
            Text = text,
            Location = new Point(x, y),
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 10),
            ForeColor = Color.FromArgb(100, 100, 110)
        });
    }

    private void AddInfoRow(Panel parent, string label, string value, int x, int y, Color valueColor)
    {
        parent.Controls.Add(new Label
        {
            Text = label,
            Location = new Point(x, y),
            AutoSize = true,
            Font = new Font("Segoe UI", 11)
        });

        parent.Controls.Add(new Label
        {
            Text = value,
            Location = new Point(250, y),
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 11),
            ForeColor = valueColor
        });
    }
}
