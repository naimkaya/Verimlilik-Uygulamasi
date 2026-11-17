using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class uykuHaftalik : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Label lblTimerGun;
        private Label lblTimerSaat;

        private Label lblBaslangic, lblBitis, lblSure;
        private DateTimePicker dtpBaslangic, dtpBitis;
        private Button btnKaydet;

        private TextBox txtOneri;
        private FlowLayoutPanel pnlDaysList;
        private Label lblOrtalamaSonuc;

        private int userId = 1;
        private DateTime? sessionStart = null;
        private int aktifGun = 1;
        private System.Windows.Forms.Timer countdownTimer;

        public uykuHaftalik()
        {
            InitializeComponent();
            DatabaseHelper.InitializeDatabase();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Haftalık Uyku Takibi";
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
                Text = "Haftalık Uyku Serüveni",
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
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            this.Controls.Add(mainLayout);
            mainLayout.BringToFront();

            // --- SOL KOLON (Zaman Girişleri ve Sayaç) ---
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

            int startY = 120; int gapY = 120;

            lblBaslangic = YeniEtiket("Uyuduğunuz Saat:", 0, startY);
            dtpBaslangic = YeniSaatKontrol(0, startY + 45);
            pnlLeft.Controls.Add(lblBaslangic); pnlLeft.Controls.Add(dtpBaslangic);

            lblBitis = YeniEtiket("Uyandığınız Saat:", 0, startY + gapY);
            dtpBitis = YeniSaatKontrol(0, startY + gapY + 45);
            pnlLeft.Controls.Add(lblBitis); pnlLeft.Controls.Add(dtpBitis);

            lblSure = new Label()
            {
                Text = "Toplam Süre: Bekleniyor...",
                ForeColor = Color.Gold,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(0, startY + gapY * 2 + 20),
                AutoSize = true
            };
            pnlLeft.Controls.Add(lblSure);

            btnKaydet = new Button()
            {
                Text = "Kaydet ve Analiz Al",
                Location = new Point(0, startY + gapY * 2 + 85),
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

            // --- ORTA KOLON (Bilimsel Öneriler) ---
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
                Text = "\r\n\r\nSol taraftan uyku saatlerinizi kaydederek 7 günlük süreci başlatın. Her gün için 24 saat süreniz olacak."
            };
            Panel borderPanelMid = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(3) };
            borderPanelMid.Controls.Add(txtOneri);
            pnlMid.Controls.Add(borderPanelMid);
            mainLayout.Controls.Add(pnlMid, 1, 0);

            // --- SAĞ KOLON (7 Gün Sıralı Akış ve Yenilenen Ortalama Kartı) ---
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
                    c.Width = GetDayLabelWidth();
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

            dtpBaslangic.ValueChanged += HesaplaCanliSure;
            dtpBitis.ValueChanged += HesaplaCanliSure;

            countdownTimer = new System.Windows.Forms.Timer();
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += CountdownTimer_Tick;

            CheckActiveSession();
            HesaplaCanliSure(null, null);
        }

        private int GetDayLabelWidth()
        {
            return pnlDaysList.ClientSize.Width > 20 ? pnlDaysList.ClientSize.Width - 15 : 250;
        }

        // =========================================================
        // SAYAÇ VE ZAMAN KİLİTLEME MANTIĞI
        // =========================================================
        private void CheckActiveSession()
        {
            sessionStart = DatabaseHelper.GetActiveUykuSessionStart(userId);
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
                lblTimerGun.Text = "HAFTALIK UYKU SERÜVENİ";
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
                DatabaseHelper.StartNewUykuSession(userId, sessionStart.Value);
                aktifGun = 1;
                countdownTimer.Start();
            }

            DateTime kayitTarihi = sessionStart.Value.Date.AddDays(aktifGun - 1);
            TimeSpan sure = (dtpBitis.Value < dtpBaslangic.Value) ? dtpBitis.Value.AddDays(1) - dtpBaslangic.Value : dtpBitis.Value - dtpBaslangic.Value;

            DatabaseHelper.SaveUykuVerisi(userId, kayitTarihi,
                dtpBaslangic.Value.ToString("HH:mm"), dtpBitis.Value.ToString("HH:mm"), sure.TotalHours);

            MessageBox.Show($"{aktifGun}. gün uyku verileriniz başarıyla veritabanına kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GenerateRecommendations(sure.TotalHours, dtpBaslangic.Value.Hour);
            RefreshRightPanel();
            CountdownTimer_Tick(null, null);
        }

        // =========================================================
        // SAĞ PANEL: SIRALI LİSTE VE YENİ SADE ORTALAMA TASARIMI
        // =========================================================
        private void RefreshRightPanel()
        {
            pnlDaysList.Controls.Clear();
            if (sessionStart == null) return;

            var kayitlar = DatabaseHelper.GetWeeklyUykuRecords(userId, sessionStart.Value);
            double sumSure = 0; int validDaysCount = 0;

            for (int i = 1; i <= 7; i++)
            {
                string gunTarih = sessionStart.Value.Date.AddDays(i - 1).ToString("yyyy-MM-dd");
                string durumMetni = ""; Color bgColor = Color.FromArgb(30, 45, 75); Color fgColor = Color.White;

                if (kayitlar.ContainsKey(gunTarih))
                {
                    var v = kayitlar[gunTarih];
                    int h = (int)v.ToplamSure; int m = (int)((v.ToplamSure - h) * 60);
                    durumMetni = $"{i}. Gün: {v.UykuBaslangic} - {v.UykuBitis}\r\nSüre: {h} saat {m} dk";
                    bgColor = Color.FromArgb(39, 174, 96);
                    sumSure += v.ToplamSure; validDaysCount++;
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

                Label lblDay = new Label()
                {
                    Text = durumMetni,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    BackColor = bgColor,
                    ForeColor = fgColor,
                    Width = GetDayLabelWidth(),
                    Height = 65,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(0, 5, 0, 5)
                };
                pnlDaysList.Controls.Add(lblDay);
            }

            // DÜZELTME: İsteğin doğrultusunda "AKILLI" ibaresi kaldırıldı, ortadaki süre korundu ve alttaki not silindi!
            if (validDaysCount > 0)
            {
                double ortSure = sumSure / validDaysCount;
                int oh = (int)ortSure; int om = (int)((ortSure - oh) * 60);
                lblOrtalamaSonuc.Text = $"📊 {validDaysCount} GÜNLÜK ORTALAMANIZ\r\n\r\n" +
                       $"🌙 Ortalama Uyku Süresi:\r\n   {oh} saat {om} dakika";
            }
            else
            {
                lblOrtalamaSonuc.Text = "📊 ORTALAMANIZ\r\n\r\nHenüz sisteme girilmiş geçerli bir veri yok.";
            }
        }

        // =========================================================
        // BİLİMSEL ÖNERİ MOTORU VE YARDIMCI METOTLAR
        // =========================================================
        private void GenerateRecommendations(double totalHours, int yatmaSaati)
        {
            var oneriler = new List<string>();

            oneriler.Add("⏳ SÜRE ANALİZİ:");
            if (totalHours < 6)
            {
                oneriler.Add($"• Uyku süreniz yetersizdir. Az uyumak bağışıklığı düşürür ve gün içinde odaklanma sorunu yaratır.");
            }
            else if (totalHours <= 9)
            {
                oneriler.Add($"• Harika bir süre! Yetişkinler için tavsiye edilen ideal uyku aralığındasınız (7-9 saat).");
            }
            else
            {
                oneriler.Add($"• İhtiyacınızdan fazla uyumuşsunuz. Aşırı uyku gün boyu süren bir uyuşukluk hissi tetikler.");
            }

            oneriler.Add("🌙 BİYOLOJİK SAAT (SİRKADİYEN RİTİM) ANALİZİ:");
            if (yatmaSaati >= 1 && yatmaSaati < 6)
            {
                oneriler.Add("• Çok geç saatte uyumuşsunuz. Uyku süreniz iyi olsa bile uykunun verimi çok düşüktür.");
                oneriler.Add("• Hücre yenileyici Melatonin hormonunun zirve yaptığı 23:00 - 03:00 arasını uyanık geçirmişsiniz. Biyolojik saatiniz (sirkadiyen ritim) zarar görmüş.");
            }
            else if (yatmaSaati >= 6 && yatmaSaati <= 18)
            {
                oneriler.Add("• Gündüz uykusunu tercih etmişsiniz. Gece uykusundaki karanlık evre ve melatonin salgısı olmadığı için tam yenilenme sağlanamaz.");
            }
            else if (yatmaSaati >= 21 || yatmaSaati == 0)
            {
                oneriler.Add("• Muazzam! Uzmanların işaret ettiği ideal zaman diliminde (22:00 - 00:00) yatağa girmişsiniz.");
                oneriler.Add("• Vücudun kendini onardığı 23:00 - 03:00 arası 'Altın Saatler'den tam verim aldınız, uykunuz son derece verimli!");
            }
            else
            {
                oneriler.Add("• Erken bir saatte uyumuşsunuz. Fiziksel yorgunluğun yüksek olduğu günlerde bu durum normaldir.");
            }

            txtOneri.Text = "\r\n" + string.Join(Environment.NewLine + Environment.NewLine, oneriler);
        }

        private void HesaplaCanliSure(object sender, EventArgs e)
        {
            TimeSpan sure = (dtpBitis.Value < dtpBaslangic.Value) ? dtpBitis.Value.AddDays(1) - dtpBaslangic.Value : dtpBitis.Value - dtpBaslangic.Value;
            lblSure.Text = $"Toplam Süre: {sure.Hours} saat {sure.Minutes} dk";
        }

        private Label YeniEtiket(string metin, int x, int y)
        {
            return new Label() { Text = metin, ForeColor = Color.White, Font = new Font("Segoe UI", 18, FontStyle.Bold), AutoSize = true, Location = new Point(x, y) };
        }

        private DateTimePicker YeniSaatKontrol(int x, int y)
        {
            return new DateTimePicker() { Location = new Point(x, y), Format = DateTimePickerFormat.Custom, CustomFormat = "HH:mm", ShowUpDown = true, Font = new Font("Segoe UI", 24, FontStyle.Bold), Width = 350, BackColor = Color.WhiteSmoke, ForeColor = Color.Black };
        }
    }
}