using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SR_Editor.Core.Exceptions
{
    partial class FormExceptionView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private const int Bosluk = 10;
        private Panel panelTop;
        internal Label lblMesaj;
        private Label lblDescription;
        private Label lblUyari;
        private Label lblDetay;
        private TextBox txtDetay;
        private SimpleButton simpleButtonDetay;
        private SimpleButton simpleButtonTamam;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelTop = new Panel();
            this.lblMesaj = new Label();
            this.lblDescription = new Label();
            this.lblUyari = new Label();
            this.lblDetay = new Label();
            this.txtDetay = new TextBox();
            this.simpleButtonDetay = new SimpleButton();
            this.simpleButtonTamam = new SimpleButton();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            this.panelTop.BackColor = SystemColors.Window;
            this.panelTop.Controls.Add((Control)this.lblMesaj);
            this.panelTop.Dock = DockStyle.Top;
            this.panelTop.Location = new Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new Size(542, 135);
            this.panelTop.TabIndex = 1;
            this.lblMesaj.Font = new Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblMesaj.Location = new Point(12, 13);
            this.lblMesaj.Name = "lblMesaj";
            this.lblMesaj.Size = new Size(518, 106);
            this.lblMesaj.TabIndex = 0;
            this.lblMesaj.Text = "Hata";
            this.lblDescription.Font = new Font("Microsoft Sans Serif", 8f);
            this.lblDescription.Location = new Point(9, 192);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new Size(387, 29);
            this.lblDescription.TabIndex = 5;
            this.lblDescription.Text = "Sistemde oluşan hata ile ilgili bir hata raporu oluşturduk ve sisteme gönderdik. Lütfen işleminizi daha sonra tekrar deneyiniz...";
            this.lblUyari.AutoSize = true;
            this.lblUyari.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
            this.lblUyari.Location = new Point(9, 167);
            this.lblUyari.Name = "lblUyari";
            this.lblUyari.Size = new Size(36, 13);
            this.lblUyari.TabIndex = 6;
            this.lblUyari.Text = "Uyarı";
            this.lblDetay.AutoSize = true;
            this.lblDetay.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)162);
            this.lblDetay.Location = new Point(14, 264);
            this.lblDetay.Name = "lblDetay";
            this.lblDetay.Size = new Size(40, 13);
            this.lblDetay.TabIndex = 9;
            this.lblDetay.Text = "Detay";
            this.txtDetay.BackColor = SystemColors.Control;
            this.txtDetay.Location = new Point(12, 302);
            this.txtDetay.Multiline = true;
            this.txtDetay.Name = "txtDetay";
            this.txtDetay.ScrollBars = ScrollBars.Vertical;
            this.txtDetay.Size = new Size(518, 336);
            this.txtDetay.TabIndex = 10;
            this.txtDetay.Text = "Detay bilgi gelecek....";
            this.simpleButtonDetay.Location = new Point(60, 259);
            this.simpleButtonDetay.Name = "simpleButtonDetay";
            this.simpleButtonDetay.Size = new Size(52, 23);
            this.simpleButtonDetay.TabIndex = 11;
            this.simpleButtonDetay.Text = " > > ";
            this.simpleButtonDetay.Click += new EventHandler(this.simpleButtonDetay_Click);
            this.simpleButtonTamam.Location = new Point(348, 259);
            this.simpleButtonTamam.Name = "simpleButtonTamam";
            this.simpleButtonTamam.Size = new Size(52, 23);
            this.simpleButtonTamam.TabIndex = 12;
            this.simpleButtonTamam.Text = "Tamam";
            this.simpleButtonTamam.Click += new EventHandler(this.simpleButtonTamam_Click);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(542, 645);
            this.Controls.Add((Control)this.simpleButtonTamam);
            this.Controls.Add((Control)this.simpleButtonDetay);
            this.Controls.Add((Control)this.txtDetay);
            this.Controls.Add((Control)this.lblDetay);
            this.Controls.Add((Control)this.lblUyari);
            this.Controls.Add((Control)this.lblDescription);
            this.Controls.Add((Control)this.panelTop);
            this.IsDialog = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = nameof(FormExceptionView);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "ExceptionView";
            this.Load += new EventHandler(this.ExceptionView_Load);
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}