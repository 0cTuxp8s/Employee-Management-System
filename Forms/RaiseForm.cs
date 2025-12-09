using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem;

public class RaiseForm : Form
{
    private readonly Employee _emp;
    private readonly EmployeeManager _manager;

    private Label lblCurrentSalary = null!;
    private Label lblNewSalary = null!;
    private NumericUpDown nudPercentage = null!;

    public RaiseForm(Employee emp, EmployeeManager manager)
    {
        _emp = emp;
        _manager = manager;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Salary Adjustment";
        Size = new Size(500, 400);
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
            Height = 70,
            BackColor = Color.FromArgb(255, 165, 0)
        };

        var lblName = new Label
        {
            Text = _emp.Name,
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 13),
            Location = new Point(20, 12),
            AutoSize = true
        };
        var lblPos = new Label
        {
            Text = $"{_emp.Position} • {_emp.Department}",
            ForeColor = Color.FromArgb(255, 240, 200),
            Font = new Font("Segoe UI", 9),
            Location = new Point(20, 40),
            AutoSize = true
        };
        panelTitle.Controls.AddRange(new Control[] { lblName, lblPos });

        // Content Panel
        var panelContent = new Panel
        {
            Location = new Point(0, 70),
            Size = new Size(450, 220),
            Padding = new Padding(25)
        };

        int y = 25;

        // Current Salary
        panelContent.Controls.Add(new Label
        {
            Text = "Current Salary:",
            Location = new Point(30, y),
            AutoSize = true,
            Font = new Font("Segoe UI", 11)
        });

        lblCurrentSalary = new Label
        {
            Text = _emp.Salary.ToString("C0") + " / year",
            Location = new Point(200, y),
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 11),
            ForeColor = Color.FromArgb(0, 123, 255)
        };
        panelContent.Controls.Add(lblCurrentSalary);
        y += 45;

        // Percentage Input
        panelContent.Controls.Add(new Label
        {
            Text = "Raise Percentage:",
            Location = new Point(30, y + 3),
            AutoSize = true,
            Font = new Font("Segoe UI", 11)
        });

        nudPercentage = new NumericUpDown
        {
            Location = new Point(200, y),
            Width = 100,
            Minimum = -50,
            Maximum = 100,
            Value = 5,
            DecimalPlaces = 1,
            Increment = 0.5m,
            Font = new Font("Segoe UI", 11)
        };
        nudPercentage.ValueChanged += (s, e) => UpdateNewSalary();
        panelContent.Controls.Add(nudPercentage);

        panelContent.Controls.Add(new Label
        {
            Text = "%",
            Location = new Point(305, y + 3),
            AutoSize = true,
            Font = new Font("Segoe UI", 11)
        });
        y += 45;

        // New Salary Preview
        panelContent.Controls.Add(new Label
        {
            Text = "New Salary:",
            Location = new Point(30, y),
            AutoSize = true,
            Font = new Font("Segoe UI", 11)
        });

        lblNewSalary = new Label
        {
            Text = "",
            Location = new Point(200, y),
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 12),
            ForeColor = Color.FromArgb(40, 167, 69)
        };
        panelContent.Controls.Add(lblNewSalary);
        UpdateNewSalary();
        y += 50;

        // Info
        panelContent.Controls.Add(new Label
        {
            Text = "Tip: Use negative percentage for salary reduction",
            Location = new Point(30, y),
            AutoSize = true,
            Font = new Font("Segoe UI", 9, FontStyle.Italic),
            ForeColor = Color.Gray
        });

        // Button Panel
        var panelButtons = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 65,
            BackColor = Color.FromArgb(240, 242, 245)
        };

        var btnApply = new Button
        {
            Text = "Apply Raise",
            Location = new Point(210, 14),
            Size = new Size(120, 40),
            BackColor = Color.FromArgb(40, 167, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 10),
            Cursor = Cursors.Hand
        };
        btnApply.FlatAppearance.BorderSize = 0;
        btnApply.Click += BtnApply_Click;

        var btnCancel = new Button
        {
            Text = "Cancel",
            Location = new Point(340, 14),
            Size = new Size(90, 40),
            BackColor = Color.FromArgb(108, 117, 125),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

        panelButtons.Controls.AddRange(new Control[] { btnApply, btnCancel });

        Controls.AddRange(new Control[] { panelTitle, panelContent, panelButtons });
    }

    private void UpdateNewSalary()
    {
        decimal newSalary = Math.Round(_emp.Salary * (1 + nudPercentage.Value / 100), 2);
        lblNewSalary.Text = newSalary.ToString("C0") + " / year";
        
        decimal diff = newSalary - _emp.Salary;
        string sign = diff >= 0 ? "+" : "";
        lblNewSalary.Text += $"  ({sign}{diff:C0})";
        lblNewSalary.ForeColor = diff >= 0 ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
    }

    private void BtnApply_Click(object? sender, EventArgs e)
    {
        var result = MessageBox.Show(
            $"Apply {nudPercentage.Value}% salary adjustment to {_emp.Name}?\n\n" +
            $"Current: {_emp.Salary:C0}\n" +
            $"New: {Math.Round(_emp.Salary * (1 + nudPercentage.Value / 100), 2):C0}",
            "Confirm Salary Change",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (result == DialogResult.Yes)
        {
            try
            {
                _manager.ApplySalaryRaise(_emp.Id, nudPercentage.Value);
                MessageBox.Show("Salary updated successfully.", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
