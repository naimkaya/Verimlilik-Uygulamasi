using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class beslenmeGunluk : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Label lblKalori, lblProtein, lblKarbonhidrat, lblYag;
        private NumericUpDown numKalori, numProtein, numKarbonhidrat, numYag;

        private Button btnHesapla;
        private TextBox txtOneri;
        private Panel pnlLeft, pnlRight;

        public beslenmeGunluk()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Beslenme - Günlük";
            this.BackColor = Color.FromArgb(10, 25, 50);

            // =========================================================
            // 1. ÜST PANEL VE BAŞLIK
            // =========================================================
            pnlTop = new Panel()
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.Transparent
            };
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
            btnGeriDon.Click += BtnGeriDon_Click;
            pnlTop.Controls.Add(btnGeriDon);

            // DÜZELTME: Özel Paint olayı kaldırıldı, düz ve pürüzsüz saf beyaz metin kullanıldı (Üst üste binme çözüldü)
            lblTitle = new Label()
            {
                Text = "Günlük Beslenme Verileri",
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

            // DÜZELTME: Yazıların kutuya girmemesi için boşluklar artırıldı
            int startY = 40;
            int gapY = 120; // Bloklar arası boşluk açıldı

            lblKalori = YeniEtiket("Kalori (kcal):", 0, startY);
            numKalori = YeniSayiKontrol(0, startY + 45, 10000); // 35'ten 45'e çıkarıldı (yazı kutuya girmeyecek)
            pnlLeft.Controls.Add(lblKalori);
            pnlLeft.Controls.Add(numKalori);

            lblProtein = YeniEtiket("Protein (gr):", 0, startY + gapY);
            numProtein = YeniSayiKontrol(0, startY + gapY + 45, 1000);
            pnlLeft.Controls.Add(lblProtein);
            pnlLeft.Controls.Add(numProtein);

            lblKarbonhidrat = YeniEtiket("Karbonhidrat (gr):", 0, startY + gapY * 2);
            numKarbonhidrat = YeniSayiKontrol(0, startY + gapY * 2 + 45, 2000);
            pnlLeft.Controls.Add(lblKarbonhidrat);
            pnlLeft.Controls.Add(numKarbonhidrat);

            lblYag = YeniEtiket("Yağ (gr):", 0, startY + gapY * 3);
            numYag = YeniSayiKontrol(0, startY + gapY * 3 + 45, 1000);
            pnlLeft.Controls.Add(lblYag);
            pnlLeft.Controls.Add(numYag);

            // Hesapla Butonu
            btnHesapla = new Button()
            {
                Text = "Hesapla ve Öneri Al",
                Location = new Point(0, startY + gapY * 4 + 10),
                Size = new Size(400, 80),
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
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
                Text = "\r\n\r\n\r\nSol taraftan günlük tüketim verilerinizi girip 'Hesapla' butonuna bastığınızda, size özel beslenme önerileriniz burada görünecektir."
            };

            Panel borderPanel = new Panel()
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(3)
            };
            borderPanel.Controls.Add(txtOneri);
            pnlRight.Controls.Add(borderPanel);

            mainLayout.Controls.Add(pnlRight, 1, 0);

            btnGeriDon.MouseEnter += (s, e) => { btnGeriDon.BackColor = Color.Silver; };
            btnGeriDon.MouseLeave += (s, e) => { btnGeriDon.BackColor = Color.FromArgb(160, 160, 160); };
        }

        // --- TASARIM METOTLARI ---
        private Label YeniEtiket(string metin, int x, int y)
        {
            return new Label()
            {
                Text = metin,
                // DÜZELTME: Okunabilirliği maksimuma çıkarmak için renk Saf Beyaz yapıldı
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(x, y)
            };
        }

        private NumericUpDown YeniSayiKontrol(int x, int y, decimal max)
        {
            return new NumericUpDown()
            {
                Location = new Point(x, y),
                Maximum = max,
                Font = new Font("Segoe UI", 22, FontStyle.Bold),
                Width = 400,
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                BorderStyle = BorderStyle.FixedSingle
            };
        }

        // --- GELİŞMİŞ VE DETAYLI ÖNERİ MOTORU ---
        private void BtnHesapla_Click(object sender, EventArgs e)
        {
            double kalori = (double)numKalori.Value;
            double protein = (double)numProtein.Value;
            double karbonhidrat = (double)numKarbonhidrat.Value;
            double yag = (double)numYag.Value;

            var oneriler = new List<string>();

            // Kalori Önerileri
            if (kalori == 0)
            {
                oneriler.Add("⚠️ Lütfen değerleri sol taraftan girerek hesapla butonuna basınız.");
                txtOneri.Text = string.Join(Environment.NewLine, oneriler);
                return;
            }
            else if (kalori < 1500)
            {
                oneriler.Add("🔥 KALORİ ALIMI: Düşük");
                oneriler.Add("• Günlük bazal metabolizma hızınızın altında kalori alıyor olabilirsiniz. Bu durum uzun vadede halsizlik, odaklanma problemi ve kas kaybına yol açabilir.");
                oneriler.Add("• Hacmi küçük ama kalorisi yüksek kuruyemiş (ceviz, badem) veya fıstık ezmesi gibi sağlıklı atıştırmalıklarla açığı kapatabilirsiniz.");
            }
            else if (kalori <= 2500)
            {
                oneriler.Add("🔥 KALORİ ALIMI: Dengeli");
                oneriler.Add("• Harika! Günlük enerji ihtiyacınızı tam kıvamında karşılıyorsunuz. Kilonuzu korumak ve verimli çalışmak için bu düzeni bozmamaya gayret edin.");
                oneriler.Add("• Bu kaloriyi boş gıdalardan ziyade vitamin değerleri yüksek tam gıdalardan aldığınızdan emin olun.");
            }
            else
            {
                oneriler.Add("🔥 KALORİ ALIMI: Yüksek");
                oneriler.Add("• İhtiyacınızdan fazla kalori alıyorsunuz. Eğer ağır bir spor yapmıyorsanız, bu durum zamanla yağlanmaya ve öğleden sonraları ağırlık çökmesine (uyku haline) sebep olabilir.");
                oneriler.Add("• Yemeklerden önce 1 büyük bardak su içmek ve salata/çorba gibi hacimli ama düşük kalorili yiyeceklere öncelik vermek tokluk hissinizi artıracaktır.");
            }

            // Protein Önerileri
            if (protein < 50)
            {
                oneriler.Add("🥩 PROTEİN ALIMI: Düşük");
                oneriler.Add("• Vücudunuzun onarımı ve bağışıklığınız için protein çok kritiktir. Yetersiz protein saç dökülmesine ve kas güçsüzlüğüne yol açabilir.");
                oneriler.Add("• Sabahları yumurta sayısını artırabilir, öğünlerinize mercimek, nohut gibi bitkisel veya tavuk/balık gibi hayvansal kaynaklar ekleyebilirsiniz.");
            }
            else if (protein <= 150)
            {
                oneriler.Add("🥩 PROTEİN ALIMI: Dengeli");
                oneriler.Add("• Kas onarımı ve tokluk hissi için mükemmel bir seviyedesiniz. Proteini tek bir öğünde yığmak yerine gün içine yayarak tüketmeniz sindiriminizi rahatlatacaktır.");
            }
            else
            {
                oneriler.Add("🥩 PROTEİN ALIMI: Yüksek");
                oneriler.Add("• Vücudunuz ihtiyacı olan proteini kullanır, fazlasını enerjiye çevirir veya yağ olarak depolar. Çok yüksek protein böbreklerinizi yorabilir.");
                oneriler.Add("• Bol bol su içerek böbreklerinize yardımcı olun ve sindirim sisteminizi rahatlatmak için proteinin yanında mutlaka lifli yeşillikler tüketin.");
            }

            // Karbonhidrat Önerileri
            if (karbonhidrat < 130)
            {
                oneriler.Add("🍞 KARBONHİDRAT ALIMI: Düşük");
                oneriler.Add("• Beyninizin temel yakıtı karbonhidrattır. Çok düşük karbonhidrat alımı 'beyin sisi' (odaklanamama) ve çabuk sinirlenme yapabilir.");
                oneriler.Add("• Beyaz ekmek yerine; yulaf, tatlı patates, tam buğday veya karabuğday gibi kaliteli (kompleks) karbonhidratları diyetinize eklemekten korkmayın.");
            }
            else if (karbonhidrat <= 300)
            {
                oneriler.Add("🍞 KARBONHİDRAT ALIMI: Dengeli");
                oneriler.Add("• Günlük zihinsel ve fiziksel aktiviteleriniz için harika bir yakıt seviyesi yakalamışsınız. Kan şekerinizin dalgalanmaması için karbonhidratları proteinlerle birlikte tüketmeye çalışın.");
            }
            else
            {
                oneriler.Add("🍞 KARBONHİDRAT ALIMI: Yüksek");
                oneriler.Add("• Fazla karbonhidrat, özellikle basit şekerlerden (tatlılar, beyaz un) geliyorsa ani kan şekeri düşüşlerine ve sürekli açlık hissine neden olur.");
                oneriler.Add("• Ağır karbonhidratlı bir yemek yediyseniz, yemekten sonra yapacağınız 15 dakikalık kısa bir yürüyüş kan şekerinizi mucizevi bir şekilde dengeleyecektir.");
            }

            // Yağ Önerileri
            if (yag < 30)
            {
                oneriler.Add("🥑 YAĞ ALIMI: Düşük");
                oneriler.Add("• Yağdan tamamen kaçmak sağlıklı değildir! A, D, E ve K vitaminlerinin vücutta emilebilmesi ve hormonlarınızın düzgün çalışması için yağa ihtiyacınız var.");
                oneriler.Add("• Salatalarınıza sızma zeytinyağı gezdirebilir, ara öğünlerde çiğ badem, fındık veya ceviz tüketebilirsiniz.");
            }
            else if (yag <= 70)
            {
                oneriler.Add("🥑 YAĞ ALIMI: Dengeli");
                oneriler.Add("• Beyin sağlığınız ve hormon dengeniz için ideal aralıktasınız. Sızma zeytinyağı, tereyağı ve avokado gibi doğal yağlara öncelik vermeye devam edin.");
            }
            else
            {
                oneriler.Add("🥑 YAĞ ALIMI: Yüksek");
                oneriler.Add("• Yağlar kalorisi çok yoğun besinlerdir (1 gramı 9 kalori). Bu aralıkta sürekli beslenmek hızlı kilo alımına sebep olabilir.");
                oneriler.Add("• Yemekleri kızartmak yerine fırınlama veya haşlama yöntemlerini deneyin. Soslardaki ve paketli gıdalardaki 'gizli yağlara' dikkat edin.");
            }

            txtOneri.Text = "\r\n" + string.Join(Environment.NewLine + Environment.NewLine, oneriler);
        }

        // Geri Dön Butonu İşlevi
        private void BtnGeriDon_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}