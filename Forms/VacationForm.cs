using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem;

public class VacationForm : Form
{
    private readonly Employee _emp;
    private readonly EmployeeManager _manager;

    private Label lblRemaining = null!;
    private NumericUpDown nudDays = null!;

    public VacationForm(Employee emp, EmployeeManager manager)
    {
        _emp = emp;
        _manager = manager;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "Vacation Management";
        Size = new Size(420, 340);
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
            BackColor = Color.FromArgb(0, 139, 139)
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
            ForeColor = Color.FromArgb(200, 230, 230),
            Font = new Font("Segoe UI", 9),
            Location = new Point(20, 40),
            AutoSize = true
        };
        panelTitle.Controls.AddRange(new Control[] { lblName, lblPos });

        // Content Panel
        var panelContent = new Panel
        {
            Location = new Point(0, 70),
            Size = new Size(420, 180),
            Padding = new Padding(25)
        };

        int y = 25;

        // Current Balance
        panelContent.Controls.Add(new Label
        {
            Text = "Current Balance:",
            Location = new Point(30, y),
            AutoSize = true,
            Font = new Font("Segoe UI", 11)
        });

        lblRemaining = new Label
        {
            Text = $"{_emp.VacationDays} days",
            Location = new Point(200, y),
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 14),
            ForeColor = _emp.VacationDays > 5 ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69)
        };
        panelContent.Controls.Add(lblRemaining);
        y += 50;

        // Days Input
        panelContent.Controls.Add(new Label
        {
            Text = "Number of Days:",
            Location = new Point(30, y + 3),
            AutoSize = true,
            Font = new Font("Segoe UI", 11)
        });

        nudDays = new NumericUpDown
        {
            Location = new Point(200, y),
            Width = 80,
            Minimum = 1,
            Maximum = 365,
            Value = 1,
            Font = new Font("Segoe UI", 11)
        };
        panelContent.Controls.Add(nudDays);
        y += 50;

        // Action Buttons
        var btnAdd = new Button
        {
            Text = "Add Days",
            Location = new Point(30, y),
            Size = new Size(160, 42),
            BackColor = Color.FromArgb(40, 167, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 10),
            Cursor = Cursors.Hand
        };
        btnAdd.FlatAppearance.BorderSize = 0;
        btnAdd.Click += BtnAdd_Click;

        var btnDeduct = new Button
        {
            Text = "Deduct Days",
            Location = new Point(210, y),
            Size = new Size(160, 42),
            BackColor = Color.FromArgb(220, 53, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 10),
            Cursor = Cursors.Hand
        };
        btnDeduct.FlatAppearance.BorderSize = 0;
        btnDeduct.Click += BtnDeduct_Click;

        panelContent.Controls.AddRange(new Control[] { btnAdd, btnDeduct });

        // Button Panel
        var panelButtons = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 60,
            BackColor = Color.FromArgb(240, 242, 245)
        };

        var btnClose = new Button
        {
            Text = "Done",
            Location = new Point(290, 12),
            Size = new Size(100, 38),
            BackColor = Color.FromArgb(0, 120, 212),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };
        panelButtons.Controls.Add(btnClose);

        Controls.AddRange(new Control[] { panelTitle, panelContent, panelButtons });
    }

    private void RefreshVacationDisplay()
    {
        var updated = _manager.FindById(_emp.Id);
        if (updated is not null)
        {
            lblRemaining.Text = $"{updated.VacationDays} days";
            lblRemaining.ForeColor = updated.VacationDays > 5 ? Color.FromArgb(40, 167, 69) : Color.FromArgb(220, 53, 69);
        }
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        try
        {
            _manager.AddVacationDays(_emp.Id, (int)nudDays.Value);
            RefreshVacationDisplay();
            MessageBox.Show($"Added {nudDays.Value} vacation day(s).", "Success", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnDeduct_Click(object? sender, EventArgs e)
    {
        try
        {
            _manager.DeductVacationDays(_emp.Id, (int)nudDays.Value);
            RefreshVacationDisplay();
            MessageBox.Show($"Deducted {nudDays.Value} vacation day(s).", "Success", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
