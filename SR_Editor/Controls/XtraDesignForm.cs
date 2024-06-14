using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using SR_Editor.Core.EF.Core;
using DevExpress.XtraBars;
using SR_Editor.LookUp;
using System.IO;
using System.Windows.Forms;

namespace SR_Editor.Core.Controls
{
    public partial class XtraDesignForm : XRDesignFormExBase
    {
        private RaporTasarim raporTasarim;
        private ReportState reportState = ReportState.None;


        public RaporTasarim RaporTasarim
        {
            get
            {
                return this.raporTasarim;
            }
            set
            {
                bool? isOtomatikYazdir;
                this.raporTasarim = value;
                this.barEditItemSablonAdi.EditValue = this.raporTasarim.SablonAdi;
                if (this.raporTasarim.IsOtomatikYazdir.IsDolu())
                {
                    BarEditItem barEditItem = this.barEditItemCheckOtomatikYazdir;
                    isOtomatikYazdir = this.raporTasarim.IsOtomatikYazdir;
                    barEditItem.EditValue = isOtomatikYazdir.Value;
                }
                if (this.raporTasarim.IsYazdirdiktanSonraKapat.IsDolu())
                {
                    BarEditItem barEditItem1 = this.barEditItemCheckYazdirilincaKapat;
                    isOtomatikYazdir = this.raporTasarim.IsYazdirdiktanSonraKapat;
                    barEditItem1.EditValue = isOtomatikYazdir.Value;
                }
            }
        }

        public XtraReport Report
        {
            get
            {
                return base.DesignPanel.Report;
            }
        }

        public XtraDesignForm()
        {
            this.InitializeComponent();
            this.barStaticItemOtomatikYazdirText.Caption = this.barStaticItemOtomatikYazdirText.Caption.CeviriYap();
            this.barStaticItemYazdirilincaKapansin.Caption = this.barStaticItemYazdirilincaKapansin.Caption.CeviriYap();
            this.barButtonItemKaydet.Caption = this.barButtonItemKaydet.Caption.CeviriYap();
            this.barStaticItemSablonAdi.Caption = this.barStaticItemSablonAdi.Caption.CeviriYap();
        }

        private void barButtonItemKaydet_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (!this.barEditItemSablonAdi.EditValue.IsBos())
            {
                if ((!this.barEditItemCheckOtomatikYazdir.EditValue.IsDolu() ? true : !Convert.ToBoolean(this.barEditItemCheckOtomatikYazdir.EditValue)))
                {
                    this.raporTasarim.IsOtomatikYazdir = new bool?(false);
                }
                else
                {
                    this.raporTasarim.IsOtomatikYazdir = new bool?(true);
                }
                if ((!this.barEditItemCheckYazdirilincaKapat.EditValue.IsDolu() ? true : !Convert.ToBoolean(this.barEditItemCheckYazdirilincaKapat.EditValue)))
                {
                    this.raporTasarim.IsYazdirdiktanSonraKapat = new bool?(false);
                }
                else
                {
                    this.raporTasarim.IsYazdirdiktanSonraKapat = new bool?(true);
                }
                this.raporTasarim.SablonAdi = this.barEditItemSablonAdi.EditValue.ToString();
                base.DialogResult = DialogResult.OK;
            }
            else
            {
                base.DialogResult = DialogResult.None;
                UtilMessage.Show(EnumUtilMessage.RaporSablonAdiKontrol, null, "Lütfen şablon adı giriniz.", "Şablon Adı", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            if (base.DialogResult != DialogResult.OK)
            {
                if (UtilMessage.Show(EnumUtilMessage.RaporDegisiklikleriniKaydetOnayKontrol, null, "Rapor tasarımında yaptığınız değişiklikleri kaydetmek istiyor musunuz?\r\nDeğişiklikler tüm kullanıcılar için geçerli olacak!", "Rapor Tasarımı", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    base.DialogResult = DialogResult.Cancel;
                }
                else
                {
                    base.DialogResult = DialogResult.OK;
                }
            }
        }

        public void OpenReport(Stream stream)
        {
            base.DesignPanel.OpenReport(stream);
        }

        protected override void RestoreLayout()
        {
        }

        protected override void SaveLayout()
        {
        }

        public void SaveReport(Stream stream)
        {
            base.DesignPanel.Report.SaveLayout(stream);
        }

        private void xrDesignPanel_ReportStateChanged(object sender, ReportStateEventArgs e)
        {
            if (e.ReportState != ReportState.None)
            {
                this.reportState = e.ReportState;
                //base.DesignPanel.ReportState = ReportState.None;
            }
        }

    }
}
