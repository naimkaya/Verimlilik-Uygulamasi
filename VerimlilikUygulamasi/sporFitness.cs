using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class sporFitness : Form
    {
        private Panel pnlTop;
        private Label lblTitle;
        private Button btnGeriDon;

        private Label lblKasGrubu, lblHareketler, lblSure;
        private ComboBox cmbKasGrubu;
        private CheckedListBox chkHareketler;
        private NumericUpDown numSureSaat, numSureDakika;
        private Label lblSaatAyraci, lblDakikaAyraci;

        private Button btnHesapla;
        private TextBox txtOneri;
        private Panel pnlRight;

        private TableLayoutPanel tlpLeftContainer;
        private TableLayoutPanel tlpHareketDetaylari;
        private Label lblDinamikBaslik;
        private List<Control> dinamikKontroller = new List<Control>();

        // Kullanıcı ID'sini tutan değişken
        private int userId;

        private Dictionary<string, List<string>> egzersizHafizasi = new Dictionary<string, List<string>>()
        {
            { "Göğüs", new List<string> { "Bench Press (Orta Göğüs)", "Incline Dumbbell Press (Üst Göğüs)", "Decline Press (Alt Göğüs)", "Chest Fly (İç Göğüs)", "Dips (Alt Göğüs/Triceps)", "Push-up / Şınav (Genel Göğüs)", "Cable Crossover (İç/Alt Göğüs)", "Dumbbell Pullover (Göğüs Kafesi)" } },
            { "Sırt", new List<string> { "Lat Pulldown (Kanat)", "Deadlift (Alt Sırt/Tüm Vücut)", "Bent Over Row (Sırt Kalınlığı)", "T-Bar Row (Orta Sırt)", "Pull-up / Barfiks (Genel Sırt Genişliği)", "Seated Cable Row (Sırt Kalınlığı)", "Face Pull (Arka Omuz/Üst Sırt)", "Hyperextension (Alt Sırt)" } },
            { "Kol", new List<string> { "Barbell Curl (Biceps - Genel)", "Hammer Curl (Biceps - Brachialis/Ön Kol)", "Incline Dumbbell Curl (Biceps - Long Head)", "Triceps Pushdown (Triceps - Lateral Head)", "Skull Crusher (Triceps - Long Head)", "Overhead Extension (Triceps - İç)", "Concentration Curl (Biceps - Kısa Baş Peak)", "Reverse Grip Curl (Ön Kol / Brachioradialis)" } },
            { "Bacak", new List<string> { "Squat (Genel Bacak/Quadriceps)", "Leg Press (Kuadriseps Yüklenme)", "Lunge (Glute/Arka Bacak)", "Leg Extension (Ön Bacak İzole)", "Leg Curl (Arka Bacak İzole)", "Romanian Deadlift (Arka Bacak/Glute)", "Standing Calf Raise (Kalf)", "Hip Thrust (Kalça Odaklı)" } },
            { "Omuz", new List<string> { "Overhead Press (Ön/Orta Omuz)", "Lateral Raise (Yan Omuz İzole)", "Front Raise (Ön Omuz İzole)", "Rear Delt Fly (Arka Omuz)", "Shrug (Trapez)", "Arnold Press (Genel Omuz)", "Upright Row (Yan Omuz/Trapez)", "Face Pull (Arka Omuz Kontrol)" } },
            { "Karın", new List<string> { "Crunch (Üst Karın)", "Leg Raise (Alt Karın)", "Plank (Core Bölgesi Statik)", "Russian Twist (Eğik Karın / Oblik)", "Cable Crunch (Üst Karın Hipertrofi)", "Hanging Knee Raise (Alt Karın Etkili)", "Bicycle Crunch (Oblik/Genel)", "Ab Wheel Rollout (Core Kuvveti)" } }
        };

        // GÜNCELLEME: Kurucu metot artık dışarıdan gelen currentUserId değerini alıyor
        public sporFitness(int currentUserId)
        {
            this.userId = currentUserId;

            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.Text = "Fitness & Ağırlık Antrenmanı Gelişmiş Analizi";
            this.BackColor = Color.FromArgb(10, 25, 50);

            // =========================================================
            // 1. ÜST PANEL VE BAŞLIK
            // =========================================================
            pnlTop = new Panel() { Dock = DockStyle.Top, Height = 100, BackColor = Color.Transparent };
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
            btnGeriDon.Location = new Point(this.ClientSize.Width - 150, 25);
            btnGeriDon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnGeriDon.Click += BtnGeriDon_Click; // Özel metoda bağlandı
            pnlTop.Controls.Add(btnGeriDon);

            lblTitle = new Label()
            {
                Text = "Biyomekanik Antrenman ve Hacim Analiz Motoru",
                Font = new Font("Segoe UI Black", 28F, FontStyle.Bold),
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
                Padding = new Padding(30, 10, 30, 30),
                BackColor = Color.Transparent
            };
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            this.Controls.Add(mainLayout);
            mainLayout.BringToFront();

            // =========================================================
            // SOL PANEL
            // =========================================================
            tlpLeftContainer = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 8,
                AutoScroll = true,
                BackColor = Color.Transparent
            };
            tlpLeftContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tlpLeftContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tlpLeftContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tlpLeftContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 190F));
            tlpLeftContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tlpLeftContainer.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpLeftContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 95F));
            tlpLeftContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
            mainLayout.Controls.Add(tlpLeftContainer, 0, 0);

            lblKasGrubu = new Label() { Text = "Çalışılan Kas Grubu Seçimi:", ForeColor = Color.White, Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true };
            cmbKasGrubu = new ComboBox() { Width = 420, Font = new Font("Segoe UI", 16, FontStyle.Bold), DropDownStyle = ComboBoxStyle.DropDownList, BackColor = Color.WhiteSmoke, ForeColor = Color.Black };
            foreach (var kas in egzersizHafizasi.Keys) cmbKasGrubu.Items.Add(kas);
            cmbKasGrubu.SelectedIndexChanged += CmbKasGrubu_SelectedIndexChanged;

            tlpLeftContainer.Controls.Add(lblKasGrubu, 0, 0);
            tlpLeftContainer.Controls.Add(cmbKasGrubu, 0, 1);

            lblHareketler = new Label() { Text = "Yaptığınız Hareketleri Seçin:", ForeColor = Color.White, Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true };
            chkHareketler = new CheckedListBox() { Width = 420, Height = 180, Font = new Font("Segoe UI", 11, FontStyle.Bold), BackColor = Color.FromArgb(25, 40, 70), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, CheckOnClick = true };
            chkHareketler.ItemCheck += ChkHareketler_ItemCheck;

            tlpLeftContainer.Controls.Add(lblHareketler, 0, 2);
            tlpLeftContainer.Controls.Add(chkHareketler, 0, 3);

            lblDinamikBaslik = new Label() { Text = "Seçilen Hareketlerin Set / Tekrar Ayarları:", ForeColor = Color.White, Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true, Visible = false };
            tlpLeftContainer.Controls.Add(lblDinamikBaslik, 0, 4);

            tlpHareketDetaylari = new TableLayoutPanel() { Width = 420, AutoSize = true, ColumnCount = 3, BackColor = Color.Transparent };
            tlpHareketDetaylari.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56F));
            tlpHareketDetaylari.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            tlpHareketDetaylari.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22F));
            tlpLeftContainer.Controls.Add(tlpHareketDetaylari, 0, 5);

            Panel pnlSureGroup = new Panel() { Width = 420, Height = 90, BackColor = Color.Transparent };
            lblSure = new Label() { Text = "Toplam Salon / Antrenman Süresi:", ForeColor = Color.White, Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true, Location = new Point(0, 0) };
            numSureSaat = new NumericUpDown() { Minimum = 0, Maximum = 5, Font = new Font("Segoe UI", 18, FontStyle.Bold), Width = 80, BackColor = Color.WhiteSmoke, BorderStyle = BorderStyle.FixedSingle, Location = new Point(0, 35) };
            lblSaatAyraci = new Label() { Text = "Saat", ForeColor = Color.LightGray, Font = new Font("Segoe UI", 11, FontStyle.Bold), Location = new Point(85, 45), AutoSize = true };
            numSureDakika = new NumericUpDown() { Minimum = 0, Maximum = 59, Font = new Font("Segoe UI", 18, FontStyle.Bold), Width = 80, BackColor = Color.WhiteSmoke, BorderStyle = BorderStyle.FixedSingle, Location = new Point(140, 35) };
            lblDakikaAyraci = new Label() { Text = "Dakika", ForeColor = Color.LightGray, Font = new Font("Segoe UI", 11, FontStyle.Bold), Location = new Point(225, 45), AutoSize = true };

            pnlSureGroup.Controls.Add(lblSure);
            pnlSureGroup.Controls.Add(numSureSaat); pnlSureGroup.Controls.Add(lblSaatAyraci);
            pnlSureGroup.Controls.Add(numSureDakika); pnlSureGroup.Controls.Add(lblDakikaAyraci);
            tlpLeftContainer.Controls.Add(pnlSureGroup, 0, 6);

            btnHesapla = new Button()
            {
                Text = "Antrenmanı Analiz Et",
                Width = 420,
                Height = 65,
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                BackColor = Color.FromArgb(39, 174, 96),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnHesapla.FlatAppearance.BorderSize = 0;
            btnHesapla.MouseEnter += (s, e) => { btnHesapla.BackColor = ControlPaint.Light(Color.FromArgb(39, 174, 96), 0.2f); btnHesapla.FlatAppearance.BorderSize = 3; btnHesapla.FlatAppearance.BorderColor = Color.White; };
            btnHesapla.MouseLeave += (s, e) => { btnHesapla.BackColor = Color.FromArgb(39, 174, 96); btnHesapla.FlatAppearance.BorderSize = 0; };
            btnHesapla.Click += BtnHesapla_Click;
            tlpLeftContainer.Controls.Add(btnHesapla, 0, 7);

            // --- Sağ Panel ---
            pnlRight = new Panel() { Dock = DockStyle.Fill, Padding = new Padding(20, 10, 0, 10) };
            mainLayout.Controls.Add(pnlRight, 1, 0);

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
                Text = "\r\n\r\n\r\nHareketlerinizi seçtiğinizde anlık olarak her harekete özel Set ve Tekrar giriş alanları açılacaktır.\r\n\r\nTüm setlerinizi ve salon sürenizi girip yeşil butona bastığınızda; set arası mola hassasiyetiniz, kas lifi gelişim türünüz ve egzersiz yapılış görselleriniz burada listelenecektir."
            };

            Panel borderPanel = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(3) };
            borderPanel.Controls.Add(txtOneri);
            pnlRight.Controls.Add(borderPanel);

            if (cmbKasGrubu.Items.Count > 0) cmbKasGrubu.SelectedIndex = 0;
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
        }

        private void ChkHareketler_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            this.BeginInvoke((MethodInvoker)delegate
            {
                tlpHareketDetaylari.Controls.Clear();
                dinamikKontroller.Clear();

                if (chkHareketler.CheckedItems.Count > 0)
                    lblDinamikBaslik.Visible = true;
                else
                    lblDinamikBaslik.Visible = false;

                foreach (var item in chkHareketler.CheckedItems)
                {
                    string hareketAdi = item.ToString();

                    Label lblName = new Label()
                    {
                        Text = hareketAdi,
                        ForeColor = Color.White,
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        AutoSize = true,
                        Anchor = AnchorStyles.Left,
                        Margin = new Padding(0, 6, 0, 0)
                    };

                    NumericUpDown numSet = new NumericUpDown() { Minimum = 1, Maximum = 20, Value = 3, Font = new Font("Segoe UI", 11, FontStyle.Bold), Width = 75, Tag = "Set_" + hareketAdi, Margin = new Padding(0, 3, 0, 3) };
                    NumericUpDown numRep = new NumericUpDown() { Minimum = 1, Maximum = 100, Value = 10, Font = new Font("Segoe UI", 11, FontStyle.Bold), Width = 75, Tag = "Rep_" + hareketAdi, Margin = new Padding(0, 3, 0, 3) };

                    tlpHareketDetaylari.Controls.Add(lblName);
                    tlpHareketDetaylari.Controls.Add(numSet);
                    tlpHareketDetaylari.Controls.Add(numRep);

                    dinamikKontroller.Add(numSet);
                    dinamikKontroller.Add(numRep);
                }
            });
        }

        private void BtnHesapla_Click(object sender, EventArgs e)
        {
            int toplamAntrenmanDakika = ((int)numSureSaat.Value * 60) + (int)numSureDakika.Value;
            int hareketSayisi = chkHareketler.CheckedItems.Count;

            var oneriler = new List<string>();

            if (hareketSayisi == 0)
            {
                oneriler.Add("⚠️ Lütfen antrenmanda yaptığınız hareketleri sol listeden işaretleyin.");
                txtOneri.Text = string.Join(Environment.NewLine, oneriler);
                return;
            }

            if (toplamAntrenmanDakika == 0)
            {
                oneriler.Add("⚠️ Lütfen antrenman sürenizi Saat ve Dakika kutularından giriniz.");
                txtOneri.Text = string.Join(Environment.NewLine, oneriler);
                return;
            }

            int toplamSetSayisi = 0;
            double toplamTekrarSayisi = 0;

            int countBiceps = 0; int countTriceps = 0;
            int countUstGogus = 0; int countOrtaAltGogus = 0;

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

                toplamSetSayisi += hSet;
                toplamTekrarSayisi += hRep;

                if (name.Contains("Biceps") || name.Contains("Hammer") || name.Contains("Curl")) countBiceps++;
                if (name.Contains("Triceps") || name.Contains("Skull") || name.Contains("Extension")) countTriceps++;
                if (name.Contains("Incline")) countUstGogus++;
                if (name.Contains("Bench Press") || name.Contains("Decline") || name.Contains("Push-up")) countOrtaAltGogus++;
            }

            double ortalamaTekrar = toplamTekrarSayisi / hareketSayisi;
            int netCalismaSuresi = toplamSetSayisi * 1;

            if (netCalismaSuresi >= toplamAntrenmanDakika)
            {
                oneriler.Add("❌ MANTIKSAL ANTRENMAN HATASI!");
                oneriler.Add("• Girdiğiniz toplam süre, yaptığınız setlerin net çalışma süresine ucu ucuna bile yetmiyor. Dinlenmeye mola kalmamış.");
                txtOneri.Text = string.Join(Environment.NewLine + Environment.NewLine, oneriler);
                return;
            }

            double molaSaniye = ((double)(toplamAntrenmanDakika - netCalismaSuresi) / (toplamSetSayisi - 1)) * 60;

            int molaDakikaParcasi = (int)molaSaniye / 60;
            int molaSaniyeParcasi = (int)molaSaniye % 60;

            // DOLAR İŞARETLERİ KALDIRILDI -> string.Format UYARLAMASI
            string molaOkunabilirYazi = molaDakikaParcasi > 0
                ? string.Format("{0} Dakika {1} Saniye", molaDakikaParcasi, molaSaniyeParcasi)
                : string.Format("{0} Saniye", molaSaniyeParcasi);

            oneriler.Add("%40 ADVANCED ANTRENMAN VE HİPERTROFİ RAPORU:");
            oneriler.Add(string.Format("• Toplam Hacim: {0} Hareket | {1} Toplam Set", hareketSayisi, toplamSetSayisi));
            oneriler.Add(string.Format("• Set Başı Hesaplanan Ortalama Dinlenme Süresi: {0}", molaOkunabilirYazi));
            oneriler.Add(string.Format("• Ortalama Tekrar Aralığınız: {0:F1} Tekrar", ortalamaTekrar));

            // Mola Analizi
            oneriler.Add("⏱️ SET ARASI MOLA ANALİZİ:");
            if (molaSaniye < 45)
            {
                oneriler.Add("• Durum: ÇOK KISA MOLA / KATABOLİK RİSK! ⚠️");
                oneriler.Add("• Setler arası dinlenmeniz çok kısa kalmış. Bu durum kas içindeki ATP ve Fosfokreatin depolarının dolmasını engeller, ağır kiloya girmene izin vermez bitirir.");
            }
            else if (molaSaniye <= 90)
            {
                oneriler.Add("• Durum: İDEAL HİPERTROFİ DENGESİ (KAS BÜYÜME ARALIĞI)! 🍃");
                oneriler.Add("• Muazzam! 60-90 saniyelik dinlenmeler kas liflerinde mikrotravma yaratarak büyüme hormonu salgılanmasını zirveye çıkarır.");
            }
            else
            {
                oneriler.Add("• Durum: SAF GÜÇ / AŞIRI UZUN BEKLEME ARALIĞI! ⚠️");
                oneriler.Add("• Set aralarında çok fazla bekliyorsunuz. Salon içinde odağınız dağılıyor veya telefona bakıyor olabilirsiniz. Kaslar soğuyacağı için sakatlık riskiniz artar.");
            }

            // Tekrar Analizi
            oneriler.Add("🧬 KAS LİFİ HEDEFLENME ENDEKSİ:");
            if (ortalamaTekrar <= 5.5)
            {
                oneriler.Add("• Gelişim Tür: SAF GÜÇ VE SİNİR SİSTEMİ ADAPTASYONU (Tip 2X Lifleri)");
            }
            else if (ortalamaTekrar <= 12.5)
            {
                oneriler.Add("• Gelişim Tür: MAKSİMUM SARKOPLAZMİK HİPERTROFİ (Kas Hacmi/Pump)");
            }
            else
            {
                oneriler.Add("• Gelişim Tür: KAS DAYANIKLILIĞI VE LAKTİK ASİT TOLERANSI (Tip 1 Lifleri)");
            }

            // Biyomekanik Çakışma Analizi
            oneriler.Add("🧠 AKILLI ANATOMİ VE ÇEŞİTLİLİK TÜYOLARI:");
            string kasTipi = cmbKasGrubu.SelectedItem.ToString();

            if (kasTipi == "Kol")
            {
                if (countBiceps >= 3)
                {
                    oneriler.Add("⚠️ BICEPS ÇAKIŞMASI TESPİT EDİLDİ! Antrenmanda çok fazla biceps hareketine yüklenmişsin. Sadece Barbell Curl yapmak yerine, kolun kalın durmasını sağlayan Brachialis kası için 'Reverse Grip Curl' veya 'Hammer Curl', biceps zirvesini (peak) doldurmak içinse 'Concentration Curl' ekleyerek kol anatomisini her açıdan vurmalısın!");
                }
                if (countTriceps >= 3)
                {
                    oneriler.Add("⚠️ TRICEPS ÇAKIŞMASI TESPİT EDİLDİ! Arka kolun tüm başlarını uyarmak önemlidir. Sadece Triceps Pushdown yerine, arka kolun en büyük parçası olan uzun başı (Long Head) büyütmek için mutlaka baş arkasından dikey yapılan 'Skull Crusher' veya 'Overhead Extension' hareketlerini ekle!");
                }
            }
            else if (kasTipi == "Göğüs")
            {
                if (countUstGogus == 0)
                {
                    oneriler.Add("💡 GÖĞÜS TASARIM TÜYOSU: Antrenmanında hiç üst göğüs (Incline) hareketi yok. Göğsün tişörtün altından dolu ve estetik durması için düz Bench Press yerine antrenmana 'Incline Dumbbell Press' ile başlamanı öneririm.");
                }
                if (countOrtaAltGogus >= 3)
                {
                    oneriler.Add("⚠️ GÖĞÜS AÇISI ÇAKIŞMASI: Sürekli düz zeminde itiş yapmışsın. İç göğüs liflerini sıkıştırmak ve göğüs çizgisini belirginleştirmek için araya bir adet izole 'Chest Fly' veya 'Cable Crossover' enjekte etmelisin.");
                }
            }
            else
            {
                oneriler.Add("• Anatomi Notu: Seçtiğiniz egzersizler kas grubunun farklı alt motor ünitelerini dengeli bir şekilde uyarıyor. Antrenmanda akıl-kas bağlantısını yüksek tutmaya bak.");
            }

            // Görsel Rehber Linkleri -> string.Format UYARLAMASI
            oneriler.Add("🎬 HAREKETLERİN DOĞRU FORMDA YAPILIŞ REHBERLERİ:");
            foreach (var item in chkHareketler.CheckedItems)
            {
                string name = item.ToString();
                string searchUrl = string.Format("https://www.youtube.com/results?search_query=how+to+do+{0}", name.Replace(" ", "+"));
                oneriler.Add(string.Format("• {0} Form Kontrolü 🎥 [Görsel Rehber Linki]: {1}", name, searchUrl));
            }

            txtOneri.Text = "\r\n" + string.Join(Environment.NewLine + Environment.NewLine, oneriler);
        }

        // GÜNCELLEME: Geri Dön butonu artık doğru parametreyle (userId) üst formu tetikliyor
        private void DelGeriDon_Click()
        {
            this.Hide();
            sporSureSecim form = new sporSureSecim("Fitness", userId);
            form.ShowDialog();
            this.Close();
        }

        private void BtnGeriDon_Click(object sender, EventArgs e)
        {
            DelGeriDon_Click();
        }
    }
}