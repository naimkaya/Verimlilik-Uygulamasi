using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class sporKardiyo : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Panel pnlLeft;
        private Label lblKardiyoTuru, lblSure;
        private ComboBox cmbKardiyoTuru;
        private Panel pnlSureGroup;
        private NumericUpDown numSureSaat, numSureDakika;
        private Label lblSaatAyraci, lblDakikaAyraci;
        private Button btnAnalizEt;

        private TextBox txtAnalizSonuc;

        // Kullanıcı ID'sini sınıf düzeyinde saklıyoruz
        private int userId;

        // Kurucu metot dışarıdan gelen currentUserId değerini alıyor
        public sporKardiyo(int currentUserId)
        {
            this.userId = currentUserId;
            this.AutoScaleMode = AutoScaleMode.None; // Bulanıklık önlemi

            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(10, 25, 50);

            // --- ÜST PANEL ---
            pnlTop = new Panel();
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 100;
            pnlTop.BackColor = Color.Transparent;
            this.Controls.Add(pnlTop);

            // GERİ DÖN BUTONU (Sağ Üst Köşeye Sabitlendi)
            btnGeriDon = new Button();
            btnGeriDon.Text = "Geri Dön";
            btnGeriDon.Size = new Size(120, 45);
            btnGeriDon.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnGeriDon.BackColor = Color.FromArgb(160, 160, 160);
            btnGeriDon.ForeColor = Color.White;
            btnGeriDon.FlatStyle = FlatStyle.Flat;
            btnGeriDon.Cursor = Cursors.Hand;
            btnGeriDon.Anchor = AnchorStyles.Top | AnchorStyles.Right; // Sağ üst köşeye kenetleme
            btnGeriDon.Click += BtnGeriDon_Click;
            pnlTop.Controls.Add(btnGeriDon);

            lblTitle = new Label();
            lblTitle.Text = "Günlük Kardiyo Analizi";
            lblTitle.Font = new Font("Segoe UI Black", 28F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            pnlTop.Controls.Add(lblTitle);

            // Butonun her zaman en önde görünmesi için
            btnGeriDon.BringToFront();

            // --- ANA DÜZEN ---
            TableLayoutPanel mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.ColumnCount = 2;
            mainLayout.RowCount = 1;
            mainLayout.Padding = new Padding(20, 40, 20, 20);
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            this.Controls.Add(mainLayout);
            mainLayout.BringToFront();

            pnlLeft = new Panel();
            pnlLeft.Dock = DockStyle.Fill;
            mainLayout.Controls.Add(pnlLeft, 0, 0);

            int currentY = 20;

            lblKardiyoTuru = new Label();
            lblKardiyoTuru.Text = "Kardiyo Egzersiz Türü Seçimi:";
            lblKardiyoTuru.ForeColor = Color.White;
            lblKardiyoTuru.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblKardiyoTuru.AutoSize = true;
            lblKardiyoTuru.Location = new Point(10, currentY);

            cmbKardiyoTuru = new ComboBox();
            cmbKardiyoTuru.Width = 350;
            cmbKardiyoTuru.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            cmbKardiyoTuru.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbKardiyoTuru.BackColor = Color.WhiteSmoke;
            cmbKardiyoTuru.Items.AddRange(new object[] { "Koşu Bandı (Treadmill)", "Kondisyon Bisikleti", "Kürek Ergovometresi", "İp Atlama", "Eliptik Bisiklet" });
            cmbKardiyoTuru.Location = new Point(10, currentY + 30);

            currentY = cmbKardiyoTuru.Bottom + 25;

            lblSure = new Label();
            lblSure.Text = "Toplam Egzersiz Süresi:";
            lblSure.ForeColor = Color.White;
            lblSure.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblSure.AutoSize = true;
            lblSure.Location = new Point(10, currentY);

            pnlSureGroup = new Panel();
            pnlSureGroup.Width = 350;
            pnlSureGroup.Height = 80;
            pnlSureGroup.Location = new Point(10, currentY + 30);

            numSureSaat = new NumericUpDown() { Minimum = 0, Maximum = 5, Font = new Font("Segoe UI", 16, FontStyle.Bold), Width = 70, Location = new Point(0, 10) };
            lblSaatAyraci = new Label() { Text = "Saat", ForeColor = Color.LightGray, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(75, 18), AutoSize = true };
            numSureDakika = new NumericUpDown() { Minimum = 0, Maximum = 59, Font = new Font("Segoe UI", 16, FontStyle.Bold), Width = 70, Location = new Point(130, 10) };
            lblDakikaAyraci = new Label() { Text = "Dakika", ForeColor = Color.LightGray, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(205, 18), AutoSize = true };

            pnlSureGroup.Controls.Add(numSureSaat);
            pnlSureGroup.Controls.Add(lblSaatAyraci);
            pnlSureGroup.Controls.Add(numSureDakika);
            pnlSureGroup.Controls.Add(lblDakikaAyraci);

            currentY = pnlSureGroup.Bottom + 30;

            btnAnalizEt = new Button();
            btnAnalizEt.Text = "Kardiyoyu Analiz Et";
            btnAnalizEt.Width = 350;
            btnAnalizEt.Height = 60;
            btnAnalizEt.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            btnAnalizEt.BackColor = Color.FromArgb(46, 204, 113);
            btnAnalizEt.ForeColor = Color.White;
            btnAnalizEt.FlatStyle = FlatStyle.Flat;
            btnAnalizEt.Cursor = Cursors.Hand;
            btnAnalizEt.Click += BtnAnalizEt_Click;
            btnAnalizEt.Location = new Point(10, currentY);

            pnlLeft.Controls.Add(lblKardiyoTuru);
            pnlLeft.Controls.Add(cmbKardiyoTuru);
            pnlLeft.Controls.Add(lblSure);
            pnlLeft.Controls.Add(pnlSureGroup);
            pnlLeft.Controls.Add(btnAnalizEt);

            Panel pnlRight = new Panel();
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Padding = new Padding(15, 0, 15, 0);
            mainLayout.Controls.Add(pnlRight, 1, 0);

            txtAnalizSonuc = new TextBox();
            txtAnalizSonuc.Dock = DockStyle.Fill;
            txtAnalizSonuc.Multiline = true;
            txtAnalizSonuc.ScrollBars = ScrollBars.Vertical;
            txtAnalizSonuc.ReadOnly = true;
            txtAnalizSonuc.Font = new Font("Segoe UI", 14);
            txtAnalizSonuc.BackColor = Color.FromArgb(20, 35, 65);
            txtAnalizSonuc.ForeColor = Color.LightGray;
            txtAnalizSonuc.BorderStyle = BorderStyle.None;
            txtAnalizSonuc.Text = string.Format("\r\n\r\nSol taraftan egzersiz türünü ve süresini seçerek kardiyo fizyolojisi analizinizi başlatın.");

            Panel borderPanel = new Panel();
            borderPanel.Dock = DockStyle.Fill;
            borderPanel.BackColor = Color.White;
            borderPanel.Padding = new Padding(2);
            borderPanel.Controls.Add(txtAnalizSonuc);
            pnlRight.Controls.Add(borderPanel);

            this.Load += SporKardiyo_Load;
            this.Resize += (s, e) => {
                if (pnlTop != null)
                {
                    btnGeriDon.Location = new Point(pnlTop.Width - 150, 25);
                }
            };
        }

        private void SporKardiyo_Load(object sender, EventArgs e)
        {
            if (cmbKardiyoTuru.Items.Count > 0) cmbKardiyoTuru.SelectedIndex = 0;
            if (pnlTop != null)
            {
                btnGeriDon.Location = new Point(pnlTop.Width - 150, 25);
            }
        }

        private void BtnAnalizEt_Click(object sender, EventArgs e)
        {
            int toplamDk = ((int)numSureSaat.Value * 60) + (int)numSureDakika.Value;
            if (toplamDk == 0)
            {
                txtAnalizSonuc.Text = "Lütfen egzersiz süresini sıfırdan büyük giriniz.";
                return;
            }

            string tur = cmbKardiyoTuru.SelectedItem.ToString();
            List<string> rapor = new List<string>();

            rapor.Add(string.Format("🏃 KARDİYO MODÜLÜ: {0}", tur));
            rapor.Add(string.Format("⏱️ SEANS SÜRESİ: {0} Dakika", toplamDk));

            if (tur.Contains("Koşu Bandı"))
            {
                rapor.Add("🦵 BİYOMEKANİK ÖNERİ: Diz ve ayak bileği eklemlerine binen stresi azaltmak için eğimi %1 - %2 seviyesinde tutarak dış mekan koşusunu simüle edin.");
            }
            else if (tur.Contains("Bisiklet"))
            {
                rapor.Add("🦵 BİYOMEKANİK ÖNERİ: Sele boyunu kalça kemiği hizanıza ayarlayın. Pedalı en alta ittiğinizde diziniz tamamen kilitlenmemeli, hafif kırık kalmalıdır.");
            }
            else if (tur.Contains("Kürek"))
            {
                rapor.Add("🦵 BİYOMEKANİK ÖNERİ: Gücü kollarınızdan değil, %60 bacak, %20 core ve %20 kol sıralamasıyla çekerek uygulayın. Omurganızı dik tutun.");
            }
            else if (tur.Contains("İp Atlama"))
            {
                rapor.Add("🦵 BİYOMEKANİK ÖNERİ: Parmak ucunda esneyerek zıplayın, topuklarınızı yere vurmaktan kaçının. Kollarınızı değil, sadece bileklerinizi çevirin.");
            }
            else if (tur.Contains("Eliptik"))
            {
                rapor.Add("🦵 BİYOMEKANİK ÖNERİ: Topuklarınızı pedaldan kaldırmadan, tüm tabanınızla basarak gücü dağıtın. Kalça kaslarınızı (glute) sıkarak itin.");
            }

            if (toplamDk < 20)
            {
                rapor.Add("🩸 FİZYOLOJİK ETKİ: ATP-CP ve hızlı glikolitik enerji sistemleri devrede. Mitokondriyal adaptasyon ve derin yağ oksidasyonu (lipoliz) için süre yetersiz.");
                rapor.Add("💡 KLİNİK ÖNERİ: VO2 Max kapasitesini artırmak ve Zone 2 endorfin salınımını tetiklemek için seans süresini en az 30-40 dakikaya çekmeye salahat edin.");
            }
            else if (toplamDk <= 50)
            {
                rapor.Add("🩸 FİZYOLOJİK ETKİ: Mükemmel Aerobik Eşik seansı. Hücre içi lipolitik enzim aktivitesi (yağ yakımı) tepe noktada. Kalp sol ventrikül hacmi destekleniyor.");
                rapor.Add("💡 KLİNİK ÖNERİ: Bu aralık, bazal metabolizma hızını (BMR) gün boyu yüksek tutar. Egzersiz sonrası 30 dakika içinde 20g protein alımı toparlanmayı hızlandırır.");
            }
            else
            {
                rapor.Add("🩸 FİZYOLOJİK ETKİ: İleri seviye dayanıklılık seansı. Glikojen depoları tükendiğinden vücut glukoneojeniz fazına geçer. Kortizol (stres hormonu) üretimi artabilir.");
                rapor.Add("💡 KLİNİK ÖNERİ: Katabolik evreye (kas yıkımı) geçmemesi için seans sonrası elektrolit (sodyum/potasyum) dengesini koruyun ve glisemik indeksi yüksek bir karbonhidrat tüketin.");
            }

            rapor.Add(string.Format("🔥 TAHMİNİ YAKILAN KALORİ: {0} kcal", toplamDk * 9));
            rapor.Add("💧 HİDRASYON: Egzersiz boyunca kaybedilen ter miktarını dengelemek için antrenman sonrasında yaklaşık " + (toplamDk * 10) + " ml su tüketmeyi unutmayın.");

            txtAnalizSonuc.Text = string.Join(Environment.NewLine + Environment.NewLine, rapor);
            txtAnalizSonuc.SelectionStart = 0;
            txtAnalizSonuc.ScrollToCaret();
        }

        private void BtnGeriDon_Click(object sender, EventArgs e)
        {
            this.Hide();
            sporSureSecim form = new sporSureSecim("Kardiyo", userId);
            form.ShowDialog();
            this.Close();
        }
    }
}