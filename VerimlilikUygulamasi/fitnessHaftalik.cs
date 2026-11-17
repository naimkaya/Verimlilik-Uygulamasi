using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class fitnessHaftalik : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Panel pnlLeft;
        private Label lblTimerGun;
        private Label lblTimerSaat;
        private Label lblKasGrubu;
        private ComboBox cmbKasGrubu;
        private Label lblHareketler;
        private CheckedListBox chkHareketler;
        private Label lblDinamikBaslik;
        private TableLayoutPanel tlpHareketDetaylari;
        private Panel pnlSureGroup;
        private Label lblSure;
        private NumericUpDown numSureSaat, numSureDakika;
        private Label lblSaatAyraci, lblDakikaAyraci;
        private Button btnKaydet;

        private TextBox txtOneri;
        private FlowLayoutPanel pnlDaysList;
        private Label lblSummarySure;
        private Label lblSummaryKas;

        private List<Control> dinamikKontroller = new List<Control>();
        private System.Windows.Forms.Timer countdownTimer;

        private int userId; // Sabit değeri kaldırdık, dışarıdan gelecek
        private DateTime? sessionStart = null;
        private int aktifGun = 1;

        private Dictionary<string, List<string>> egzersizHafizasi = new Dictionary<string, List<string>>()
        {
            { "Göğüs", new List<string> { "Bench Press", "Incline Dumbbell Press", "Decline Press", "Chest Fly", "Dips", "Push-up / Şınav", "Cable Crossover" } },
            { "Sırt", new List<string> { "Lat Pulldown", "Deadlift", "Bent Over Row", "T-Bar Row", "Pull-up / Barfiks", "Seated Cable Row", "Face Pull" } },
            { "Kol", new List<string> { "Barbell Curl", "Hammer Curl", "Triceps Pushdown", "Skull Crusher", "Overhead Extension", "Concentration Curl", "Reverse Grip Curl" } },
            { "Bacak", new List<string> { "Squat", "Leg Press", "Lunge", "Leg Extension", "Leg Curl", "Romanian Deadlift", "Standing Calf Raise" } },
            { "Omuz", new List<string> { "Overhead Press", "Lateral Raise", "Front Raise", "Rear Delt Fly", "Shrug", "Arnold Press", "Upright Row" } },
            { "Karın", new List<string> { "Crunch", "Leg Raise", "Plank", "Russian Twist", "Cable Crunch", "Bicycle Crunch" } }
        };

        // GÜNCELLENEN CONSTRUCTOR: userId parametresi eklendi
        public fitnessHaftalik(int currentUserId)
        {
            this.userId = currentUserId;
            this.AutoScaleMode = AutoScaleMode.None; // Bulanıklık önlemi

            // Eğer InitializeComponent hata verirse tasarımcı kodun yok demektir, başına // koyabilirsin.
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(10, 25, 50);

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
            lblTitle.Text = "Haftalık Fitness Gelişim Serüveni";
            lblTitle.Font = new Font("Segoe UI Black", 28F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Dock = DockStyle.Fill;
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            pnlTop.Controls.Add(lblTitle);
            this.Controls.Add(pnlTop);

            TableLayoutPanel mainLayout = new TableLayoutPanel();
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.ColumnCount = 3;
            mainLayout.RowCount = 1;
            mainLayout.Padding = new Padding(20, 50, 20, 20);

            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28F));
            this.Controls.Add(mainLayout);

            mainLayout.BringToFront();

            pnlLeft = new Panel();
            pnlLeft.Dock = DockStyle.Fill;
            pnlLeft.AutoScroll = true;
            mainLayout.Controls.Add(pnlLeft, 0, 0);

            lblTimerGun = new Label();
            lblTimerGun.Text = "1. GÜN İÇ乘 KANAL SÜRE";
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

            lblKasGrubu = new Label();
            lblKasGrubu.Text = "Çalışılan Kas Grubu Seçimi:";
            lblKasGrubu.ForeColor = Color.White;
            lblKasGrubu.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblKasGrubu.AutoSize = true;

            cmbKasGrubu = new ComboBox();
            cmbKasGrubu.Width = 350;
            cmbKasGrubu.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            cmbKasGrubu.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbKasGrubu.BackColor = Color.WhiteSmoke;
            foreach (var kas in egzersizHafizasi.Keys) cmbKasGrubu.Items.Add(kas);
            cmbKasGrubu.SelectedIndexChanged += CmbKasGrubu_SelectedIndexChanged;

            pnlLeft.Controls.Add(lblKasGrubu);
            pnlLeft.Controls.Add(cmbKasGrubu);

            lblHareketler = new Label();
            lblHareketler.Text = "Yaptığınız Hareketleri Seçin:";
            lblHareketler.ForeColor = Color.White;
            lblHareketler.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblHareketler.AutoSize = true;

            chkHareketler = new CheckedListBox();
            chkHareketler.Width = 350;
            chkHareketler.Height = 130;
            chkHareketler.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            chkHareketler.BackColor = Color.FromArgb(25, 40, 70);
            chkHareketler.ForeColor = Color.White;
            chkHareketler.CheckOnClick = true;
            chkHareketler.ItemCheck += ChkHareketler_ItemCheck;

            pnlLeft.Controls.Add(lblHareketler);
            pnlLeft.Controls.Add(chkHareketler);

            lblDinamikBaslik = new Label();
            lblDinamikBaslik.Text = "Set / Tekrar Ayarları:";
            lblDinamikBaslik.ForeColor = Color.White;
            lblDinamikBaslik.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblDinamikBaslik.AutoSize = true;
            lblDinamikBaslik.Visible = false;

            tlpHareketDetaylari = new TableLayoutPanel();
            tlpHareketDetaylari.Width = 350;
            tlpHareketDetaylari.AutoSize = true;
            tlpHareketDetaylari.ColumnCount = 3;
            tlpHareketDetaylari.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpHareketDetaylari.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpHareketDetaylari.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            pnlLeft.Controls.Add(lblDinamikBaslik);
            pnlLeft.Controls.Add(tlpHareketDetaylari);

            pnlSureGroup = new Panel();
            pnlSureGroup.Width = 350;
            pnlSureGroup.Height = 90;

            lblSure = new Label();
            lblSure.Text = "Toplam Antrenman Süresi:";
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
            txtOneri.Text = "Sol taraftan hareketlerinizi, set ve antrenman sürenizi girerek 7 günlük spor takibini başlatın. Süreç boyunca antrenman hacminiz, dinlenme molalarınız ve biyomekanik gelişiminiz burada analiz edilecektir.";

            Panel borderPanel = new Panel();
            borderPanel.Dock = DockStyle.Fill;
            borderPanel.BackColor = Color.White;
            borderPanel.Padding = new Padding(5);
            borderPanel.Controls.Add(txtOneri);
            pnlMiddle.Controls.Add(borderPanel);

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
            pnlDaysList.Padding = new Padding(0, 10, 0, 10);
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
            lblSummaryKas.Text = "En Yoğun Bölge: -";
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

            this.Load += FitnessHaftalik_Load;
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

            lblKasGrubu.Location = new Point(10, currentY);
            currentY = lblKasGrubu.Bottom + 5;

            cmbKasGrubu.Location = new Point(10, currentY);
            currentY = cmbKasGrubu.Bottom + 25;

            lblHareketler.Location = new Point(10, currentY);
            currentY = lblHareketler.Bottom + 5;

            chkHareketler.Location = new Point(10, currentY);
            currentY = chkHareketler.Bottom + 25;

            if (lblDinamikBaslik.Visible)
            {
                lblDinamikBaslik.Location = new Point(10, currentY);
                currentY = lblDinamikBaslik.Bottom + 10;

                tlpHareketDetaylari.Location = new Point(10, currentY);
                currentY = tlpHareketDetaylari.Bottom + 25;
            }

            pnlSureGroup.Location = new Point(10, currentY);
            currentY = pnlSureGroup.Bottom + 25;

            btnKaydet.Location = new Point(10, currentY);

            pnlLeft.AutoScroll = true;
        }

        private void FitnessHaftalik_Load(object sender, EventArgs e)
        {
            btnGeriDon.Location = new Point(this.ClientSize.Width - btnGeriDon.Width - 40, 25);
            btnGeriDon.BringToFront();
            if (cmbKasGrubu.Items.Count > 0) cmbKasGrubu.SelectedIndex = 0;

            HizalaSolPanelElemanlari();
            CheckActiveSession();
        }

        private void CheckActiveSession()
        {
            sessionStart = DatabaseHelper.GetActiveSporSessionStart(userId);
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

        private void CmbKasGrubu_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkHareketler.Items.Clear();
            tlpHareketDetaylari.Controls.Clear();
            dinamikKontroller.Clear();
            lblDinamikBaslik.Visible = false;

            string secilenKas = cmbKasGrubu.SelectedItem.ToString();
            if (egzersizHafizasi.ContainsKey(secilenKas))
            {
                foreach (var hareket in egzersizHafizasi[secilenKas])
                    chkHareketler.Items.Add(hareket);
            }
            HizalaSolPanelElemanlari();
        }

        private void ChkHareketler_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                tlpHareketDetaylari.Controls.Clear();
                dinamikKontroller.Clear();
                lblDinamikBaslik.Visible = chkHareketler.CheckedItems.Count > 0;

                foreach (var item in chkHareketler.CheckedItems)
                {
                    string hareketAdi = item.ToString();

                    Label lblName = new Label();
                    lblName.Text = hareketAdi;
                    lblName.ForeColor = Color.White;
                    lblName.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    lblName.AutoSize = true;
                    lblName.Anchor = AnchorStyles.Left;
                    lblName.Margin = new Padding(0, 6, 0, 0);

                    NumericUpDown numSet = new NumericUpDown();
                    numSet.Minimum = 1;
                    numSet.Maximum = 20;
                    numSet.Value = 3;
                    numSet.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    numSet.Width = 60;
                    numSet.Tag = "Set_" + hareketAdi;

                    NumericUpDown numRep = new NumericUpDown();
                    numRep.Minimum = 1;
                    numRep.Maximum = 100;
                    numRep.Value = 10;
                    numRep.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                    numRep.Width = 60;
                    numRep.Tag = "Rep_" + hareketAdi;

                    tlpHareketDetaylari.Controls.Add(lblName);
                    tlpHareketDetaylari.Controls.Add(numSet);
                    tlpHareketDetaylari.Controls.Add(numRep);

                    dinamikKontroller.Add(numSet);
                    dinamikKontroller.Add(numRep);
                }

                tlpHareketDetaylari.Height = tlpHareketDetaylari.GetPreferredSize(new Size(tlpHareketDetaylari.Width, 0)).Height;
                HizalaSolPanelElemanlari();
            });
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            int toplamDk = ((int)numSureSaat.Value * 60) + (int)numSureDakika.Value;
            int hareketSayisi = chkHareketler.CheckedItems.Count;
            var oneriler = new List<string>();

            if (hareketSayisi == 0 || toplamDk == 0)
            {
                txtOneri.Text = "Lütfen en az 1 hareket seçin ve antrenman süresini sıfırdan büyük girin.";
                return;
            }

            int toplamSet = 0;
            double toplamTekrar = 0;
            string kasTipi = cmbKasGrubu.SelectedItem.ToString();

            foreach (var item in chkHareketler.CheckedItems)
            {
                string name = item.ToString();
                int hSet = 3; int hRep = 10;
                foreach (Control c in dinamikKontroller)
                {
                    if (c is NumericUpDown num)
                    {
                        if (num.Tag.ToString() == "Set_" + name) hSet = (int)num.Value;
                        if (num.Tag.ToString() == "Rep_" + name) hRep = (int)num.Value;
                    }
                }
                toplamSet += hSet;
                toplamTekrar += hRep;
            }

            int netCalisma = toplamSet * 1;
            if (netCalisma >= toplamDk)
            {
                txtOneri.Text = "HATA: Girdiğiniz antrenman süresi, setlerin çalışma süresinden daha kısa! Süreyi artırın.";
                return;
            }

            double molaSaniye = ((double)(toplamDk - netCalisma) / (toplamSet - 1)) * 60;
            double ortalamaTekrar = toplamTekrar / hareketSayisi;

            oneriler.Add(string.Format("KAS GRUBU: {0} ({1} Hareket, {2} Toplam Set)", kasTipi, hareketSayisi, toplamSet));
            oneriler.Add(string.Format("ORTALAMA MOLA: {0} Saniye", (int)molaSaniye));

            if (molaSaniye < 45) oneriler.Add("DURUM: Çok kısa mola. Kas büyümesi ve güç gelişimi yavaşlar.");
            else if (molaSaniye <= 90) oneriler.Add("DURUM: İdeal Hipertrofi (Büyüme) dengesi. Harika!");
            else oneriler.Add("DURUM: Saf güç odaklı veya uzun mola. Kaslar soğuyabilir.");

            if (ortalamaTekrar <= 5.5) oneriler.Add("LİF TİPİ: Saf Güç Odaklı (Miyofibriler)");
            else if (ortalamaTekrar <= 12.5) oneriler.Add("LİF TİPİ: Maksimum Hacim (Hipertrofi/Sarkoplazmik)");
            else oneriler.Add("LİF TİPİ: Dayanıklılık Odaklı (Kondisyon)");

            txtOneri.Text = string.Join(Environment.NewLine + Environment.NewLine, oneriler);

            if (sessionStart == null)
            {
                sessionStart = DateTime.Now;
                DatabaseHelper.StartNewSporSession(userId, sessionStart.Value);
                countdownTimer.Start();
                aktifGun = 1;
            }

            DateTime kayitTarihi = sessionStart.Value.Date.AddDays(aktifGun - 1);
            string detayFormatli = string.Format("{0} ({1} Hareket)", kasTipi, hareketSayisi);
            DatabaseHelper.SaveSporVerisi(userId, kayitTarihi, 0, (double)toplamDk, detayFormatli);

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
                lblSummaryKas.Text = "En Yoğun Bölge: -";
                pnlDaysList.AutoScroll = true;
                return;
            }

            var kayitlar = DatabaseHelper.GetWeeklySporRecords(userId, sessionStart.Value);
            int toplamSure = 0;
            int gunSayisi = 0;
            Dictionary<string, int> kasSayaclari = new Dictionary<string, int>();

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

                    string gosterilecekBolge = string.IsNullOrEmpty(k.AntrenmanTipi) ? "Veri Yok" : k.AntrenmanTipi;
                    lblInfo.Text = string.Format("{0}. Gün | Bölge: {1} | Süre: {2} dk", i, gosterilecekBolge, k.EgzersizSuresi);
                    lblInfo.ForeColor = Color.White;

                    toplamSure += (int)k.EgzersizSuresi;
                    gunSayisi++;

                    string safKas = k.AntrenmanTipi;
                    if (safKas != null && safKas.Contains(" (")) safKas = safKas.Substring(0, safKas.IndexOf(" ("));

                    if (!string.IsNullOrEmpty(safKas))
                    {
                        if (kasSayaclari.ContainsKey(safKas)) kasSayaclari[safKas]++;
                        else kasSayaclari[safKas] = 1;
                    }
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

            string enYogunKas = "-";
            int max = 0;
            foreach (var kvp in kasSayaclari)
            {
                if (kvp.Value > max) { max = kvp.Value; enYogunKas = kvp.Key; }
            }

            int ortalamaSure = gunSayisi > 0 ? toplamSure / gunSayisi : 0;
            lblSummarySure.Text = string.Format("Ortalama Süre: {0} dk", ortalamaSure);
            lblSummaryKas.Text = string.Format("En Yoğun Bölge: {0}", enYogunKas);

            pnlDaysList.AutoScroll = true;
        }

        private void BtnGeriDon_Click(object sender, EventArgs e)
        {
            this.Hide();
            sporSureSecim form = new sporSureSecim("Fitness", userId); // Hatalı boş çağırma düzeltildi
            form.ShowDialog();
            this.Close();
        }
    }
}