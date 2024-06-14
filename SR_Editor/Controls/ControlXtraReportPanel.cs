using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SR_Editor.Core.EF.Core;
using DevExpress.XtraReports.UI;
using System.IO;

namespace SR_Editor.Core.Controls
{
    public partial class ControlXtraReportPanel : XtraUserControl
    {
        private CoreEntities coreEntities;

        private RaporTasarim raporTasarim;

        private int? defaultCopyCount = null;

        private bool reportInitialized;

        private bool canAutoGenerate;

        private string toEmail;

        private string designData;

        private ControlXtraReportViewer reportViewer;

        private XtraReport report;

        private UtilParameters formParams;

        private ModuleInfo moduleInfo;

        public bool CanAutoGenerate
        {
            get
            {
                return this.canAutoGenerate;
            }
            set
            {
                this.canAutoGenerate = value;
            }
        }

        public int? DefaultCopyCount
        {
            get
            {
                return this.defaultCopyCount;
            }
            set
            {
                this.defaultCopyCount = value;
            }
        }

        public string DesignData
        {
            get
            {
                if (this.raporTasarim != null)
                {
                    this.DefaultCopyCount = new int?(this.raporTasarim.KopyaSayisi.IsBosIse(1));
                    this.designData = this.raporTasarim.DesignData;
                }
                return this.designData;
            }
            set
            {
                string str = this.designData;
                string str1 = value;
                if (!string.IsNullOrEmpty(str1))
                {
                    str1 = str1.Replace("\0", "");
                }
                this.designData = str1;
                if (str != value)
                {
                }
            }
        }

        public Stream DesignDataStream
        {
            get
            {
                Stream memoryStream = new MemoryStream();
                StreamWriter streamWriter = new StreamWriter(memoryStream);
                streamWriter.Write(this.DesignData);
                streamWriter.Flush();
                memoryStream.Position = (long)0;
                return memoryStream;
            }
            set
            {
                StreamReader streamReader = new StreamReader(value);
                value.Seek((long)0, SeekOrigin.Begin);
                string end = streamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(end))
                {
                    end = end.Replace("\0", "");
                }
                streamReader.Close();
                this.designData = end;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UtilParameters FormParams
        {
            get
            {
                if (this.formParams == null)
                {
                    this.formParams = new UtilParameters();
                }
                return this.formParams;
            }
            set
            {
                this.formParams = value;
                this.FormParamsChanged();
            }
        }

        public ModuleInfo Module
        {
            get
            {
                return this.moduleInfo;
            }
            set
            {
                this.moduleInfo = value;
            }
        }

        public RaporTasarim RaporTasarim
        {
            get
            {
                return this.raporTasarim;
            }
            set
            {
                this.raporTasarim = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public XtraReport Report
        {
            get
            {
                return this.report;
            }
            set
            {
                this.report = value;
            }
        }

        public bool ReportInitialized
        {
            get
            {
                return this.reportInitialized;
            }
            set
            {
                this.reportInitialized = value;
            }
        }

        public string ToEmail
        {
            get
            {
                return this.toEmail;
            }
            set
            {
                this.toEmail = value;
            }
        }

        public ControlXtraReportViewer ViewerControl
        {
            get
            {
                return this.reportViewer;
            }
            set
            {
                this.reportViewer = value;
            }
        }

        public ControlXtraReportPanel()
        {
            this.InitializeComponent();
            this.coreEntities = CoreEntities.Yeni();
        }

        protected virtual XtraReport CreateReport()
        {
            return null;
        }

        public void Design()
        {
            XtraDesignForm xtraDesignForm = new XtraDesignForm();
            try
            {
                XtraReport xtraReport = Activator.CreateInstance(this.report.GetType()) as XtraReport;
                xtraReport.SaveComponents += new EventHandler<SaveComponentsEventArgs>(this.Report_SaveComponents);
                xtraDesignForm.OpenReport(xtraReport);
                if (!string.IsNullOrEmpty(this.DesignData))
                {
                    string str = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "\\ReportDesigner.repx");
                    Stream fileStream = null;
                    StreamWriter streamWriter = null;
                    try
                    {
                        try
                        {
                            fileStream = new FileStream(str, FileMode.Create);
                            streamWriter = new StreamWriter(fileStream);
                            streamWriter.Write(this.DesignData);
                            streamWriter.Close();
                            fileStream.Close();
                            fileStream = new FileStream(str, FileMode.Open);
                            xtraDesignForm.OpenReport(fileStream);
                            fileStream.Close();
                        }
                        catch
                        {
                        }
                    }
                    finally
                    {
                        if (streamWriter != null)
                        {
                            streamWriter.Close();
                        }
                        if (fileStream != null)
                        {
                            fileStream.Close();
                        }
                    }
                }
                if (xtraDesignForm.ShowDialog() == DialogResult.OK)
                {
                    Stream memoryStream = new MemoryStream();
                    xtraDesignForm.SaveReport(memoryStream);
                    this.DesignDataStream = memoryStream;
                    memoryStream.Close();
                    this.coreEntities.RaporTasarimAdapter.Set(this.raporTasarim, this.DesignData);
                    this.coreEntities.SaveChanges();
                    this.PrepareReport();
                }
            }
            finally
            {
                xtraDesignForm.Dispose();
            }
        }

        public virtual void FormParamsChanged()
        {
        }

        protected void GenerateReport()
        {
            this.ViewerControl.GenerateReport();
        }

        private List<int> GetItemIndexes(ComponentCollection components, Type targetType)
        {
            List<int> nums = new List<int>();
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].GetType() == targetType)
                {
                    nums.Add(i);
                }
            }
            return nums;
        }

