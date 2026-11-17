using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class kardiyoHaftalik : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Panel pnlLeft;
        private Label lblTimerGun;
        private Label lblTimerSaat;
        private Label lblKardiyoTuru;
        private ComboBox cmbKardiyoTuru;
        private Panel pnlSureGroup;
        private Label lblSure;
        private NumericUpDown numSureSaat, numSureDakika;
        private Label lblSaatAyraci, lblDakikaAyraci;
        private Button btnKaydet;

        private TextBox txtOneri;
        private Panel borderPanel;
        private FlowLayoutPanel pnlDaysList;
        private Label lblSummarySure;
        private Label lblSummaryKas;

        private System.Windows.Forms.Timer countdownTimer;

        private int userId;
        private DateTime? sessionStart = null;
        private int aktifGun = 1;

        public kardiyoHaftalik(int currentUserId)
        {
            this.userId = currentUserId;
            this.AutoScaleMode = AutoScaleMode.None;

            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(10, 25, 50);

            // --- 1. ÜST PANEL (BAŞLIK) ---
            pnlTop = new Panel();
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Height = 100;
            pnlTop.BackColor = Color.Transparent;

            btnGeriDon = new Button();
            btnGeriDon.Text = "Geri Dön";
            btnGeriDon.Size = new Size(120, 45);
            btnGeriDon.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnGeriDon.BackColor = Color.FromArgb(160, 160, 160);
            btnGeriDon.ForeColor = Color.White;
            btnGeriDon.FlatStyle = FlatStyle.Flat;
            btnGeriDon.Cursor = Cursors.Hand;
            btnGeriDon.Click += BtnGeriDon_Click;
            pnlTop.Controls.Add(btnGeriDon);

            lblTitle = new Label();
            lblTitle.Text = "Haftalık Kardiyo Gelişim Serüveni";
            lblTitle.Font = new Font("Segoe UI Black", 28F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            pnlTop.Controls.Add(lblTitle);

            // --- 2. ANA DÜZEN (İÇERİK) ---
            TableLayoutPanel mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.ColumnCount = 3;
            mainLayout.RowCount = 1;
            mainLayout.Padding = new Padding(20, 30, 20, 20);
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));

            this.Controls.Add(mainLayout);
            this.Controls.Add(pnlTop);
            pnlTop.SendToBack();
            mainLayout.BringToFront();

            // --- SOL PANEL ---
            pnlLeft = new Panel();
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.AutoScroll = true;
            mainLayout.Controls.Add(pnlLeft, 0, 0);

            lblTimerGun = new Label();
            lblTimerGun.Text = "1. GÜN İÇİN KALAN SÜRE";
            lblTimerGun.ForeColor = Color.Gold;
            lblTimerGun.Font = new Font("Segoe UI Black", 14F, FontStyle.Bold);
            lblTimerGun.AutoSize = true;

            lblTimerSaat = new Label();
            lblTimerSaat.Text = "00:00:00";
            lblTimerSaat.ForeColor = Color.Cyan;
            lblTimerSaat.Font = new Font("Segoe UI Black", 28F, FontStyle.Bold);
            lblTimerSaat.AutoSize = true;

            pnlLeft.Controls.Add(lblTimerGun);
            pnlLeft.Controls.Add(lblTimerSaat);

            lblKardiyoTuru = new Label();
            lblKardiyoTuru.Text = "Kardiyo Türü Seçimi:";
            lblKardiyoTuru.ForeColor = Color.White;
            lblKardiyoTuru.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblKardiyoTuru.AutoSize = true;

            cmbKardiyoTuru = new ComboBox();
            cmbKardiyoTuru.Width = 350;
            cmbKardiyoTuru.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            cmbKardiyoTuru.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbKardiyoTuru.BackColor = Color.WhiteSmoke;
            cmbKardiyoTuru.Items.AddRange(new object[] { "Koşu Bandı", "Kondisyon Bisikleti", "Kürek", "İp Atlama", "Eliptik Bisiklet" });

            pnlLeft.Controls.Add(lblKardiyoTuru);
            pnlLeft.Controls.Add(cmbKardiyoTuru);

            pnlSureGroup = new Panel();
            pnlSureGroup.Width = 350;
            pnlSureGroup.Height = 90;

            lblSure = new Label();
            lblSure.Text = "Toplam Egzersiz Süresi:";
            lblSure.ForeColor = Color.White;
            lblSure.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblSure.AutoSize = true;
            lblSure.Location = new Point(0, 0);

            numSureSaat = new NumericUpDown();
            numSureSaat.Minimum = 0;
            numSureSaat.Maximum = 5;
            numSureSaat.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            numSureSaat.Width = 70;
            numSureSaat.Location = new Point(0, 30);

            lblSaatAyraci = new Label();
            lblSaatAyraci.Text = "Saat";
            lblSaatAyraci.ForeColor = Color.LightGray;
            lblSaatAyraci.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblSaatAyraci.Location = new Point(75, 38);
            lblSaatAyraci.AutoSize = true;

            numSureDakika = new NumericUpDown();
            numSureDakika.Minimum = 0;
            numSureDakika.Maximum = 59;
            numSureDakika.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            numSureDakika.Width = 70;
            numSureDakika.Location = new Point(130, 30);

            lblDakikaAyraci = new Label();
            lblDakikaAyraci.Text = "Dakika";
            lblDakikaAyraci.ForeColor = Color.LightGray;
            lblDakikaAyraci.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblDakikaAyraci.Location = new Point(205, 38);
            lblDakikaAyraci.AutoSize = true;

            pnlSureGroup.Controls.Add(lblSure);
            pnlSureGroup.Controls.Add(numSureSaat);
            pnlSureGroup.Controls.Add(lblSaatAyraci);
            pnlSureGroup.Controls.Add(numSureDakika);
            pnlSureGroup.Controls.Add(lblDakikaAyraci);
            pnlLeft.Controls.Add(pnlSureGroup);

            btnKaydet = new Button();
            btnKaydet.Text = "Kaydet ve Analiz Al";
            btnKaydet.Width = 350;
            btnKaydet.Height = 60;
            btnKaydet.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            btnKaydet.BackColor = Color.FromArgb(39, 174, 96);
            btnKaydet.ForeColor = Color.White;
            btnKaydet.FlatStyle = FlatStyle.Flat;
            btnKaydet.Cursor = Cursors.Hand;
            btnKaydet.Click += BtnKaydet_Click;
            pnlLeft.Controls.Add(btnKaydet);

            // --- ORTA PANEL ---
            Panel pnlMiddle = new Panel();
            pnlMiddle.Dock = DockStyle.Fill;
            pnlMiddle.Padding = new Padding(15, 0, 15, 0);
            mainLayout.Controls.Add(pnlMiddle, 1, 0);

            txtOneri = new TextBox();
            txtOneri.Dock = DockStyle.Fill;
            txtOneri.Multiline = true;
            txtOneri.ScrollBars = ScrollBars.Vertical;
            txtOneri.ReadOnly = true;
            txtOneri.Font = new Font("Segoe UI", 13);
            txtOneri.BackColor = Color.FromArgb(20, 35, 65);
            txtOneri.ForeColor = Color.LightGray;
            txtOneri.BorderStyle = BorderStyle.None;
            txtOneri.Text = "Sol taraftan günlük kardiyo türünüzü ve sürenizi girerek 7 günlük dayanıklılık takibini başlatın. Süreç boyunca nabız alanlarınız, lipoliz (yağ yakım) oranınız ve VO2 Max kapasite gelişiminiz burada analiz edilecektir.";

            borderPanel = new Panel();
            borderPanel.Dock = DockStyle.Fill;
            borderPanel.BackColor = Color.White;
            borderPanel.Padding = new Padding(5);
            borderPanel.Controls.Add(txtOneri);
            pnlMiddle.Controls.Add(borderPanel);

            // --- SAĞ PANEL ---
            TableLayoutPanel rightSplit = new TableLayoutPanel();
            rightSplit.Dock = DockStyle.Fill;
            rightSplit.RowCount = 2;
            rightSplit.ColumnCount = 1;
            rightSplit.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            rightSplit.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            mainLayout.Controls.Add(rightSplit, 2, 0);

            pnlDaysList = new FlowLayoutPanel();
            pnlDaysList.Dock = DockStyle.Fill;
            pnlDaysList.AutoScroll = true;
            pnlDaysList.FlowDirection = FlowDirection.TopDown;
            pnlDaysList.WrapContents = false;
            pnlDaysList.Padding = new Padding(0, 0, 0, 10);
            rightSplit.Controls.Add(pnlDaysList, 0, 0);

            Panel pnlOzetBox = new Panel();
            pnlOzetBox.Dock = DockStyle.Fill;
            pnlOzetBox.BackColor = Color.FromArgb(40, 35, 60);
            pnlOzetBox.Margin = new Padding(0, 10, 0, 0);

            Label lblOzetTitle = new Label();
            lblOzetTitle.Text = "HAFTALIK ÖZETİNİZ";
            lblOzetTitle.ForeColor = Color.White;
            lblOzetTitle.Font = new Font("Segoe UI Black", 12, FontStyle.Bold);
            lblOzetTitle.AutoSize = true;
            lblOzetTitle.Location = new Point(10, 10);

            lblSummarySure = new Label();
            lblSummarySure.Text = "Ortalama Süre: 0 dk";
            lblSummarySure.ForeColor = Color.LightGray;
            lblSummarySure.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblSummarySure.AutoSize = true;
            lblSummarySure.Location = new Point(10, 45);

            lblSummaryKas = new Label();
            lblSummaryKas.Text = "En Yoğun Kardiyo: -";
            lblSummaryKas.ForeColor = Color.LightGray;
            lblSummaryKas.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblSummaryKas.AutoSize = true;
            lblSummaryKas.Location = new Point(10, 75);

            pnlOzetBox.Controls.Add(lblOzetTitle);
            pnlOzetBox.Controls.Add(lblSummarySure);
            pnlOzetBox.Controls.Add(lblSummaryKas);
            rightSplit.Controls.Add(pnlOzetBox, 0, 1);

            countdownTimer = new System.Windows.Forms.Timer();
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += CountdownTimer_Tick;

            this.Load += KardiyoHaftalik_Load;
        }

        private void HizalaSolPanelElemanlari()
        {
            pnlLeft.AutoScroll = false;
            pnlLeft.VerticalScroll.Value = 0;

            int currentY = 10;

            lblTimerGun.Location = new Point(10, currentY);
            currentY = lblTimerGun.Bottom + 5;

            lblTimerSaat.Location = new Point(10, currentY);
            currentY = lblTimerSaat.Bottom + 40;

            lblKardiyoTuru.Location = new Point(10, currentY);
            currentY = lblKardiyoTuru.Bottom + 5;

            cmbKardiyoTuru.Location = new Point(10, currentY);
            currentY = cmbKardiyoTuru.Bottom + 40;

            pnlSureGroup.Location = new Point(10, currentY);
            currentY = pnlSureGroup.Bottom + 40;

            btnKaydet.Location = new Point(10, currentY);

            pnlLeft.AutoScroll = true;
        }

        private void KardiyoHaftalik_Load(object sender, EventArgs e)
        {
            DatabaseHelper.InitializeDatabase();

            btnGeriDon.Location = new Point(pnlTop.Width - btnGeriDon.Width - 30, 25);
            btnGeriDon.BringToFront();

            if (cmbKardiyoTuru.Items.Count > 0) cmbKardiyoTuru.SelectedIndex = 0;

            HizalaSolPanelElemanlari();
            CheckActiveSession();
        }

        private void CheckActiveSession()
        {
            sessionStart = DatabaseHelper.GetActiveKardiyoSessionStart(userId);
            if (sessionStart != null)
            {
                countdownTimer.Start();
                CountdownTimer_Tick(null, null);
            }
            else
            {
                lblTimerGun.Text = "SEANS BAŞLAMADI";
                lblTimerSaat.Text = "00:00:00";
            }
            RefreshList();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            if (sessionStart == null) return;

            TimeSpan diff = DateTime.Now - sessionStart.Value;
            aktifGun = (int)diff.TotalDays + 1;

            if (aktifGun > 7)
            {
                countdownTimer.Stop();
                lblTimerGun.Text = "HAFTALIK SÜREÇ BİTTİ";
                lblTimerSaat.Text = "00:00:00";
                btnKaydet.Enabled = false;
                return;
            }

            lblTimerGun.Text = string.Format("{0}. GÜN İÇİN KALAN SÜRE", aktifGun);
            TimeSpan passedInDay = new TimeSpan(diff.Hours, diff.Minutes, diff.Seconds);
            TimeSpan remaining = TimeSpan.FromHours(24) - passedInDay;
            lblTimerSaat.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", remaining.Hours, remaining.Minutes, remaining.Seconds);
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            int toplamDk = ((int)numSureSaat.Value * 60) + (int)numSureDakika.Value;

            if (toplamDk == 0)
            {
                txtOneri.Text = "Lütfen antrenman süresini sıfırdan büyük girin.";
                return;
            }

            string tur = cmbKardiyoTuru.SelectedItem.ToString();
            List<string> oneriler = new List<string>();

            oneriler.Add(string.Format("KARDİYO TÜRÜ: {0} | SÜRE: {1} Dakika", tur, toplamDk));

            if (toplamDk < 20)
            {
                oneriler.Add("DURUM: Hafif tempo ısınma/soğuma aralığı.");
                oneriler.Add("FİZYOLOJİ: Yağ yakımı için süre yetersiz. Glikojen depoları kullanılıyor.");
            }
            else if (toplamDk <= 50)
            {
                oneriler.Add("DURUM: Optimum Aerobik Seans.");
                oneriler.Add("FİZYOLOJİ: Yağ oksidasyonu (lipoliz) tepe noktasında. Kondisyon için harika.");
            }
            else
            {
                oneriler.Add("DURUM: İleri Seviye Dayanıklılık.");
                oneriler.Add("FİZYOLOJİ: Glikojen tükendi. Katabolik yıkıma geçmemesi için karbonhidrat/protein takviyesi şart.");
            }

            txtOneri.Text = string.Join(Environment.NewLine + Environment.NewLine, oneriler);
            txtOneri.SelectionStart = 0;
            txtOneri.ScrollToCaret();

            if (sessionStart == null)
            {
                sessionStart = DateTime.Now;
                DatabaseHelper.StartNewKardiyoSession(userId, sessionStart.Value);
                countdownTimer.Start();
                aktifGun = 1;
            }

            DateTime kayitTarihi = sessionStart.Value.Date.AddDays(aktifGun - 1);

            DatabaseHelper.SaveKardiyoVerisi(userId, kayitTarihi, 0, (double)toplamDk, tur);

            RefreshList();
        }

        private void RefreshList()
        {
            pnlDaysList.AutoScroll = false;
            pnlDaysList.VerticalScroll.Value = 0;
            pnlDaysList.Controls.Clear();

            if (sessionStart == null)
            {
                Label lblBekleme = new Label();
                lblBekleme.Text = "Haftalık süreç başlamadı.\nSol taraftan verilerinizi kaydederek 7 günlük süreci başlatabilirsiniz.";
                lblBekleme.ForeColor = Color.White;
                lblBekleme.Font = new Font("Segoe UI", 12, FontStyle.Italic);
                lblBekleme.AutoSize = true;
                pnlDaysList.Controls.Add(lblBekleme);
                lblSummarySure.Text = "Ortalama Süre: 0 dk";
                lblSummaryKas.Text = "En Yoğun Kardiyo: -";
                pnlDaysList.AutoScroll = true;
                return;
            }

            var kayitlar = DatabaseHelper.GetWeeklyKardiyoRecords(userId, sessionStart.Value);
            int toplamSure = 0;
            int gunSayisi = 0;
            Dictionary<string, int> turSayaclari = new Dictionary<string, int>();

            for (int i = 1; i <= 7; i++)
            {
                DateTime loopDate = sessionStart.Value.Date.AddDays(i - 1);
                string dateKey = loopDate.ToString("yyyy-MM-dd");

                Panel card = new Panel();
                card.Size = new Size(pnlDaysList.ClientSize.Width - 15, 65);
                card.Margin = new Padding(5);

                Label lblInfo = new Label();
                lblInfo.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                lblInfo.Dock = DockStyle.Fill;
                lblInfo.TextAlign = ContentAlignment.MiddleCenter;

                if (kayitlar.ContainsKey(dateKey))
                {
                    var k = kayitlar[dateKey];
                    card.BackColor = (i == aktifGun) ? Color.FromArgb(155, 89, 182) : Color.FromArgb(44, 62, 80);

                    lblInfo.Text = string.Format("{0}. Gün | Tür: {1} | Süre: {2} dk", i, k.AntrenmanTipi, k.EgzersizSuresi);
                    lblInfo.ForeColor = Color.White;

                    toplamSure += (int)k.EgzersizSuresi;
                    gunSayisi++;

                    if (turSayaclari.ContainsKey(k.AntrenmanTipi)) turSayaclari[k.AntrenmanTipi]++;
                    else turSayaclari[k.AntrenmanTipi] = 1;
                }
                else
                {
                    card.BackColor = Color.FromArgb(30, 40, 55);
                    lblInfo.Text = string.Format("{0}. Gün: Henüz Veri Girişi Yapılmamıştır.", i);
                    lblInfo.ForeColor = Color.Gray;
                }

                card.Controls.Add(lblInfo);
                pnlDaysList.Controls.Add(card);
            }

            string enYogunTur = "-";
            int max = 0;
            foreach (var kvp in turSayaclari)
            {
                if (kvp.Value > max) { max = kvp.Value; enYogunTur = kvp.Key; }
            }

            int ortalamaSure = gunSayisi > 0 ? toplamSure / gunSayisi : 0;
            lblSummarySure.Text = string.Format("Ortalama Süre: {0} dk", ortalamaSure);
            lblSummaryKas.Text = string.Format("En Yoğun Kardiyo: {0}", enYogunTur);

            pnlDaysList.AutoScroll = true;
        }

        private void BtnGeriDon_Click(object sender, EventArgs e)
        {
            this.Hide();
            // Geri dönme işleminde eksik olan userId parametresi eklendi
            sporSureSecim form = new sporSureSecim("Kardiyo", userId);
            form.ShowDialog();
            this.Close();
        }
    }
}