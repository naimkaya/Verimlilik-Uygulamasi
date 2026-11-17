using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    // ÇÖZÜM: partial değiştiricisi eklendi, derleme hatası düzeltildi!
    public partial class suHaftalik : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Label lblTimerGun;
        private Label lblTimerSaat;

        private Label lblSuMiktari;
        private NumericUpDown numSu;
        private Button btnKaydet;

        private TextBox txtOneri;
        private FlowLayoutPanel pnlDaysList;
        private Label lblOrtalamaSonuc;

        private int userId = 1;
        private DateTime? sessionStart = null;
        private int aktifGun = 1;
        private System.Windows.Forms.Timer countdownTimer;

        public suHaftalik()
        {
            InitializeComponent();
            DatabaseHelper.InitializeDatabase();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Haftalık Su Takibi";
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
                Text = "Haftalık Su Serüveni",
                Font = new Font("Segoe UI Black", 32F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            pnlTop.Controls.Add(lblTitle);

            // =========================================================
            // 2. ANA YERLEŞİM PLANI (3 Kolonlu Dashboard)
            // =========================================================
            TableLayoutPanel mainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(40, 20, 40, 40),
                BackColor = Color.Transparent
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));  // Sol: Giriş ve Sayaç
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));  // Orta: Analiz Penceresi
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));  // Sağ: 7 Gün Akış Listesi
            this.Controls.Add(mainLayout);
            mainLayout.BringToFront();

            // --- SOL KOLON (Giriş Elemanları ve Canlı Sayaç) ---
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

            int startY = 130;

            lblSuMiktari = YeniEtiket("İçtiğiniz Su (Litre):", 0, startY);
            numSu = YeniSayiKontrol(0, startY + 45, 15);
            pnlLeft.Controls.Add(lblSuMiktari); pnlLeft.Controls.Add(numSu);

            btnKaydet = new Button()
            {
                Text = "Kaydet ve Analiz Al",
                Location = new Point(0, startY + 160),
                Size = new Size(350, 70),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                BackColor = Color.FromArgb(39, 174, 96), // Canlı Yeşil Buton
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

            // --- ORTA KOLON (Bilimsel Su Tüketim Önerileri) ---
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
                Text = "\r\n\r\nSol taraftan günlük su tüketiminizi litre bazında kaydederek 7 günlük süreci başlatın. Her gün için 24 saat süreniz olacak."
            };
            Panel borderPanelMid = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(3) };
            borderPanelMid.Controls.Add(txtOneri);
            pnlMid.Controls.Add(borderPanelMid);
            mainLayout.Controls.Add(pnlMid, 1, 0);

            // --- SAĞ KOLON (7 Gün Sıralı Liste ve Saf Beyaz Temiz Ortalama) ---
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
                Padding = new Padding(10) // Saf Beyaz Yazı
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
            sessionStart = DatabaseHelper.GetActiveSuSessionStart(userId);
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
                lblTimerGun.Text = "HAFTALIK SU SERÜVENİ";
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
                DatabaseHelper.StartNewSuSession(userId, sessionStart.Value);
                aktifGun = 1;
                countdownTimer.Start();
            }

            DateTime kayitTarihi = sessionStart.Value.Date.AddDays(aktifGun - 1);
            DatabaseHelper.SaveSuVerisi(userId, kayitTarihi, (double)numSu.Value);

            MessageBox.Show($"{aktifGun}. gün su verileriniz başarıyla veritabanına kaydedildi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);

            GenerateRecommendations((double)numSu.Value);
            RefreshRightPanel();
            CountdownTimer_Tick(null, null);
        }

        // =========================================================
        // SAĞ PANEL: SIRALI AKIŞ LİSTESİ VE YENİ SADE ORTALAMA
        // =========================================================
        private void RefreshRightPanel()
        {
            pnlDaysList.Controls.Clear();
            if (sessionStart == null) return;

            var kayitlar = DatabaseHelper.GetWeeklySuRecords(userId, sessionStart.Value);
            double sumLitre = 0; int validDaysCount = 0;

            for (int i = 1; i <= 7; i++)
            {
                string gunTarih = sessionStart.Value.Date.AddDays(i - 1).ToString("yyyy-MM-dd");
                string durumMetni = ""; Color bgColor = Color.FromArgb(30, 45, 75); Color fgColor = Color.White;

                if (kayitlar.ContainsKey(gunTarih))
                {
                    var v = kayitlar[gunTarih];
                    durumMetni = $"{i}. Gün: {v.Litre:F2} Litre Su";
                    bgColor = Color.FromArgb(39, 174, 96); // Veri girilen gün yeşil
                    sumLitre += v.Litre; validDaysCount++;
                }
                else if (i < aktifGun)
                {
                    durumMetni = $"{i}. Gün: VERİ GİRİLMEMİŞTİR\r\n(Geri Dönülemez - Kilitli)";
                    bgColor = Color.FromArgb(192, 57, 43); // Süresi kaçan gün kırmızı
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

            // Tam istediğin gibi: "{X} GÜNLÜK ORTALAMANIZ" yapısı kuruldu, not silindi.
            if (validDaysCount > 0)
            {
                double ortLitre = sumLitre / validDaysCount;
                lblOrtalamaSonuc.Text = $"📊 {validDaysCount} GÜNLÜK ORTALAMANIZ\r\n\r\n" +
                       $"💧 Ortalama Su Tüketimi:\r\n   {ortLitre:F2} Litre";
            }
            else
            {
                lblOrtalamaSonuc.Text = "📊 ORTALAMANIZ\r\n\r\nHenüz sisteme girilmiş geçerli bir veri yok.";
            }
        }

        // =========================================================
        // BİLİMSEL ÖNERİ MOTORU VE TASARIM AYARLARI
        // =========================================================
        private void GenerateRecommendations(double suMiktari)
        {
            var oneriler = new List<string>();

            // 1. ANA DURUM VE HİDRASYON ANALİZİ
            oneriler.Add("💧 1. GENEL HİDRASYON VE VÜCUT ANALİZİ:");
            if (suMiktari < 1.5)
            {
                oneriler.Add($"• Günlük {suMiktari} litre su tüketimi kritik seviyenin altındadır. Bedeniniz şu an 'Hücresel Kıtlık' ve Dehidrasyon yaşıyor olabilir.");
                oneriler.Add("• Vücut susuz kaldığında ilk olarak kan hacmini korumaya çalışır, bu da dokulara ve beyne giden oksijeni azaltarak kronik baş ağrısı ve yorgunluk yapar.");
                oneriler.Add("• Böbreklerin süzme yeteneği yavaşlar ve karaciğer ek mesai yapmaya başlar. İdrar renginizi kontrol edin; koyu sarı veya kehribar tonlarındaysa acilen su bardağına sarılmalısınız.");
            }
            else if (suMiktari <= 3.5)
            {
                oneriler.Add($"• Tebrikler! Günlük {suMiktari} litre su tüketimi ile tam olarak ideal biyolojik aralıktasınız (Altın Oran).");
                oneriler.Add("• Hücre içi ve dışı sıvı dengeniz kusursuz işliyor. Bu sayede gün boyu zihinsel odaklanmanız, enerjiniz ve refleksleriniz en üst performansında kalacaktır.");
                oneriler.Add("• Organlarınızın etrafındaki sıvı ve eklemlerinizdeki kayganlık maksimum seviyede korunarak uzun vadeli fizyolojik sağlığınız destekleniyor.");
            }
            else
            {
                oneriler.Add($"• Günlük {suMiktari} litre su tüketmişsiniz. Yoğun terleten ağır antrenmanlar yapmıyorsanız veya aşırı sıcakta değilseniz bu miktar sınır sınırıdır.");
                oneriler.Add("• Kısa sürede çok fazla su yüklenmek kandaki sodyum seviyesini düşürerek 'Hiponatremi' riskini doğurabilir. Suyu birden içmek yerine zamana yayarak tüketin.");
            }

            // 2. METABOLİZMA VE YAĞ YAKIMI (KİLO KONTROLÜ)
            oneriler.Add("🔥 2. METABOLİZMA VE KİLO KONTROLÜNE ETKİSİ:");
            if (suMiktari < 1.5)
            {
                oneriler.Add("• Yetersiz su tüketimi metabolizma hızınızı doğrudan yavaşlatır. Vücut yağları yakmak yerine koruma moduna geçer.");
                oneriler.Add("• Çoğu zaman beyndeki susuzluk sinyalleri ile açlık sinyalleri karışır. Canınız tatlı veya abur cubur çekiyorsa, aslında vücudunuz sadece su istiyor olabilir.");
            }
            else if (suMiktari <= 3.5)
            {
                oneriler.Add("• İdeal su alımı, dinlenme halindeki metabolizma hızınızı %24 ila %30 arasında artırır. Bu da oturduğunuz yerde bile daha fazla kalori yakmanızı sağlar.");
                oneriler.Add("• Lipoliz (yağların parçalanması) sürecinin ilk adımı hidrolizdir, yani vücudun yağ yakabilmesi için moleküler düzeyde suya ihtiyacı vardır. Tam olarak yağ yakım modundasınız!");
            }

            // 3. DETOKS, ÖDEM VE CİLT SAĞLIĞI
            oneriler.Add("✨ 3. TOKSİN ATILIMI, ÖDEM VE CİLT SAĞLIĞI:");
            if (suMiktari < 1.5)
            {
                oneriler.Add("• Vücudunuz 'su bulamayacağım' korkusuyla mevcut sıvıyı dokularda hapsetmeye başlar. Yaşadığınız şişkinlik ve ödemlerin en büyük sebebi aslında az su içmenizdir.");
                oneriler.Add("• Toksinler ter ve idrarla atılamadığı için ciltte birikir; bu da kuruluk, akne eğilimi ve erken kırışıklık belirtilerine davetiye çıkarır.");
            }
            else if (suMiktari <= 3.5)
            {
                oneriler.Add("• Kan dolaşımınız harika durumda olduğu için cildiniz içten dışa nemleniyor, elastikiyeti artıyor ve daha parlak/canlı bir görünüme kavuşuyor.");
                oneriler.Add("• Lenf sisteminiz tıkır tıkır çalışarak ödemleri ve zararlı atıkları vücudunuzdan hızla uzaklaştırıyor. Doğal bir detoks sürecindesiniz.");
            }

            // 4. KAFEİN VE GÜNLÜK İÇECEK ALIŞKANLIKLARI
            oneriler.Add("☕ 4. ÇAY, KAHVE VE ALTERNATİF İÇECEK KURALI:");
            oneriler.Add("• Gün içinde içtiğiniz çay, kahve, kola veya enerji içecekleri asla suyun yerini tutmaz.");
            oneriler.Add("• Aksine, bu içeceklerin içindeki kafein 'Diüretik' (idrar söktürücü) etkiye sahiptir. Yani aldığınız her bir bardak kahve, vücudunuzdan iki bardak suyun eksilmesine neden olur.");
            oneriler.Add("• Altın Kural: Tükettiğiniz her fincan kahve veya büyük bardak çayın ardından, vücudu dengelemek için mutlaka ek olarak 1 büyük bardak saf su içmeyi alışkanlık haline getirin.");

            // 5. ENERJİ VE ZİHİNSEL PERFORMANS
            oneriler.Add("🧠 5. ZİHİNSEL PERFORMANS VE BEYİN SİSİ:");
            if (suMiktari < 2.0)
            {
                oneriler.Add("• Beyninizin %75'inden fazlası sudan oluşur. Sadece %1-2'lik küçük bir sıvı kaybı bile odaklanma sürelerinde daralmaya, hafif unutkanlıklara ve 'beyin sisi' dediğimiz zihinsel bulanıklığa yol açar.");
                oneriler.Add("• Öğleden sonra gelen ani uyku basmaları ve motivasyon kayıplarının birçoğu susuzluk kökenlidir.");
            }
            else
            {
                oneriler.Add("• Beyin omurilik sıvınız dengeli, nöronlar arası sinyal iletim hızı maksimum seviyede. Gün boyu yüksek odaklanma ve güçlü bir hafıza performansı için harika bir temel hazırladınız.");
            }

            txtOneri.Text = "\r\n" + string.Join(Environment.NewLine + Environment.NewLine, oneriler);
        }

        private Label YeniEtiket(string metin, int x, int y)
        {
            return new Label() { Text = metin, ForeColor = Color.White, Font = new Font("Segoe UI", 18, FontStyle.Bold), AutoSize = true, Location = new Point(x, y) };
        }

        private NumericUpDown YeniSayiKontrol(int x, int y, decimal max)
        {
            return new NumericUpDown() { Location = new Point(x, y), Maximum = max, Minimum = 0, DecimalPlaces = 2, Increment = 0.1M, Font = new Font("Segoe UI", 24, FontStyle.Bold), Width = 350, BackColor = Color.WhiteSmoke, ForeColor = Color.Black };
        }
    }
}