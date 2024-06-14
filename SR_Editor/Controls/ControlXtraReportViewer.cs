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
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using System.Drawing.Printing;
using DevExpress.XtraPrinting.Control;

namespace SR_Editor.Core.Controls
{
    public partial class ControlXtraReportViewer : XtraUserControl
    {
        private bool showPreview = true;

        protected ControlXtraReportPanel handler;

        protected XtraReport report;

        public ControlXtraReportPanel Handler
        {
            get
            {
                return this.handler;
            }
            set
            {
                this.handler = value;
                if (this.handler != null)
                {
                    this.Report = this.handler.Report;
                    this.previewBar1.Visible = this.handler.ShowMenuBar();
                }
                else
                {
                    this.Report = null;
                }
            }
        }

        public XtraReport Report
        {
            get
            {
                return this.report;
            }
            set
            {
                if (this.report != value)
                {
                    if (this.report != null)
                    {
                        this.report.Dispose();
                    }
                    this.report = value;
                    if (this.report != null)
                    {
                        this.report.PrintingSystem.PageSettingsChanged += new EventHandler(this.printingSystem_PageSettingsChanged);
                        this.report.PrintingSystem.StartPrint += new PrintDocumentEventHandler(this.printingSystem_StartPrint);
                        this.printControl1.PrintingSystem = this.report.PrintingSystem;
                        this.report.PrintingSystem.ClearContent();
                        this.printPreviewBarItem4.Enabled = true;
                        base.Invalidate();
                        base.Update();
                        this.report.BeforePrint += new PrintEventHandler(this.report_BeforePrint);
                    }
                }
            }
        }

        public ControlXtraReportViewer()
        {
            this.InitializeComponent();
        }

        public virtual void GenerateReport()
        {
            ProgressReflector.RegisterReflector(this.printControl.ProgressReflector);
            try
            {
                if (this.Report != null)
                {
                    this.Report.CreateDocument();
                }
            }
            finally
            {
                ProgressReflector.UnregisterReflector(this.printControl.ProgressReflector);
            }
        }

        public bool Print()
        {
            return this.Print(false, -1);
        }

        public bool Print(int numberOfCopies)
        {
            return this.Print(false, numberOfCopies);
        }

        public bool Print(bool useSettedPrinter)
        {
            return this.Print(useSettedPrinter, 1);
        }

        public bool Print(bool useSettedPrinter, int numberOfCopies)
        {
            bool flag;
            if (useSettedPrinter)
            {
                if (!this.Handler.DefaultCopyCount.HasValue)
                {
                    this.Handler.DefaultCopyCount = new int?(1);
                }
                string lower = this.Handler.Report.Tag.ToString().ToLower();
                if (lower != null)
                {
                    if (lower == "barkod")
                    {
                        this.Handler.Report.PrinterName = "";
                        this.showPreview = true;
                    }
                    else if (lower == "fatura")
                    {
                        this.Handler.Report.PrinterName = "";
                        this.showPreview = true;
                    }
                    else
                    {
                        if (lower != "normal")
                        {
                            goto Label1;
                        }
                        this.Handler.Report.PrinterName = "";
                        this.showPreview = true;
                    }
                    if (this.showPreview)
                    {
                        flag = false;
                        return flag;
                    }
                    else if (string.IsNullOrEmpty(this.Handler.Report.PrinterName))
                    {
                        flag = false;
                        return flag;
                    }
                    else
                    {
                        this.report.Print(this.Handler.Report.PrinterName);
                        flag = true;
                        return flag;
                    }
                }
                Label1:
                flag = false;
            }
            else
            {
                if (numberOfCopies != -1)
                {
                    this.Handler.DefaultCopyCount = new int?(Convert.ToInt16(numberOfCopies));
                }
                else
                {
                    this.Handler.DefaultCopyCount = new int?(1);
                }
                this.report.PrintingSystem.ExecCommand(PrintingSystemCommand.Print);
                flag = true;
            }
            return flag;
        }

        private void printingSystem_PageSettingsChanged(object sender, EventArgs e)
        {
            if (this.Report != null)
            {
            }
        }

        private void printingSystem_StartPrint(object sender, PrintDocumentEventArgs e)
        {
            e.PrintDocument.PrinterSettings.Copies = (short)this.Handler.DefaultCopyCount.IsBosIse(1);
            if ((!this.Handler.RaporTasarim.IsDolu() ? false : this.Handler.RaporTasarim.EkYaziciKodu.IsDolu()))
            {
                this.Handler.Report.PrinterName = this.Handler.RaporTasarim.EkYaziciKodu;
                e.PrintDocument.PrinterSettings.PrinterName = this.Handler.Report.PrinterName;
            }
        }

        private void report_BeforePrint(object sender, PrintEventArgs e)
        {
        }

        public bool Yazdir()
        {
            bool flag;
            if ((!this.Handler.RaporTasarim.IsDolu() || !this.Handler.RaporTasarim.EkYaziciKodu.IsDolu() ? true : !this.handler.RaporTasarim.IsOtomatikYazdir.IsBosIse(false)))
            {
                if ((!this.Handler.RaporTasarim.IsDolu() ? false : this.Handler.RaporTasarim.EkYaziciKodu.IsDolu()))
                {
                    this.report.PrinterName = this.Handler.RaporTasarim.EkYaziciKodu;
                }
                flag = this.report.PrintDialog() == DialogResult.OK;
            }
            else
            {
                this.report.Print(this.Handler.RaporTasarim.EkYaziciKodu);
                flag = true;
            }
            return flag;
        }

        public class ExPrintDocumentEventArgs : EventArgs
        {
            private PrintDocument printDocument;

            public PrintDocument PrintDocument
            {
                get
                {
                    return this.printDocument;
                }
            }

            internal ExPrintDocumentEventArgs(PrintDocument printDocument)
            {
                this.printDocument = printDocument;
            }
        }

        public delegate void ExPrintDocumentEventHandler(object sender, ControlXtraReportViewer.ExPrintDocumentEventArgs e);

        public class ExPrintingSystem : PrintingSystem
        {
            internal short NumberOfCopies = 1;

            public ExPrintingSystem(IContainer container) : base(container)
            {
            }

            protected override void PrintDocument(PrintDocument pd)
            {
                pd.PrinterSettings.Copies = this.NumberOfCopies;
                base.PrintDocument(pd);
                if (this.EndPrint != null)
                {
                    this.EndPrint(this, new ControlXtraReportViewer.ExPrintDocumentEventArgs(pd));
                }
            }

            public event ControlXtraReportViewer.ExPrintDocumentEventHandler EndPrint;
        }

        private class ReflectPrintControl : PrintControl
        {
            public new ProgressReflector ProgressReflector
            {
                get
                {
                    return base.ProgressReflector;
                }
            }

            public ReflectPrintControl()
            {
            }
        }
    }
}
