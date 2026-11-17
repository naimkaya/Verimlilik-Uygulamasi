using System;
using System.Drawing;
using System.Net.Mail;
using System.Windows.Forms;

namespace VerimlilikUygulamasi
{
    public partial class SifreSifirlamaForm : Form
    {
        private Label lblBilgi;
        private TextBox txtEmail;
        private Button btnGonder;
        private Label lblDurum;

        public SifreSifirlamaForm()
        {
            this.Text = "Şifre Sıfırlama";
            this.Size = new Size(550, 320);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(10, 25, 50);
            this.ForeColor = Color.White;

            Font fontLabel = new Font("Segoe UI", 12);

            lblBilgi = new Label()
            {
                Text = "Lütfen kayıtlı e-posta adresinizi giriniz:",
                Font = fontLabel,
                Location = new Point(20, 20),
                AutoSize = true,
                ForeColor = Color.White
            };
            this.Controls.Add(lblBilgi);

            txtEmail = new TextBox()
            {
                Location = new Point(20, 60),
                Width = 500,
                Font = new Font("Segoe UI", 12),
                ForeColor = Color.Black, // Sabit siyah renk
                Text = "" // Boş başlatılıyor
            };
            this.Controls.Add(txtEmail);

            btnGonder = new Button()
            {
                Text = "Gönder",
                Location = new Point(20, 120),
                Width = 500,
                Height = 50,
                BackColor = Color.FromArgb(42, 144, 210),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 14, FontStyle.Bold)
            };
            btnGonder.FlatAppearance.BorderSize = 0;
            btnGonder.MouseEnter += (s, e) => btnGonder.BackColor = Color.FromArgb(28, 107, 160);
            btnGonder.MouseLeave += (s, e) => btnGonder.BackColor = Color.FromArgb(42, 144, 210);
            btnGonder.Click += BtnGonder_Click;
            this.Controls.Add(btnGonder);

            lblDurum = new Label()
            {
                Location = new Point(20, 190),
                Width = 500,
                Height = 60,
                ForeColor = Color.LightGreen,
                Font = new Font("Segoe UI Semibold", 12),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = ""
            };
            this.Controls.Add(lblDurum);
        }

        private void BtnGonder_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Lütfen e-posta adresinizi giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblDurum.Text = "";
                return;
            }

            if (!IsValidEmail(email))
            {
                lblDurum.ForeColor = Color.Red;
                lblDurum.Text = "Lütfen geçerli bir e-posta adresi giriniz.";
                return;
            }

            lblDurum.ForeColor = Color.LightGreen;
            lblDurum.Text = $"Şifre sıfırlama bilgisi {email} adresine gönderildi.";
            lblDurum.Refresh();

            // Buraya mail gönderme kodunu ekleyebilirsin
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
