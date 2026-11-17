using System;
using System.Drawing;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class beslenmeModSecim : Form
    {
        private Color tekSeferlikColor = Color.FromArgb(41, 128, 185);
        private Color haftalikColor = Color.FromArgb(39, 174, 96);

        public beslenmeModSecim()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(10, 25, 50);
            this.Text = "Beslenme Mod Seçimi";

            StyleButton(btnTekSeferlik, tekSeferlikColor);
            StyleButton(btnHaftalik, haftalikColor);

            this.Load += beslenmeModSecim_Load;

            // Ekran boyutu değiştiğinde tasarımların bozulmaması için yeniden çizdiriyoruz
            this.Resize += (s, e) => {
                beslenmeModSecim_Load(s, e);
                this.Invalidate(); // Arka plandaki şık kartların yeniden çizilmesini sağlar
            };

            // YARATICI DOKUNUŞ: Arka plana modern şekiller çizecek olayı bağlıyoruz
            this.Paint += BeslenmeModSecim_Paint;
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

        private void beslenmeModSecim_Load(object sender, EventArgs e)
        {
            int formWidth = this.ClientSize.Width;
            int formHeight = this.ClientSize.Height;

            // Başlığı ve Geri Dön butonunu hizalama
            lblTitle.Width = formWidth;
            btnGeriDon.Location = new Point(formWidth - btnGeriDon.Width - 40, 30);

            // Seçenekleri tam ortaya gruplama
            int spacingX = 180;
            int spacingY = 25;

            int totalBtnWidth = btnTekSeferlik.Width + btnHaftalik.Width + spacingX;
            int startX = (formWidth - totalBtnWidth) / 2;
            int startY = formHeight / 2 - 100; // Ekranın tam ortasına yakın bir yere sabitle

            btnTekSeferlik.Location = new Point(startX, startY);
            btnHaftalik.Location = new Point(startX + btnTekSeferlik.Width + spacingX, startY);

            lblTekSeferlikNote.Location = new Point(
                btnTekSeferlik.Left + (btnTekSeferlik.Width - lblTekSeferlikNote.Width) / 2,
                btnTekSeferlik.Bottom + spacingY);

            lblHaftalikNote.Location = new Point(
                btnHaftalik.Left + (btnHaftalik.Width - lblHaftalikNote.Width) / 2,
                btnHaftalik.Bottom + spacingY);
        }

        // =========================================================
        // EKRANIN BOŞLUĞUNU DOLDURAN YARATICI ARKA PLAN ÇİZİMLERİ
        // =========================================================
        private void BeslenmeModSecim_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 1. Başlığın altına estetik ve ince bir çizgi çiz
            int lineY = lblTitle.Bottom - 10;
            int lineWidth = 800;
            int lineX = (this.ClientSize.Width - lineWidth) / 2;

            using (Pen p = new Pen(Color.FromArgb(40, 255, 255, 255), 2)) // Yarı saydam beyaz
            {
                e.Graphics.DrawLine(p, lineX, lineY, lineX + lineWidth, lineY);
            }

            // 2. Butonların ve açıklamaların arkasına modern "Kart" görünümleri çiz
            DrawCardBackground(e.Graphics, btnTekSeferlik, lblTekSeferlikNote);
            DrawCardBackground(e.Graphics, btnHaftalik, lblHaftalikNote);
        }

        // Kart çizen özel metod
        private void DrawCardBackground(Graphics g, Button btn, Label lbl)
        {
            int padding = 40; // Kartın iç boşluğu

            // Kartın koordinatlarını buton ve metne göre hesapla
            int x = Math.Min(btn.Left, lbl.Left) - padding;
            int y = btn.Top - padding;
            int right = Math.Max(btn.Right, lbl.Right) + padding;
            int bottom = lbl.Bottom + padding;

            Rectangle cardRect = new Rectangle(x, y, right - x, bottom - y);

            // Kartın içi (Çok hafif, cama benzeyen yarı saydam beyaz bir arka plan)
            using (SolidBrush brush = new SolidBrush(Color.FromArgb(12, 255, 255, 255)))
            {
                g.FillRectangle(brush, cardRect);
            }

            // Kartın dış çerçevesi (İnce, zarif bir sınır)
            using (Pen pen = new Pen(Color.FromArgb(25, 255, 255, 255), 1))
            {
                g.DrawRectangle(pen, cardRect);
            }
        }

        // --- YÖNLENDİRMELER ---
        private void btnTekSeferlik_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new beslenmeGunluk();
            form.ShowDialog();
            this.Close();
        }

        private void btnHaftalik_Click(object sender, EventArgs e)
        {
            this.Hide();
            var form = new beslenmeHaftalik();
            form.ShowDialog();
            this.Close();
        }

        private void btnGeriDon_Click(object sender, EventArgs e)
        {
            this.Hide();
            var menuForm = new MenuForm();
            menuForm.ShowDialog();
            this.Close();
        }
    }
}