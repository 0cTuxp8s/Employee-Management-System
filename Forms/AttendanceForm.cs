using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using System.Drawing.Drawing2D;

namespace EmployeeManagementSystem;

public class AttendanceForm : Form
{
    private readonly Employee _emp;
    private readonly EmployeeManager _manager;

    // Office hours: 8 AM to 6 PM
    private static readonly TimeSpan OfficeStart = new TimeSpan(8, 0, 0);
    private static readonly TimeSpan OfficeEnd = new TimeSpan(18, 0, 0);

    private DataGridView dgvAttendance = null!;
    private DateTimePicker dtpDate = null!;
    private ComboBox cmbStatus = null!;
    private DateTimePicker dtpCheckIn = null!;
    private DateTimePicker dtpCheckOut = null!;
    private TextBox txtNotes = null!;
    private DateTimePicker dtpMonth = null!;

    private Panel panelPresent = null!;
    private Panel panelAbsent = null!;
    private Panel panelLeave = null!;
    private Panel panelLate = null!;
    private Panel panelEarly = null!;
    private Panel panelRate = null!;
    private Panel panelChart = null!;

    private bool _isEditMode = false;

    public AttendanceForm(Employee emp, EmployeeManager manager)
    {
        _emp = emp;
        _manager = manager;
        InitializeComponent();
        LoadMonthData();
    }

    private void InitializeComponent()
    {
        Text = "Attendance Management";
        Size = new Size(1100, 750);
        MinimumSize = new Size(1050, 700);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Font = new Font("Segoe UI", 10);
        BackColor = Color.FromArgb(245, 247, 250);

        // Header
        var panelHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 80,
            BackColor = Color.FromArgb(0, 120, 212)
        };

