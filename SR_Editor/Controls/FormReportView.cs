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
using SR_Editor.Core.Base;
using SR_Editor.Core.EF.Core;
using DevExpress.XtraBars;
using SR_Editor.LookUp;
using System.IO;
using DevExpress.XtraPrinting;

namespace SR_Editor.Core.Controls
{
    public partial class FormReportView : FormBase
    {
        private readonly Timer startTimer;

        private List<RaporTasarim> listRaporTasarim = new List<RaporTasarim>();

        private ControlXtraReportPanel reportInstance;
        
        public ControlXtraReportPanel ReportInstance
        {
            get
            {
                return this.reportInstance;
            }
            set
            {
                this.reportInstance = value;
                if (this.reportInstance != null)
                {
                    this.reportInstance.InitializeReport();
                }
                this.UpdateControls();
                this.InitRight();
                this.InitDesign();
                this.LoadDigerTasarimlar();
            }
        }

        public FormReportView()
        {
            this.InitializeComponent();
            this.startTimer = new Timer();
            this.startTimer.Tick += new EventHandler(this.startTimer_Tick);
            this.startTimer.Interval = 100;
            this.barButtonItemMailGonder.Enabled = false;// EditorApplication.Yetki.Sistem.MailGonderebilir;
        }

        private void barButtonItemMailGonder_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.MailGonder();
        }

        private void barButtonItemOrajinalTasarim_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (UtilMessage.Show(EnumUtilMessage.RaporOrjinalTasarimiDonOnayKontrol, null, "Orijinal rapor tasarımına dönmek ve mevcut tasarımı silmek istediğinize emin misiniz?", "Rapor Tasarımı", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.ReportInstance.ResetDesign();
                this.ReportInstance.ResetReport();
                this.UpdateControls();
            }
        }

