using System.Drawing.Drawing2D;

namespace EmployeeManagementSystem;

public class AboutForm : Form
{
    public AboutForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "About";
        Size = new Size(520, 420);
        MinimumSize = new Size(500, 380);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        BackColor = Color.White;
        Font = new Font("Segoe UI", 10);

        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(20),
            BackColor = Color.White,
        };
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // header
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // description
        panel.RowStyles.Add(new RowStyle(SizeType.AutoSize)); // copyright/contact
        panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // spacer for button alignment

        // Header (photo + basic info)
        var header = new TableLayoutPanel
        {
            ColumnCount = 2,
            RowCount = 1,
            Dock = DockStyle.Top,
            AutoSize = true,
        };
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
        header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        var pic = new PictureBox
        {
            Size = new Size(100, 100),
            Dock = DockStyle.Fill,
            SizeMode = PictureBoxSizeMode.Zoom,
            Image = LoadPhotoOrPlaceholder()
        };

        var infoPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            AutoSize = true,
            WrapContents = false,
            Padding = new Padding(8, 0, 0, 0)
        };

        infoPanel.Controls.Add(new Label
        {
            Text = "Employee Management System",
            Font = new Font("Segoe UI Semibold", 13),
            AutoSize = true
        });
        infoPanel.Controls.Add(new Label
        {
            Text = "Version 1.2.0\nRelease Date: December 10, 2024",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(70, 70, 80),
            AutoSize = true
        });
        infoPanel.Controls.Add(new Label
        {
            Text = ".NET 7 | Windows Forms",
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(70, 70, 80),
            AutoSize = true
        });

        header.Controls.Add(pic, 0, 0);
        header.Controls.Add(infoPanel, 1, 0);

        var lblDescription = new Label
        {
            Text = "A streamlined solution for managing employees, payroll, vacation tracking, attendance, and analytics.",
            AutoSize = true,
            MaximumSize = new Size(460, 0),
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(60, 60, 70),
            Padding = new Padding(0, 12, 0, 8)
        };

        var lblCopyright = new Label
        {
            Text = "© 2025 Salman Ahmed.\nContact: 2442130255@ntu.edu.cn\nArtificial Intelligence School (CST - 2024)",
            AutoSize = true,
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(90, 90, 100),
            Padding = new Padding(0, 0, 0, 8)
        };

        var btnClose = new Button
        {
            Text = "Close",
            Anchor = AnchorStyles.Bottom,
            AutoSize = false,
            Size = new Size(100, 36),
            BackColor = Color.FromArgb(0, 120, 212),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI Semibold", 10),
            Cursor = Cursors.Hand,
            Margin = new Padding(0, 16, 0, 0)
        };
        btnClose.FlatAppearance.BorderSize = 0;
        btnClose.Click += (s, e) => Close();

        var buttonHost = new Panel
        {
            Dock = DockStyle.Fill,
            Height = 50
        };
        btnClose.Location = new Point((buttonHost.Width - btnClose.Width) / 2, 0);
        btnClose.Anchor = AnchorStyles.None;
        buttonHost.Controls.Add(btnClose);
        buttonHost.Resize += (s, e) =>
        {
            btnClose.Left = (buttonHost.Width - btnClose.Width) / 2;
            btnClose.Top = 0;
        };

        panel.Controls.Add(header, 0, 0);
        panel.Controls.Add(lblDescription, 0, 1);
        panel.Controls.Add(lblCopyright, 0, 2);
        panel.Controls.Add(buttonHost, 0, 3);

        Controls.Add(panel);
    }

    private Image LoadPhotoOrPlaceholder()
    {
        // Try loading an external photo if present
        var candidateFiles = new[] { "about.png" };
        foreach (var file in candidateFiles)
        {
            if (File.Exists(file))
            {
                try { return Image.FromFile(file); }
                catch { /* fallback to placeholder */ }
            }
        }
        return CreatePlaceholderImage(100, 100);
    }

    private Image CreatePlaceholderImage(int width, int height)
    {
        var bmp = new Bitmap(width, height);
        using var g = Graphics.FromImage(bmp);
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var brush = new LinearGradientBrush(new Rectangle(0, 0, width, height),
            Color.FromArgb(0, 120, 212), Color.FromArgb(0, 90, 170), LinearGradientMode.ForwardDiagonal);
        g.FillRectangle(brush, 0, 0, width, height);
        using var font = new Font("Segoe UI Semibold", 28);
        var text = "EMS";
        var size = g.MeasureString(text, font);
        g.DrawString(text, font, Brushes.White, (width - size.Width) / 2, (height - size.Height) / 2);
        return bmp;
    }
}
