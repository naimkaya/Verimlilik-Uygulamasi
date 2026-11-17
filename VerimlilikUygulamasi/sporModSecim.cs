using System;
using System.Drawing;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class sporModSecim : Form
    {
        private Label lblTitle;
        private Button btnKardiyo;
        private Button btnFitness;
        private Label lblKardiyoNote;
        private Label lblFitnessNote;
        private Button btnGeriDon;

        private Color kardiyoColor = Color.FromArgb(231, 76, 60);
        private Color fitnessColor = Color.FromArgb(230, 126, 34);

        // Kullanıcı ID'sini tutan değişken
        private int userId;

        // Düzenlenen Kurucu Metot: Artık dışarıdan userId alıyor
        public sporModSecim(int currentUserId)
        {
            this.userId = currentUserId;
            this.AutoScaleMode = AutoScaleMode.None; // Bulanıklık önlemi

            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(10, 25, 50);
            this.Text = "Spor Aktivite Mod Seçimi";

            lblTitle = new Label()
            {
                Text = "Yapacağınız Antrenman Türünü Seçiniz",
                Font = new Font("Segoe UI Black", 32, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                Height = 120
            };
            this.Controls.Add(lblTitle);

            btnKardiyo = new Button()
            {
                Text = "Kardiyo",
                Size = new Size(320, 80),
                Font = new Font("Segoe UI", 18, FontStyle.Bold)
            };
            btnKardiyo.Click += BtnKardiyo_Click;
            StyleButton(btnKardiyo, kardiyoColor);
            this.Controls.Add(btnKardiyo);

            lblKardiyoNote = new Label()
            {
                Text = "Koşu, yüzme ve bisiklet gibi yağ yakma ve kalp sağlığı odaklı antrenmanlarınızı takip edin.",
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.TopCenter,
                Size = new Size(320, 100)
            };
            this.Controls.Add(lblKardiyoNote);

            btnFitness = new Button()
            {
                Text = "Fitness / Ağırlık",
                Size = new Size(320, 80),
                Font = new Font("Segoe UI", 18, FontStyle.Bold)
            };
            btnFitness.Click += BtnFitness_Click;
            StyleButton(btnFitness, fitnessColor);
            this.Controls.Add(btnFitness);

            lblFitnessNote = new Label()
            {
                Text = "Ağırlık çalışmaları, kas geliştirme ve güç odaklı egzersizlerinizi takip edin.",
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.TopCenter,
                Size = new Size(320, 100)
            };
            this.Controls.Add(lblFitnessNote);

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

            btnGeriDon.BringToFront();

            this.Load += SporModSecim_Load;
            this.Resize += (s, e) => {
                SporModSecim_Load(s, e);
                this.Invalidate();
            };

            this.Paint += SporModSecim_Paint;
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

        private void SporModSecim_Load(object sender, EventArgs e)
        {
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            int spacingX = 140;
            int spacingY = 40;

            lblTitle.Width = formWidth;
            lblTitle.Location = new Point(0, 50);

            btnGeriDon.Location = new Point(formWidth - btnGeriDon.Width - 60, 40);
            btnGeriDon.BringToFront();

            int totalButtonWidth = btnKardiyo.Width + btnFitness.Width + spacingX;
            int startX = (formWidth - totalButtonWidth) / 2;
            int startY = formHeight / 2 - 100;

            btnKardiyo.Location = new Point(startX, startY);
            btnFitness.Location = new Point(startX + btnKardiyo.Width + spacingX, startY);

            lblKardiyoNote.Location = new Point(
                btnKardiyo.Left + (btnKardiyo.Width - lblKardiyoNote.Width) / 2,
                btnKardiyo.Bottom + spacingY);

            lblFitnessNote.Location = new Point(
                btnFitness.Left + (btnFitness.Width - lblFitnessNote.Width) / 2,
                btnFitness.Bottom + spacingY);
        }

        private void SporModSecim_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawCardBackground(e.Graphics, btnKardiyo, lblKardiyoNote);
            DrawCardBackground(e.Graphics, btnFitness, lblFitnessNote);
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

        private void BtnKardiyo_Click(object sender, EventArgs e)
        {
            this.Hide();
            // Süre seçimine giderken userId değerini gönderiyoruz
            var form = new sporSureSecim("Kardiyo", userId);
            form.ShowDialog();
            this.Close();
        }

        private void BtnFitness_Click(object sender, EventArgs e)
        {
            this.Hide();
            // Süre seçimine giderken userId değerini gönderiyoruz
            var form = new sporSureSecim("Fitness", userId);
            form.ShowDialog();
            this.Close();
        }

        private void BtnGeriDon_Click(object sender, EventArgs e)
        {
            this.Hide();
            var menuForm = new MenuForm(userId);
            menuForm.ShowDialog();
            this.Close();
        }
    }
}