using System;
using System.Drawing;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class sporSureSecim : Form
    {
        private string secilenMod;
        private int userId;

        private Label lblTitle;
        private Button btnGunluk;
        private Button btnHaftalik;
        private Label lblGunlukNote;
        private Label lblHaftalikNote;
        private Button btnGeriDon;

        private Color gunlukColor = Color.FromArgb(155, 89, 182);
        private Color haftalikColor = Color.FromArgb(41, 128, 185);

        public sporSureSecim(string mod, int currentUserId)
        {
            this.secilenMod = mod;
            this.userId = currentUserId;
            this.AutoScaleMode = AutoScaleMode.None;

            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(10, 25, 50);
            this.Text = "Spor Aktivite Mod Seçimi";

            lblTitle = new Label()
            {
                Text = string.Format("{0} Takibi İçin Mod Seçiniz", secilenMod),
                Font = new Font("Segoe UI Black", 32, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent,
                Height = 120
            };
            this.Controls.Add(lblTitle);

            btnGunluk = new Button()
            {
                Text = "Günlük Analiz",
                Size = new Size(320, 80),
                Font = new Font("Segoe UI", 18, FontStyle.Bold)
            };
            btnGunluk.Click += BtnGunluk_Click;
            StyleButton(btnGunluk, gunlukColor);
            this.Controls.Add(btnGunluk);

            lblGunlukNote = new Label()
            {
                Text = "Bu mod tek seferlik egzersiz süresi ve fizyolojik analiz yapmanıza yarar ve verileriniz kayıtlı kalmaz.",
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.TopCenter,
                Size = new Size(320, 100)
            };
            this.Controls.Add(lblGunlukNote);

            btnHaftalik = new Button()
            {
                Text = "Haftalık",
                Size = new Size(320, 80),
                Font = new Font("Segoe UI", 18, FontStyle.Bold)
            };
            btnHaftalik.Click += BtnHaftalik_Click;
            StyleButton(btnHaftalik, haftalikColor);
            this.Controls.Add(btnHaftalik);

            lblHaftalikNote = new Label()
            {
                Text = "Bu mod günlük spor verilerinizi saklar, 7 günlük süreç takibi yapar ve haftalık istatistiksel analiz sunar.",
                ForeColor = Color.LightGray,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = false,
                TextAlign = ContentAlignment.TopCenter,
                Size = new Size(320, 100)
            };
            this.Controls.Add(lblHaftalikNote);

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

            this.Load += SporSureSecim_Load;
            this.Resize += (s, e) => {
                SporSureSecim_Load(s, e);
                this.Invalidate();
            };

            this.Paint += SporSureSecim_Paint;
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

        private void SporSureSecim_Load(object sender, EventArgs e)
        {
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            int spacingX = 140;
            int spacingY = 40;

            lblTitle.Width = formWidth;
            lblTitle.Location = new Point(0, 50);

            btnGeriDon.Location = new Point(formWidth - btnGeriDon.Width - 60, 40);
            btnGeriDon.BringToFront();

            int totalButtonWidth = btnGunluk.Width + btnHaftalik.Width + spacingX;
            int startX = (formWidth - totalButtonWidth) / 2;
            int startY = formHeight / 2 - 100;

            btnGunluk.Location = new Point(startX, startY);
            btnHaftalik.Location = new Point(startX + btnGunluk.Width + spacingX, startY);

            lblGunlukNote.Location = new Point(
                btnGunluk.Left + (btnGunluk.Width - lblGunlukNote.Width) / 2,
                btnGunluk.Bottom + spacingY);

            lblHaftalikNote.Location = new Point(
                btnHaftalik.Left + (btnHaftalik.Width - lblHaftalikNote.Width) / 2,
                btnHaftalik.Bottom + spacingY);
        }

        private void SporSureSecim_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawCardBackground(e.Graphics, btnGunluk, lblGunlukNote);
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

        private void BtnGunluk_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (secilenMod == "Kardiyo")
            {
                var form = new sporKardiyo(userId);
                form.ShowDialog();
            }
            else if (secilenMod == "Fitness")
            {
                // NOT: Projenizdeki günlük fitness formunun adı neyse "sporFitness" yerine onu yazın
                var form = new sporFitness(userId);
                form.ShowDialog();
            }
            this.Close();
        }

        private void BtnHaftalik_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (secilenMod == "Kardiyo")
            {
                var form = new kardiyoHaftalik(userId);
                form.ShowDialog();
            }
            else if (secilenMod == "Fitness")
            {
                var form = new fitnessHaftalik(userId);
                form.ShowDialog();
            }
            this.Close();
        }

        private void BtnGeriDon_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new sporModSecim(userId);
            form.ShowDialog();
            this.Close();
        }
    }
}