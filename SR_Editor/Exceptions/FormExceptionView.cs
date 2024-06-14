using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SR_Editor.Core.Exceptions
{
    public partial class FormExceptionView : FormBase, IExceptionView
    {
        public FormExceptionView()
        {
            this.InitializeComponent();
            this.lblMesaj.ForeColor = Color.Black;
            this.simpleButtonTamam.Text = "Tamam".CeviriYap();
            this.lblDetay.Text = "Detay".CeviriYap();
            this.lblUyari.Text = "Uyarı".CeviriYap();
        }

        public bool TumDetayGosterilsinMi { get; set; }

        public string DetayMesaj { get; set; }

        public void ExceptionGoster(ExceptionBase exception)
        {
            this.ExceptionGoster((Exception)exception, exception.ToString());
        }

        public void ExceptionGoster(Exception exception, string detayMesaj)
        {
            this.DetayMesaj = detayMesaj;
            if (exception is ExceptionBase)
            {
                ExceptionBase exceptionBase = exception as ExceptionBase;
                this.lblMesaj.Text = StringExtension.IsNull(exceptionBase.Mesaj) ? exceptionBase.Message : exceptionBase.Mesaj;
                this.lblUyari.Visible = this.lblDescription.Visible = this.lblDetay.Visible = this.simpleButtonDetay.Visible = this.TumDetayGosterilsinMi;
                this.txtDetay.Text = this.DetayMesaj;
                this.Text = exceptionBase.Baslik;
            }
            else
            {
                this.lblMesaj.Text = exception.Message;
                this.lblUyari.Visible = true;
                this.txtDetay.Text = this.DetayMesaj;
            }
            int num = (int)this.ShowDialog();
        }

        private void ExceptionView_Load(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.simpleButtonDetay.Text = " > > ";
            this.Height = this.simpleButtonDetay.Top + this.simpleButtonDetay.Height + 10 + 45;
            this.txtDetay.Visible = false;
            this.txtDetay.Anchor = AnchorStyles.None;
            this.ResumeLayout();
            this.CenterToScreen();
        }

        private void simpleButtonDetay_Click(object sender, EventArgs e)
        {
            if (this.simpleButtonDetay.Text == " > > ")
            {
                this.Height += 300;
                this.txtDetay.Location = new Point(this.lblDetay.Left, this.lblDetay.Top + this.lblDetay.Height + 10);
                this.txtDetay.Height = this.ClientSize.Height - this.txtDetay.Top - 45;
                this.txtDetay.Width = this.ClientSize.Width - 20;
                this.txtDetay.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
                this.txtDetay.Visible = true;
                this.simpleButtonTamam.Focus();
                this.simpleButtonDetay.Text = " < < ";
            }
            else
            {
                this.SuspendLayout();
                this.simpleButtonDetay.Text = " > > ";
                this.Height = this.simpleButtonDetay.Top + this.simpleButtonDetay.Height + 10 + 45;
                this.txtDetay.Visible = false;
                this.txtDetay.Anchor = AnchorStyles.None;
                this.ResumeLayout();
            }
        }

        private void simpleButtonTamam_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}