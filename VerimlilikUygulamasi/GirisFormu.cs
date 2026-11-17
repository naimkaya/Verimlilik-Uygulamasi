using System;
using System.Drawing;
using System.Windows.Forms;
namespace VerimlilikUygulamasi

{

    public partial class GirisFormu : Form

    {

        private Label lblKullanici, lblSifre;

        private TextBox txtKullanici, txtSifre;

        private CheckBox chkBeniHatirla;

        private LinkLabel linkSifremiUnuttum;

        private Button btnGiris;

        private Button btnGeriDon;

        private Label lblMesaj;



        public GirisFormu()

        {

            InitializeComponent();



            this.WindowState = FormWindowState.Maximized;

            this.BackColor = Color.FromArgb(10, 25, 50);

            this.DoubleBuffered = true;

            this.Text = "Giriş Yap";



            Font labelFont = new Font("Segoe UI Semibold", 14);

            Font inputFont = new Font("Segoe UI", 12);



            lblKullanici = new Label() { Text = "Kullanıcı Adı:", AutoSize = true, Font = labelFont, ForeColor = Color.White };

            txtKullanici = new TextBox() { Width = 300, Font = inputFont };



            lblSifre = new Label() { Text = "Şifre:", AutoSize = true, Font = labelFont, ForeColor = Color.White };

            txtSifre = new TextBox() { Width = 300, Font = inputFont, UseSystemPasswordChar = true };



            chkBeniHatirla = new CheckBox()

            {

                Text = "Beni Hatırla",

                AutoSize = true,

                Font = inputFont,

                ForeColor = Color.White,

                BackColor = Color.Transparent

            };



            linkSifremiUnuttum = new LinkLabel()

            {

                Text = "Şifremi Unuttum?",

                AutoSize = true,

                LinkColor = Color.LightBlue,

                Font = new Font("Segoe UI", 10, FontStyle.Regular),

                BackColor = Color.Transparent,

                Cursor = Cursors.Hand

            };

            linkSifremiUnuttum.Click += LinkSifremiUnuttum_Click;



            btnGiris = new Button()

            {

                Text = "Giriş Yap",

                Width = 320,

                Height = 50,

                Font = new Font("Segoe UI Semibold", 16, FontStyle.Bold),

                BackColor = Color.FromArgb(42, 144, 210),

                ForeColor = Color.White,

                FlatStyle = FlatStyle.Flat

            };

            btnGiris.FlatAppearance.BorderSize = 0;

            btnGiris.MouseEnter += (s, e) => btnGiris.BackColor = Color.FromArgb(28, 107, 160);

            btnGiris.MouseLeave += (s, e) => btnGiris.BackColor = Color.FromArgb(42, 144, 210);

            btnGiris.Click += BtnGiris_Click;



            btnGeriDon = new Button()

            {

                Text = "Geri Dön",

                Size = new Size(120, 50),

                Font = new Font("Segoe UI Semibold", 13, FontStyle.Bold),

                BackColor = Color.Gray,

                ForeColor = Color.White,

                FlatStyle = FlatStyle.Flat

            };

            btnGeriDon.FlatAppearance.BorderSize = 0;

            btnGeriDon.MouseEnter += (s, e) => btnGeriDon.BackColor = Color.DimGray;

            btnGeriDon.MouseLeave += (s, e) => btnGeriDon.BackColor = Color.Gray;

            btnGeriDon.Click += BtnGeriDon_Click;



            lblMesaj = new Label()

            {

                ForeColor = Color.Red,

                AutoSize = false,

                Font = labelFont,

                BackColor = Color.Transparent,

                TextAlign = ContentAlignment.MiddleCenter

            };



            Controls.Add(lblKullanici);

            Controls.Add(txtKullanici);

            Controls.Add(lblSifre);

            Controls.Add(txtSifre);

            Controls.Add(chkBeniHatirla);

            Controls.Add(linkSifremiUnuttum);

            Controls.Add(btnGiris);

            Controls.Add(btnGeriDon);

            Controls.Add(lblMesaj);



            this.Load += GirisFormu_Layout;

            this.Resize += GirisFormu_Layout;

        }



        private void GirisFormu_Layout(object? sender, EventArgs e)

        {

            int formWidth = this.ClientSize.Width;

            int formHeight = this.ClientSize.Height;



            int spacing = 15;



            int totalHeight = lblKullanici.Height + spacing +

            txtKullanici.Height + spacing +

            lblSifre.Height + spacing +

            txtSifre.Height + spacing +

            btnGiris.Height + spacing;



            int startY = (formHeight - totalHeight) / 2;



            int currentY = startY;



            void PlaceCentered(Control ctrl)

            {

                ctrl.Left = (formWidth - ctrl.Width) / 2;

                ctrl.Top = currentY;

                currentY += ctrl.Height + spacing;

            }



            PlaceCentered(lblKullanici);

            PlaceCentered(txtKullanici);

            PlaceCentered(lblSifre);

            PlaceCentered(txtSifre);



            // “Beni Hatırla” ve “Şifremi Unuttum?” yan yana konumlandırılıyor

            int controlsTop = currentY; // Aynı dikey konumda

            int totalWidth = chkBeniHatirla.Width + 20 + linkSifremiUnuttum.Width;

            int startX = (formWidth - totalWidth) / 2;



            chkBeniHatirla.Location = new Point(startX, controlsTop);

            linkSifremiUnuttum.Location = new Point(startX + chkBeniHatirla.Width + 20, controlsTop);



            currentY += chkBeniHatirla.Height + spacing;



            PlaceCentered(btnGiris);



            lblMesaj.Size = new Size(formWidth - 100, 40);

            lblMesaj.Left = (formWidth - lblMesaj.Width) / 2;

            lblMesaj.Top = btnGiris.Bottom + 10;



            int margin = 20;

            btnGeriDon.Location = new Point(formWidth - btnGeriDon.Width - margin, formHeight - btnGeriDon.Height - margin);

        }



        private void BtnGiris_Click(object? sender, EventArgs e)

        {

            string kullanici = txtKullanici.Text.Trim();

            string sifre = txtSifre.Text;



            if (string.IsNullOrEmpty(kullanici) || string.IsNullOrEmpty(sifre))

            {

                lblMesaj.Text = "Kullanıcı adı ve şifre boş bırakılamaz.";

                return;

            }



            try

            {

                // Menü formu veya giriş sonrası yapılacak işlemler...

                MenuForm menuForm = new MenuForm();

                this.Hide();

                menuForm.ShowDialog();

                this.Show();

                lblMesaj.Text = "";

            }

            catch (Exception ex)

            {

                MessageBox.Show("Menü formu bulunamadı, lütfen oluşturunuz.\n\n" + ex.Message,

                "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }



        private void BtnGeriDon_Click(object? sender, EventArgs e)

        {

            this.Close();

        }



        private void LinkSifremiUnuttum_Click(object? sender, EventArgs e)

        {

            SifreSifirlamaForm sifreForm = new SifreSifirlamaForm();

            sifreForm.ShowDialog();

        }

    }

}