        private void barButtonItemTasarim_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.ReportInstance.Design();
            this.ReportInstance.ResetReport();
            this.UpdateControls();
        }

        private void barButtonItemYazdir_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Yazdir();
        }

        private void barButtonItemYenile_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Yenile();
        }

        private void barEditItemDigerTasarimlar_EditValueChanged(object sender, EventArgs e)
        {
            if (this.barEditItemDigerTasarimlar.EditValue != null)
            {
                int editValue = (int)this.barEditItemDigerTasarimlar.EditValue;
                RaporTasarim designData = this.listRaporTasarim.Find((RaporTasarim t) => t.Id == editValue);
                if (designData.IsDolu())
                {
                    if (designData.DesignData.IsBos())
                    {
                        designData.DesignData = CoreEntities.Instance.RaporTasarimQuery.GetById(designData.Id).DesignData;
                    }
                    this.ReportInstance.RaporTasarim = this.listRaporTasarim.Find((RaporTasarim t) => t.Id == editValue);
                    this.ReportInstance.Report = null;
                    this.ReportInstance.InitializeReport();
                    this.ReportInstance.RefreshReport();
                }
            }
        }

        public override void FormParamsChanged()
        {
            this.InitRight();
            this.InitDesign();
        }

        private void FormReportView_KeyDown(object sender, KeyEventArgs e)
        {
            this.SetKisaYol(e);
        }

        private void FormReportView_Shown(object sender, EventArgs e)
        {
            this.ReportInstance.RefreshReport();
            if ((!this.ReportInstance.IsDolu() || !this.ReportInstance.Report.IsDolu() || !this.reportInstance.FormParams.IsDolu() || !this.reportInstance.FormParams.Contains("IsRTF") ? true : !Convert.ToBoolean(this.reportInstance.FormParams["IsRTF"])))
            {
                bool flag = this.ReportInstance.ViewerControl.Yazdir();
                if ((!flag ? false : this.reportInstance != null))
                {
                    this.reportInstance.Yazdir();
                }
                if ((this.ReportInstance.RaporTasarim == null || !flag ? false : this.ReportInstance.RaporTasarim.IsYazdirdiktanSonraKapat.IsBosIse(false)))
                {
                    base.Close();
                }
            }
            else
            {
                MemoryStream memoryStream = new MemoryStream();
                RtfExportOptions rtfExportOption = new RtfExportOptions()
                {
                    ExportMode = RtfExportMode.SingleFile
                };
                this.ReportInstance.Report.ExportToRtf(memoryStream, rtfExportOption);
                this.reportInstance.FormParams.Add("RTFStream", memoryStream);
                base.Close();
            }
        }

        public void InitDesign()
        {
            this.bar1.Visible = this.ReportInstance.ShowMenuBar();
        }

        public void InitRight()
        {
        }

        private void LoadDigerTasarimlar()
        {
            bool flag;
            UtilLookUp.InitLookupEdit(this.repositoryItemLookUpEditDigerTasarimlar, "Id", "SablonAdi");
            this.listRaporTasarim = CoreEntities.Instance.RaporTasarimQuery.GetByFormReporViewLookUp(this.ReportInstance.Module.FullKey);
            if (this.ReportInstance.Module.FullKey == "Editor.HastaKabul.Rapor.ControlProtokolDinamikIcerik")
            {
                flag = true;
            }
            else
            {
                flag = false;//(!this.reportInstance.IsDolu() || !this.reportInstance.RaporTasarim.IsDolu() ? false : this.reportInstance.RaporTasarim.EkIsDigerTasarimlarReadOnly.IsBosIse(false));
            }
            if (!flag)
            {
                this.repositoryItemLookUpEditDigerTasarimlar.DataSource = this.listRaporTasarim;
            }
        }

        private void MailGonder()
        {
            /*if (this.reportInstance.ToEmail == null)
            {
                this.reportInstance.ToEmail = "";
            }
            EditorMail EditorMail = new EditorMail();
            MemoryStream memoryStream = new MemoryStream();
            this.reportInstance.Report.ExportToPdf(memoryStream);
            List<Stream> streams = new List<Stream>()
            {
                memoryStream
            };
            EditorIslemSonuc EditorIslemSonuc = new EditorIslemSonuc(EnumSonucTipi.IslemBasarili);
            if (this.reportInstance.ToEmail.IsNull())
            {
                this.reportInstance.ToEmail = "Email";
            }
            FormEmailAddress formEmailAddress = new FormEmailAddress();
            string toEmail = this.reportInstance.ToEmail;
            formEmailAddress.ShowDialog(this, ref toEmail);
            if (!toEmail.IsDolu())
            {
                UtilMessage.Show(EnumUtilMessage.MailGonderimAdresKontrol, null, "Lütfen geçerli bir mail adresi belirtiniz.", "Mail Adresi Girin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.reportInstance.ToEmail = toEmail;
                string str = this.reportInstance.ToEmail;
                char[] chrArray = new char[] { ';' };
                IEnumerable<string> strs =
                    from i in str.Split(chrArray)
                    where i.IsNotNull()
                    select i;
                foreach (string str1 in strs)
                {
                    EditorIslemSonuc = EditorMail.Gonder(string.Concat(EditorMail.Settings.UserName, " -  Bilgilendirme"), "Bilgilendirme", str1, streams);
                }
                if (EditorIslemSonuc.SonucKodu == EnumSonucTipi.IslemBasarili)
                {
                    UtilMessage.Show(EnumUtilMessage.TestMailGonderIslemSonuc, null, "Mail başarı ile gönderildi", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    this.ReportInstance.MailGonderildi();
                }
                else if (EditorIslemSonuc.MesajIcerik.IsDolu())
                {
                    UtilMessage.Show(EnumUtilMessage.MailBasarisizGonderim, null, string.Concat("Mail gönderiminde hatalar meydana geldi!\r\nHata=", EditorIslemSonuc.MesajIcerik), "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            memoryStream.Close();
            memoryStream.Flush();*/
        }

        private void SetKisaYol(KeyEventArgs e)
        {
            if (e.KeyData == (Keys.ShiftKey | Keys.P | Keys.Control))
            {
                this.Yazdir();
            }
            else if (e.KeyData == (Keys.LButton | Keys.MButton | Keys.XButton1 | Keys.Back | Keys.Tab | Keys.Clear | Keys.Return | Keys.Enter | Keys.A | Keys.D | Keys.E | Keys.H | Keys.I | Keys.L | Keys.M | Keys.Control))
            {
                this.MailGonder();
            }
            e.Handled = true;
        }

        private void startTimer_Tick(object sender, EventArgs e)
        {
            this.Yenile();
        }

        protected void UpdateControls()
        {
            this.panelInstanceParent.Visible = false;
            this.panelViewer.Controls.Clear();
            this.panelInstanceParent.SuspendLayout();
            this.panelViewer.SuspendLayout();
            try
            {
                this.panelInstance.Controls.Clear();
                bool flag = (this.ReportInstance == null ? false : this.ReportInstance.ReportInitialized);
                if (flag)
                {
                    if (this.ReportInstance.Controls.Count > 0)
                    {
                        PanelControl vertical = this.panelInstanceParent;
                        int height = this.ReportInstance.Height;
                        Padding padding = this.panelInstanceParent.Padding;
                        vertical.Height = height + padding.Vertical + 4;
                        this.ReportInstance.Padding = new Padding(0, 3, 0, 0);
                        this.ReportInstance.Dock = DockStyle.Top;
                        this.panelInstance.Controls.Add(this.ReportInstance);
                    }
                    this.ReportInstance.ViewerControl.Size = this.panelViewer.ClientSize;
                    this.ReportInstance.ViewerControl.Dock = DockStyle.Fill;
                    this.panelViewer.Controls.Add(this.ReportInstance.ViewerControl);
                }
                this.barButtonItemOrajinalTasarim.Enabled = flag;
            }
            finally
            {
                this.panelInstanceParent.ResumeLayout();
                this.panelViewer.ResumeLayout();
                this.panelInstanceParent.Visible = this.panelInstance.Controls.Count > 0;
            }
        }

        private void Yazdir()
        {
            this.reportInstance.ViewerControl.Print(1);
            this.reportInstance.Yazdir();
        }

        private void Yenile()
        {
            try
            {
                this.ReportInstance.RefreshReport();
            }
            catch (Exception exception)
            {
            }
            if (this.reportInstance.ViewerControl.Print(true))
            {
                base.Close();
            }
        }

        public class ControlProperty
        {
            private string name;

            private Point location;

            private Size size;

            public Point Location
            {
                get
                {
                    return this.location;
                }
                set
                {
                    this.location = value;
                }
            }

            public string Name
            {
                get
                {
                    return this.name;
                }
                set
                {
                    this.name = value;
                }
            }

            public Size Size
            {
                get
                {
                    return this.size;
                }
                set
                {
                    this.size = value;
                }
            }

            public ControlProperty()
            {
                this.name = "";
                this.location = new Point();
                this.size = new Size();
            }

            public ControlProperty(string name, Point location, Size size)
            {
                this.name = name;
                this.location = location;
                this.size = size;
            }
        }

        public class ControlPropertyList
        {
            private readonly List<FormReportView.ControlProperty> list = new List<FormReportView.ControlProperty>();

            public List<FormReportView.ControlProperty> List
            {
                get
                {
                    return this.list;
                }
            }

            public ControlPropertyList()
            {
            }

            public void Add(FormReportView.ControlProperty controlProperty)
            {
                bool flag = false;
                foreach (FormReportView.ControlProperty location in this.list)
                {
                    if (!(location.Name == controlProperty.Name))
                    {
                        continue;
                    }
                    location.Location = controlProperty.Location;
                    location.Size = controlProperty.Size;
                    flag = true;
                    break;
                }
                if (!flag)
                {
                    this.list.Add(new FormReportView.ControlProperty(controlProperty.Name, controlProperty.Location, controlProperty.Size));
                }
            }
        }
    }
}