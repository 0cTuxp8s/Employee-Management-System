using System.ComponentModel.DataAnnotations;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem;

public partial class MainForm : Form
{
    private readonly EmployeeManager _manager = new();

    // ── Menu & Status ──
    private MenuStrip menuStrip = null!;
    private StatusStrip statusStrip = null!;
    private ToolStripStatusLabel lblStatusLeft = null!;
    private ToolStripStatusLabel lblStatusRight = null!;

    // ── Main Layout ──
    private Panel panelTop = null!;
    private Panel panelLeft = null!;
    private DataGridView dgvEmployees = null!;

    // ── Search/Filter Controls ──
    private TextBox txtSearch = null!;
    private ComboBox cmbDepartment = null!;
    private ComboBox cmbStatus = null!;

    public MainForm()
    {
        InitializeComponent();
        LoadDepartments();
        LoadGrid(_manager.GetAll());
    }

    private void InitializeComponent()
    {
        Text = "Employee Management System";
        Size = new Size(1150, 720);
        MinimumSize = new Size(950, 550);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9.5f);
        BackColor = Color.FromArgb(240, 242, 245);

        CreateMenuStrip();
        CreateStatusStrip();
        CreateTopPanel();
        CreateLeftPanel();
        CreateDataGrid();

        Controls.Add(dgvEmployees);
        Controls.Add(panelLeft);
        Controls.Add(panelTop);
        Controls.Add(statusStrip);
        Controls.Add(menuStrip);
    }

    // ═══════════════════════════════════════════════════════════════
    // MENU STRIP
    // ═══════════════════════════════════════════════════════════════

    private void CreateMenuStrip()
    {
        menuStrip = new MenuStrip 
        { 
            BackColor = Color.White,
            Padding = new Padding(8, 2, 0, 2)
        };

        // File Menu
        var fileMenu = new ToolStripMenuItem("&File");
        fileMenu.DropDownItems.Add("Export to CSV", null, (s, e) => ExportCsv());
        fileMenu.DropDownItems.Add(new ToolStripSeparator());
        fileMenu.DropDownItems.Add("Exit", null, (s, e) => Close());

        // Employee Menu
        var empMenu = new ToolStripMenuItem("&Employee");
        empMenu.DropDownItems.Add("Add New Employee", null, (s, e) => AddEmployee());
        empMenu.DropDownItems.Add("Edit Selected", null, (s, e) => EditEmployee());
        empMenu.DropDownItems.Add("Delete Selected", null, (s, e) => DeleteEmployee());
        empMenu.DropDownItems.Add(new ToolStripSeparator());
        empMenu.DropDownItems.Add("Manage Attendance", null, (s, e) => ManageAttendance());
        empMenu.DropDownItems.Add("Manage Vacation", null, (s, e) => ManageVacation());
        empMenu.DropDownItems.Add("Give Salary Raise", null, (s, e) => GiveRaise());

        // Reports Menu
        var reportsMenu = new ToolStripMenuItem("&Reports");
        reportsMenu.DropDownItems.Add("Payroll Summary", null, (s, e) => ShowPayroll());
        reportsMenu.DropDownItems.Add("Statistics Dashboard", null, (s, e) => ShowStatistics());

        // Help Menu
        var helpMenu = new ToolStripMenuItem("&Help");
        helpMenu.DropDownItems.Add("About", null, (s, e) => ShowAbout());

        menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, empMenu, reportsMenu, helpMenu });
    }

    // ═══════════════════════════════════════════════════════════════
    // STATUS STRIP
    // ═══════════════════════════════════════════════════════════════

    private void CreateStatusStrip()
    {
        statusStrip = new StatusStrip 
        { 
            BackColor = Color.FromArgb(0, 120, 212),
            SizingGrip = true 
        };

        lblStatusLeft = new ToolStripStatusLabel
        {
            Text = "Ready",
            ForeColor = Color.White,
            Spring = true,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 9)
        };

        lblStatusRight = new ToolStripStatusLabel
        {
            Text = DateTime.Now.ToString("dddd, MMMM dd, yyyy"),
            ForeColor = Color.White,
            TextAlign = ContentAlignment.MiddleRight
        };

        statusStrip.Items.AddRange(new ToolStripItem[] { lblStatusLeft, lblStatusRight });
    }

    // ═══════════════════════════════════════════════════════════════
    // TOP PANEL (Search & Filter)
    // ═══════════════════════════════════════════════════════════════

    private void CreateTopPanel()
    {
        panelTop = new Panel
        {
            Dock = DockStyle.Top,
            Height = 90,
            BackColor = Color.White,
            Padding = new Padding(16, 12, 16, 8)
        };

        panelTop.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(220, 220, 225), 1);
            e.Graphics.DrawLine(pen, 0, panelTop.Height - 1, panelTop.Width, panelTop.Height - 1);
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 8,
            RowCount = 1,
            AutoSize = false,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            Padding = new Padding(0),
            Margin = new Padding(0)
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Search label
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300)); // Search box
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Dept label
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160)); // Dept combo
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize)); // Status label
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130)); // Status combo
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); // Search button
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90)); // Clear button

        var lblSearch = new Label
        {
            Text = "Search:",
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Font = new Font("Segoe UI Semibold", 10)
        };

        txtSearch = new TextBox
        {
            Anchor = AnchorStyles.Left,
            Width = 280,
            Height = 32,
            Margin = new Padding(6, 0, 6, 0),
            Font = new Font("Segoe UI", 10)
        };
        txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) PerformSearch(); };

        var lblDept = new Label
        {
            Text = "Department:",
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Font = new Font("Segoe UI Semibold", 10)
        };

        cmbDepartment = new ComboBox
        {
            Anchor = AnchorStyles.Left,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 10),
            Margin = new Padding(6, 0, 6, 0)
        };

        var lblStat = new Label
        {
            Text = "Status:",
            AutoSize = true,
            Anchor = AnchorStyles.Left,
            Font = new Font("Segoe UI Semibold", 10)
        };

        cmbStatus = new ComboBox
        {
            Anchor = AnchorStyles.Left,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 10),
            Margin = new Padding(6, 0, 6, 0)
        };
        cmbStatus.Items.AddRange(new object[] { "All", "Active", "On Leave", "Resigned", "Terminated" });
        cmbStatus.SelectedIndex = 0;

        var btnSearch = new Button
        {
            Text = "Search",
            Anchor = AnchorStyles.Left,
            Size = new Size(100, 32),
            BackColor = Color.FromArgb(0, 120, 212),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 9),
            Cursor = Cursors.Hand,
            Margin = new Padding(6, 0, 6, 0)
        };
        btnSearch.FlatAppearance.BorderSize = 0;
        btnSearch.Click += (s, e) => PerformSearch();

        var btnClear = new Button
        {
            Text = "Clear",
            Anchor = AnchorStyles.Left,
            Size = new Size(90, 32),
            BackColor = Color.FromArgb(108, 117, 125),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 9),
            Cursor = Cursors.Hand,
            Margin = new Padding(6, 0, 0, 0)
        };
        btnClear.FlatAppearance.BorderSize = 0;
        btnClear.Click += (s, e) => ClearFilters();

        layout.Controls.Add(lblSearch, 0, 0);
        layout.Controls.Add(txtSearch, 1, 0);
        layout.Controls.Add(lblDept, 2, 0);
        layout.Controls.Add(cmbDepartment, 3, 0);
        layout.Controls.Add(lblStat, 4, 0);
        layout.Controls.Add(cmbStatus, 5, 0);
        layout.Controls.Add(btnSearch, 6, 0);
        layout.Controls.Add(btnClear, 7, 0);

        panelTop.Controls.Add(layout);
    }

    // ═══════════════════════════════════════════════════════════════
    // LEFT PANEL (Action Buttons)
    // ═══════════════════════════════════════════════════════════════

    private void CreateLeftPanel()
    {
        panelLeft = new Panel
        {
            Dock = DockStyle.Left,
            Width = 180,
            BackColor = Color.FromArgb(45, 52, 60),
            Padding = new Padding(12)
        };

        int y = 25;
        int btnHeight = 48;
        int gap = 10;
        int btnWidth = 154;

        // EMPLOYEE SECTION
        var lblEmployee = new Label
        {
            Text = "EMPLOYEE",
            ForeColor = Color.FromArgb(140, 150, 165),
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            Location = new Point(14, y),
            AutoSize = true
        };
        y += 28;

        var btnAdd = CreateSideButton("Add New", y, btnWidth, btnHeight, Color.FromArgb(34, 139, 34));
        btnAdd.Click += (s, e) => AddEmployee();
        y += btnHeight + gap;

        var btnEdit = CreateSideButton("Edit", y, btnWidth, btnHeight, Color.FromArgb(30, 144, 255));
        btnEdit.Click += (s, e) => EditEmployee();
        y += btnHeight + gap;

        var btnDelete = CreateSideButton("Delete", y, btnWidth, btnHeight, Color.FromArgb(220, 20, 60));
        btnDelete.Click += (s, e) => DeleteEmployee();
        y += btnHeight + gap + 25;

        // MANAGEMENT SECTION
        var lblManage = new Label
        {
            Text = "MANAGEMENT",
            ForeColor = Color.FromArgb(140, 150, 165),
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            Location = new Point(14, y),
            AutoSize = true
        };
        y += 28;

        var btnVacation = CreateSideButton("Vacation", y, btnWidth, btnHeight, Color.FromArgb(0, 139, 139));
        btnVacation.Click += (s, e) => ManageVacation();
        y += btnHeight + gap;

        var btnAttendance = CreateSideButton("Attendance", y, btnWidth, btnHeight, Color.FromArgb(0, 102, 204));
        btnAttendance.Click += (s, e) => ManageAttendance();
        y += btnHeight + gap;

        var btnRaise = CreateSideButton("Salary Raise", y, btnWidth, btnHeight, Color.FromArgb(255, 165, 0));
        btnRaise.Click += (s, e) => GiveRaise();
        y += btnHeight + gap + 25;

        // REPORTS SECTION
        var lblReports = new Label
        {
            Text = "REPORTS",
            ForeColor = Color.FromArgb(140, 150, 165),
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            Location = new Point(14, y),
            AutoSize = true
        };
        y += 28;

        var btnPayroll = CreateSideButton("Payroll", y, btnWidth, btnHeight, Color.FromArgb(138, 43, 226));
        btnPayroll.Click += (s, e) => ShowPayroll();
        y += btnHeight + gap;

        var btnStats = CreateSideButton("Statistics", y, btnWidth, btnHeight, Color.FromArgb(75, 0, 130));
        btnStats.Click += (s, e) => ShowStatistics();
        y += btnHeight + gap + 25;

        // REFRESH
        var btnRefresh = CreateSideButton("Refresh All", y, btnWidth, btnHeight, Color.FromArgb(70, 80, 90));
        btnRefresh.Click += (s, e) => RefreshData();

        panelLeft.Controls.AddRange(new Control[] { 
            lblEmployee, btnAdd, btnEdit, btnDelete,
            lblManage, btnVacation, btnAttendance, btnRaise,
            lblReports, btnPayroll, btnStats, btnRefresh 
        });
    }

    private Button CreateSideButton(string text, int y, int width, int height, Color backColor)
    {
        var btn = new Button
        {
            Text = text,
            Location = new Point(12, y),
            Size = new Size(width, height),
            BackColor = backColor,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Font = new Font("Segoe UI", 10),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(12, 0, 0, 0)
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(backColor, 0.15f);
        return btn;
    }

    // ═══════════════════════════════════════════════════════════════
    // DATA GRID
    // ═══════════════════════════════════════════════════════════════

    private void CreateDataGrid()
    {
        dgvEmployees = new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            RowHeadersVisible = false,
            EnableHeadersVisualStyles = false,
            Font = new Font("Segoe UI", 9.5f),
            GridColor = Color.FromArgb(230, 230, 235)
        };

        // Header Style
        dgvEmployees.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(0, 120, 212),
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 10),
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            Padding = new Padding(10, 0, 0, 0)
        };
        dgvEmployees.ColumnHeadersHeight = 50;
        dgvEmployees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

        // Row Style
        dgvEmployees.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.White,
            ForeColor = Color.FromArgb(40, 40, 45),
            SelectionBackColor = Color.FromArgb(0, 120, 212),
            SelectionForeColor = Color.White,
            Padding = new Padding(10, 0, 0, 0)
        };
        dgvEmployees.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(248, 250, 252)
        };
        dgvEmployees.RowTemplate.Height = 45;

        // Double-click to edit
        dgvEmployees.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) EditEmployee(); };
    }

    // ═══════════════════════════════════════════════════════════════
    // DATA LOADING
    // ═══════════════════════════════════════════════════════════════

    private void LoadDepartments()
    {
        cmbDepartment.Items.Clear();
        cmbDepartment.Items.Add("All");
        foreach (var dept in _manager.GetAllDepartments())
            cmbDepartment.Items.Add(dept);
        cmbDepartment.SelectedIndex = 0;
    }

    private void LoadGrid(IEnumerable<Employee> list)
    {
        var data = list.Select(e => new
        {
            e.Id,
            e.Name,
            e.Department,
            e.Position,
            e.Age,
            Salary = e.Salary.ToString("C0"),
            Joined = e.JoinDate.ToString("MMM dd, yyyy"),
            Vacation = $"{e.VacationDays} days",
            e.Status
        }).ToList();

        dgvEmployees.DataSource = data;

        // Adjust column widths proportionally
        if (dgvEmployees.Columns.Count > 0)
        {
            dgvEmployees.Columns["Id"].FillWeight = 8;
            dgvEmployees.Columns["Name"].FillWeight = 18;
            dgvEmployees.Columns["Department"].FillWeight = 14;
            dgvEmployees.Columns["Position"].FillWeight = 14;
            dgvEmployees.Columns["Age"].FillWeight = 7;
            dgvEmployees.Columns["Salary"].FillWeight = 12;
            dgvEmployees.Columns["Joined"].FillWeight = 12;
            dgvEmployees.Columns["Vacation"].FillWeight = 10;
            dgvEmployees.Columns["Status"].FillWeight = 10;
        }

        UpdateStatus();
    }

    private void UpdateStatus()
    {
        lblStatusLeft.Text = $"Total: {_manager.TotalEmployees} employees  |  Active: {_manager.ActiveEmployees}  |  Monthly Payroll: {_manager.TotalMonthlyPayroll():C0}";
    }

    private Employee? GetSelectedEmployee()
    {
        if (dgvEmployees.SelectedRows.Count == 0) return null;
        int id = (int)dgvEmployees.SelectedRows[0].Cells["Id"].Value;
        return _manager.FindById(id);
    }

    // ═══════════════════════════════════════════════════════════════
    // SEARCH & FILTER
    // ═══════════════════════════════════════════════════════════════

    private void PerformSearch()
    {
        string? name = string.IsNullOrWhiteSpace(txtSearch.Text) ? null : txtSearch.Text;
        string? dept = cmbDepartment.SelectedIndex <= 0 ? null : cmbDepartment.Text;

        var results = _manager.Search(name, dept);

        // Filter by status
        if (cmbStatus.SelectedIndex > 0)
        {
            var status = (EmployeeStatus)(cmbStatus.SelectedIndex - 1);
            results = results.Where(e => e.Status == status).ToList();
        }

        LoadGrid(results);
        lblStatusLeft.Text = $"Found: {results.Count} employee(s) matching criteria";
    }

    private void ClearFilters()
    {
        txtSearch.Clear();
        cmbDepartment.SelectedIndex = 0;
        cmbStatus.SelectedIndex = 0;
        LoadGrid(_manager.GetAll());
    }

    private void RefreshData()
    {
        LoadDepartments();
        ClearFilters();
        lblStatusLeft.Text = "Data refreshed successfully";
    }

    // ═══════════════════════════════════════════════════════════════
    // CRUD ACTIONS
    // ═══════════════════════════════════════════════════════════════

    private void AddEmployee()
    {
        using var dlg = new AddEditEmployeeForm(null, _manager);
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            RefreshData();
            lblStatusLeft.Text = "New employee added successfully.";
        }
    }

    private void EditEmployee()
    {
        var emp = GetSelectedEmployee();
        if (emp is null)
        {
            MessageBox.Show("Please select an employee from the list first.", "No Selection", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new AddEditEmployeeForm(emp, _manager);
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            RefreshData();
            lblStatusLeft.Text = "Employee updated successfully.";
        }
    }

    private void DeleteEmployee()
    {
        var emp = GetSelectedEmployee();
        if (emp is null)
        {
            MessageBox.Show("Please select an employee from the list first.", "No Selection", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var result = MessageBox.Show(
            $"Are you sure you want to delete:\n\nName: {emp.Name}\nRole: {emp.Position}\nDepartment: {emp.Department}\n\nThis action cannot be undone.",
            "Confirm Delete",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning);

        if (result == DialogResult.Yes)
        {
            _manager.Delete(emp.Id);
            RefreshData();
            lblStatusLeft.Text = "Employee deleted successfully.";
        }
    }

    private void ManageVacation()
    {
        var emp = GetSelectedEmployee();
        if (emp is null)
        {
            MessageBox.Show("Please select an employee from the list first.", "No Selection", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new VacationForm(emp, _manager);
        if (dlg.ShowDialog() == DialogResult.OK)
            RefreshData();
    }

    private void GiveRaise()
    {
        var emp = GetSelectedEmployee();
        if (emp is null)
        {
            MessageBox.Show("Please select an employee from the list first.", "No Selection", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new RaiseForm(emp, _manager);
        if (dlg.ShowDialog() == DialogResult.OK)
        {
            RefreshData();
            lblStatusLeft.Text = "Salary updated successfully.";
        }
    }

    // ═══════════════════════════════════════════════════════════════
    // REPORTS & DIALOGS
    // ═══════════════════════════════════════════════════════════════

    private void ShowPayroll()
    {
        using var dlg = new PayrollForm(_manager);
        dlg.ShowDialog();
    }

    private void ShowStatistics()
    {
        using var dlg = new StatisticsForm(_manager);
        dlg.ShowDialog();
    }

    private void ShowAbout()
    {
        using var dlg = new AboutForm();
        dlg.ShowDialog(this);
    }

    private void ExportCsv()
    {
        using var dlg = new SaveFileDialog
        {
            Filter = "CSV Files (*.csv)|*.csv",
            DefaultExt = "csv",
            FileName = $"employees_export_{DateTime.Now:yyyyMMdd_HHmmss}"
        };

        if (dlg.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllText(dlg.FileName, _manager.ExportToCsv());
            MessageBox.Show($"Successfully exported to:\n\n{dlg.FileName}", "Export Complete", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            lblStatusLeft.Text = "Export completed successfully.";
        }
    }

    private void ManageAttendance()
    {
        var emp = GetSelectedEmployee();
        if (emp is null)
        {
            MessageBox.Show("Please select an employee from the list first.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var dlg = new AttendanceForm(emp, _manager);
        dlg.ShowDialog();
    }
}
