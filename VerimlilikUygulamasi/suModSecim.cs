using System;
using System.Drawing;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class suModSecim : Form
    {
        private Label lblTitle;
        private Button btnTekSeferlik;
        private Button btnHaftalik;
        private Label lblTekSeferlikNote;
        private Label lblHaftalikNote;
        private Button btnGeriDon;

        private Color tekSeferlikColor = Color.FromArgb(41, 128, 185); // Modern Mavi
        private Color haftalikColor = Color.FromArgb(39, 174, 96);     // Modern Yeşil

        public suModSecim()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(10, 25, 50);
            this.Text = "Su Mod Seçimi";

            // 1. BAŞLIK
            lblTitle = new Label()
            {
                Text = "Su Kategorisi İçin Mod Seçiniz",
                Font = new Font("Segoe UI Black", 32, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                Height = 120
            };
            this.Controls.Add(lblTitle);

            // 2. TEK SEFERLİK BUTONU
            btnTekSeferlik = new Button()
            {
                Text = "Tek Seferlik",
                Size = new Size(250, 80),
                Font = new Font("Segoe UI", 18, FontStyle.Bold)
            };
            btnTekSeferlik.Click += BtnTekSeferlik_Click;
            StyleButton(btnTekSeferlik, tekSeferlikColor);
            this.Controls.Add(btnTekSeferlik);

            // 3. TEK SEFERLİK AÇIKLAMASI
            lblTekSeferlikNote = new Label()
            {
                Text = "Bu mod tek seferlik işlem yapmanıza yarar ve verileriniz kayıtlı kalmaz.",
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 11),
                AutoSize = false,
                TextAlign = ContentAlignment.TopCenter,
                Size = new Size(350, 100)
            };
            this.Controls.Add(lblTekSeferlikNote);

            // 4. HAFTALIK BUTONU
            btnHaftalik = new Button()
            {
                Text = "Haftalık",
                Size = new Size(250, 80),
                Font = new Font("Segoe UI", 18, FontStyle.Bold)
            };
            btnHaftalik.Click += BtnHaftalik_Click;
            StyleButton(btnHaftalik, haftalikColor);
            this.Controls.Add(btnHaftalik);

            // 5. HAFTALIK AÇIKLAMASI
            lblHaftalikNote = new Label()
            {
                Text = "Bu mod günlük olarak verilerinizi saklar ve haftalık bilgilerinizi görmenize yardımcı olur. Not: Verilerinizi gün sonu girmeye özen gösteriniz",
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 11),
                AutoSize = false,
                TextAlign = ContentAlignment.TopCenter,
                Size = new Size(350, 100)
            };
            this.Controls.Add(lblHaftalikNote);

            // 6. GERİ DÖN BUTONU
            btnGeriDon = new Button()
            {
                Text = "Geri Dön",
                Size = new Size(120, 45),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(160, 160, 160),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnGeriDon.FlatAppearance.BorderSize = 0;
            btnGeriDon.Click += BtnGeriDon_Click;
            this.Controls.Add(btnGeriDon);

            // Ekranda butonun en önde görünmesini garanti ediyoruz
            btnGeriDon.BringToFront();

            this.Load += suModSecim_Load;
            this.Resize += (s, e) => {
                suModSecim_Load(s, e);
                this.Invalidate();
            };

            this.Paint += SuModSecim_Paint;
        }

        private void StyleButton(Button btn, Color baseColor)
        {
            btn.BackColor = baseColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Cursor = Cursors.Hand;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = ControlPaint.Light(baseColor, 0.2f);
                btn.FlatAppearance.BorderColor = Color.White;
                btn.FlatAppearance.BorderSize = 3;
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = baseColor;
                btn.FlatAppearance.BorderSize = 0;
            };
        }

        private void suModSecim_Load(object sender, EventArgs e)
        {
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            int spacingX = 180;
            int spacingY = 25;

            lblTitle.Width = formWidth;
            lblTitle.Location = new Point(0, 50);

            // DÜZELTME: Butonun tam görünmesi için koordinatlar güvenli alana çekildi
            btnGeriDon.Location = new Point(formWidth - btnGeriDon.Width - 60, 40);
            btnGeriDon.BringToFront();

            int totalButtonWidth = btnTekSeferlik.Width + btnHaftalik.Width + spacingX;
            int startX = (formWidth - totalButtonWidth) / 2;
            int startY = formHeight / 2 - 100;

            btnTekSeferlik.Location = new Point(startX, startY);
            btnHaftalik.Location = new Point(startX + btnTekSeferlik.Width + spacingX, startY);

            lblTekSeferlikNote.Location = new Point(btnTekSeferlik.Left + (btnTekSeferlik.Width - lblTekSeferlikNote.Width) / 2, btnTekSeferlik.Bottom + spacingY);
            lblHaftalikNote.Location = new Point(btnHaftalik.Left + (btnHaftalik.Width - lblHaftalikNote.Width) / 2, btnHaftalik.Bottom + spacingY);
        }

        private void SuModSecim_Paint(object sender, PaintEventArgs e)
        {
            // DÜZELTME: Başlığın altındaki çizme kodu tamamen kaldırıldı.
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Sadece şık kart arka planları çiziliyor
            DrawCardBackground(e.Graphics, btnTekSeferlik, lblTekSeferlikNote);
            DrawCardBackground(e.Graphics, btnHaftalik, lblHaftalikNote);
        }

        private void DrawCardBackground(Graphics g, Button btn, Label lbl)
        {
            int padding = 40;
            int x = Math.Min(btn.Left, lbl.Left) - padding;
            int y = btn.Top - padding;
            int right = Math.Max(btn.Right, lbl.Right) + padding;
            int bottom = lbl.Bottom + padding;

            Rectangle cardRect = new Rectangle(x, y, right - x, bottom - y);

            using (SolidBrush brush = new SolidBrush(Color.FromArgb(12, 255, 255, 255)))
            {
                g.FillRectangle(brush, cardRect);
            }
            using (Pen pen = new Pen(Color.FromArgb(25, 255, 255, 255), 1))
            {
                g.DrawRectangle(pen, cardRect);
            }
        }

        private void BtnTekSeferlik_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new suGunluk();
            form.ShowDialog();
            this.Close();
        }

        private void BtnHaftalik_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new suHaftalik();
            form.ShowDialog();
            this.Close();
        }

        private void BtnGeriDon_Click(object sender, EventArgs e)
        {
            this.Hide();
            var menuForm = new MenuForm();
            menuForm.ShowDialog();
            this.Close();
        }
    }
}