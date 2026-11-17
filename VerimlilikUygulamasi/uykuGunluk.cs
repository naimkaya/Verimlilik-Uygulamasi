using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class uykuGunluk : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Label lblBaslangic, lblBitis;
        private DateTimePicker dtpBaslangic, dtpBitis;
        private Label lblSure;

        private Button btnHesapla;
        private TextBox txtOneri;
        private Panel pnlLeft, pnlRight;

        public uykuGunluk()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Uyku - Günlük Veri Girişi";
            this.BackColor = Color.FromArgb(10, 25, 50);

            // =========================================================
            // 1. ÜST PANEL VE BAŞLIK
            // =========================================================
            pnlTop = new Panel() { Dock = DockStyle.Top, Height = 120, BackColor = Color.Transparent };
            this.Controls.Add(pnlTop);

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
            btnGeriDon.Location = new Point(this.ClientSize.Width - 150, 35);
            btnGeriDon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGeriDon.Click += (s, e) => { this.Close(); };
            pnlTop.Controls.Add(btnGeriDon);

            lblTitle = new Label()
            {
                Text = "Günlük Uyku Analizi",
                Font = new Font("Segoe UI Black", 36F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            pnlTop.Controls.Add(lblTitle);
            btnGeriDon.BringToFront();

            // =========================================================
            // 2. ANA YERLEŞİM PLANI
            // =========================================================
            TableLayoutPanel mainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(80, 40, 80, 80),
                BackColor = Color.Transparent
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            this.Controls.Add(mainLayout);
            mainLayout.BringToFront();

            // --- Sol Panel (Daha Büyük Veri Girişleri) ---
            pnlLeft = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(20) };

            // DÜZELTME: Giriş alanlarının büyümesiyle dikey boşluk (gapY) 140'a çıkarıldı.
            int startY = 30;
            int gapY = 140;

            lblBaslangic = YeniEtiket("Uyuduğunuz Saat:", 0, startY);
            dtpBaslangic = YeniSaatKontrol(0, startY + 50); // Boşluk 50'ye çıkarıldı
            pnlLeft.Controls.Add(lblBaslangic); pnlLeft.Controls.Add(dtpBaslangic);

            lblBitis = YeniEtiket("Uyandığınız Saat:", 0, startY + gapY);
            dtpBitis = YeniSaatKontrol(0, startY + gapY + 50);
            pnlLeft.Controls.Add(lblBitis); pnlLeft.Controls.Add(dtpBitis);

            lblSure = new Label()
            {
                Text = "Toplam Süre: Bekleniyor...",
                ForeColor = Color.Gold,
                Font = new Font("Segoe UI", 22, FontStyle.Bold), // 18'den 22'ye büyütüldü
                Location = new Point(0, startY + gapY * 2 + 30),
                AutoSize = true
            };
            pnlLeft.Controls.Add(lblSure);

            // DÜZELTME: Buton boyutları (450x90) ve Font boyutu (24) devasa yapıldı!
            btnHesapla = new Button()
            {
                Text = "Analiz Et ve Öneri Al",
                Location = new Point(0, startY + gapY * 2 + 110),
                Size = new Size(450, 90),
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                BackColor = Color.FromArgb(39, 174, 96), // Beslenmedeki şık yeşil tonu
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnHesapla.FlatAppearance.BorderSize = 0;

            btnHesapla.MouseEnter += (s, e) => {
                btnHesapla.BackColor = ControlPaint.Light(Color.FromArgb(39, 174, 96), 0.2f);
                btnHesapla.FlatAppearance.BorderSize = 3;
                btnHesapla.FlatAppearance.BorderColor = Color.White;
            };
            btnHesapla.MouseLeave += (s, e) => {
                btnHesapla.BackColor = Color.FromArgb(39, 174, 96);
                btnHesapla.FlatAppearance.BorderSize = 0;
            };

            btnHesapla.Click += BtnHesapla_Click;
            pnlLeft.Controls.Add(btnHesapla);

            mainLayout.Controls.Add(pnlLeft, 0, 0);

            // --- Sağ Panel (Sonuç ve Öneriler Ekranı) ---
            pnlRight = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(40, 20, 0, 20) };

            txtOneri = new TextBox()
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Segoe UI", 16),
                BackColor = Color.FromArgb(20, 35, 65),
                ForeColor = Color.LightGray,
                BorderStyle = BorderStyle.None,
                Text = "\r\n\r\n\r\nSol taraftan uyuma ve uyanma saatlerinizi girip 'Analiz Et' butonuna bastığınızda, bilimsel uyku önerileriniz burada görünecektir."
            };

            Panel borderPanel = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(3) };
            borderPanel.Controls.Add(txtOneri);
            pnlRight.Controls.Add(borderPanel);

            mainLayout.Controls.Add(pnlRight, 1, 0);

            btnGeriDon.MouseEnter += (s, e) => { btnGeriDon.BackColor = Color.Silver; };
            btnGeriDon.MouseLeave += (s, e) => { btnGeriDon.BackColor = Color.FromArgb(160, 160, 160); };

            dtpBaslangic.ValueChanged += HesaplaCanliSure;
            dtpBitis.ValueChanged += HesaplaCanliSure;
            HesaplaCanliSure(null, null);
        }

        // --- TASARIM METOTLARI (YAZILAR VE KUTULAR BÜYÜTÜLDÜ) ---
        private Label YeniEtiket(string metin, int x, int y)
        {
            return new Label() { Text = metin, ForeColor = Color.White, Font = new Font("Segoe UI", 22, FontStyle.Bold), AutoSize = true, Location = new Point(x, y) }; // 18'den 22'ye büyütüldü
        }

        private DateTimePicker YeniSaatKontrol(int x, int y)
        {
            return new DateTimePicker()
            {
                Location = new Point(x, y),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "HH:mm",
                ShowUpDown = true,
                Font = new Font("Segoe UI", 30, FontStyle.Bold), // 24'ten 30'a çıkarıldı
                Width = 450, // Genişlik 400'den 450'ye çıkarıldı
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black
            };
        }

        private void HesaplaCanliSure(object sender, EventArgs e)
        {
            TimeSpan sure = (dtpBitis.Value < dtpBaslangic.Value)
                ? dtpBitis.Value.AddDays(1) - dtpBaslangic.Value
                : dtpBitis.Value - dtpBaslangic.Value;

            lblSure.Text = $"Toplam Süre: {sure.Hours} saat {sure.Minutes} dk";
        }

        private void BtnHesapla_Click(object sender, EventArgs e)
        {
            TimeSpan sure = (dtpBitis.Value < dtpBaslangic.Value)
                ? dtpBitis.Value.AddDays(1) - dtpBaslangic.Value
                : dtpBitis.Value - dtpBaslangic.Value;

            double totalHours = sure.TotalHours;
            int yatmaSaati = dtpBaslangic.Value.Hour;

            var oneriler = new List<string>();

            // 1. SÜRE ANALİZİ
            oneriler.Add("⏳ SÜRE ANALİZİ:");
            if (totalHours < 6)
            {
                oneriler.Add($"• {sure.Hours} saat {sure.Minutes} dakika uyumuşsunuz. Bu süre yetişkin bir insan için yetersizdir.");
                oneriler.Add("• Yetersiz uyku; gün içinde odaklanma sorunu, hafıza zayıflığı ve bağışıklık sisteminin düşmesine neden olur.");
            }
            else if (totalHours <= 9)
            {
                oneriler.Add($"• {sure.Hours} saat {sure.Minutes} dakika. Yetişkinler için tavsiye edilen ideal uyku süresindesiniz (7-9 saat).");
                oneriler.Add("• Fiziksel ve zihinsel dinlenmeniz için harika bir süre ayırmışsınız.");
            }
            else
            {
                oneriler.Add($"• {sure.Hours} saat {sure.Minutes} dakika. İhtiyacınızdan çok daha fazla uyumuşsunuz.");
                oneriler.Add("• Aşırı uyku ('uyku sarhoşluğu') gün boyu süren bir uyuşukluk, baş ağrısı ve motivasyon eksikliği yaratabilir.");
            }

            // 2. BİYOLOJİK SAAT (SİRKADİYEN RİTİM) ANALİZİ
            oneriler.Add("🌙 BİYOLOJİK SAAT (SİRKADİYEN RİTİM) ANALİZİ:");

            if (yatmaSaati >= 1 && yatmaSaati < 6)
            {
                oneriler.Add("• Çok geç saatte uykuya dalmışsınız. Uyku süreniz yeterli olsa bile uykunuzun 'kalitesi' düşüktür.");
                oneriler.Add("• Uykunun en verimli olduğu zaman dilimi, hücre yenileyici Melatonin hormonunun zirve yaptığı 23:00 - 03:00 saatleri arasıdır. Siz bu altın saatleri uyanık geçirmişsiniz.");
                oneriler.Add("• Biyolojik saatiniz (Sirkadiyen Ritim) güneş ışığına göre çalışır. Gece uyumayıp gündüz uyumak, gün içinde yorgun ve halsiz hissetmenize yol açar.");
            }
            else if (yatmaSaati >= 6 && yatmaSaati <= 18)
            {
                oneriler.Add("• Gece uykusu yerine gündüz uykusunu tercih etmişsiniz.");
                oneriler.Add("• Gündüz uykusu, ne kadar uzun olursa olsun, zifiri karanlıkta salgılanan Melatonin hormonunun eksikliği nedeniyle bedenin tam anlamıyla yenilenmesini sağlayamaz.");
            }
            else if (yatmaSaati >= 21 || yatmaSaati == 0)
            {
                oneriler.Add("• Tebrikler! Tam da uzmanların önerdiği saat diliminde (22:00 - 00:00) uykuya dalmışsınız.");
                oneriler.Add("• Vücudunuzun kendini onardığı, bağışıklık sisteminin güçlendiği ve büyüme hormonunun salgılandığı 23:00 - 03:00 arasındaki 'Altın Saatler'den tam verim almışsınız.");
            }
            else
            {
                oneriler.Add("• Normalden biraz daha erken uyumuşsunuz. Eğer gün içinde çok yorulduysanız bu vücudun doğal bir tepkisidir, ancak bunu sürekli hale getirmek sabahları çok erken uyanmanıza sebep olabilir.");
            }

            oneriler.Add("💡 ALTIN TAVSİYELER:");
            oneriler.Add("• Uyandıktan sonraki ilk 1 saat içinde güneş ışığı görmek, gece uykunuzun kalitesini doğrudan artırır.");
            oneriler.Add("• Uyumadan en az 1 saat önce telefon ve bilgisayar ekranından (mavi ışık) uzak durmak Melatonin salgısını hızlandırır.");

            txtOneri.Text = "\r\n" + string.Join(Environment.NewLine + Environment.NewLine, oneriler);
        }
    }
}