        protected virtual void InitData()
        {
        }

        public void InitializeReport()
        {
            this.reportInitialized = false;
            try
            {
                this.reportInitialized = this.PrepareReport();
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                throw new Exception(string.Format("{0} raporu oluşturulamadı!", this.Module.Caption), exception);
            }
        }

        public virtual void MailGonderildi()
        {
        }

        protected bool PrepareReport()
        {
            if (this.ViewerControl == null)
            {
                this.ViewerControl = new ControlXtraReportViewer();
            }
            if (this.report == null)
            {
                this.report = this.CreateReport();
                if (this.report == null)
                {
                    throw new Exception(string.Format("{0} raporu bulunamadı!", this.Module.Caption));
                }
                if (!string.IsNullOrEmpty(this.DesignData))
                {
                    Stream designDataStream = this.DesignDataStream;
                    StreamReader streamReader = new StreamReader(designDataStream);
                    this.report.LoadLayout(designDataStream);
                    streamReader.Close();
                    designDataStream.Close();
                }
                this.ViewerControl.Handler = this;
            }
            return this.report != null;
        }

        public void RefreshReport()
        {
            try
            {
                this.InitData();
                this.GenerateReport();
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                throw new Exception(string.Format("{0} raporu oluşturulamadı!", this.Module.Caption), exception);
            }
        }

        private void Report_SaveComponents(object sender, SaveComponentsEventArgs e)
        {
            foreach (int itemIndex in this.GetItemIndexes((sender as XtraReport).Container.Components, typeof(BindingSource)))
            {
                e.Components.Add((sender as XtraReport).Container.Components[itemIndex]);
            }
        }

        public void ResetDesign()
        {
            if (!string.IsNullOrEmpty(this.DesignData))
            {
                this.DesignData = null;
                this.coreEntities.RaporTasarimAdapter.Set(this.raporTasarim, this.DesignData);
                this.coreEntities.SaveChanges();
                RaporTasarim.List.Remove(this.raporTasarim);
            }
            this.ResetReport();
        }

        public void ResetReport()
        {
            this.Report = null;
            this.PrepareReport();
        }

        public virtual bool ShowMenuBar()
        {
            return true;
        }

        public virtual void Yazdir()
        {
        }
    }
}
