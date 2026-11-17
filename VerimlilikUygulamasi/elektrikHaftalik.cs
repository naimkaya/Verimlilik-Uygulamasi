using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class elektrikHaftalik : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Label lblTimerGun;
        private Label lblTimerSaat;

        private Label lblKwh, lblKisiSayisi, lblCihazSayisi;
        private NumericUpDown numKwh, numKisiSayisi, numCihazSayisi;
        private Button btnKaydet;

        private TextBox txtOneri;
        private FlowLayoutPanel pnlDaysList;
        private Label lblOrtalamaSonuc;

        private int userId = 1;
        private DateTime? sessionStart = null;
        private int aktifGun = 1;
        private System.Windows.Forms.Timer countdownTimer;

        public elektrikHaftalik()
        {
            InitializeComponent();
            DatabaseHelper.InitializeDatabase();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Haftalık Elektrik Takibi";
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
                Text = "Haftalık Elektrik Serüveni",
                Font = new Font("Segoe UI Black", 32F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            pnlTop.Controls.Add(lblTitle);

            // =========================================================
            // 2. ANA YERLEŞİM PLANI (3 Kolonlu Modern Dashboard)
            // =========================================================
            TableLayoutPanel mainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(40, 20, 40, 40),
                BackColor = Color.Transparent
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));  // Sol: Girişler ve Sayaç
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));  // Orta: Dinamik Öneriler
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));  // Sağ: 7 Gün Akış Listesi
            this.Controls.Add(mainLayout);
            mainLayout.BringToFront();

            // --- SOL KOLON (3 Giriş Elemanı ve Canlı Sayaç) ---
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
            pnlLeft.Controls.Add(lblTimerGun); pnlLeft.Controls.Add(lblTimerSaat);

            int startY = 110;
            int gapY = 100;

            // 1. Tüketilen Elektrik (kWh)
            lblKwh = YeniEtiket("Tüketilen Enerji (kWh):", 0, startY);
            numKwh = YeniSayiKontrol(0, startY + 35, 500, true);
            pnlLeft.Controls.Add(lblKwh); pnlLeft.Controls.Add(numKwh);

            // 2. Evdeki Kişi Sayısı
            lblKisiSayisi = YeniEtiket("Evdeki Kişi Sayısı:", 0, startY + gapY);
            numKisiSayisi = YeniSayiKontrol(0, startY + gapY + 35, 20, false);
            numKisiSayisi.Value = 1;
            pnlLeft.Controls.Add(lblKisiSayisi); pnlLeft.Controls.Add(numKisiSayisi);

            // 3. Cihaz Sayısı
            lblCihazSayisi = YeniEtiket("Aktif Önemli Cihaz Sayısı:", 0, startY + gapY * 2);
            numCihazSayisi = YeniSayiKontrol(0, startY + gapY * 2 + 35, 100, false);
            pnlLeft.Controls.Add(lblCihazSayisi); pnlLeft.Controls.Add(numCihazSayisi);

            btnKaydet = new Button()
            {
                Text = "Kaydet ve Analiz Al",
                Location = new Point(0, startY + gapY * 3 + 10),
                Size = new Size(350, 70),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnKaydet.FlatAppearance.BorderSize = 0;
            btnKaydet.MouseEnter += (s, e) => { btnKaydet.BackColor = ControlPaint.Light(Color.FromArgb(39, 174, 96), 0.2f); btnKaydet.FlatAppearance.BorderSize = 3; btnKaydet.FlatAppearance.BorderColor = Color.White; };
            btnKaydet.MouseLeave += (s, e) => { btnKaydet.BackColor = Color.FromArgb(39, 174, 96); btnKaydet.FlatAppearance.BorderSize = 0; };
            btnKaydet.Click += BtnKaydet_Click;
            pnlLeft.Controls.Add(btnKaydet);
            mainLayout.Controls.Add(pnlLeft, 0, 0);

            // --- ORTA KOLON (Kişi Başı Odaklı Bilimsel Analizler) ---
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
                Text = "\r\n\r\nSol taraftan günlük elektrik tüketiminizi, kişi sayısını ve cihaz sayısını kaydederek 7 günlük süreci başlatın. Her gün için 24 saat süreniz olacak."
            };
            Panel borderPanelMid = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(3) };
            borderPanelMid.Controls.Add(txtOneri);
            pnlMid.Controls.Add(borderPanelMid);
            mainLayout.Controls.Add(pnlMid, 1, 0);

            // --- SAĞ KOLON (7 Gün Sıralı Liste ve Ortalama Kartı) ---
            Panel pnlRight = new Panel() { Dock = DockStyle.Fill };
            pnlDaysList = new FlowLayoutPanel()
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = Color.Transparent,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false
            };
            pnlDaysList.Resize += (s, e) => {
                foreach (Control c in pnlDaysList.Controls)
                    c.Width = pnlDaysList.ClientSize.Width > 20 ? pnlDaysList.ClientSize.Width - 15 : 250;
            };
            pnlRight.Controls.Add(pnlDaysList);

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

            countdownTimer = new System.Windows.Forms.Timer();
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += CountdownTimer_Tick;

            CheckActiveSession();
        }

        // =========================================================
        // SAYAÇ VE SÜREÇ TAKİP ALGORİTMASI
        // =========================================================
        private void CheckActiveSession()
        {
            sessionStart = DatabaseHelper.GetActiveElektrikSessionStart(userId);
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
                lblTimerGun.Text = "HAFTALIK ENERJİ SERÜVENİ";
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
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");

            if (sessionStart == null)
            {
                sessionStart = DateTime.Now;
                DatabaseHelper.StartNewElektrikSession(userId, sessionStart.Value);
                aktifGun = 1;
                countdownTimer.Start();
            }

            DateTime kayitTarihi = sessionStart.Value.Date.AddDays(aktifGun - 1);

            // DatabaseHelper üzerindeki güncel 5 parametreli metodu tetikliyoruz
            DatabaseHelper.SaveElektrikVerisi(userId, kayitTarihi,
                (double)numKwh.Value, (int)numKisiSayisi.Value, (int)numCihazSayisi.Value);

            MessageBox.Show($"{aktifGun}. gün elektrik tüketim verileriniz başarıyla kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GenerateRecommendations((double)numKwh.Value, (int)numKisiSayisi.Value, (int)numCihazSayisi.Value);
            RefreshRightPanel();
            CountdownTimer_Tick(null, null);
        }

        // =========================================================
        // SAĞ PANEL: SIRALI AKIŞ LİSTESİ (KİŞİ SAYISI VE KWH ORANI)
        // =========================================================
        private void RefreshRightPanel()
        {
            pnlDaysList.Controls.Clear();
            if (sessionStart == null) return;

            var kayitlar = DatabaseHelper.GetWeeklyElektrikRecords(userId, sessionStart.Value);
            double sumKwh = 0; double sumKisiBasiKwh = 0; int validDaysCount = 0;

            for (int i = 1; i <= 7; i++)
            {
                string gunTarih = sessionStart.Value.Date.AddDays(i - 1).ToString("yyyy-MM-dd");
                string durumMetni = ""; Color bgColor = Color.FromArgb(30, 45, 75); Color fgColor = Color.White;

                if (kayitlar.ContainsKey(gunTarih))
                {
                    var v = kayitlar[gunTarih];
                    int k = v.KisiSayisi <= 0 ? 1 : v.KisiSayisi;
                    double kbKwh = v.Kwh / k;

                    durumMetni = $"{i}. Gün: {v.Kwh:F1} kWh ({k} Kişi)\r\nKişi Başı: {kbKwh:F1} kWh";
                    bgColor = Color.FromArgb(39, 174, 96);

                    sumKwh += v.Kwh;
                    sumKisiBasiKwh += kbKwh;
                    validDaysCount++;
                }
                else if (i < aktifGun)
                {
                    durumMetni = $"{i}. Gün: VERİ GİRİLMEMİŞTİR\r\n(Geri Dönülemez - Kilitli)";
                    bgColor = Color.FromArgb(192, 57, 43);
                    fgColor = Color.LightGray;
                }
                else
                {
                    durumMetni = $"{i}. Gün: Henüz Veri Girişi\r\nYapılmamıştır.";
                    bgColor = Color.FromArgb(40, 50, 60); fgColor = Color.Gray;
                }

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
                double ortKwh = sumKwh / validDaysCount;
                double ortKbKwh = sumKisiBasiKwh / validDaysCount;

                lblOrtalamaSonuc.Text = $"📊 {validDaysCount} GÜNLÜK ORTALAMANIZ\r\n\r\n" +
                       $"⚡ Ortalama Toplam Tüketim: {ortKwh:F2} kWh\r\n" +
                       $"👥 Ortalama Kişi Başı Yük: {ortKbKwh:F2} kWh";
            }
            else
            {
                lblOrtalamaSonuc.Text = "📊 ORTALAMANIZ\r\n\r\nHenüz sisteme girilmiş geçerli bir veri yok.";
            }
        }

        // =========================================================
        // KİŞİ SAYISINA GÖRE KARAR VEREN HESAP MOTORU
        // =========================================================
        private void GenerateRecommendations(double kwh, int kisi, int cihaz)
        {
            if (kisi <= 0) kisi = 1;
            double kisiBasiKwh = kwh / kisi;

            var oneriler = new List<string>();
            oneriler.Add("⚡ DETAYLI ENERJİ VE VERİMLİLİK ANALİZİ:");

            if (kisiBasiKwh <= 2.0)
            {
                oneriler.Add($"• Kişi Başı Tüketim {kisiBasiKwh:F2} kWh: HARİKA EKOLOJİK MOD! 🍃");
                oneriler.Add($"• {kisi} kişilik hane için toplam {kwh} kWh tüketim oldukça başarılı bir tasarruf düzeyidir.");
            }
            else if (kisiBasiKwh <= 4.0)
            {
                oneriler.Add($"• Kişi Başı Tüketim {kisiBasiKwh:F2} kWh: STANDART / DENGELİ SEVİYE. 📊");
                oneriler.Add("• Enerji paylaşım oranınız makul sınırlarda. İsraf noktaları yok denecek kadar az.");
            }
            else
            {
                oneriler.Add($"• Kişi Başı Tüketim {kisiBasiKwh:F2} kWh: YÜKSEK TÜKETİM VE İSRAF ALARMI! ⚠️");
                oneriler.Add($"• Evde toplam {kwh} kWh enerji yakılıyor. Kişi başı harcamanız çok yüksek.");
                oneriler.Add("• Kullanılmayan odalarda açık duran lambalar veya stand-by modunda bırakılan cihazlar bu israfa neden oluyor olabilir.");
            }

            if (cihaz > 0)
            {
                oneriler.Add("🔌 CİHAZ YOĞUNLUK NOTU:");
                oneriler.Add($"• Evdeki {cihaz} aktif cihaz için stand-by (bekleme) modunu kapatmak bile haftalık faturanızda doğrudan gözle görülür bir düşüş sağlayacaktır.");
            }

            oneriler.Add("💡 ENERJİ TASARRUFU İÇİN ALTIN TÜYOLAR:");
            oneriler.Add("• Beyaz eşyaları tam doldurmadan çalıştırmamak ve akıllı tarifedeyseniz yıkama işlemlerini gece (22.00-06.00) saatlerine kaydırmak faturayı doğrudan %30 düşürür.");
            oneriler.Add("• Tasarruflu LED ampullere geçmek, klasik ampullere kıyasla aydınlatma maliyetinizi tam %80 oranında azaltır.");

            txtOneri.Text = "\r\n" + string.Join(Environment.NewLine + Environment.NewLine, oneriler);
        }

        private Label YeniEtiket(string metin, int x, int y)
        {
            return new Label() { Text = metin, ForeColor = Color.White, Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true, Location = new Point(x, y) };
        }

        private NumericUpDown YeniSayiKontrol(int x, int y, decimal max, bool isDecimal = false)
        {
            var num = new NumericUpDown() { Location = new Point(x, y), Maximum = max, Minimum = 0, Font = new Font("Segoe UI", 20, FontStyle.Bold), Width = 350, BackColor = Color.WhiteSmoke, ForeColor = Color.Black, BorderStyle = BorderStyle.FixedSingle };
            if (isDecimal)
            {
                num.DecimalPlaces = 2;
                num.Increment = 0.5M;
            }
            return num;
        }
    }
}