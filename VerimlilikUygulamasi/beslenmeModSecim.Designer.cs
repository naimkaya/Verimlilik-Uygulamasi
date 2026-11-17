namespace VerimlilikUygulamasi
{
    partial class beslenmeModSecim
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnTekSeferlik;
        private System.Windows.Forms.Label lblTekSeferlikNote;
        private System.Windows.Forms.Button btnHaftalik;
        private System.Windows.Forms.Label lblHaftalikNote;
        private System.Windows.Forms.Button btnGeriDon;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnTekSeferlik = new System.Windows.Forms.Button();
            this.lblTekSeferlikNote = new System.Windows.Forms.Label();
            this.btnHaftalik = new System.Windows.Forms.Button();
            this.lblHaftalikNote = new System.Windows.Forms.Label();
            this.btnGeriDon = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            // DÜZELTME: 'ç' harfinin kesilmemesi için Font 32'ye çekildi ve Height (Yükseklik) 120 yapıldı.
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Black", 32F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 50);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1200, 120);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Beslenme Kategorisi İçin Mod Seçiniz";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnTekSeferlik
            // 
            this.btnTekSeferlik.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.btnTekSeferlik.Location = new System.Drawing.Point(250, 220);
            this.btnTekSeferlik.Name = "btnTekSeferlik";
            this.btnTekSeferlik.Size = new System.Drawing.Size(250, 80);
            this.btnTekSeferlik.TabIndex = 1;
            this.btnTekSeferlik.Text = "Tek Seferlik";
            this.btnTekSeferlik.UseVisualStyleBackColor = true;
            this.btnTekSeferlik.Click += new System.EventHandler(this.btnTekSeferlik_Click);
            // 
            // lblTekSeferlikNote
            // 
            // DÜZELTME: Yazının kesilmemesi için Yükseklik 100 yapıldı.
            this.lblTekSeferlikNote.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblTekSeferlikNote.ForeColor = System.Drawing.Color.LightGray;
            this.lblTekSeferlikNote.Location = new System.Drawing.Point(200, 315);
            this.lblTekSeferlikNote.Name = "lblTekSeferlikNote";
            this.lblTekSeferlikNote.Size = new System.Drawing.Size(350, 100);
            this.lblTekSeferlikNote.TabIndex = 2;
            this.lblTekSeferlikNote.Text = "Bu mod verilerinizi kayıtlı tutmaz.Her veri girişiniz bir sonraki veri girişinizde silinir.";
            this.lblTekSeferlikNote.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnHaftalik
            // 
            this.btnHaftalik.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.btnHaftalik.Location = new System.Drawing.Point(600, 220);
            this.btnHaftalik.Name = "btnHaftalik";
            this.btnHaftalik.Size = new System.Drawing.Size(250, 80);
            this.btnHaftalik.TabIndex = 3;
            this.btnHaftalik.Text = "Haftalık";
            this.btnHaftalik.UseVisualStyleBackColor = true;
            this.btnHaftalik.Click += new System.EventHandler(this.btnHaftalik_Click);
            // 
            // lblHaftalikNote
            // 
            // DÜZELTME: Yazının kesilmemesi için Yükseklik 100 yapıldı.
            this.lblHaftalikNote.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.lblHaftalikNote.ForeColor = System.Drawing.Color.LightGray;
            this.lblHaftalikNote.Location = new System.Drawing.Point(550, 315);
            this.lblHaftalikNote.Name = "lblHaftalikNote";
            this.lblHaftalikNote.Size = new System.Drawing.Size(350, 100);
            this.lblHaftalikNote.TabIndex = 4;
            this.lblHaftalikNote.Text = "Bu mod günlük olarak verilerinizi saklar ve haftalık bilgilerinizi görmenize yardımcı olur. Not: Verilerinizi gün sonu girmeye özen gösteriniz.";
            this.lblHaftalikNote.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnGeriDon
            // 
            this.btnGeriDon.BackColor = System.Drawing.Color.FromArgb(160, 160, 160);
            this.btnGeriDon.FlatAppearance.BorderSize = 0;
            this.btnGeriDon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGeriDon.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnGeriDon.ForeColor = System.Drawing.Color.White;
            this.btnGeriDon.Location = new System.Drawing.Point(1050, 30);
            this.btnGeriDon.Name = "btnGeriDon";
            this.btnGeriDon.Size = new System.Drawing.Size(120, 45);
            this.btnGeriDon.TabIndex = 5;
            this.btnGeriDon.Text = "Geri Dön";
            this.btnGeriDon.UseVisualStyleBackColor = false;
            this.btnGeriDon.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGeriDon.Click += new System.EventHandler(this.btnGeriDon_Click);
            // 
            // beslenmeModSecim
            // 
            this.BackColor = System.Drawing.Color.FromArgb(10, 25, 50);
            this.ClientSize = new System.Drawing.Size(1200, 500);
            this.Controls.Add(this.btnGeriDon);
            this.Controls.Add(this.lblHaftalikNote);
            this.Controls.Add(this.btnHaftalik);
            this.Controls.Add(this.lblTekSeferlikNote);
            this.Controls.Add(this.btnTekSeferlik);
            this.Controls.Add(this.lblTitle);
            this.Name = "beslenmeModSecim";
            this.Text = "Beslenme Mod Seçimi";
            this.ResumeLayout(false);
        }
    }
}