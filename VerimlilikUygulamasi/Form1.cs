using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.Data.Sqlite;

namespace VerimlilikUygulamasi
{
    public partial class Form1 : Form
    {
        private Label lblTitle;
        private Button btnGiris, btnKayit, btnCikis;
        private Image? backgroundImg;

        public Form1()
        {
            // Çözünürlük Bağımsızlığı (DPI Aware) ve Başlangıç Tam Ekran Ayarları
            this.AutoScaleMode = AutoScaleMode.None;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "Verimlilik Uygulaması";
            this.BackColor = Color.FromArgb(10, 25, 50);
            this.DoubleBuffered = true;

            // F11 Tuşunu Yakalamak İçin Gerekli Ayarlar
            this.KeyPreview = true;
            this.KeyDown += Form1_KeyDown;

            try { backgroundImg = Image.FromFile(@"C:\background.jpg"); }
            catch { backgroundImg = null; }

            lblTitle = new Label()
            {
                Text = "",
                Font = new Font("Segoe UI Black", 48, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Size = new Size(2222, 100)
            };
            lblTitle.Paint += LblTitle_Paint;
            Controls.Add(lblTitle);

            CreateButtons();

            this.Load += Form1_Layout;
            this.Resize += Form1_Layout;
            this.Paint += Form1_Paint;
        }

        private void CreateButtons()
        {
            btnGiris = CreateButton("Giriş Yap", 260, 80, Color.FromArgb(42, 144, 210), Color.FromArgb(28, 107, 160), BtnGiris_Click);
            btnKayit = CreateButton("Kayıt Ol", 260, 80, Color.FromArgb(0, 168, 144), Color.FromArgb(0, 126, 111), BtnKayit_Click);
            btnCikis = CreateButton("Çıkış Yap", 140, 50, Color.FromArgb(220, 50, 50), Color.FromArgb(180, 20, 20), (s, e) => Application.Exit());

            Controls.Add(btnGiris);
            Controls.Add(btnKayit);
            Controls.Add(btnCikis);
        }

        private Button CreateButton(string text, int width, int height, Color topColor, Color bottomColor, EventHandler onClick)
        {
            Button btn = new Button()
            {
                Text = text,
                Size = new Size(width, height),
                Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Paint += (s, e) => PaintButtonGradient(s, e, topColor, bottomColor, 8);
            btn.MouseEnter += (s, e) => btn.ForeColor = Color.LightYellow;
            btn.MouseLeave += (s, e) => btn.ForeColor = Color.White;
            btn.Click += onClick;
            return btn;
        }

        private void Form1_Layout(object? sender, EventArgs e)
        {
            int w = ClientSize.Width;
            int h = ClientSize.Height;
            lblTitle.Location = new Point((w - lblTitle.Width) / 2, h / 5);
            btnGiris.Location = new Point((w - btnGiris.Width) / 2, lblTitle.Bottom + 80);
            btnKayit.Location = new Point((w - btnKayit.Width) / 2, btnGiris.Bottom + 40);
            btnCikis.Location = new Point(w - btnCikis.Width - 20, h - btnCikis.Height - 20);
            ApplyRoundedRegion(btnGiris, 8);
            ApplyRoundedRegion(btnKayit, 8);
            ApplyRoundedRegion(btnCikis, 8);
        }

        private void ApplyRoundedRegion(Control ctrl, int radius)
        {
            if (ctrl == null || ctrl.Width <= 0 || ctrl.Height <= 0) return;
            ctrl.Region = new Region(GetRoundedRectanglePath(new Rectangle(0, 0, ctrl.Width, ctrl.Height), radius));
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new();
            int d = radius * 2;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void LblTitle_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is not Label lbl) return;
            string text = "Verimlilik Uygulamasına Hoş Geldiniz!";
            Rectangle rect = lbl.ClientRectangle;
            StringFormat format = new() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using GraphicsPath path = new();
            path.AddString(text, lbl.Font.FontFamily, (int)lbl.Font.Style, e.Graphics.DpiY * lbl.Font.Size / 72, rect, format);
            using Pen pen = new(Color.Black, 6) { LineJoin = LineJoin.Round };
            e.Graphics.DrawPath(pen, path);
            using Brush brush = new SolidBrush(Color.White);
            e.Graphics.FillPath(brush, path);
        }

        private void Form1_Paint(object? sender, PaintEventArgs e)
        {
            if (backgroundImg != null)
            {
                e.Graphics.DrawImage(backgroundImg, ClientRectangle);
                using Brush overlay = new SolidBrush(Color.FromArgb(120, 10, 25, 50));
                e.Graphics.FillRectangle(overlay, ClientRectangle);
            }
        }

        private void PaintButtonGradient(object? sender, PaintEventArgs e, Color topColor, Color bottomColor, int radius)
        {
            if (sender is not Button btn) return;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = btn.ClientRectangle;
            using LinearGradientBrush brush = new(rect, topColor, bottomColor, LinearGradientMode.Vertical);
            e.Graphics.FillPath(brush, GetRoundedRectanglePath(rect, radius));
            TextRenderer.DrawText(e.Graphics, btn.Text, btn.Font, rect, btn.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            using Pen pen = new(Color.White, 2);
            e.Graphics.DrawPath(pen, GetRoundedRectanglePath(rect, radius));
        }

        // F11 Gerçek Tam Ekran Tetikleyicisi
        private void Form1_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                if (this.FormBorderStyle == FormBorderStyle.None)
                {
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Normal;
                    this.WindowState = FormWindowState.Maximized;
                }
                e.Handled = true;
            }
        }