        var lblName = new Label
        {
            Text = _emp.Name,
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 16),
            Location = new Point(24, 14),
            AutoSize = true
        };
        var lblMeta = new Label
        {
            Text = $"{_emp.Position} | {_emp.Department}",
            ForeColor = Color.FromArgb(200, 225, 255),
            Font = new Font("Segoe UI", 10),
            Location = new Point(24, 46),
            AutoSize = true
        };
        var lblOfficeHours = new Label
        {
            Text = "Office Hours: 8:00 AM - 6:00 PM",
            ForeColor = Color.FromArgb(180, 210, 255),
            Font = new Font("Segoe UI", 9),
            Location = new Point(850, 30),
            AutoSize = true
        };
        panelHeader.Controls.AddRange(new Control[] { lblName, lblMeta, lblOfficeHours });

        // Summary panel with stat cards and pie chart
        var panelSummary = new Panel
        {
            Dock = DockStyle.Top,
            Height = 140,
            BackColor = Color.White,
            Padding = new Padding(20, 16, 20, 12)
        };
        panelSummary.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(220, 225, 230), 1);
            e.Graphics.DrawLine(pen, 0, panelSummary.Height - 1, panelSummary.Width, panelSummary.Height - 1);
        };

        var lblMonth = new Label
        {
            Text = "Month:",
            Location = new Point(20, 18),
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 10)
        };
        dtpMonth = new DateTimePicker
        {
            Format = DateTimePickerFormat.Custom,
            CustomFormat = "MMMM yyyy",
            ShowUpDown = true,
            Location = new Point(85, 14),
            Width = 160,
            Font = new Font("Segoe UI", 10)
        };
        dtpMonth.ValueChanged += (s, e) => LoadMonthData();

        // Stat cards
        int cardX = 280;
        int cardY = 10;
        int cardW = 90;
        int cardH = 55;
        int cardGap = 10;

        panelPresent = CreateStatCard("Present", "0", cardX, cardY, cardW, cardH, Color.FromArgb(40, 167, 69));
        panelAbsent = CreateStatCard("Absent", "0", cardX + cardW + cardGap, cardY, cardW, cardH, Color.FromArgb(220, 53, 69));
        panelLeave = CreateStatCard("Leave", "0", cardX + (cardW + cardGap) * 2, cardY, cardW, cardH, Color.FromArgb(255, 193, 7));
        panelLate = CreateStatCard("Late", "0", cardX + (cardW + cardGap) * 3, cardY, cardW, cardH, Color.FromArgb(255, 127, 80));
        panelEarly = CreateStatCard("Early Out", "0", cardX + (cardW + cardGap) * 4, cardY, cardW, cardH, Color.FromArgb(138, 43, 226));
        panelRate = CreateStatCard("Rate", "0%", cardX + (cardW + cardGap) * 5, cardY, cardW, cardH, Color.FromArgb(0, 120, 212));

        // Mini pie chart
        panelChart = new Panel
        {
            Location = new Point(20, 75),
            Size = new Size(220, 55),
            BackColor = Color.Transparent
        };
        panelChart.Paint += PanelChart_Paint;

        panelSummary.Controls.AddRange(new Control[] { lblMonth, dtpMonth, panelPresent, panelAbsent, panelLeave, panelLate, panelEarly, panelRate, panelChart });

        // Input panel
        var panelInput = new Panel
        {
            Dock = DockStyle.Top,
            Height = 140,
            BackColor = Color.White,
            Padding = new Padding(20, 12, 20, 12)
        };
        panelInput.Paint += (s, e) =>
        {
            using var pen = new Pen(Color.FromArgb(220, 225, 230), 1);
            e.Graphics.DrawLine(pen, 0, panelInput.Height - 1, panelInput.Width, panelInput.Height - 1);
        };

        int row1Y = 12;
        int row2Y = 56;

        // Row 1: Date + Status
        panelInput.Controls.Add(new Label
        {
            Text = "Date:",
            Location = new Point(20, row1Y + 4),
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 10),
            ForeColor = Color.FromArgb(60, 60, 70)
        });
        dtpDate = new DateTimePicker
        {
            Location = new Point(80, row1Y),
            Width = 150,
            Font = new Font("Segoe UI", 10),
            Format = DateTimePickerFormat.Short
        };
        dtpDate.ValueChanged += (s, e) => LoadExistingEntry();
        panelInput.Controls.Add(dtpDate);

        panelInput.Controls.Add(new Label
        {
            Text = "Status:",
            Location = new Point(260, row1Y + 4),
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 10),
            ForeColor = Color.FromArgb(60, 60, 70)
        });
        cmbStatus = new ComboBox
        {
            Location = new Point(330, row1Y),
            Width = 140,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Font = new Font("Segoe UI", 10)
        };
        cmbStatus.DataSource = Enum.GetValues(typeof(AttendanceStatus));
        panelInput.Controls.Add(cmbStatus);

        // Row 2: Check-in + Check-out (always enabled, default to office hours)
        panelInput.Controls.Add(new Label
        {
            Text = "Check-in:",
            Location = new Point(20, row2Y + 4),
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 10),
            ForeColor = Color.FromArgb(60, 60, 70)
        });
        dtpCheckIn = new DateTimePicker
        {
            Format = DateTimePickerFormat.Time,
            ShowUpDown = true,
            Location = new Point(110, row2Y),
            Width = 120,
            Font = new Font("Segoe UI", 10),
            Value = DateTime.Today.Add(OfficeStart)
        };
        panelInput.Controls.Add(dtpCheckIn);

        panelInput.Controls.Add(new Label
        {
            Text = "Check-out:",
            Location = new Point(260, row2Y + 4),
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 10),
            ForeColor = Color.FromArgb(60, 60, 70)
        });
        dtpCheckOut = new DateTimePicker
        {
            Format = DateTimePickerFormat.Time,
            ShowUpDown = true,
            Location = new Point(360, row2Y),
            Width = 120,
            Font = new Font("Segoe UI", 10),
            Value = DateTime.Today.Add(OfficeEnd)
        };
        panelInput.Controls.Add(dtpCheckOut);

        panelInput.Controls.Add(new Label
        {
            Text = "Notes:",
            Location = new Point(510, row1Y + 4),
            AutoSize = true,
            Font = new Font("Segoe UI Semibold", 10),
            ForeColor = Color.FromArgb(60, 60, 70)
        });
        txtNotes = new TextBox
        {
            Location = new Point(570, row1Y),
            Width = 200,
            Height = 28,
            Font = new Font("Segoe UI", 10)
        };
        panelInput.Controls.Add(txtNotes);

        // Buttons
        var btnSave = new Button
        {
            Text = "Save",
            Location = new Point(800, row1Y),
            Size = new Size(110, 36),
            BackColor = Color.FromArgb(40, 167, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 10),
            Cursor = Cursors.Hand
        };
        btnSave.FlatAppearance.BorderSize = 0;
        btnSave.Click += (s, e) => SaveAttendance();

        var btnClear = new Button
        {
            Text = "Clear / New",
            Location = new Point(920, row1Y),
            Size = new Size(110, 36),
            BackColor = Color.FromArgb(108, 117, 125),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 10),
            Cursor = Cursors.Hand
        };
        btnClear.FlatAppearance.BorderSize = 0;
        btnClear.Click += (s, e) => ClearForm();

        var btnClose = new Button
        {
            Text = "Close",
            Location = new Point(920, row2Y),
            Size = new Size(110, 36),
            BackColor = Color.FromArgb(220, 53, 69),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 10),
            Cursor = Cursors.Hand
        };
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.Click += (s, e) => Close();

        var lblEditHint = new Label
        {
            Text = "Tip: Double-click a row in the grid to edit that entry.",
            Location = new Point(510, row2Y + 8),
            AutoSize = true,
            Font = new Font("Segoe UI", 9, FontStyle.Italic),
            ForeColor = Color.Gray
        };

        panelInput.Controls.AddRange(new Control[] { btnSave, btnClear, btnClose, lblEditHint });

        // Grid with improved styling
        dgvAttendance = new DataGridView
        {
            Dock = DockStyle.Fill,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            RowHeadersVisible = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            EnableHeadersVisualStyles = false,
            GridColor = Color.FromArgb(230, 235, 240)
        };

        dgvAttendance.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(0, 120, 212),
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 10),
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0)
        };
        dgvAttendance.ColumnHeadersHeight = 45;
        dgvAttendance.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

        dgvAttendance.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.White,
            ForeColor = Color.FromArgb(40, 40, 50),
            Font = new Font("Segoe UI", 10),
            SelectionBackColor = Color.FromArgb(220, 235, 252),
            SelectionForeColor = Color.Black,
            Padding = new Padding(8, 0, 0, 0)
        };
        dgvAttendance.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Color.FromArgb(248, 250, 252)
        };
        dgvAttendance.RowTemplate.Height = 40;

        // Double-click to edit entry
        dgvAttendance.CellDoubleClick += DgvAttendance_CellDoubleClick;

        // Add controls in correct dock order
        Controls.Add(dgvAttendance);
        Controls.Add(panelInput);
        Controls.Add(panelSummary);
        Controls.Add(panelHeader);
    }

    private Panel CreateStatCard(string title, string value, int x, int y, int w, int h, Color color)
    {
        var card = new Panel
        {
            Location = new Point(x, y),
            Size = new Size(w, h),
            BackColor = color
        };

        var lblTitle = new Label
        {
            Text = title,
            ForeColor = Color.FromArgb(255, 255, 255, 200),
            Font = new Font("Segoe UI", 8),
            Location = new Point(8, 6),
            AutoSize = true
        };

        var lblValue = new Label
        {
            Text = value,
            ForeColor = Color.White,
            Font = new Font("Segoe UI Semibold", 14),
            Location = new Point(8, 24),
            AutoSize = true,
            Tag = "value"
        };

        card.Controls.AddRange(new Control[] { lblTitle, lblValue });
        return card;
    }

    private void UpdateStatCard(Panel card, string value)
    {
        foreach (Control c in card.Controls)
        {
            if (c is Label lbl && lbl.Tag?.ToString() == "value")
            {
                lbl.Text = value;
                break;
            }
        }
    }

    private void PanelChart_Paint(object? sender, PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        var summary = _manager.GetAttendanceSummaryForMonth(_emp.Id, dtpMonth.Value.Year, dtpMonth.Value.Month);
        int total = summary.Present + summary.Absent + summary.Leave + summary.Late + summary.EarlyLeave;

        if (total == 0)
        {
            g.DrawString("No attendance data", new Font("Segoe UI", 9), Brushes.Gray, 10, 18);
            return;
        }

        var data = new (int count, Color color, string label)[]
        {
            (summary.Present, Color.FromArgb(40, 167, 69), "Present"),
            (summary.Absent, Color.FromArgb(220, 53, 69), "Absent"),
            (summary.Leave, Color.FromArgb(255, 193, 7), "Leave"),
            (summary.Late, Color.FromArgb(255, 127, 80), "Late"),
            (summary.EarlyLeave, Color.FromArgb(138, 43, 226), "Early")
        };

        float startAngle = -90;
        int size = 45;
        int cx = 5, cy = 5;

        foreach (var (count, color, label) in data)
        {
            if (count == 0) continue;
            float sweep = 360f * count / total;
            using var brush = new SolidBrush(color);
            g.FillPie(brush, cx, cy, size, size, startAngle, sweep);
            startAngle += sweep;
        }

        // Legend
        int legendX = 60;
        int legendY = 5;
        int legendGap = 10;
        int i = 0;
        foreach (var (count, color, label) in data)
        {
            if (count == 0) continue;
            using var brush = new SolidBrush(color);
            g.FillRectangle(brush, legendX, legendY + i * legendGap, 8, 8);
            g.DrawString($"{label}: {count}", new Font("Segoe UI", 7.5f), Brushes.Black, legendX + 12, legendY + i * legendGap - 1);
            i++;
        }
    }

    private void DgvAttendance_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;

        var dateStr = dgvAttendance.Rows[e.RowIndex].Cells["Date"].Value?.ToString();
        if (DateTime.TryParse(dateStr, out var date))
        {
            dtpDate.Value = date;
            LoadExistingEntry();
            _isEditMode = true;
        }
    }

    private void LoadExistingEntry()
    {
        var date = dtpDate.Value.Date;
        var record = _emp.Attendance.FirstOrDefault(a => a.Date.Date == date);

        if (record is not null)
        {
            cmbStatus.SelectedItem = record.Status;
            dtpCheckIn.Value = DateTime.Today.Add(record.CheckIn ?? OfficeStart);
            dtpCheckOut.Value = DateTime.Today.Add(record.CheckOut ?? OfficeEnd);
            txtNotes.Text = record.Notes;
            _isEditMode = true;
        }
        else
        {
            ClearFormValues();
            _isEditMode = false;
        }
    }

    private void ClearForm()
    {
        dtpDate.Value = DateTime.Today;
        ClearFormValues();
        _isEditMode = false;
    }

    private void ClearFormValues()
    {
        cmbStatus.SelectedIndex = 0;
        dtpCheckIn.Value = DateTime.Today.Add(OfficeStart);
        dtpCheckOut.Value = DateTime.Today.Add(OfficeEnd);
        txtNotes.Clear();
    }

    private void SaveAttendance()
    {
        var status = (AttendanceStatus)cmbStatus.SelectedItem!;
        var date = dtpDate.Value.Date;
        var checkIn = dtpCheckIn.Value.TimeOfDay;
        var checkOut = dtpCheckOut.Value.TimeOfDay;
        var notes = txtNotes.Text;

        // Validate times
        if (checkOut <= checkIn)
        {
            MessageBox.Show("Check-out time must be after check-in time.", "Validation Error",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        _manager.MarkAttendance(_emp.Id, date, status, checkIn, checkOut, notes);
        LoadMonthData();

        string action = _isEditMode ? "updated" : "saved";
        MessageBox.Show($"Attendance {action} successfully.", "Success",
            MessageBoxButtons.OK, MessageBoxIcon.Information);

        _isEditMode = false;
    }

    private void LoadMonthData()
    {
        var month = dtpMonth.Value;
        var list = _manager.GetAttendanceForMonth(_emp.Id, month.Year, month.Month)
            .Select(a => new
            {
                Date = a.Date.ToString("yyyy-MM-dd"),
                Day = a.Date.ToString("ddd"),
                Status = a.Status.ToString(),
                CheckIn = a.CheckIn.HasValue ? DateTime.Today.Add(a.CheckIn.Value).ToString("hh:mm tt") : "-",
                CheckOut = a.CheckOut.HasValue ? DateTime.Today.Add(a.CheckOut.Value).ToString("hh:mm tt") : "-",
                Hours = (a.CheckIn.HasValue && a.CheckOut.HasValue)
                    ? $"{(a.CheckOut.Value - a.CheckIn.Value).TotalHours:F1}h"
                    : "-",
                Notes = string.IsNullOrWhiteSpace(a.Notes) ? "-" : a.Notes
            })
            .ToList();

        dgvAttendance.DataSource = list;

        // Apply column formatting
        if (dgvAttendance.Columns.Count > 0)
        {
            dgvAttendance.Columns["Date"].FillWeight = 14;
            dgvAttendance.Columns["Day"].FillWeight = 8;
            dgvAttendance.Columns["Status"].FillWeight = 12;
            dgvAttendance.Columns["CheckIn"].FillWeight = 12;
            dgvAttendance.Columns["CheckOut"].FillWeight = 12;
            dgvAttendance.Columns["Hours"].FillWeight = 8;
            dgvAttendance.Columns["Notes"].FillWeight = 26;
        }

        UpdateSummary(month.Year, month.Month);
        panelChart?.Invalidate();
    }

    private void UpdateSummary(int year, int month)
    {
        var summary = _manager.GetAttendanceSummaryForMonth(_emp.Id, year, month);

        UpdateStatCard(panelPresent, summary.Present.ToString());
        UpdateStatCard(panelAbsent, summary.Absent.ToString());
        UpdateStatCard(panelLeave, summary.Leave.ToString());
        UpdateStatCard(panelLate, summary.Late.ToString());
        UpdateStatCard(panelEarly, summary.EarlyLeave.ToString());
        UpdateStatCard(panelRate, $"{summary.AttendancePercentage:F0}%");
    }
}
