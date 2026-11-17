using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class MenuForm : Form
    {
        private Panel pnlTop;
        private Label lblHeader;
        private Button btnGeriDon;
        private TableLayoutPanel tableLayoutPanel;

        private Button btnBeslenme, btnUyku, btnSu, btnElektrik, btnTelefon, btnSpor;

        // Oturum açan kullanıcının id değerini saklayacak değişken
        private int userId;

        // MenuForm artık zorunlu olarak bir userId bekliyor (Hataları önlemek için varsayılan olarak 1 atandı)
        public MenuForm(int currentUserId = 1)
        {
            this.userId = currentUserId;
            this.AutoScaleMode = AutoScaleMode.None; // Bulanıklık Önlemi

            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Ana Menü";
            this.BackColor = Color.FromArgb(10, 25, 50);

            pnlTop = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 130,
                BackColor = Color.Transparent
            };
            this.Controls.Add(pnlTop);

            btnGeriDon = new Button()
            {
                Text = "Geri Dön",
                Size = new Size(120, 45),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(160, 160, 160),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(this.ClientSize.Width - 150, 35)
            };
            btnGeriDon.FlatAppearance.BorderSize = 0;
            btnGeriDon.Click += BtnGeriDon_Click;
            pnlTop.Controls.Add(btnGeriDon);

            pnlTop.Resize += (s, e) => btnGeriDon.Location = new Point(pnlTop.Width - 150, 35);

            lblHeader = new Label()
            {
                Text = "",
                Font = new Font("Segoe UI Black", 54, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            lblHeader.Paint += LblHeader_Paint;
            pnlTop.Controls.Add(lblHeader);
            lblHeader.BringToFront();
            btnGeriDon.BringToFront();

            tableLayoutPanel = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 2,
                Padding = new Padding(60, 20, 60, 60),
                BackColor = Color.Transparent
            };
            this.Controls.Add(tableLayoutPanel);
            tableLayoutPanel.BringToFront();

            for (int i = 0; i < 3; i++)
                tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            for (int i = 0; i < 2; i++)
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            btnBeslenme = CreateCategoryButton("Beslenme", Color.FromArgb(30, 87, 153));
            btnUyku = CreateCategoryButton("Uyku", Color.FromArgb(39, 174, 96));
            btnSu = CreateCategoryButton("Su", Color.FromArgb(41, 128, 185));
            btnElektrik = CreateCategoryButton("Elektrik", Color.FromArgb(241, 196, 15));
            btnTelefon = CreateCategoryButton("Telefon Kullanımı", Color.FromArgb(155, 89, 182));
            btnSpor = CreateCategoryButton("Spor", Color.FromArgb(231, 76, 60));

            tableLayoutPanel.Controls.Add(btnBeslenme, 0, 0);
            tableLayoutPanel.Controls.Add(btnUyku, 1, 0);
            tableLayoutPanel.Controls.Add(btnSu, 2, 0);
            tableLayoutPanel.Controls.Add(btnElektrik, 0, 1);
            tableLayoutPanel.Controls.Add(btnTelefon, 1, 1);
            tableLayoutPanel.Controls.Add(btnSpor, 2, 1);

            btnBeslenme.Click += BtnBeslenme_Click;
            btnUyku.Click += BtnUyku_Click;
            btnSu.Click += BtnSu_Click;
            btnElektrik.Click += BtnElektrik_Click;
            btnTelefon.Click += BtnTelefon_Click;
            btnSpor.Click += BtnSpor_Click;
        }

        private Button CreateCategoryButton(string text, Color baseColor)
        {
            Button btn = new Button()
            {
                Text = text,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                BackColor = baseColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Margin = new Padding(20),
                Cursor = Cursors.Hand
            };

            btn.FlatAppearance.BorderSize = 0;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = ControlPaint.Light(baseColor, 0.2f);
                btn.FlatAppearance.BorderColor = Color.White;
                btn.FlatAppearance.BorderSize = 5;
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = baseColor;
                btn.FlatAppearance.BorderSize = 0;
            };

            return btn;
        }

        private void BtnBeslenme_Click(object sender, EventArgs e)
        {
            this.Hide();
            using var beslenmeModSecim = new beslenmeModSecim();
            beslenmeModSecim.ShowDialog();
            this.Show();
        }

        private void BtnUyku_Click(object sender, EventArgs e)
        {
            this.Hide();
            using var uykuModSecim = new uykuModSecim();
            uykuModSecim.ShowDialog();
            this.Show();
        }

        private void BtnSu_Click(object sender, EventArgs e)
        {
            this.Hide();
            using var suModForm = new suModSecim();
            suModForm.ShowDialog();
            this.Show();
        }

        private void BtnElektrik_Click(object sender, EventArgs e)
        {
            this.Hide();
            using var elektrikForm = new elektrikModSecim();
            elektrikForm.ShowDialog();
            this.Show();
        }

        private void BtnTelefon_Click(object sender, EventArgs e)
        {
            this.Hide();
            using var telefonForm = new telefonModSecim();
            telefonForm.ShowDialog();
            this.Show();
        }

        private void BtnSpor_Click(object sender, EventArgs e)
        {
            this.Hide();
            // Spor mod seçimine giderken kullanıcının gerçek ID bilgisini paslıyoruz
            using var sporForm = new sporModSecim(userId);
            sporForm.ShowDialog();
            this.Show();
        }

        private void BtnGeriDon_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form1 = new Form1();
            form1.ShowDialog();
            this.Close();
        }

        private void LblHeader_Paint(object sender, PaintEventArgs e)
        {
            if (sender is not Label lbl) return;

            string text = "Kategoriler";

            using GraphicsPath path = new GraphicsPath();
            StringFormat sf = new() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

            path.AddString(text, lbl.Font.FontFamily, (int)lbl.Font.Style,
                e.Graphics.DpiY * lbl.Font.Size / 72, lbl.ClientRectangle, sf);

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using var pen = new Pen(Color.Black, 8) { LineJoin = LineJoin.Round };
            e.Graphics.DrawPath(pen, path);
            e.Graphics.FillPath(Brushes.White, path);
        }
    }
}