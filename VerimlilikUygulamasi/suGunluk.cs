using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class suGunluk : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Label lblSuMiktari;
        private NumericUpDown numSu;

        private Button btnHesapla;
        private TextBox txtOneri;
        private Panel pnlLeft, pnlRight;

        public suGunluk()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Su - Günlük Veri Girişi";
            this.BackColor = Color.FromArgb(10, 25, 50); // Koyu lacivert tema

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
                Text = "Günlük Su Analizi",
                Font = new Font("Segoe UI Black", 36F, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            pnlTop.Controls.Add(lblTitle);
            btnGeriDon.BringToFront();

            // =========================================================
            // 2. ANA YERLEŞİM PLANI (Sol: Giriş, Sağ: Sonuç Kartı)
            // =========================================================
            TableLayoutPanel mainLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(80, 40, 80, 80),
                BackColor = Color.Transparent
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F)); // Sol taraf %45
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F)); // Sağ taraf %55
            this.Controls.Add(mainLayout);
            mainLayout.BringToFront();

            // --- Sol Panel (Daha Büyük Giriş Elemanları) ---
            pnlLeft = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(20) };

            int startY = 60;
            int gapY = 140;

            lblSuMiktari = YeniEtiket("İçtiğiniz Su Miktarı (Litre):", 0, startY);
            numSu = YeniSayiKontrol(0, startY + 50, 15);
            pnlLeft.Controls.Add(lblSuMiktari);
            pnlLeft.Controls.Add(numSu);

            // Analiz Et Butonu (Beslenmedeki Canlı Yeşil Butonla Birebir Aynı)
            btnHesapla = new Button()
            {
                Text = "Analiz Et ve Öneri Al",
                Location = new Point(0, startY + gapY + 20),
                Size = new Size(450, 90),
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                BackColor = Color.FromArgb(39, 174, 96), // Şık Canlı Yeşil
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

            // --- Sağ Panel (Sonuç ve Bilimsel Öneriler Ekranı) ---
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
                Text = "\r\n\r\n\r\nSol taraftan günlük su tüketim miktarınızı litre cinsinden girip 'Analiz Et' butonuna bastığınızda, vücudunuzun hidrasyon durumuna özel analizler burada görünecektir."
            };

            Panel borderPanel = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(3) };
            borderPanel.Controls.Add(txtOneri);
            pnlRight.Controls.Add(borderPanel);

            mainLayout.Controls.Add(pnlRight, 1, 0);

            btnGeriDon.MouseEnter += (s, e) => { btnGeriDon.BackColor = Color.Silver; };
            btnGeriDon.MouseLeave += (s, e) => { btnGeriDon.BackColor = Color.FromArgb(160, 160, 160); };
        }

        // --- TASARIM METOTLARI ---
        private Label YeniEtiket(string metin, int x, int y)
        {
            return new Label() { Text = metin, ForeColor = Color.White, Font = new Font("Segoe UI", 22, FontStyle.Bold), AutoSize = true, Location = new Point(x, y) };
        }

        private NumericUpDown YeniSayiKontrol(int x, int y, decimal max)
        {
            return new NumericUpDown()
            {
                Location = new Point(x, y),
                Maximum = max,
                Minimum = 0,
                DecimalPlaces = 2,    // 0.1 gibi ondalıklı girişleri destekler
                Increment = 0.1M,
                Font = new Font("Segoe UI", 30, FontStyle.Bold),
                Width = 450,
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        // =========================================================
        // GELİŞMİŞ BİYOLOJİK SU ANALİZ MOTORU
        // =========================================================
        private void BtnHesapla_Click(object sender, EventArgs e)
        {
            double suMiktari = (double)numSu.Value;
            var oneriler = new List<string>();

            if (suMiktari == 0)
            {
                oneriler.Add("⚠️ Lütfen sol taraftan içtiğiniz su miktarını girerek analiz butonuna basınız.");
                txtOneri.Text = string.Join(Environment.NewLine, oneriler);
                return;
            }

            oneriler.Add("💧 HİDRASYON VE VÜCUT ANALİZİ:");

            if (suMiktari < 1.5)
            {
                oneriler.Add($"• Günlük {suMiktari} litre su tüketmişsiniz. Vücudunuz şu an DEHİDRASYON (ciddi susuzluk) sınırında!");
                oneriler.Add("• Hücrelerin, iç organların ve özellikle beynin verimli çalışabilmesi için su miktarını acilen artırmalısınız.");
                oneriler.Add("• Bu seviyedeki susuzluk; gün içinde kronik baş ağrısı, ani yorgunluk, odaklanma güçlüğü (beyin sisi) ve metabolizmanın yavaşlamasına doğrudan sebep olur.");
                oneriler.Add("• Böbrekleriniz atıkları süzmek için çok ağır bir yük altına girer. İdrar renginiz koyu sarı tonlarındaysa bu vücudunuzun acil yardım çığlığıdır.");
            }
            else if (suMiktari < 2.5)
            {
                oneriler.Add($"• Günlük {suMiktari} litre su tüketmişsiniz. Yaşamsal fonksiyonlar için minimum sınırı geçmişsiniz ancak optimum verimlilik için yetersiz.");
                oneriler.Add("• Gün içinde enerjinizin aniden düştüğünü veya aç olmadığınız halde tatlı krizlerinizin geldiğini hissediyorsanız sebebi gizli susuzluk olabilir.");
                oneriler.Add("• Toksinlerin vücuttan rahatça atılması, sindirim sisteminizin rahatlaması ve cildinizin nem dengesini koruması için her gün yarım litre daha su eklemeyi deneyin.");
                oneriler.Add("• Çalışma masanıza büyük bir sürahi koymak veya telefonunuza hatırlatıcı kurmak bu miktarı ideal seviyeye çekmenize yardımcı olacaktır.");
            }
            else if (suMiktari <= 3.5)
            {
                oneriler.Add($"• Muazzam! Günlük {suMiktari} litre su tüketimi ile vücudunuzun biyolojik olarak ihtiyaç duyduğu ALTIN ARALIKTASINIZ.");
                oneriler.Add("• Bu harika seviye sayesinde sindirim sisteminiz tıkır tıkır çalışır, metabolizma hızınız desteklenir ve besinler hücrelerinize en hızlı şekilde taşınır.");
                oneriler.Add("• Zihinsel performansınızın, üretkenliğinizin ve gün boyu enerjinizin en tepe noktada kalmasını sağlıyorsunuz. Bu harika ve sağlıklı alışkanlığı kesinlikle bozmayın!");
            }
            else
            {
                oneriler.Add($"• Günlük {suMiktari} litre su tüketmişsiniz. Eğer çok ağır sporlar yapmıyor veya aşırı sıcak bir şantiyede/ortamda çalışmıyorsanız, bu miktar vücudunuz için biraz fazladır.");
                oneriler.Add("• Kısa sürede çok fazla su tüketmek, kandaki sodyum dengesini bozarak 'Hiponatremi' (Su Zehirlenmesi) riskini doğurabilir ve böbrekleri gereksiz yere fazla çalıştırıp yorar.");
                oneriler.Add("• Su tüketimini bir anda yüklenmek yerine günün geneline yayarak biraz azaltmanızı ve vücudun mineral kaybı yaşamasını engellemenizi tavsiye ederiz.");
            }

            oneriler.Add("💡 UNUTMAMANIZ GEREKEN BİLGİLER:");
            oneriler.Add("• Çay, kahve veya asitli içecekler suyun yerini tutmaz; aksine içerdikleri kafein yüzünden vücuttan su atılmasına neden olurlar. Her kahve fincanının yanında mutlaka 1 büyük bardak su tüketin.");

            txtOneri.Text = "\r\n" + string.Join(Environment.NewLine + Environment.NewLine, oneriler);
        }
    }
}