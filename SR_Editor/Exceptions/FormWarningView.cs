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
    public partial class FormWarningView : FormBase, IExceptionView
    {
        public FormWarningView()
        {
            this.InitializeComponent();
            this.lblMesaj.ForeColor = Color.Black;
            this.simpleButtonTamam.Text = "Tamam".CeviriYap();
        }

        public bool TumDetayGosterilsinMi { get; set; }

        public string DetayMesaj { get; set; }

        public void ExceptionGoster(ExceptionBase exception)
        {
            this.ExceptionGoster((Exception)exception, exception.Mesaj);
        }

        public void ExceptionGoster(Exception exception, string detayMesaj)
        {
            this.DetayMesaj = detayMesaj;
            if (exception is ExceptionBase)
            {
                ExceptionBase exceptionBase = exception as ExceptionBase;
                this.lblMesaj.Text = StringExtension.IsNull(exceptionBase.Mesaj) ? exceptionBase.Message : exceptionBase.Mesaj;
                this.Text = exceptionBase.Baslik;
            }
            int num = (int)this.ShowDialog();
        }

        private void simpleButtonTamam_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}