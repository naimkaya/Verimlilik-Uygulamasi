using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class telefonHaftalik : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Label lblTimerGun;
        private Label lblTimerSaat;

        private Label lblIsOkulSure, lblSosyalSure;
        private NumericUpDown numIsSaat, numIsDakika;
        private NumericUpDown numSosyalSaat, numSosyalDakika;
        private Button btnKaydet;

        private TextBox txtOneri;
        private FlowLayoutPanel pnlDaysList;
        private Label lblOrtalamaSonuc;

        private int userId = 1;
        private DateTime? sessionStart = null;
        private int aktifGun = 1;
        private System.Windows.Forms.Timer countdownTimer;

        public telefonHaftalik()
        {
            InitializeComponent();
            DatabaseHelper.InitializeDatabase();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Haftalık Telefon Kullanım Takibi";
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
                Text = "Haftalık Gelişmiş Detoks Serüveni",
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
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));  // Sol: Girişler ve Sayaç
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));  // Orta: Genişletilmiş Nörolojik Öneriler
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));  // Sağ: 7 Gün Akış Listesi
            this.Controls.Add(mainLayout);
            mainLayout.BringToFront();

            // --- SOL KOLON (Kullanım Amacı Girişleri ve Sayaç) ---
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
            int gapY = 110;

            // 1. İş / Okul Süresi
            lblIsOkulSure = YeniEtiket("İş / Okul Amaçlı Süre (Saat / Dakika):", 0, startY);
            numIsSaat = YeniSayiKontrol(0, startY + 35, 24, 160);
            numIsDakika = YeniSayiKontrol(180, startY + 35, 59, 160);
            pnlLeft.Controls.Add(lblIsOkulSure); pnlLeft.Controls.Add(numIsSaat); pnlLeft.Controls.Add(numIsDakika);

            // 2. Sosyal Medya / Kişisel Süre
            lblSosyalSure = YeniEtiket("Sosyal Medya / Kişisel (Saat / Dakika):", 0, startY + gapY);
            numSosyalSaat = YeniSayiKontrol(0, startY + gapY + 35, 24, 160);
            numSosyalDakika = YeniSayiKontrol(180, startY + gapY + 35, 59, 160);
            pnlLeft.Controls.Add(lblSosyalSure); pnlLeft.Controls.Add(numSosyalSaat); pnlLeft.Controls.Add(numSosyalDakika);

            btnKaydet = new Button()
            {
                Text = "Kaydet ve Analiz Al",
                Location = new Point(0, startY + gapY * 2 + 20),
                Size = new Size(340, 70),
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

            // --- ORTA KOLON (Genişletilmiş Derin Detoks Önerileri) ---
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
                Text = "\r\n\r\nSol taraftan günlük İş/Okul ve Sosyal Medya ekran sürelerinizi girerek 7 günlük takibi başlatın. Süreç boyunca dopamin seviyeniz, derin odaklanma kayıplarınız ve haftalık üretkenlik endeksiniz burada kapsamlıca analiz edilecektir."
            };
            Panel borderPanelMid = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(3) };
            borderPanelMid.Controls.Add(txtOneri);
            pnlMid.Controls.Add(borderPanelMid);
            mainLayout.Controls.Add(pnlMid, 1, 0);

            // --- SAĞ KOLON (7 Gün Sıralı Liste ve Saf Beyaz Ortalama Kartı) ---
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
            sessionStart = DatabaseHelper.GetActiveTelefonSessionStart(userId);
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
                lblTimerGun.Text = "HAFTALIK DETOKS SERÜVENİ";
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

            int isDakika = ((int)numIsSaat.Value * 60) + (int)numIsDakika.Value;
            int sosyalDakika = ((int)numSosyalSaat.Value * 60) + (int)numSosyalDakika.Value;
            int toplamDakika = isDakika + sosyalDakika;

            if (toplamDakika == 0)
            {
                MessageBox.Show("Lütfen alanlara geçerli süreler giriniz!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (toplamDakika > 1440)
            {
                MessageBox.Show("Girdiğiniz sürelerin toplamı bir günün sınırı olan 24 saati aşamaz!", "Zamanlama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (sessionStart == null)
            {
                sessionStart = DateTime.Now;
                DatabaseHelper.StartNewTelefonSession(userId, sessionStart.Value);
                aktifGun = 1;
                countdownTimer.Start();
            }

            DateTime kayitTarihi = sessionStart.Value.Date.AddDays(aktifGun - 1);
            DatabaseHelper.SaveTelefonVerisi(userId, kayitTarihi, toplamDakika, isDakika, sosyalDakika);

            MessageBox.Show($"{aktifGun}. gün ekran süresi kırılımlarınız başarıyla veritabanına kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GenerateRecommendations(toplamDakika, isDakika, sosyalDakika);
            RefreshRightPanel();
            CountdownTimer_Tick(null, null);
        }

        // =========================================================
        // SAĞ PANEL: SIRALI AKIŞ LİSTESİ VE HAFTALIK ÖZET KARTI
        // =========================================================
        private void RefreshRightPanel()
        {
            pnlDaysList.Controls.Clear();
            if (sessionStart == null) return;

            var kayitlar = DatabaseHelper.GetWeeklyTelefonRecords(userId, sessionStart.Value);
            double sumToplam = 0; double sumIs = 0; double sumSosyal = 0; int validDaysCount = 0;

            for (int i = 1; i <= 7; i++)
            {
                string gunTarih = sessionStart.Value.Date.AddDays(i - 1).ToString("yyyy-MM-dd");
                string durumMetni = ""; Color bgColor = Color.FromArgb(30, 45, 75); Color fgColor = Color.White;

                if (kayitlar.ContainsKey(gunTarih))
                {
                    var v = kayitlar[gunTarih];
                    durumMetni = $"{i}. Gün: {(v.ToplamDakika / 60):F1} Saat\r\n(Üretken: {(v.IsOkulDakika / 60):F1} Sa)";
                    bgColor = Color.FromArgb(142, 68, 173); // Şık Telefon Teması Moru

                    sumToplam += v.ToplamDakika;
                    sumIs += v.IsOkulDakika;
                    sumSosyal += v.SosyalMedyaDakika;
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
                double ortToplam = (sumToplam / validDaysCount) / 60.0;
                double ortIs = (sumIs / validDaysCount) / 60.0;
                double ortSosyal = (sumSosyal / validDaysCount) / 60.0;
                double totalHaftalikUretkenlik = (sumIs / (sumToplam > 0 ? sumToplam : 1)) * 100;

                lblOrtalamaSonuc.Text = $"📊 {validDaysCount} GÜNLÜK ORTALAMANIZ\r\n\r\n" +
                       $"📱 Ortalama Ekran Süresi: {ortToplam:F2} Saat\r\n" +
                       $"💼 Üretken (İş/Okul): {ortIs:F2} Saat\r\n" +
                       $"🎈 Sosyal Medya/Eğlence: {ortSosyal:F2} Saat\r\n" +
                       $"📈 Genel Üretkenlik Oranı: %{totalHaftalikUretkenlik:F0}";
            }
            else
            {
                lblOrtalamaSonuc.Text = "📊 ORTALAMANIZ\r\n\r\nHenüz sisteme girilmiş geçerli bir veri yok.";
            }
        }

        // =========================================================
        // GENİŞLETİLMİŞ HAFTALIK DİJİTAL DETOKS VE DOPAMİN MOTORU
        // =========================================================
        private void GenerateRecommendations(double toplam, double isOkul, double sosyal)
        {
            var oneriler = new List<string>();
            oneriler.Add("📱 NÖROLOJİK DİJİTAL SAĞLIK RAPORU:");

            double toplamSaat = toplam / 60.0;
            double uretkenlikYuzdesi = (isOkul / (toplam > 0 ? toplam : 1)) * 100;

            oneriler.Add($"• Toplam Aktif Ekran Süresi: {toplam / 60:F0} saat {toplam % 60:F0} dakika");
            oneriler.Add($"• Dijital Üretkenlik Endeksiniz: %{uretkenlikYuzdesi:F0}");

            // 1. GENİŞLETİLMİŞ DURUM KARNESİ
            if (toplamSaat <= 2.5)
            {
                oneriler.Add("• Genel Durum: ÜSTÜN DİJİTAL BİLİNÇ! 🍃");
                oneriler.Add("• Tebrikler! Telefon ekranınız bir bağımlılık mekanizması değil, tamamen fonksiyonel bir araç olarak çalışıyor. Beyninizdeki nöronlar anlık bildirim stresine maruz kalmıyor, odaklanma süreniz maksimumda.");
            }
            else if (toplamSaat <= 5.0)
            {
                oneriler.Add("• Genel Durum: ORTALAMA TÜKETİM VE RİSK SINIRI. 📊");
                oneriler.Add("• Günümüz modern dünyası için standart bir tüketim seviyesi. Ancak dopamin reseptörlerinizin tembelleşmemesi ve uyku evrelerinizin bozulmaması için akşam saatlerinde ekran kullanımını kısıtlamalısınız.");
            }
            else
            {
                oneriler.Add("• Genel Durum: YOĞUN DOPAMİN ESARETİ VE BAĞIMLILIK ALARMI! ⚠️");
                oneriler.Add("• Gününüzün çok büyük bir bölümünü parlak bir ekrana bakarak geçiriyorsunuz. Beyniniz sürekli olarak sahte ve ucuz uyarıcılara (kaydırma, beğeni, anlık video) maruz kaldığı için gerçek dünyadaki zor görevlere odaklanma kaslarınız köreliyor olabilir.");
            }

            // 2. KULLANIM AMACI VE DERİN ZİHİNSEL ANALİZ
            oneriler.Add("📊 KULLANIM AMACI VE ZİHİNSEL ODAK ANALİZİ:");
            oneriler.Add($"• İş / Okul Odaklı (Geliştirici Süre): {isOkul / 60:F0} saat {isOkul % 60:F0} dakika");
            oneriler.Add($"• Sosyal Medya / Kişisel (Tüketici Süre): {sosyal / 60:F0} saat {sosyal % 60:F0} dakika");

            if (sosyal > isOkul)
            {
                oneriler.Add("• Dikkat Eksikliği Uyarısı (Deep Work Kaybı): Sosyal medyada harcadığınız süre, üretkenlik sürenizin üzerine çıkmış durumda. Beyin bu duruma alışırsa, ders çalışırken veya projenize odaklanırken ilk 5 dakikada sıkılıp elinizi refleks olarak telefona götürmenize neden olan 'dijital huzursuzluk' sendromu tetiklenir.");
            }
            else
            {
                oneriler.Add("• Verimlilik Notu: Telefonu bir tüketim çılgınlığı yerine üretim ve eğitim odaklı kullanmayı başarmışsınız. Dijital disiplininiz takdire şayan.");
            }

            // 3. GENİŞLETİLMİŞ DİJİTAL DETOKS REHBERİ
            oneriler.Add("💡 TEKNOLOJİK VE PSİKOLOJİK ALTIN TAVSİYELER:");
            oneriler.Add("• Gri Ekran Modu (Nörolojik Bariyer): Telefon ekranınızı ayarlardan tamamen siyah-beyaz (gri tonlama) moduna alın. Görsellerdeki renk pikselleri yok olduğunda, beyninizin o uygulamalara duyduğu görsel dopamin arzusu kendiliğinden %40 oranında düşecektir.");
            oneriler.Add("• İlk ve Son 1 Saat Kuralı (Kortizol Kilidi): Sabah uyanır uyanmaz ilk 60 dakika ve gece yatağa girmeden önceki son 60 dakika telefona kesinlikle dokunmayın. Güne bildirim bombardımanıyla başlamak stres hormonunuzu (kortizol) tavan yaptırır.");
            oneriler.Add("• Fiziksel Uzaklık Taktiği (Odaklanma Bariyeri): Ders çalışırken veya bilgisayar başında odaklanırken telefonunuzu kesinlikle farklı bir odada bulundurun. Telefon göz önünde durduğunda, ona dokunmasanız bile beyninizin bir kısmı refleks olarak bildirimleri bekler ve odağınızı %20 baltalar.");
            oneriler.Add("• Radikal Sosyal Medya Diyeti: Keşfet akışında anlamsızca aşağı kaydırma (doomscrolling) yapmaya başladığınızı fark ettiğiniz an, uygulamayı kapatıp 10 saniye boyunca derin nefes alarak bilincinizi fiziksel odanıza geri getirin.");

            txtOneri.Text = "\r\n" + string.Join(Environment.NewLine + Environment.NewLine, oneriler);
        }

        private Label YeniEtiket(string metin, int x, int y)
        {
            return new Label() { Text = metin, ForeColor = Color.White, Font = new Font("Segoe UI", 15, FontStyle.Bold), AutoSize = true, Location = new Point(x, y) };
        }

        private NumericUpDown YeniSayiKontrol(int x, int y, decimal max, int width)
        {
            return new NumericUpDown() { Location = new Point(x, y), Maximum = max, Minimum = 0, Font = new Font("Segoe UI", 18, FontStyle.Bold), Width = width, BackColor = Color.WhiteSmoke, ForeColor = Color.Black, BorderStyle = BorderStyle.FixedSingle };
        }
    }
}