        private void BtnGiris_Click(object? sender, EventArgs e)
        {
            using (GirisFormu giris = new GirisFormu())
            {
                if (giris.ShowDialog() == DialogResult.OK)
                {
                    this.Hide();
                    using (MenuForm menu = new MenuForm())
                    {
                        menu.ShowDialog();
                    }
                    this.Show();
                }
            }
        }

        private void BtnKayit_Click(object? sender, EventArgs e)
        {
            using (KayitFormu kayit = new KayitFormu())
            {
                kayit.ShowDialog();
            }
        }

        [STAThread]
        static void Main()
        {
            InitializeDatabase();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        static void InitializeDatabase()
        {
            using var connection = new SqliteConnection("Data Source=verimlilik.db");
            connection.Open();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Users (UserId INTEGER PRIMARY KEY AUTOINCREMENT, Gmail TEXT UNIQUE NOT NULL, KullAdi TEXT NOT NULL, Sifre TEXT NOT NULL, Kilogram REAL, BoyCm REAL, HareketDurumu TEXT);
                CREATE TABLE IF NOT EXISTS BeslenmeVerileri (Id INTEGER PRIMARY KEY AUTOINCREMENT, UserId INTEGER, Tarih TEXT, Kalori REAL, Protein REAL, Karbonhidrat REAL, Yag REAL, FOREIGN KEY(UserId) REFERENCES Users(UserId));
                CREATE TABLE IF NOT EXISTS UykuVerileri (Id INTEGER PRIMARY KEY AUTOINCREMENT, UserId INTEGER, Tarih TEXT, BaslamaSaati TEXT, BitisSaati TEXT, UykudaKesilme INTEGER, FOREIGN KEY(UserId) REFERENCES Users(UserId));
                CREATE TABLE IF NOT EXISTS SuVerileri (Id INTEGER PRIMARY KEY AUTOINCREMENT, UserId INTEGER, Tarih TEXT, SuLitresi REAL, FOREIGN KEY(UserId) REFERENCES Users(UserId));
                CREATE TABLE IF NOT EXISTS ElektrikVerileri (Id INTEGER PRIMARY KEY AUTOINCREMENT, UserId INTEGER, Tarih TEXT, ElektrikKWh REAL, KisiSayisi INTEGER, CihazSayisi INTEGER, FOREIGN KEY(UserId) REFERENCES Users(UserId));
                CREATE TABLE IF NOT EXISTS TelefonKullanimi (Id INTEGER PRIMARY KEY AUTOINCREMENT, UserId INTEGER, Tarih TEXT, IsSaatleri REAL, SosyalMedyaSaat REAL, FOREIGN KEY(UserId) REFERENCES Users(UserId));
                CREATE TABLE IF NOT EXISTS SporVerileri (Id INTEGER PRIMARY KEY AUTOINCREMENT, UserId INTEGER, Tarih TEXT, KasGrubu TEXT, Hareketler TEXT, AntrenmanSuresi REAL, FOREIGN KEY(UserId) REFERENCES Users(UserId));";
            command.ExecuteNonQuery();
        }
    }
}