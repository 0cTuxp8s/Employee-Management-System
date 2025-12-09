using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem;

public class AddEditEmployeeForm : Form
{
    private readonly Employee? _existing;
    private readonly EmployeeManager _manager;

    // Form Controls
    private TextBox txtId = null!;
    private TextBox txtName = null!;
    private NumericUpDown nudAge = null!;
    private ComboBox cmbDepartment = null!;
    private TextBox txtPosition = null!;
    private TextBox txtEmail = null!;
    private TextBox txtPhone = null!;
    private NumericUpDown nudSalary = null!;
    private DateTimePicker dtpJoinDate = null!;
    private NumericUpDown nudVacation = null!;
    private ComboBox cmbStatus = null!;

    public AddEditEmployeeForm(Employee? existing, EmployeeManager manager)
    {
        _existing = existing;
        _manager = manager;
        InitializeComponent();
        if (_existing is not null) PopulateFields();
    }

    private void InitializeComponent()
    {
        Text = _existing is null ? "Add New Employee" : "Edit Employee";
        Size = new Size(560, 640);
        MinimumSize = new Size(520, 620);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(248, 249, 250);
        AutoScroll = true;

        // Title Panel
        var panelTitle = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = Color.FromArgb(0, 120, 212)
        };
        var lblTitle = new Label
        {
            Text = _existing is null ? "Add New Employee" : "Edit Employee Details",
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 14),
            Location = new Point(16, 14),
            AutoSize = true
        };
        panelTitle.Controls.Add(lblTitle);

        // Form Content
        var panelContent = new Panel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(25),
            AutoScroll = true
        };

        int lblX = 25, ctrlX = 160, y = 20, rowH = 40;

        // ID
        AddLabel(panelContent, "Employee ID:", lblX, y);
        txtId = new TextBox 
        { 
            Location = new Point(ctrlX, y - 3), 
            Width = 100,
            ReadOnly = _existing is not null,
            BackColor = _existing is not null ? Color.FromArgb(240, 240, 240) : Color.White
        };
        if (_existing is null) txtId.Text = _manager.GetNextId().ToString();
        panelContent.Controls.Add(txtId);
        y += rowH;

        // Name
        AddLabel(panelContent, "Full Name: *", lblX, y);
        txtName = new TextBox { Location = new Point(ctrlX, y + 2), Width = 300 };
        panelContent.Controls.Add(txtName);
        y += rowH;

        // Age
        AddLabel(panelContent, "Age:", lblX, y);
        nudAge = new NumericUpDown 
        { 
            Location = new Point(ctrlX, y - 3), 
            Width = 80, 
            Minimum = 18, 
            Maximum = 100, 
            Value = 25 
        };
        panelContent.Controls.Add(nudAge);
        y += rowH;

        // Department
        AddLabel(panelContent, "Department:", lblX, y);
        cmbDepartment = new ComboBox 
        { 
            Location = new Point(ctrlX, y - 3), 
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDown
        };
        cmbDepartment.Items.AddRange(new object[] { "IT", "HR", "Finance", "Marketing", "Operations", "Sales", "Engineering", "Administration" });
        foreach (var dept in _manager.GetAllDepartments())
            if (!cmbDepartment.Items.Contains(dept)) cmbDepartment.Items.Add(dept);
        panelContent.Controls.Add(cmbDepartment);
        y += rowH;

        // Position
        AddLabel(panelContent, "Position:", lblX, y);
        txtPosition = new TextBox { Location = new Point(ctrlX, y - 3), Width = 250 };
        panelContent.Controls.Add(txtPosition);
        y += rowH;

        // Email
        AddLabel(panelContent, "Email:", lblX, y);
        txtEmail = new TextBox { Location = new Point(ctrlX, y - 3), Width = 280 };
        panelContent.Controls.Add(txtEmail);
        y += rowH;

        // Phone
        AddLabel(panelContent, "Phone:", lblX, y);
        txtPhone = new TextBox { Location = new Point(ctrlX, y - 3), Width = 180 };
        panelContent.Controls.Add(txtPhone);
        y += rowH;

        // Salary
        AddLabel(panelContent, "Annual Salary:", lblX, y);
        nudSalary = new NumericUpDown 
        { 
            Location = new Point(ctrlX, y - 3), 
            Width = 150, 
            Minimum = 0, 
            Maximum = 10_000_000, 
            DecimalPlaces = 2, 
            Increment = 1000,
            ThousandsSeparator = true
        };
        panelContent.Controls.Add(nudSalary);
        y += rowH;

        // Join Date
        AddLabel(panelContent, "Join Date:", lblX, y);
        dtpJoinDate = new DateTimePicker 
        { 
            Location = new Point(ctrlX, y - 3), 
            Width = 160, 
            Format = DateTimePickerFormat.Short 
        };
        panelContent.Controls.Add(dtpJoinDate);
        y += rowH;

        // Vacation Days
        AddLabel(panelContent, "Vacation Days:", lblX, y);
        nudVacation = new NumericUpDown 
        { 
            Location = new Point(ctrlX, y - 3), 
            Width = 80, 
            Minimum = 0, 
            Maximum = 365, 
            Value = 20 
        };
        panelContent.Controls.Add(nudVacation);
        y += rowH;

        // Status
        AddLabel(panelContent, "Status:", lblX, y);
        cmbStatus = new ComboBox 
        { 
            Location = new Point(ctrlX, y - 3), 
            Width = 140,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        cmbStatus.Items.AddRange(new object[] { "Active", "On Leave", "Resigned", "Terminated" });
        cmbStatus.SelectedIndex = 0;
        panelContent.Controls.Add(cmbStatus);

        // Button Panel
        var panelButtons = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 80,
            Padding = new Padding(20, 16, 20, 16),
            BackColor = Color.FromArgb(240, 242, 245)
        };

        var btnSave = new Button
        {
            Text = "Save",
            Location = new Point(300, 18),
            Size = new Size(110, 40),
            BackColor = Color.FromArgb(40, 167, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 10),
            Cursor = Cursors.Hand
        };
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.Click += BtnSave_Click;

        var btnCancel = new Button
        {
            Text = "Cancel",
            Location = new Point(420, 18),
            Size = new Size(100, 40),
            BackColor = Color.FromArgb(108, 117, 125),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 10),
            Cursor = Cursors.Hand
        };
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

        panelButtons.Controls.AddRange(new Control[] { btnSave, btnCancel });

        // Dock order: bottom first, then fill, then top to ensure buttons are visible
        Controls.Add(panelButtons);
        Controls.Add(panelContent);
        Controls.Add(panelTitle);
    }

    private void AddLabel(Panel parent, string text, int x, int y)
    {
        parent.Controls.Add(new Label 
        { 
            Text = text, 
            Location = new Point(x, y), 
            AutoSize = true,
            Font = new Font("Segoe UI", 10)
        });
    }

    private void PopulateFields()
    {
        txtId.Text = _existing!.Id.ToString();
        txtName.Text = _existing.Name;
        nudAge.Value = Math.Clamp(_existing.Age, 18, 100);
        cmbDepartment.Text = _existing.Department;
        txtPosition.Text = _existing.Position;
        txtEmail.Text = _existing.Email;
        txtPhone.Text = _existing.Phone;
        nudSalary.Value = Math.Min(_existing.Salary, nudSalary.Maximum);
        dtpJoinDate.Value = _existing.JoinDate;
        nudVacation.Value = Math.Min(_existing.VacationDays, 365);
        cmbStatus.SelectedIndex = (int)_existing.Status;
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        // Validate ID
        if (!int.TryParse(txtId.Text, out int id) || id <= 0)
        {
            ShowError("Employee ID must be a positive number.");
            txtId.Focus();
            return;
        }

        // Validate Name
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            ShowError("Employee name is required.");
            txtName.Focus();
            return;
        }

        // Validate Email format if provided
        if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !txtEmail.Text.Contains('@'))
        {
            ShowError("Please enter a valid email address.");
            txtEmail.Focus();
            return;
        }

        var emp = new Employee
        {
            Id = id,
            Name = txtName.Text.Trim(),
            Age = (int)nudAge.Value,
            Department = cmbDepartment.Text.Trim(),
            Position = txtPosition.Text.Trim(),
            Email = txtEmail.Text.Trim(),
            Phone = txtPhone.Text.Trim(),
            Salary = nudSalary.Value,
            JoinDate = dtpJoinDate.Value.Date,
            VacationDays = (int)nudVacation.Value,
            Status = (EmployeeStatus)cmbStatus.SelectedIndex
        };

        try
        {
            if (_existing is null)
                _manager.Add(emp);
            else
                _manager.Update(emp);

            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message);
        }
    }

    private void ShowError(string message)
    {
        MessageBox.Show(message, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
