using System;

using System.Drawing;

using System.Drawing.Drawing2D;

using System.Windows.Forms;

using Microsoft.Data.Sqlite;

using System.Linq;

using System.Net.Mail;



namespace VerimlilikUygulamasi

{

    public partial class KayitFormu : Form

    {

        Label lblKullaniciAdi, lblEmail, lblSifre, lblSifreTekrar, lblDurum, lblGuvenlikSeviyesi;

        TextBox txtKullaniciAdi, txtEmail, txtSifre, txtSifreTekrar;

        Button btnKayitOl, btnGosterSifre, btnGosterSifreTekrar, btnGeriDon;

        Panel pnlGuvenlikCubugu;

        Label lblBuyukHarf, lblKucukHarf, lblRakam, lblOzelKarakter, lblMinUzunluk;



        // Form1'deki ana veritabanı bağlantı dizesi

        private string connectionString = "Data Source=verimlilik.db";



        public KayitFormu()

        {

            InitializeComponent();

            this.Text = "Kayıt Ol";

            this.AutoScaleMode = AutoScaleMode.None;

            this.WindowState = FormWindowState.Maximized;

            this.BackColor = Color.FromArgb(10, 25, 50);

            this.ForeColor = Color.White;



            Font labelFont = new Font("Segoe UI Semibold", 14);

            Font inputFont = new Font("Segoe UI", 12);

            Font kosulFont = new Font("Segoe UI", 10, FontStyle.Regular);



            // Kullanıcı Adı

            lblKullaniciAdi = new Label() { Text = "Kullanıcı Adı:", Font = labelFont, AutoSize = true };

            txtKullaniciAdi = new TextBox() { Font = inputFont, Width = 500 };



            // E-posta

            lblEmail = new Label() { Text = "E-posta Adresi:", Font = labelFont, AutoSize = true };

            txtEmail = new TextBox() { Font = inputFont, Width = 500 };



            // Şifre

            lblSifre = new Label() { Text = "Şifre:", Font = labelFont, AutoSize = true };

            txtSifre = new TextBox() { Font = inputFont, Width = 470, UseSystemPasswordChar = true };

            txtSifre.TextChanged += TxtSifre_TextChanged;



            // Şifre göster/gizle butonu

            btnGosterSifre = new Button()

            {

                Width = 30,

                Height = txtSifre.Height,

                BackColor = Color.FromArgb(10, 25, 50),

                FlatStyle = FlatStyle.Flat,

                ForeColor = Color.White,

                Text = "👁",

                Font = new Font("Segoe UI", 12),

                TabStop = false,

                Cursor = Cursors.Hand

            };

            btnGosterSifre.FlatAppearance.BorderSize = 0;

            btnGosterSifre.Click += (s, e) =>

            {

                txtSifre.UseSystemPasswordChar = !txtSifre.UseSystemPasswordChar;

            };



            // Şifre güvenlik cubugu (ince çizgi)

            pnlGuvenlikCubugu = new Panel()

            {

                Width = 0,

                Height = 5,

                BackColor = Color.Red

            };



            // Güvenlik seviyesi yazısı

            lblGuvenlikSeviyesi = new Label()

            {

                Text = "Zayıf",

                Font = new Font("Segoe UI", 10, FontStyle.Bold),

                ForeColor = Color.Red,

                AutoSize = true,

                TextAlign = ContentAlignment.MiddleLeft

            };



            // Şifre şartları etiketleri

            lblMinUzunluk = KosulLabel("\u2717 En az 8 karakter", kosulFont);

            lblBuyukHarf = KosulLabel("\u2717 En az 1 büyük harf", kosulFont);

            lblKucukHarf = KosulLabel("\u2717 En az 1 küçük harf", kosulFont);

            lblRakam = KosulLabel("\u2717 En az 1 rakam", kosulFont);

            lblOzelKarakter = KosulLabel("\u2717 En az 1 özel karakter (!@#$...)", kosulFont);



            // Şifre Tekrar

            lblSifreTekrar = new Label() { Text = "Şifre Tekrar:", Font = labelFont, AutoSize = true };

            txtSifreTekrar = new TextBox() { Font = inputFont, Width = 470, UseSystemPasswordChar = true };



            // Şifre tekrar göster/gizle butonu

            btnGosterSifreTekrar = new Button()

            {

                Width = 30,

                Height = txtSifreTekrar.Height,

                BackColor = Color.FromArgb(10, 25, 50),

                FlatStyle = FlatStyle.Flat,

                ForeColor = Color.White,

                Text = "👁",

                Font = new Font("Segoe UI", 12),

                TabStop = false,

                Cursor = Cursors.Hand

            };

            btnGosterSifreTekrar.FlatAppearance.BorderSize = 0;

            btnGosterSifreTekrar.Click += (s, e) =>

            {

                txtSifreTekrar.UseSystemPasswordChar = !txtSifreTekrar.UseSystemPasswordChar;

            };



            // Kayıt Ol Butonu

            btnKayitOl = new Button()

            {

                Text = "Kayıt Ol",

                Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),

                Width = 500,

                Height = 50,

                BackColor = Color.FromArgb(42, 144, 210),

                ForeColor = Color.White,

                FlatStyle = FlatStyle.Flat

            };

