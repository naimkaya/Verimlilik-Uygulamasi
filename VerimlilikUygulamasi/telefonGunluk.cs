using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class telefonGunluk : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Label lblIsOkulSure, lblSosyalSure;
        private NumericUpDown numIsSaat, numIsDakika;
        private NumericUpDown numSosyalSaat, numSosyalDakika;

        private Button btnHesapla;
        private TextBox txtOneri;
        private Panel pnlLeft, pnlRight;

        public telefonGunluk()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Telefon Kullanımı - Günlük Gelişmiş Analiz";
            this.BackColor = Color.FromArgb(10, 25, 50); // Standart Koyu Temamız

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
                Text = "Gelişmiş Ekran Süresi Analiz Motoru",
                Font = new Font("Segoe UI Black", 36F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            pnlTop.Controls.Add(lblTitle);
            btnGeriDon.BringToFront();

            // =========================================================
            // 2. ANA YERLEŞİM PLANI (%45 Sol Giriş - %55 Sağ Öneri)
            // =========================================================
            TableLayoutPanel mainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(80, 20, 80, 80),
                BackColor = Color.Transparent
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            this.Controls.Add(mainLayout);
            mainLayout.BringToFront();

            // --- Sol Panel ---
            pnlLeft = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(20) };
            mainLayout.Controls.Add(pnlLeft, 0, 0);

            int startY = 30;
            int gapY = 150;

            // 1. İş / Okul Kullanım Süresi
            lblIsOkulSure = YeniEtiket("İş / Okul Amaçlı Süre (Saat / Dakika):", 0, startY);
            numIsSaat = YeniSayiKontrol(0, startY + 45, 24, 210);
            numIsDakika = YeniSayiKontrol(240, startY + 45, 59, 210);
            pnlLeft.Controls.Add(lblIsOkulSure); pnlLeft.Controls.Add(numIsSaat); pnlLeft.Controls.Add(numIsDakika);

            // 2. Sosyal Medya / Kişisel Kullanım Süresi
            lblSosyalSure = YeniEtiket("Sosyal Medya / Kişisel Süre (Saat / Dakika):", 0, startY + gapY);
            numSosyalSaat = YeniSayiKontrol(0, startY + gapY + 45, 24, 210);
            numSosyalDakika = YeniSayiKontrol(240, startY + gapY + 45, 59, 210);
            pnlLeft.Controls.Add(lblSosyalSure); pnlLeft.Controls.Add(numSosyalSaat); pnlLeft.Controls.Add(numSosyalDakika);

            // Analiz Et Butonu (Canlı Yeşil)
            btnHesapla = new Button()
            {
                Text = "Kullanımı Analiz Et",
                Location = new Point(0, startY + gapY * 2 + 30),
                Size = new Size(450, 85),
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnHesapla.FlatAppearance.BorderSize = 0;
            btnHesapla.MouseEnter += (s, e) => { btnHesapla.BackColor = ControlPaint.Light(Color.FromArgb(39, 174, 96), 0.2f); btnHesapla.FlatAppearance.BorderSize = 3; btnHesapla.FlatAppearance.BorderColor = Color.White; };
            btnHesapla.MouseLeave += (s, e) => { btnHesapla.BackColor = Color.FromArgb(39, 174, 96); btnHesapla.FlatAppearance.BorderSize = 0; };
            btnHesapla.Click += BtnHesapla_Click;
            pnlLeft.Controls.Add(btnHesapla);

            // --- Sağ Panel ---
            pnlRight = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(40, 20, 0, 20) };
            mainLayout.Controls.Add(pnlRight, 1, 0);

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
                Text = "\r\n\r\n\r\nSol taraftan günlük sürelerinizi girip 'Kullanımı Analiz Et' butonuna bastığınızda, dijital sağlık raporunuz ve genişletilmiş derin detoks rehberiniz burada listelenecektir."
            };

            Panel borderPanel = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(3) };
            borderPanel.Controls.Add(txtOneri);
            pnlRight.Controls.Add(borderPanel);
        }

        private Label YeniEtiket(string metin, int x, int y)
        {
            return new Label() { Text = metin, ForeColor = Color.White, Font = new Font("Segoe UI", 18, FontStyle.Bold), AutoSize = true, Location = new Point(x, y) };
        }

        private NumericUpDown YeniSayiKontrol(int x, int y, decimal max, int width)
        {
            return new NumericUpDown()
            {
                Location = new Point(x, y),
                Maximum = max,
                Minimum = 0,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                Width = width,
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        // =========================================================
        // GENİŞLETİLMİŞ DİJİTAL SAĞLIK VE DERİN DETOKS ANALİZ MOTORU
        // =========================================================
        private void BtnHesapla_Click(object sender, EventArgs e)
        {
            int isDakika = ((int)numIsSaat.Value * 60) + (int)numIsDakika.Value;
            int sosyalDakika = ((int)numSosyalSaat.Value * 60) + (int)numSosyalDakika.Value;
            int toplamDakika = isDakika + sosyalDakika;

            var oneriler = new List<string>();

            if (toplamDakika == 0)
            {
                oneriler.Add("⚠️ Lütfen sol taraftaki alanlara süre girerek analiz butonuna basınız.");
                txtOneri.Text = string.Join(Environment.NewLine, oneriler);
                return;
            }

            if (toplamDakika > 1440)
            {
                oneriler.Add("❌ MANTIKSAL ZAMAN HATASI!");
                oneriler.Add("• Girdiğiniz sürelerin toplamı bir günün sınırı olan 24 saati aşmaktadır.");
                txtOneri.Text = string.Join(Environment.NewLine + Environment.NewLine, oneriler);
                return;
            }

            double toplamSaat = toplamDakika / 60.0;
            double uretkenlikYuzdesi = ((double)isDakika / toplamDakika) * 100;

            oneriler.Add("📱 NÖROLOJİK DİJİTAL SAĞLIK RAPORU:");
            oneriler.Add($"• Toplam Aktif Ekran Süresi: {toplamDakika / 60} saat {toplamDakika % 60} dakika");
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
            oneriler.Add($"• İş / Okul Odaklı (Geliştirici Süre): {isDakika / 60} saat {isDakika % 60} dakika");
            oneriler.Add($"• Sosyal Medya / Kişisel (Tüketici Süre): {sosyalDakika / 60} saat {sosyalDakika % 60} dakika");

            if (sosyalDakika > isDakika)
            {
                oneriler.Add("• Dikkat Eksikliği Uyarısı (Deep Work Kaybı): Sosyal medyada harcadığınız süre, üretkenlik sürenizin üzerine çıkmış durumda. Beyin bu duruma alışırsa, ders çalışırken veya kod yazarken ilk 5 dakikada sıkılıp elinizi refleks olarak telefona götürmenize neden olan 'dijital huzursuzluk' sendromu tetiklenir.");
            }
            else
            {
                oneriler.Add("• Verimlilik Notu: Telefonu bir tüketim çılgınlığı yerine üretim ve eğitim odaklı kullanmayı başarmışsınız. Dijital disiplininiz takdire şayan.");
            }

            // 3. GENİŞLETİLMİŞ DİJİTAL DETOKS REHBERİ (YENİ EKLEMELER)
            oneriler.Add("💡 TEKNOLOJİK VE PSİKOLOJİK ALTIN TAVSİYELER:");

            oneriler.Add("• Gri Ekran Modu (Nörolojik Bariyer): Telefon ekranınızı ayarlardan tamamen siyah-beyaz (gri tonlama) moduna alın. Görsellerdeki renk pikselleri yok olduğunda, beyninizin o uygulamalara duyduğu görsel dopamin arzusu kendiliğinden %40 oranında düşecektir.");

            oneriler.Add("• İlk ve Son 1 Saat Kuralı (Kortizol Kilidi): Sabah uyanır uyanmaz ilk 60 dakika ve gece yatağa girmeden önceki son 60 dakika telefona kesinlikle dokunmayın. Güne bildirim bombardımanıyla başlamak stres hormonunuzu (kortizol) tavan yaptırır.");

            oneriler.Add("• Fiziksel Uzaklık Taktiği (Odaklanma Bariyeri): Ders çalışırken, masa başında kod yazarken veya kitap okurken telefonunuzu kesinlikle aynı odada bulundurmayın. Telefon gözünün önünde durduğunda, ona dokunmasan bile beyninin bir kısmı 'bildirim gelecek mi' sorusunu işlemek için %20 ekstra enerji harcar.");

            oneriler.Add("• Radikal Sosyal Medya Diyeti: Keşfet akışında anlamsızca aşağı kaydırma (doomscrolling) yapmaya başladığınızı fark ettiğiniz an, uygulamayı kapatıp 10 saniye boyunca derin nefes alarak bilincinizi fiziksel odanıza geri getirin.");

            txtOneri.Text = "\r\n" + string.Join(Environment.NewLine + Environment.NewLine, oneriler);
        }
    }
}