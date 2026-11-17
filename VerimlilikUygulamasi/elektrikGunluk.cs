using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization; // ÇÖZÜM İÇİN EKLENDİ
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class elektrikGunluk : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Label lblKwh, lblKisiSayisi, lblCihazSayisi;
        private NumericUpDown numKwh, numKisiSayisi, numCihazSayisi;

        private Button btnHesapla;
        private TextBox txtOneri;
        private Panel pnlLeft, pnlRight;

        public elektrikGunluk()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Elektrik - Günlük Tüketim Analizi";
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
                Text = "Günlük Enerji Analizi",
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

            int startY = 20;
            int gapY = 130;

            lblKwh = YeniEtiket("Günlük Tüketim (kWh):", 0, startY);
            numKwh = YeniSayiKontrol(0, startY + 45, 500, true);
            pnlLeft.Controls.Add(lblKwh); pnlLeft.Controls.Add(numKwh);

            lblKisiSayisi = YeniEtiket("Evdeki Kişi Sayısı:", 0, startY + gapY);
            numKisiSayisi = YeniSayiKontrol(0, startY + gapY + 45, 20, false);
            numKisiSayisi.Value = 1;
            pnlLeft.Controls.Add(lblKisiSayisi); pnlLeft.Controls.Add(numKisiSayisi);

            lblCihazSayisi = YeniEtiket("Aktif Önemli Cihaz Sayısı:", 0, startY + gapY * 2);
            numCihazSayisi = YeniSayiKontrol(0, startY + gapY * 2 + 45, 100, false);
            pnlLeft.Controls.Add(lblCihazSayisi); pnlLeft.Controls.Add(numCihazSayisi);

            btnHesapla = new Button()
            {
                Text = "Analiz Et ve Öneri Ver",
                Location = new Point(0, startY + gapY * 3 + 20),
                Size = new Size(450, 85),
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                BackColor = Color.FromArgb(39, 174, 96),
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

            // --- Sağ Panel ---
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
                Text = "\r\n\r\n\r\nSol taraftan günlük tüketim miktarınızı (kWh), kişi sayısını ve cihaz sayısını girip 'Analiz Et' butonuna bastığınızda, evinizin enerji verimliliği karnesi ve faturayı düşürecek akıllı tasarruf tüyoları burada listelenecektir."
            };

            Panel borderPanel = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(3) };
            borderPanel.Controls.Add(txtOneri);
            pnlRight.Controls.Add(borderPanel);

            mainLayout.Controls.Add(pnlRight, 1, 0);

            btnGeriDon.MouseEnter += (s, e) => { btnGeriDon.BackColor = Color.Silver; };
            btnGeriDon.MouseLeave += (s, e) => { btnGeriDon.BackColor = Color.FromArgb(160, 160, 160); };
        }

        private Label YeniEtiket(string metin, int x, int y)
        {
            return new Label() { Text = metin, ForeColor = Color.White, Font = new Font("Segoe UI", 20, FontStyle.Bold), AutoSize = true, Location = new Point(x, y) };
        }

        private NumericUpDown YeniSayiKontrol(int x, int y, decimal max, bool isDecimal)
        {
            var num = new NumericUpDown()
            {
                Location = new Point(x, y),
                Maximum = max,
                Minimum = 0,
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                Width = 450,
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };

            if (isDecimal)
            {
                num.DecimalPlaces = 2;
                num.Increment = 0.5M;
            }
            return num;
        }

        // =========================================================
        // AKILLI ENERJİ VERİMLİLİĞİ VE İSRAF ANALİZ MOTORU
        // =========================================================
        private void BtnHesapla_Click(object sender, EventArgs e)
        {
            // ÇÖZÜM: Kültür ayarını TR yaparak basamak kayma bug'ını tamamen temizliyoruz
            System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("tr-TR");

            double kwh = (double)numKwh.Value;
            int kisi = (int)numKisiSayisi.Value;
            int cihaz = (int)numCihazSayisi.Value;

            if (kisi <= 0) kisi = 1;

            var oneriler = new List<string>();

            if (kwh == 0)
            {
                oneriler.Add("⚠️ Lütfen sol taraftan elektrik tüketim miktarını (kWh) girerek analiz butonuna basınız.");
                txtOneri.Text = string.Join(Environment.NewLine, oneriler);
                return;
            }

            double kisiBasiKwh = kwh / kisi;

            oneriler.Add("⚡ ENERJİ TÜKETİM VE VERİMLİLİK KARNESİ:");

            if (kisiBasiKwh <= 2.0)
            {
                oneriler.Add($"• Kişi Başı Tüketim {kisiBasiKwh:F2} kWh: HARİKA EKOLOJİK MOD! 🍃");
                oneriler.Add($"• Evde toplam {kwh} kWh tüketiliyor. Kişi başına düşen yük mükemmel seviyede. Karbon ayak iziniz son derece düşük.");
            }
            else if (kisiBasiKwh <= 4.0)
            {
                oneriler.Add($"• Kişi Başı Tüketim {kisiBasiKwh:F2} kWh: STANDART / DENGELİ SEVİYE. 📊");
                oneriler.Add($"• Evde toplam {kwh} kWh tüketiliyor. Türkiye standartlarında, israf içermeyen, makul ve dengeli bir enerji kullanımı sürdürüyorsunuz.");
            }
            else
            {
                oneriler.Add($"• Kişi Başı Tüketim {kisiBasiKwh:F2} kWh: YÜKSEK TÜKETİM VE İSRAF ALARMI! ⚠️");
                oneriler.Add($"• Evde toplam {kwh} kWh tüketiliyor. {kisi} kişilik bir hane için kişi başına {kisiBasiKwh:F2} kWh yük düşmesi çok fazla.");
                oneriler.Add("• Odalarda açık bırakılan lambalar, stand-by (bekleme) modunda fişe takılı unutulan cihazlar veya yüksek güç çeken aletler israfa yol açıyor olabilir.");
            }

            if (cihaz > 0)
            {
                oneriler.Add("🔌 CİHAZ VE İSRAF YOĞUNLUĞU:");
                oneriler.Add($"• Evinizde {cihaz} adet aktif önemli cihaz listelenmiş.");

                double cihazBasiYuk = kwh / cihaz;
                if (cihazBasiYuk > 1.5)
                {
                    oneriler.Add("• Cihaz başına düşen tüketim yükü çok yüksek! Muhtemelen bu cihazlar arasında A+ sınıfı olmayan eski buzdolapları, sürekli açık duran masaüstü bilgisayarlar veya elektrikli ısıtıcılar/klimalar var.");
                }
                oneriler.Add("• Vampir Akımlara Dikkat: Elektronik cihazları kapatıp 'Stand-by' (Bekleme) modunda bırakmak, toplam faturanızın yaklaşık %5 ila %10'unu oluşturan gizli bir elektrik israfına yol açar. Kullanılmayan cihazları fişten çekin.");
            }

            oneriler.Add("💡 FATURAYI CİDDİ ORANDA DÜŞÜRECEK GENİŞ REHBER:");
            oneriler.Add("• Akıllı Zamanlama (Üç Zamanlı Tarife): Eğer üç zamanlı tarifedeyseniz, çamaşır makinesi, bulaşık makinesi ve kurutma makinesi gibi yüksek watt çeken cihazları kesinlikle elektrik birim fiyatının en ucuz olduğu 'Gece 22:00 - Sabah 06:00' saatleri arasında çalıştırın.");
            oneriler.Add("• Mutfaktaki Gizli Tüketim (Buzdolabı): Buzdolabınızın arkasında en az 10 cm boşluk bırakmak, motorunun rahat nefes almasını sağlayarak elektrik tüketimini %15 düşürür. Ayrıca buzluk kapaklarının contalarını kontrol edin; hava kaçıran buzdolabı iki kat fazla enerji harcar.");
            oneriler.Add("• Aydınlatma Devrimi: Evdeki klasik eski ampulleri tasarruflu LED ampullerle değiştirmek, aydınlatma maliyetinizi net olarak %80 oranında azaltacaktır.");
            oneriler.Add("• Klima ve Isıtıcı Ayarı: Klimaları yazın 24°C'nin, kışın ise 21°C'nin altına ayarlamayın. Klimayı her 1 derece daha soğuk/sıcak çalıştırmak, cihazın elektrik tüketimini %6 ila %8 oranında artırır.");
            oneriler.Add("• Ütü Kullanım Tüyosu: Ütüler evde en çok anlık akım çeken cihazlardan biridir. Ütüleme işlemi bitmeden 5 dakika önce ütünün fişini çekip mevcut kalan taban ısısıyla son kıyafetleri ütülemek ciddi bir elektrik tasarrufu sağlar.");
            oneriler.Add("• Elektronik Aletler ve Bilgisayarlar: Masaüstü oyun bilgisayarları ve büyük ekran televizyonlar yüksek güç harcar. Bilgisayar başında olmadığınızda cihazı uyku moduna almak yerine tamamen kapatmayı veya kapatma zamanlayıcısı kurmayı alışkanlık haline getirin.");

            txtOneri.Text = "\r\n" + string.Join(Environment.NewLine + Environment.NewLine, oneriler);
        }
    }
}