            btnKayitOl.FlatAppearance.BorderSize = 0;

            btnKayitOl.MouseEnter += (s, e) => btnKayitOl.BackColor = Color.FromArgb(28, 107, 160);

            btnKayitOl.MouseLeave += (s, e) => btnKayitOl.BackColor = Color.FromArgb(42, 144, 210);

            btnKayitOl.Click += BtnKayitOl_Click;



            // Durum Mesajı

            lblDurum = new Label()

            {

                Width = 500,

                Height = 40,

                Font = new Font("Segoe UI Semibold", 12),

                TextAlign = ContentAlignment.MiddleCenter,

                ForeColor = Color.LightGreen

            };



            // Geri Dön Butonu sağ alt köşede

            btnGeriDon = new Button()

            {

                Text = "Geri Dön",

                Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold),

                Size = new Size(100, 40),

                BackColor = Color.Gray,

                ForeColor = Color.White,

                FlatStyle = FlatStyle.Flat,

                Cursor = Cursors.Hand

            };

            btnGeriDon.FlatAppearance.BorderSize = 0;

            btnGeriDon.Click += BtnGeriDon_Click;



            this.Controls.Add(lblKullaniciAdi);

            this.Controls.Add(txtKullaniciAdi);

            this.Controls.Add(lblEmail);

            this.Controls.Add(txtEmail);

            this.Controls.Add(lblSifre);

            this.Controls.Add(txtSifre);

            this.Controls.Add(btnGosterSifre);

            this.Controls.Add(pnlGuvenlikCubugu);

            this.Controls.Add(lblGuvenlikSeviyesi);



            this.Controls.Add(lblMinUzunluk);

            this.Controls.Add(lblBuyukHarf);

            this.Controls.Add(lblKucukHarf);

            this.Controls.Add(lblRakam);

            this.Controls.Add(lblOzelKarakter);



            this.Controls.Add(lblSifreTekrar);

            this.Controls.Add(txtSifreTekrar);

            this.Controls.Add(btnGosterSifreTekrar);

            this.Controls.Add(btnKayitOl);

            this.Controls.Add(lblDurum);

            this.Controls.Add(btnGeriDon);



            this.Load += KayitFormu_Layout;

