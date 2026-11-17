using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class beslenmeHaftalik : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Label lblTimerGun;
        private Label lblTimerSaat;

        private Label lblKalori, lblProtein, lblKarbonhidrat, lblYag;
        private NumericUpDown numKalori, numProtein, numKarbonhidrat, numYag;
        private Button btnKaydet;

        private TextBox txtOneri;
        private FlowLayoutPanel pnlDaysList;
        private Label lblOrtalamaSonuc;

        private int userId = 1;
        private DateTime? sessionStart = null;
        private int aktifGun = 1;
        private System.Windows.Forms.Timer countdownTimer;

        public beslenmeHaftalik()
        {
            InitializeComponent();
            DatabaseHelper.InitializeDatabase();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Haftalık Beslenme Takibi";
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
                Text = "Haftalık Beslenme Verileri",
                Font = new Font("Segoe UI Black", 32F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            pnlTop.Controls.Add(lblTitle);

            // =========================================================
            // 2. ANA YERLEŞİM (3 Kolonlu Tasarım)
            // =========================================================
            TableLayoutPanel mainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(40, 20, 40, 40),
                BackColor = Color.Transparent
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.Controls.Add(mainLayout);
            mainLayout.BringToFront();

            // --- SOL KOLON (Veri Girişi ve Sayaç) ---
            Panel pnlLeft = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(10) };

            lblTimerGun = new Label()
            {
                Text = "SÜRECİ BAŞLATMAK İÇİN",
                Font = new Font("Segoe UI Black", 14F, FontStyle.Bold),
                ForeColor = Color.Gold,
                AutoSize = true,
                Location = new Point(0, 0)
            };

            lblTimerSaat = new Label()
            {
                Text = "İlk Veriyi Girin.",
                Font = new Font("Segoe UI Black", 26F, FontStyle.Bold),
                ForeColor = Color.Cyan,
                AutoSize = true,
                Location = new Point(0, 25)
            };

            pnlLeft.Controls.Add(lblTimerGun);
            pnlLeft.Controls.Add(lblTimerSaat);

            int startY = 110; int gapY = 100;

            lblKalori = YeniEtiket("Kalori (kcal):", 0, startY);
            numKalori = YeniSayiKontrol(0, startY + 35, 10000);
            pnlLeft.Controls.Add(lblKalori); pnlLeft.Controls.Add(numKalori);

            lblProtein = YeniEtiket("Protein (gr):", 0, startY + gapY);
            numProtein = YeniSayiKontrol(0, startY + gapY + 35, 1000);
            pnlLeft.Controls.Add(lblProtein); pnlLeft.Controls.Add(numProtein);

            lblKarbonhidrat = YeniEtiket("Karbonhidrat (gr):", 0, startY + gapY * 2);
            numKarbonhidrat = YeniSayiKontrol(0, startY + gapY * 2 + 35, 2000);
            pnlLeft.Controls.Add(lblKarbonhidrat); pnlLeft.Controls.Add(numKarbonhidrat);

            lblYag = YeniEtiket("Yağ (gr):", 0, startY + gapY * 3);
            numYag = YeniSayiKontrol(0, startY + gapY * 3 + 35, 1000);
            pnlLeft.Controls.Add(lblYag); pnlLeft.Controls.Add(numYag);

            btnKaydet = new Button()
            {
                Text = "Kaydet ve Öneri Al",
                Location = new Point(0, startY + gapY * 4 + 10),
                Size = new Size(350, 70),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnKaydet.FlatAppearance.BorderSize = 0;
            btnKaydet.Click += BtnKaydet_Click;
            pnlLeft.Controls.Add(btnKaydet);
            mainLayout.Controls.Add(pnlLeft, 0, 0);

            // --- ORTA KOLON (Öneriler) ---
            Panel pnlMid = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(20, 0, 20, 0) };
            txtOneri = new TextBox()
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Segoe UI", 15),
                BackColor = Color.FromArgb(20, 35, 65),
                ForeColor = Color.LightGray,
                BorderStyle = BorderStyle.None,
                Text = "\r\n\r\nSol taraftan değerlerinizi girip süreci başlatın. Her gün için 24 saat süreniz olacak."
            };
            Panel borderPanelMid = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(3) };
            borderPanelMid.Controls.Add(txtOneri);
            pnlMid.Controls.Add(borderPanelMid);
            mainLayout.Controls.Add(pnlMid, 1, 0);

            // --- SAĞ KOLON (7 Gün Liste ve Ortalama) ---
            Panel pnlRight = new Panel() { Dock = DockStyle.Fill };

            // DÜZELTME: Yan yana kaymayı (wrap) tamamen engellemek için FlowDirection eklendi.
            pnlDaysList = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.TopDown, // Kesinlikle yukarıdan aşağı sırala
                WrapContents = false // İçerikleri sığdırmak için asla yan yana dizme
            };

            // Ekran boyutu değiştiğinde içindeki etiketlerin de boyutunu otomatik ayarlaması için event
            pnlDaysList.Resize += (s, e) => {
                foreach (Control c in pnlDaysList.Controls)
                {
                    c.Width = pnlDaysList.ClientSize.Width - 15; // Scrollbar payı bırakarak genişlet
                }
            };
            pnlRight.Controls.Add(pnlDaysList);

            // DÜZELTME: Yazı rengi ForeColor = Color.White yapıldı
            lblOrtalamaSonuc = new Label()
            {
                Dock = DockStyle.Bottom,
                Height = 250,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                BackColor = Color.FromArgb(40, 40, 60),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10)
            };
            pnlRight.Controls.Add(lblOrtalamaSonuc);
            mainLayout.Controls.Add(pnlRight, 2, 0);

            // --- SAYAÇ VE BAŞLANGIÇ AYARLARI ---
            countdownTimer = new System.Windows.Forms.Timer();
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += CountdownTimer_Tick;

            CheckActiveSession();
        }

        // =========================================================
        // SAYAÇ VE OYUN MEKANİĞİ MANTIĞI
        // =========================================================
        private void CheckActiveSession()
        {
            sessionStart = DatabaseHelper.GetActiveSessionStart(userId);
            if (sessionStart != null)
            {
                countdownTimer.Start();
                CountdownTimer_Tick(null, null);
            }
            RefreshRightPanel();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (sessionStart == null) return;

            TimeSpan diff = DateTime.Now - sessionStart.Value;
            aktifGun = (int)diff.TotalDays + 1;

            if (aktifGun > 7)
            {
                countdownTimer.Stop();
                lblTimerGun.Text = "HAFTALIK SERÜVEN";
                lblTimerSaat.Text = "TAMAMLANDI!";
                lblTimerSaat.ForeColor = Color.LightGreen;
                btnKaydet.Enabled = false;
                return;
            }

            DateTime nextDayStart = sessionStart.Value.AddDays(aktifGun);
            TimeSpan remaining = nextDayStart - DateTime.Now;

            lblTimerGun.Text = $"{aktifGun}. GÜN İÇİN KALAN SÜRE";
            lblTimerSaat.Text = $"{remaining.Hours:D2}:{remaining.Minutes:D2}:{remaining.Seconds:D2}";

            if (remaining.TotalSeconds <= 1) { RefreshRightPanel(); }
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (sessionStart == null)
            {
                sessionStart = DateTime.Now;
                DatabaseHelper.StartNewSession(userId, sessionStart.Value);
                aktifGun = 1;
                countdownTimer.Start();
            }

            DateTime kayitTarihi = sessionStart.Value.Date.AddDays(aktifGun - 1);

            DatabaseHelper.SaveBeslenmeVerisi(userId, kayitTarihi,
                (double)numKalori.Value, (double)numProtein.Value, (double)numKarbonhidrat.Value, (double)numYag.Value);

            MessageBox.Show($"{aktifGun}. gün verileriniz başarıyla kaydedildi!", "Süreç Devam Ediyor", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GenerateRecommendations();
            RefreshRightPanel();

            CountdownTimer_Tick(null, null);
        }

        // =========================================================
        // SAĞ PANEL: 7 GÜNLÜK LİSTE VE AKILLI ORTALAMA
        // =========================================================
        private void RefreshRightPanel()
        {
            pnlDaysList.Controls.Clear();
            if (sessionStart == null) return;

            var kayitlar = DatabaseHelper.GetWeeklyRecords(userId, sessionStart.Value);

            double sumKalori = 0, sumPro = 0, sumKarb = 0, sumYag = 0;
            int validDaysCount = 0;

            for (int i = 1; i <= 7; i++)
            {
                string gunTarih = sessionStart.Value.Date.AddDays(i - 1).ToString("yyyy-MM-dd");
                string durumMetni = "";
                Color bgColor = Color.FromArgb(30, 45, 75);
                Color fgColor = Color.White;

                if (kayitlar.ContainsKey(gunTarih))
                {
                    var v = kayitlar[gunTarih];
                    durumMetni = $"{i}. Gün: {v.Kalori}kcal | {v.Protein}g P\r\n{v.Karbonhidrat}g K | {v.Yag}g Y";
                    bgColor = Color.FromArgb(39, 174, 96);

                    sumKalori += v.Kalori; sumPro += v.Protein; sumKarb += v.Karbonhidrat; sumYag += v.Yag;
                    validDaysCount++;
                }
                else if (i < aktifGun)
                {
                    durumMetni = $"{i}. Gün: VERİ GİRİLMEMİŞTİR\r\n(Süre Doldu)";
                    bgColor = Color.FromArgb(192, 57, 43);
                    fgColor = Color.LightGray;
                }
                else
                {
                    durumMetni = $"{i}. Gün: Henüz Veri Girişi\r\nYapılmamıştır.";
                    bgColor = Color.FromArgb(40, 50, 60);
                    fgColor = Color.Gray;
                }

                // Genişlik Panel boyutuna göre dinamik olarak hesaplanıyor (Scrollbar için -15px pay)
                int labelWidth = pnlDaysList.ClientSize.Width > 20 ? pnlDaysList.ClientSize.Width - 15 : 250;

                Label lblDay = new Label()
                {
                    Text = durumMetni,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    BackColor = bgColor,
                    ForeColor = fgColor,
                    Width = labelWidth,
                    Height = 65,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(0, 5, 0, 5)
                };
                pnlDaysList.Controls.Add(lblDay);
            }

            if (validDaysCount > 0)
            {
                lblOrtalamaSonuc.Text = $"📊 {validDaysCount} GÜNLÜK ORTALAMANIZ\r\n\r\n" +
                       $"🔥 Kalori: {(sumKalori / validDaysCount):F1} kcal\r\n" +
                       $"🥩 Protein: {(sumPro / validDaysCount):F1} gr\r\n" +
                       $"🍞 Karbonh: {(sumKarb / validDaysCount):F1} gr\r\n" +
                       $"🥑 Yağ: {(sumYag / validDaysCount):F1} gr";
            }
            else
            {
                lblOrtalamaSonuc.Text = "📊 ORTALAMA\r\n\r\nHenüz geçerli veri yok.";
            }
        }

        // =========================================================
        // GÖRSEL TASARIM VE ÖNERİ MOTORU 
        // =========================================================
        private Label YeniEtiket(string metin, int x, int y)
        {
            return new Label() { Text = metin, ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true, Location = new Point(x, y) };
        }

        private NumericUpDown YeniSayiKontrol(int x, int y, decimal max)
        {
            return new NumericUpDown() { Location = new Point(x, y), Maximum = max, Font = new Font("Segoe UI", 20, FontStyle.Bold), Width = 350, BackColor = Color.WhiteSmoke, BorderStyle = BorderStyle.FixedSingle };
        }

        private void GenerateRecommendations()
        {
            double kalori = (double)numKalori.Value; double protein = (double)numProtein.Value;
            double karbonhidrat = (double)numKarbonhidrat.Value; double yag = (double)numYag.Value;
            var oneriler = new List<string>();

            if (kalori < 1500) oneriler.Add("🔥 KALORİ ALIMI: Düşük\r\n\r\n• Günlük bazal metabolizma hızınızın altında kalori alıyorsunuz.\r\n• Halsizlik yaşayabilirsiniz.");
            else if (kalori <= 2500) oneriler.Add("🔥 KALORİ ALIMI: Dengeli\r\n\r\n• Harika! Günlük enerji ihtiyacınızı tam kıvamında karşılıyorsunuz.");
            else oneriler.Add("🔥 KALORİ ALIMI: Yüksek\r\n\r\n• İhtiyacınızdan fazla kalori alıyorsunuz.\r\n• Kilo kontrolüne dikkat edin.");

            if (protein < 50) oneriler.Add("🥩 PROTEİN ALIMI: Düşük\r\n\r\n• Kas onarımı ve bağışıklık için yetersiz.\r\n• Yumurta, et ve baklagil tüketiminizi artırın.");
            else if (protein <= 150) oneriler.Add("🥩 PROTEİN ALIMI: Dengeli\r\n\r\n• Kas onarımı ve tokluk hissi için mükemmel bir seviyedesiniz.");
            else oneriler.Add("🥩 PROTEİN ALIMI: Yüksek\r\n\r\n• Gereğinden fazla protein böbrekleri yorabilir.\r\n• Bol su içmeyi unutmayın.");

            if (karbonhidrat < 130) oneriler.Add("🍞 KARBONHİDRAT: Düşük\r\n\r\n• Beyninizin temel yakıtı eksik.\r\n• Odaklanma sorunları (beyin sisi) yaşayabilirsiniz.");
            else if (karbonhidrat <= 300) oneriler.Add("🍞 KARBONHİDRAT: Dengeli\r\n\r\n• Zihinsel ve fiziksel aktiviteleriniz için harika bir yakıt seviyesi.");
            else oneriler.Add("🍞 KARBONHİDRAT: Yüksek\r\n\r\n• Kan şekeri dalgalanmalarına ve yorgunluğa sebep olabilir.\r\n• Yemek sonrası yürüyüş yapın.");

            if (yag < 30) oneriler.Add("🥑 YAĞ ALIMI: Düşük\r\n\r\n• Hormon sağlığınız için zeytinyağı, ceviz, badem gibi faydalı yağlar ekleyin.");
            else if (yag <= 70) oneriler.Add("🥑 YAĞ ALIMI: Dengeli\r\n\r\n• Beyin sağlığınız ve hormon dengeniz için ideal aralıktasınız.");
            else oneriler.Add("🥑 YAĞ ALIMI: Yüksek\r\n\r\n• Yağlar yoğun kalorilidir.\r\n• Kızartmalardan ve yoğun soslardan uzak durun.");

            txtOneri.Text = "\r\n" + string.Join(Environment.NewLine + Environment.NewLine, oneriler);
        }
    }
}