            this.Resize += KayitFormu_Layout;

        }



        private Label KosulLabel(string text, Font font)

        {

            return new Label

            {

                Text = text,

                Font = font,

                ForeColor = Color.Gray,

                AutoSize = true,

                TextAlign = ContentAlignment.MiddleLeft

            };

        }



        private void KayitFormu_Layout(object sender, EventArgs e)

        {

            int ortaX = this.ClientSize.Width / 2;

            int controlWidth = 500;

            int startY = 150;

            int verticalGap = 12;

            int currentY = startY;



            lblKullaniciAdi.Left = ortaX - (lblKullaniciAdi.Width / 2);

            lblKullaniciAdi.Top = currentY;

            currentY = lblKullaniciAdi.Bottom + 3;



            txtKullaniciAdi.Width = controlWidth;

            txtKullaniciAdi.Left = ortaX - (controlWidth / 2);

            txtKullaniciAdi.Top = currentY;

            currentY = txtKullaniciAdi.Bottom + verticalGap;



            lblEmail.Left = ortaX - (lblEmail.Width / 2);

            lblEmail.Top = currentY;

            currentY = lblEmail.Bottom + 3;



            txtEmail.Width = controlWidth;

            txtEmail.Left = ortaX - (controlWidth / 2);

            txtEmail.Top = currentY;

            currentY = txtEmail.Bottom + verticalGap;



            lblSifre.Left = ortaX - (lblSifre.Width / 2);

            lblSifre.Top = currentY;

            currentY = lblSifre.Bottom + 3;



            txtSifre.Width = controlWidth - btnGosterSifre.Width - 5;

            txtSifre.Left = ortaX - (controlWidth / 2);

            txtSifre.Top = currentY;

            btnGosterSifre.Top = txtSifre.Top;

            btnGosterSifre.Left = txtSifre.Right + 5;

            currentY = txtSifre.Bottom + 8;



            pnlGuvenlikCubugu.Width = txtSifre.Width;

            pnlGuvenlikCubugu.Left = txtSifre.Left;

            pnlGuvenlikCubugu.Top = currentY;

            currentY = pnlGuvenlikCubugu.Bottom + 3;



            lblGuvenlikSeviyesi.Left = txtSifre.Left;

            lblGuvenlikSeviyesi.Top = currentY;

            currentY = lblGuvenlikSeviyesi.Bottom + 8;



            Label[] kosullar = { lblMinUzunluk, lblBuyukHarf, lblKucukHarf, lblRakam, lblOzelKarakter };

            foreach (var kosul in kosullar)

            {

                kosul.Left = txtSifre.Left;

                kosul.Top = currentY;

                currentY = kosul.Bottom + 2;

            }

            currentY += 8;



            lblSifreTekrar.Left = ortaX - (lblSifreTekrar.Width / 2);

            lblSifreTekrar.Top = currentY;

            currentY = lblSifreTekrar.Bottom + 3;



            txtSifreTekrar.Width = controlWidth - btnGosterSifreTekrar.Width - 5;

            txtSifreTekrar.Left = ortaX - (controlWidth / 2);

            txtSifreTekrar.Top = currentY;

            btnGosterSifreTekrar.Top = txtSifreTekrar.Top;

            btnGosterSifreTekrar.Left = txtSifreTekrar.Right + 5;

            currentY = txtSifreTekrar.Bottom + verticalGap + 5;



            btnKayitOl.Left = ortaX - (btnKayitOl.Width / 2);

            btnKayitOl.Top = currentY;

            currentY = btnKayitOl.Bottom + verticalGap;



            lblDurum.Left = ortaX - (lblDurum.Width / 2);

            lblDurum.Top = currentY;



            btnGeriDon.Left = this.ClientSize.Width - btnGeriDon.Width - 20;

            btnGeriDon.Top = this.ClientSize.Height - btnGeriDon.Height - 20;

        }



        private void TxtSifre_TextChanged(object sender, EventArgs e)

        {

            string sifre = txtSifre.Text;

            int skor = SifreGuvenlikSkoru(sifre);

            pnlGuvenlikCubugu.Width = (int)(skor / 100.0 * 470);



            if (skor < 40)

            {

                pnlGuvenlikCubugu.BackColor = Color.Red;

                lblGuvenlikSeviyesi.Text = "Zayıf";

                lblGuvenlikSeviyesi.ForeColor = Color.Red;

            }

            else if (skor < 75)

            {

                pnlGuvenlikCubugu.BackColor = Color.Orange;

                lblGuvenlikSeviyesi.Text = "Orta";

                lblGuvenlikSeviyesi.ForeColor = Color.Orange;

            }

            else

            {

                pnlGuvenlikCubugu.BackColor = Color.LimeGreen;

                lblGuvenlikSeviyesi.Text = "Güçlü";

                lblGuvenlikSeviyesi.ForeColor = Color.LimeGreen;

            }



            UpdateKosulLabel(lblMinUzunluk, sifre.Length >= 8);

            UpdateKosulLabel(lblBuyukHarf, sifre.Any(char.IsUpper));

            UpdateKosulLabel(lblKucukHarf, sifre.Any(char.IsLower));

            UpdateKosulLabel(lblRakam, sifre.Any(char.IsDigit));

            UpdateKosulLabel(lblOzelKarakter, sifre.Any(ch => "!@#$%^&*()_+-=[]{}|;:',.<>?/".Contains(ch)));

        }



        private void UpdateKosulLabel(Label label, bool saglandi)

        {

            if (saglandi)

            {

                label.Text = label.Text.Replace('\u2717', '\u2713');

                label.ForeColor = Color.LimeGreen;

            }

            else

            {

                label.Text = label.Text.Replace('\u2713', '\u2717');

                label.ForeColor = Color.Gray;

            }

        }



        private int SifreGuvenlikSkoru(string sifre)

        {

            if (string.IsNullOrEmpty(sifre)) return 0;

            int skor = 0;



            if (sifre.Length >= 8) skor += 30;

            else skor += sifre.Length * 3;



            if (sifre.Any(char.IsLower)) skor += 10;

            if (sifre.Any(char.IsUpper)) skor += 20;

            if (sifre.Any(char.IsDigit)) skor += 20;

            if (sifre.Any(ch => "!@#$%^&*()_+-=[]{}|;:',.<>?/".Contains(ch))) skor += 20;



            if (sifre.Any(char.IsUpper) && sifre.Any(char.IsDigit) && sifre.Any(ch => "!@#$%^&*()_+-=[]{}|;:',.<>?/".Contains(ch)))

                skor += 10;



            return skor > 100 ? 100 : skor;

        }



        private void BtnKayitOl_Click(object sender, EventArgs e)

        {

            string kullaniciAdi = txtKullaniciAdi.Text.Trim();

            string email = txtEmail.Text.Trim();

            string sifre = txtSifre.Text;

            string sifreTekrar = txtSifreTekrar.Text;



            lblDurum.ForeColor = Color.Red;



            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre) || string.IsNullOrEmpty(sifreTekrar))

            {

                lblDurum.Text = "Lütfen tüm alanları doldurun.";

                return;

            }



            if (!IsValidEmail(email))

            {

                lblDurum.Text = "Geçerli bir e-posta adresi giriniz.";

                return;

            }



            if (sifre != sifreTekrar)

            {

                lblDurum.Text = "Şifreler eşleşmiyor.";

                return;

            }



            if (sifre.Length < 8 || !sifre.Any(char.IsUpper) || !sifre.Any(char.IsDigit) || !sifre.Any(ch => "!@#$%^&*()_+-=[]{}|;:',.<>?/".Contains(ch)))

            {

                lblDurum.Text = "Şifre tüm güvenlik şartlarını karşılamalı.";

                return;

            }



            try

            {

                // Form1 ana menüsündeki "Users" tablosu ile tam senkronize bağlantı açılır

                using (var conn = new SqliteConnection(connectionString))

                {

                    conn.Open();



                    // Form1 veri modelindeki kolonlar: Gmail, KullAdi, Sifre

                    string sql = "INSERT INTO Users (Gmail, KullAdi, Sifre) VALUES (@email, @kadi, @sifre)";

                    using (var cmd = new SqliteCommand(sql, conn))

                    {

                        cmd.Parameters.AddWithValue("@email", email);

                        cmd.Parameters.AddWithValue("@kadi", kullaniciAdi);

                        cmd.Parameters.AddWithValue("@sifre", sifre);

                        cmd.ExecuteNonQuery();

                    }

                }



                lblDurum.ForeColor = Color.LimeGreen;

                lblDurum.Text = kullaniciAdi + " başarıyla kayıt oldu! Giriş yapabilirsiniz.";



                // Formu temizle ve kapat

                txtKullaniciAdi.Clear();

                txtEmail.Clear();

                txtSifre.Clear();

                txtSifreTekrar.Clear();



                this.DialogResult = DialogResult.OK;

                this.Close();

            }

            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // SQLite UNIQUE kısıtlama hata kodu

            {

                lblDurum.Text = "Bu e-posta adresi sistemde zaten kayıtlı.";

            }

            catch (Exception ex)

            {

                lblDurum.Text = "Kayıt sırasında bir hata oluştu: " + ex.Message;

            }

        }



        private void BtnGeriDon_Click(object sender, EventArgs e)

        {

            this.DialogResult = DialogResult.Cancel;

            this.Close();

        }



        private bool IsValidEmail(string email)

        {

            try

            {

                var addr = new MailAddress(email);

                return addr.Address == email;

            }

            catch

            {

                return false;

            }

        }

    }

}