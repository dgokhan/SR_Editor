using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrinting.Control;
using DevExpress.XtraPrinting.Preview;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SR_Editor.Core.Controls
{
    partial class ControlXtraReportViewer
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        /// 
        private ControlXtraReportViewer.ReflectPrintControl printControl;

        private IContainer components;

        private PrintBarManager printBarManager1;

        private PreviewBar previewBar1;

        private PrintPreviewBarItem printPreviewBarItem1;

        private PrintPreviewBarItem printPreviewBarItem2;

        private PrintPreviewBarItem printPreviewBarItem3;

        private PrintPreviewBarItem printPreviewBarItem4;

        private PrintPreviewBarItem printPreviewBarItem6;

        private PrintPreviewBarItem printPreviewBarItem7;

        private PrintPreviewBarItem printPreviewBarItem8;

        private PrintPreviewBarItem printPreviewBarItem9;

        private PrintPreviewBarItem printPreviewBarItem10;

        private ZoomBarEditItem zoomBarEditItem1;

        private PrintPreviewRepositoryItemComboBox printPreviewRepositoryItemComboBox1;

        private PrintPreviewBarItem printPreviewBarItem11;

        private PrintPreviewBarItem printPreviewBarItem12;

        private PrintPreviewBarItem printPreviewBarItem13;

        private PrintPreviewBarItem printPreviewBarItem14;

        private PrintPreviewBarItem printPreviewBarItem15;

        private PrintPreviewBarItem printPreviewBarItem16;

        private MultiplePagesControlContainer multiplePagesControlContainer1;

        private PrintPreviewBarItem printPreviewBarItem17;

        private ColorPopupControlContainer colorPopupControlContainer1;

        private PrintPreviewBarItem printPreviewBarItem18;

        private PrintPreviewBarItem printPreviewBarItem19;

        private PrintPreviewBarItem printPreviewBarItem20;

        private PrintPreviewBarItem printPreviewBarItem21;

        private PreviewBar previewBar2;

        private PrintPreviewStaticItem printPreviewStaticItem1;

        private PrintPreviewStaticItem printPreviewStaticItem2;

        private PrintPreviewStaticItem printPreviewStaticItem3;

        private PreviewBar previewBar3;

        private BarSubItem barSubItem1;

        private BarSubItem barSubItem2;

        private BarSubItem barSubItem4;

        private PrintPreviewBarItem printPreviewBarItem22;

        private PrintPreviewBarItem printPreviewBarItem23;

        private BarToolbarsListItem barToolbarsListItem1;

        private BarSubItem barSubItem3;

        private BarDockControl barDockControlTop;

        private BarDockControl barDockControlBottom;

        private BarDockControl barDockControlLeft;

        private BarDockControl barDockControlRight;

        private PrintControl printControl1;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem1;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem2;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem3;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem4;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem5;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem6;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem7;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem8;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem9;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem10;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem11;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem12;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem13;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem14;

        private PrintPreviewBarCheckItem printPreviewBarCheckItem15;

        private Timer timer1;

        private PanelControl panelControl1;


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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ControlXtraReportViewer));
            this.printControl = new ControlXtraReportViewer.ReflectPrintControl();
            this.panelControl1 = new PanelControl();
            this.printBarManager1 = new PrintBarManager();
            this.previewBar1 = new PreviewBar();
            this.printPreviewBarItem1 = new PrintPreviewBarItem();
            this.printPreviewBarItem2 = new PrintPreviewBarItem();
            this.printPreviewBarItem3 = new PrintPreviewBarItem();
            this.printPreviewBarItem4 = new PrintPreviewBarItem();
            this.printPreviewBarItem6 = new PrintPreviewBarItem();
            this.printPreviewBarItem7 = new PrintPreviewBarItem();
            this.printPreviewBarItem8 = new PrintPreviewBarItem();
            this.printPreviewBarItem9 = new PrintPreviewBarItem();
            this.printPreviewBarItem10 = new PrintPreviewBarItem();
            this.zoomBarEditItem1 = new ZoomBarEditItem();
            this.printPreviewRepositoryItemComboBox1 = new PrintPreviewRepositoryItemComboBox();
            this.printPreviewBarItem11 = new PrintPreviewBarItem();
            this.printPreviewBarItem12 = new PrintPreviewBarItem();
            this.printPreviewBarItem13 = new PrintPreviewBarItem();
            this.printPreviewBarItem14 = new PrintPreviewBarItem();
            this.printPreviewBarItem15 = new PrintPreviewBarItem();
            this.printPreviewBarItem16 = new PrintPreviewBarItem();
            this.printPreviewBarItem17 = new PrintPreviewBarItem();
            this.printPreviewBarItem18 = new PrintPreviewBarItem();
            this.printPreviewBarItem19 = new PrintPreviewBarItem();
            this.printPreviewBarItem20 = new PrintPreviewBarItem();
            this.printPreviewBarItem21 = new PrintPreviewBarItem();
            this.previewBar2 = new PreviewBar();
            this.printPreviewStaticItem1 = new PrintPreviewStaticItem();
            this.printPreviewStaticItem2 = new PrintPreviewStaticItem();
            this.printPreviewStaticItem3 = new PrintPreviewStaticItem();
            this.previewBar3 = new PreviewBar();
            this.barSubItem1 = new BarSubItem();
            this.barSubItem2 = new BarSubItem();
            this.barSubItem4 = new BarSubItem();
            this.printPreviewBarItem22 = new PrintPreviewBarItem();
            this.printPreviewBarItem23 = new PrintPreviewBarItem();
            this.barToolbarsListItem1 = new BarToolbarsListItem();
            this.barSubItem3 = new BarSubItem();
            this.barDockControlTop = new BarDockControl();
            this.barDockControlBottom = new BarDockControl();
            this.barDockControlLeft = new BarDockControl();
            this.barDockControlRight = new BarDockControl();
            this.printPreviewBarCheckItem1 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem2 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem3 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem4 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem5 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem6 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem7 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem8 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem9 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem10 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem11 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem12 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem13 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem14 = new PrintPreviewBarCheckItem();
            this.printPreviewBarCheckItem15 = new PrintPreviewBarCheckItem();
            this.printControl1 = new PrintControl();
            this.timer1 = new Timer(this.components);
            ((ISupportInitialize)this.panelControl1).BeginInit();
            this.panelControl1.SuspendLayout();
            ((ISupportInitialize)this.printBarManager1).BeginInit();
            ((ISupportInitialize)this.printPreviewRepositoryItemComboBox1).BeginInit();
            base.SuspendLayout();
            this.printControl.BackColor = Color.Empty;
            this.printControl.Dock = DockStyle.Fill;
            this.printControl.ForeColor = Color.Empty;
            this.printControl.IsMetric = true;
            this.printControl.Location = new Point(2, 2);
            this.printControl.Name = "printControl";
            this.printControl.Size = new Size(696, 314);
            this.printControl.TabIndex = 1;
            this.printControl.TabStop = false;
            this.printControl.TooltipFont = new Font("Tahoma", 8.25f);
            this.panelControl1.BorderStyle = BorderStyles.Flat;
            this.panelControl1.Controls.Add(this.printControl);
            this.panelControl1.Dock = DockStyle.Fill;
            this.panelControl1.Location = new Point(0, 53);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new Size(700, 318);
            this.panelControl1.TabIndex = 4;
            this.printBarManager1.AllowQuickCustomization = false;
            printBarManager1.Bars.AddRange(new Bar[] { this.previewBar1, this.previewBar2, this.previewBar3 });
            this.printBarManager1.DockControls.Add(this.barDockControlTop);
            this.printBarManager1.DockControls.Add(this.barDockControlBottom);
            this.printBarManager1.DockControls.Add(this.barDockControlLeft);
            this.printBarManager1.DockControls.Add(this.barDockControlRight);
            this.printBarManager1.Form = this;
            this.printBarManager1.ImageStream = printBarManager1.ImageStream;
            printBarManager1.Items.AddRange(new BarItem[] { this.printPreviewBarItem17, this.printPreviewStaticItem1, this.printPreviewStaticItem2, this.printPreviewStaticItem3, this.printPreviewBarItem1, this.printPreviewBarItem2, this.printPreviewBarItem3, this.printPreviewBarItem4, this.printPreviewBarItem6, this.printPreviewBarItem7, this.printPreviewBarItem8, this.printPreviewBarItem9, this.printPreviewBarItem10, this.zoomBarEditItem1, this.printPreviewBarItem11, this.printPreviewBarItem12, this.printPreviewBarItem13, this.printPreviewBarItem14, this.printPreviewBarItem15, this.printPreviewBarItem16, this.printPreviewBarItem18, this.printPreviewBarItem19, this.printPreviewBarItem20, this.printPreviewBarItem21, this.barSubItem1, this.barSubItem2, this.barSubItem3, this.barSubItem4, this.printPreviewBarItem22, this.printPreviewBarItem23, this.barToolbarsListItem1, this.printPreviewBarCheckItem1, this.printPreviewBarCheckItem2, this.printPreviewBarCheckItem3, this.printPreviewBarCheckItem4, this.printPreviewBarCheckItem5, this.printPreviewBarCheckItem6, this.printPreviewBarCheckItem7, this.printPreviewBarCheckItem8, this.printPreviewBarCheckItem9, this.printPreviewBarCheckItem10, this.printPreviewBarCheckItem11, this.printPreviewBarCheckItem12, this.printPreviewBarCheckItem13, this.printPreviewBarCheckItem14, this.printPreviewBarCheckItem15 });
            this.printBarManager1.MainMenu = this.previewBar3;
            this.printBarManager1.MaxItemId = 47;
            this.printBarManager1.PreviewBar = this.previewBar1;
            this.printBarManager1.PrintControl = this.printControl1;
            this.printBarManager1.RepositoryItems.AddRange(new RepositoryItem[] { this.printPreviewRepositoryItemComboBox1 });
            this.printBarManager1.StatusBar = this.previewBar2;
            this.previewBar1.BarName = "Toolbar";
            this.previewBar1.DockCol = 0;
            this.previewBar1.DockRow = 0;
            this.previewBar1.DockStyle = BarDockStyle.Top;
            previewBar1.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.printPreviewBarItem1), new LinkPersistInfo(this.printPreviewBarItem2), new LinkPersistInfo(this.printPreviewBarItem3, true), new LinkPersistInfo(BarLinkUserDefines.None, false, this.printPreviewBarItem4, false), new LinkPersistInfo(this.printPreviewBarItem6), new LinkPersistInfo(this.printPreviewBarItem7), new LinkPersistInfo(this.printPreviewBarItem8, true), new LinkPersistInfo(this.printPreviewBarItem9), new LinkPersistInfo(this.printPreviewBarItem10, true), new LinkPersistInfo(this.zoomBarEditItem1), new LinkPersistInfo(this.printPreviewBarItem11), new LinkPersistInfo(this.printPreviewBarItem12, true), new LinkPersistInfo(this.printPreviewBarItem13), new LinkPersistInfo(this.printPreviewBarItem14), new LinkPersistInfo(this.printPreviewBarItem15), new LinkPersistInfo(this.printPreviewBarItem16, true), new LinkPersistInfo(this.printPreviewBarItem17), new LinkPersistInfo(this.printPreviewBarItem18), new LinkPersistInfo(this.printPreviewBarItem19, true), new LinkPersistInfo(this.printPreviewBarItem20), new LinkPersistInfo(this.printPreviewBarItem21, true) });
            this.previewBar1.OptionsBar.AllowQuickCustomization = false;
            this.previewBar1.OptionsBar.DisableCustomization = true;
            this.previewBar1.OptionsBar.DrawDragBorder = false;
            this.previewBar1.OptionsBar.UseWholeRow = true;
            this.previewBar1.Text = "Toolbar";
            this.printPreviewBarItem1.ButtonStyle = BarButtonStyle.Check;
            this.printPreviewBarItem1.Caption = "Document Map";
            this.printPreviewBarItem1.Command = PrintingSystemCommand.DocumentMap;
            this.printPreviewBarItem1.Enabled = false;
            this.printPreviewBarItem1.Hint = "Document Map";
            this.printPreviewBarItem1.Id = 3;
            this.printPreviewBarItem1.ImageIndex = 19;
            this.printPreviewBarItem1.Name = "printPreviewBarItem1";
            this.printPreviewBarItem1.Visibility = BarItemVisibility.Never;
            this.printPreviewBarItem2.Caption = "Search";
            this.printPreviewBarItem2.Command = PrintingSystemCommand.Find;
            this.printPreviewBarItem2.Enabled = false;
            this.printPreviewBarItem2.Hint = "Search";
            this.printPreviewBarItem2.Id = 4;
            this.printPreviewBarItem2.ImageIndex = 20;
            this.printPreviewBarItem2.Name = "printPreviewBarItem2";
            this.printPreviewBarItem3.ButtonStyle = BarButtonStyle.Check;
            this.printPreviewBarItem3.Caption = "Customize";
            this.printPreviewBarItem3.Command = PrintingSystemCommand.Customize;
            this.printPreviewBarItem3.Enabled = false;
            this.printPreviewBarItem3.Hint = "Customize";
            this.printPreviewBarItem3.Id = 5;
            this.printPreviewBarItem3.ImageIndex = 14;
            this.printPreviewBarItem3.Name = "printPreviewBarItem3";
            this.printPreviewBarItem4.ButtonStyle = BarButtonStyle.Check;
            this.printPreviewBarItem4.Caption = "&Print...";
            this.printPreviewBarItem4.Enabled = false;
            this.printPreviewBarItem4.Hint = "Print";
            this.printPreviewBarItem4.Id = 6;
            this.printPreviewBarItem4.ImageIndex = 0;
            this.printPreviewBarItem4.Name = "printPreviewBarItem4";
            this.printPreviewBarItem6.ButtonStyle = BarButtonStyle.Check;
            this.printPreviewBarItem6.Caption = "Page Set&up...";
            this.printPreviewBarItem6.Command = PrintingSystemCommand.PageSetup;
            this.printPreviewBarItem6.Enabled = false;
            this.printPreviewBarItem6.Hint = "Page Setup";
            this.printPreviewBarItem6.Id = 8;
            this.printPreviewBarItem6.ImageIndex = 2;
            this.printPreviewBarItem6.Name = "printPreviewBarItem6";
            this.printPreviewBarItem7.Caption = "Header And Footer";
            this.printPreviewBarItem7.Command = PrintingSystemCommand.EditPageHF;
            this.printPreviewBarItem7.Enabled = false;
            this.printPreviewBarItem7.Hint = "Header And Footer";
            this.printPreviewBarItem7.Id = 9;
            this.printPreviewBarItem7.ImageIndex = 15;
            this.printPreviewBarItem7.Name = "printPreviewBarItem7";
            this.printPreviewBarItem8.ButtonStyle = BarButtonStyle.Check;
            this.printPreviewBarItem8.Caption = "Hand Tool";
            this.printPreviewBarItem8.Command = PrintingSystemCommand.HandTool;
            this.printPreviewBarItem8.Enabled = false;
            this.printPreviewBarItem8.Hint = "Hand Tool";
            this.printPreviewBarItem8.Id = 10;
            this.printPreviewBarItem8.ImageIndex = 16;
            this.printPreviewBarItem8.Name = "printPreviewBarItem8";
            this.printPreviewBarItem9.ButtonStyle = BarButtonStyle.Check;
            this.printPreviewBarItem9.Caption = "Magnifier";
            this.printPreviewBarItem9.Command = PrintingSystemCommand.Magnifier;
            this.printPreviewBarItem9.Enabled = false;
            this.printPreviewBarItem9.Hint = "Magnifier";
            this.printPreviewBarItem9.Id = 11;
            this.printPreviewBarItem9.ImageIndex = 3;
            this.printPreviewBarItem9.Name = "printPreviewBarItem9";
            this.printPreviewBarItem10.Caption = "Zoom Out";
            this.printPreviewBarItem10.Command = PrintingSystemCommand.ZoomOut;
            this.printPreviewBarItem10.Enabled = false;
            this.printPreviewBarItem10.Hint = "Zoom Out";
            this.printPreviewBarItem10.Id = 12;
            this.printPreviewBarItem10.ImageIndex = 5;
            this.printPreviewBarItem10.Name = "printPreviewBarItem10";
            this.zoomBarEditItem1.Caption = "Zoom";
            this.zoomBarEditItem1.Edit = this.printPreviewRepositoryItemComboBox1;
            this.zoomBarEditItem1.EditValue = "100%";
            this.zoomBarEditItem1.Enabled = false;
            this.zoomBarEditItem1.Hint = "Zoom";
            this.zoomBarEditItem1.Id = 13;
            this.zoomBarEditItem1.Name = "zoomBarEditItem1";
            this.zoomBarEditItem1.Width = 70;
            this.printPreviewRepositoryItemComboBox1.AutoComplete = false;
            this.printPreviewRepositoryItemComboBox1.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.printPreviewRepositoryItemComboBox1.DropDownRows = 11;
            this.printPreviewRepositoryItemComboBox1.Name = "printPreviewRepositoryItemComboBox1";
            this.printPreviewBarItem11.Caption = "Zoom In";
            this.printPreviewBarItem11.Command = PrintingSystemCommand.ZoomIn;
            this.printPreviewBarItem11.Enabled = false;
            this.printPreviewBarItem11.Hint = "Zoom In";
            this.printPreviewBarItem11.Id = 14;
            this.printPreviewBarItem11.ImageIndex = 4;
            this.printPreviewBarItem11.Name = "printPreviewBarItem11";
            this.printPreviewBarItem12.Caption = "First Page";
            this.printPreviewBarItem12.Command = PrintingSystemCommand.ShowFirstPage;
            this.printPreviewBarItem12.Enabled = false;
            this.printPreviewBarItem12.Hint = "First Page";
            this.printPreviewBarItem12.Id = 15;
            this.printPreviewBarItem12.ImageIndex = 7;
            this.printPreviewBarItem12.Name = "printPreviewBarItem12";
            this.printPreviewBarItem13.Caption = "Previous Page";
            this.printPreviewBarItem13.Command = PrintingSystemCommand.ShowPrevPage;
            this.printPreviewBarItem13.Enabled = false;
            this.printPreviewBarItem13.Hint = "Previous Page";
            this.printPreviewBarItem13.Id = 16;
            this.printPreviewBarItem13.ImageIndex = 8;
            this.printPreviewBarItem13.Name = "printPreviewBarItem13";
            this.printPreviewBarItem14.Caption = "Next Page";
            this.printPreviewBarItem14.Command = PrintingSystemCommand.ShowNextPage;
            this.printPreviewBarItem14.Enabled = false;
            this.printPreviewBarItem14.Hint = "Next Page";
            this.printPreviewBarItem14.Id = 17;
            this.printPreviewBarItem14.ImageIndex = 9;
            this.printPreviewBarItem14.Name = "printPreviewBarItem14";
            this.printPreviewBarItem15.Caption = "Last Page";
            this.printPreviewBarItem15.Command = PrintingSystemCommand.ShowLastPage;
            this.printPreviewBarItem15.Enabled = false;
            this.printPreviewBarItem15.Hint = "Last Page";
            this.printPreviewBarItem15.Id = 18;
            this.printPreviewBarItem15.ImageIndex = 10;
            this.printPreviewBarItem15.Name = "printPreviewBarItem15";
            this.printPreviewBarItem16.ActAsDropDown = true;
            this.printPreviewBarItem16.ButtonStyle = BarButtonStyle.DropDown;
            this.printPreviewBarItem16.Caption = "Multiple Pages";
            this.printPreviewBarItem16.Command = PrintingSystemCommand.MultiplePages;
            this.printPreviewBarItem16.Enabled = false;
            this.printPreviewBarItem16.Hint = "Multiple Pages";
            this.printPreviewBarItem16.Id = 19;
            this.printPreviewBarItem16.ImageIndex = 11;
            this.printPreviewBarItem16.Name = "printPreviewBarItem16";
            this.printPreviewBarItem17.ActAsDropDown = true;
            this.printPreviewBarItem17.ButtonStyle = BarButtonStyle.DropDown;
            this.printPreviewBarItem17.Caption = "&Color...";
            this.printPreviewBarItem17.Command = PrintingSystemCommand.FillBackground;
            this.printPreviewBarItem17.Enabled = false;
            this.printPreviewBarItem17.Hint = "Background";
            this.printPreviewBarItem17.Id = 20;
            this.printPreviewBarItem17.ImageIndex = 12;
            this.printPreviewBarItem17.Name = "printPreviewBarItem17";
            this.printPreviewBarItem18.Caption = "&Watermark...";
            this.printPreviewBarItem18.Command = PrintingSystemCommand.Watermark;
            this.printPreviewBarItem18.Enabled = false;
            this.printPreviewBarItem18.Hint = "Watermark";
            this.printPreviewBarItem18.Id = 21;
            this.printPreviewBarItem18.ImageIndex = 21;
            this.printPreviewBarItem18.Name = "printPreviewBarItem18";
            this.printPreviewBarItem19.ButtonStyle = BarButtonStyle.DropDown;
            this.printPreviewBarItem19.Caption = "Export Document...";
            this.printPreviewBarItem19.Command = PrintingSystemCommand.ExportFile;
            this.printPreviewBarItem19.Enabled = false;
            this.printPreviewBarItem19.Hint = "Export Document...";
            this.printPreviewBarItem19.Id = 22;
            this.printPreviewBarItem19.ImageIndex = 18;
            this.printPreviewBarItem19.Name = "printPreviewBarItem19";
            this.printPreviewBarItem20.ButtonStyle = BarButtonStyle.DropDown;
            this.printPreviewBarItem20.Caption = "Send E-mail...";
            this.printPreviewBarItem20.Command = PrintingSystemCommand.SendFile;
            this.printPreviewBarItem20.Enabled = false;
            this.printPreviewBarItem20.Hint = "Send E-mail...";
            this.printPreviewBarItem20.Id = 23;
            this.printPreviewBarItem20.ImageIndex = 17;
            this.printPreviewBarItem20.Name = "printPreviewBarItem20";
            this.printPreviewBarItem21.Caption = "E&xit";
            this.printPreviewBarItem21.Command = PrintingSystemCommand.ClosePreview;
            this.printPreviewBarItem21.Enabled = false;
            this.printPreviewBarItem21.Hint = "Close Preview";
            this.printPreviewBarItem21.Id = 24;
            this.printPreviewBarItem21.ImageIndex = 13;
            this.printPreviewBarItem21.Name = "printPreviewBarItem21";
            this.previewBar2.BarName = "Status Bar";
            this.previewBar2.CanDockStyle = BarCanDockStyle.Bottom;
            this.previewBar2.DockCol = 0;
            this.previewBar2.DockRow = 0;
            this.previewBar2.DockStyle = BarDockStyle.Bottom;
            previewBar2.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.printPreviewStaticItem1), new LinkPersistInfo(this.printPreviewStaticItem2), new LinkPersistInfo(this.printPreviewStaticItem3) });
            this.previewBar2.OptionsBar.AllowQuickCustomization = false;
            this.previewBar2.OptionsBar.DrawDragBorder = false;
            this.previewBar2.OptionsBar.UseWholeRow = true;
            this.previewBar2.Text = "Status Bar";
            this.printPreviewStaticItem1.AutoSize = BarStaticItemSize.Spring;
            this.printPreviewStaticItem1.Caption = "Current Page No: none";
            this.printPreviewStaticItem1.Id = 0;
            this.printPreviewStaticItem1.LeftIndent = 1;
            this.printPreviewStaticItem1.Name = "printPreviewStaticItem1";
            this.printPreviewStaticItem1.RightIndent = 1;
            this.printPreviewStaticItem1.TextAlignment = StringAlignment.Near;
            this.printPreviewStaticItem1.Type = "CurrentPageNo";
            this.printPreviewStaticItem1.Width = 200;
            this.printPreviewStaticItem2.AutoSize = BarStaticItemSize.Spring;
            this.printPreviewStaticItem2.Caption = "Total Page No: 0";
            this.printPreviewStaticItem2.Id = 1;
            this.printPreviewStaticItem2.LeftIndent = 1;
            this.printPreviewStaticItem2.Name = "printPreviewStaticItem2";
            this.printPreviewStaticItem2.RightIndent = 1;
            this.printPreviewStaticItem2.TextAlignment = StringAlignment.Near;
            this.printPreviewStaticItem2.Type = "TotalPageNo";
            this.printPreviewStaticItem2.Width = 200;
            this.printPreviewStaticItem3.AutoSize = BarStaticItemSize.Spring;
            this.printPreviewStaticItem3.Caption = "100%";
            this.printPreviewStaticItem3.Id = 2;
            this.printPreviewStaticItem3.LeftIndent = 1;
            this.printPreviewStaticItem3.Name = "printPreviewStaticItem3";
            this.printPreviewStaticItem3.RightIndent = 1;
            this.printPreviewStaticItem3.TextAlignment = StringAlignment.Near;
            this.printPreviewStaticItem3.Type = "ZoomFactor";
            this.printPreviewStaticItem3.Width = 200;
            this.previewBar3.BarName = "Main Menu";
            this.previewBar3.DockCol = 0;
            this.previewBar3.DockRow = 1;
            this.previewBar3.DockStyle = BarDockStyle.Top;
            previewBar3.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.barSubItem1), new LinkPersistInfo(this.barSubItem2), new LinkPersistInfo(this.barSubItem3) });
            this.previewBar3.OptionsBar.MultiLine = true;
            this.previewBar3.OptionsBar.UseWholeRow = true;
            this.previewBar3.Text = "Main Menu";
            this.previewBar3.Visible = false;
            this.barSubItem1.Caption = "&File";
            this.barSubItem1.Id = 25;
            barSubItem1.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.printPreviewBarItem6), new LinkPersistInfo(this.printPreviewBarItem4), new LinkPersistInfo(this.printPreviewBarItem19), new LinkPersistInfo(this.printPreviewBarItem20), new LinkPersistInfo(this.printPreviewBarItem21) });
            this.barSubItem1.Name = "barSubItem1";
            this.barSubItem2.Caption = "&View";
            this.barSubItem2.Id = 26;
            barSubItem2.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.barSubItem4, true), new LinkPersistInfo(this.barToolbarsListItem1, true) });
            this.barSubItem2.Name = "barSubItem2";
            this.barSubItem4.Caption = "&Page Layout";
            this.barSubItem4.Id = 28;
            barSubItem4.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.printPreviewBarItem22), new LinkPersistInfo(this.printPreviewBarItem23) });
            this.barSubItem4.Name = "barSubItem4";
            this.printPreviewBarItem22.ButtonStyle = BarButtonStyle.Check;
            this.printPreviewBarItem22.Caption = "&Facing";
            this.printPreviewBarItem22.Command = PrintingSystemCommand.PageLayoutFacing;
            this.printPreviewBarItem22.Enabled = false;
            this.printPreviewBarItem22.GroupIndex = 100;
            this.printPreviewBarItem22.Id = 29;
            this.printPreviewBarItem22.Name = "printPreviewBarItem22";
            this.printPreviewBarItem23.ButtonStyle = BarButtonStyle.Check;
            this.printPreviewBarItem23.Caption = "&Continuous";
            this.printPreviewBarItem23.Command = PrintingSystemCommand.PageLayoutContinuous;
            this.printPreviewBarItem23.Enabled = false;
            this.printPreviewBarItem23.GroupIndex = 100;
            this.printPreviewBarItem23.Id = 30;
            this.printPreviewBarItem23.Name = "printPreviewBarItem23";
            this.barToolbarsListItem1.Caption = "Bars";
            this.barToolbarsListItem1.Id = 31;
            this.barToolbarsListItem1.Name = "barToolbarsListItem1";
            this.barSubItem3.Caption = "&Background";
            this.barSubItem3.Id = 27;
            barSubItem3.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.printPreviewBarItem17), new LinkPersistInfo(this.printPreviewBarItem18) });
            this.barSubItem3.Name = "barSubItem3";
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = DockStyle.Top;
            this.barDockControlTop.Location = new Point(0, 0);
            this.barDockControlTop.Size = new Size(700, 53);
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = DockStyle.Bottom;
            this.barDockControlBottom.Location = new Point(0, 371);
            this.barDockControlBottom.Size = new Size(700, 25);
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = DockStyle.Left;
            this.barDockControlLeft.Location = new Point(0, 53);
            this.barDockControlLeft.Size = new Size(0, 318);
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = DockStyle.Right;
            this.barDockControlRight.Location = new Point(700, 53);
            this.barDockControlRight.Size = new Size(0, 318);
            this.printPreviewBarCheckItem1.Caption = "PDF Document";
            this.printPreviewBarCheckItem1.Checked = true;
            this.printPreviewBarCheckItem1.Command = PrintingSystemCommand.ExportPdf;
            this.printPreviewBarCheckItem1.Enabled = false;
            this.printPreviewBarCheckItem1.GroupIndex = 2;
            this.printPreviewBarCheckItem1.Hint = "PDF Document";
            this.printPreviewBarCheckItem1.Id = 32;
            this.printPreviewBarCheckItem1.Name = "printPreviewBarCheckItem1";
            this.printPreviewBarCheckItem2.Caption = "HTML Document";
            this.printPreviewBarCheckItem2.Command = PrintingSystemCommand.ExportHtm;
            this.printPreviewBarCheckItem2.Enabled = false;
            this.printPreviewBarCheckItem2.GroupIndex = 2;
            this.printPreviewBarCheckItem2.Hint = "HTML Document";
            this.printPreviewBarCheckItem2.Id = 33;
            this.printPreviewBarCheckItem2.Name = "printPreviewBarCheckItem2";
            this.printPreviewBarCheckItem3.Caption = "Text Document";
            this.printPreviewBarCheckItem3.Command = PrintingSystemCommand.ExportTxt;
            this.printPreviewBarCheckItem3.Enabled = false;
            this.printPreviewBarCheckItem3.GroupIndex = 2;
            this.printPreviewBarCheckItem3.Hint = "Text Document";
            this.printPreviewBarCheckItem3.Id = 34;
            this.printPreviewBarCheckItem3.Name = "printPreviewBarCheckItem3";
            this.printPreviewBarCheckItem4.Caption = "CSV Document";
            this.printPreviewBarCheckItem4.Command = PrintingSystemCommand.ExportCsv;
            this.printPreviewBarCheckItem4.Enabled = false;
            this.printPreviewBarCheckItem4.GroupIndex = 2;
            this.printPreviewBarCheckItem4.Hint = "CSV Document";
            this.printPreviewBarCheckItem4.Id = 35;
            this.printPreviewBarCheckItem4.Name = "printPreviewBarCheckItem4";
            this.printPreviewBarCheckItem5.Caption = "MHT Document";
            this.printPreviewBarCheckItem5.Command = PrintingSystemCommand.ExportMht;
            this.printPreviewBarCheckItem5.Enabled = false;
            this.printPreviewBarCheckItem5.GroupIndex = 2;
            this.printPreviewBarCheckItem5.Hint = "MHT Document";
            this.printPreviewBarCheckItem5.Id = 36;
            this.printPreviewBarCheckItem5.Name = "printPreviewBarCheckItem5";
            this.printPreviewBarCheckItem6.Caption = "Excel Document";
            this.printPreviewBarCheckItem6.Command = PrintingSystemCommand.ExportXls;
            this.printPreviewBarCheckItem6.Enabled = false;
            this.printPreviewBarCheckItem6.GroupIndex = 2;
            this.printPreviewBarCheckItem6.Hint = "Excel Document";
            this.printPreviewBarCheckItem6.Id = 37;
            this.printPreviewBarCheckItem6.Name = "printPreviewBarCheckItem6";
            this.printPreviewBarCheckItem7.Caption = "Rich Text Document";
            this.printPreviewBarCheckItem7.Command = PrintingSystemCommand.ExportRtf;
            this.printPreviewBarCheckItem7.Enabled = false;
            this.printPreviewBarCheckItem7.GroupIndex = 2;
            this.printPreviewBarCheckItem7.Hint = "Rich Text Document";
            this.printPreviewBarCheckItem7.Id = 38;
            this.printPreviewBarCheckItem7.Name = "printPreviewBarCheckItem7";
            this.printPreviewBarCheckItem8.Caption = "Graphic Document";
            this.printPreviewBarCheckItem8.Command = PrintingSystemCommand.ExportGraphic;
            this.printPreviewBarCheckItem8.Enabled = false;
            this.printPreviewBarCheckItem8.GroupIndex = 2;
            this.printPreviewBarCheckItem8.Hint = "Graphic Document";
            this.printPreviewBarCheckItem8.Id = 39;
            this.printPreviewBarCheckItem8.Name = "printPreviewBarCheckItem8";
            this.printPreviewBarCheckItem9.Caption = "PDF Document";
            this.printPreviewBarCheckItem9.Checked = true;
            this.printPreviewBarCheckItem9.Command = PrintingSystemCommand.SendPdf;
            this.printPreviewBarCheckItem9.Enabled = false;
            this.printPreviewBarCheckItem9.GroupIndex = 1;
            this.printPreviewBarCheckItem9.Hint = "PDF Document";
            this.printPreviewBarCheckItem9.Id = 40;
            this.printPreviewBarCheckItem9.Name = "printPreviewBarCheckItem9";
            this.printPreviewBarCheckItem10.Caption = "Text Document";
            this.printPreviewBarCheckItem10.Command = PrintingSystemCommand.SendTxt;
            this.printPreviewBarCheckItem10.Enabled = false;
            this.printPreviewBarCheckItem10.GroupIndex = 1;
            this.printPreviewBarCheckItem10.Hint = "Text Document";
            this.printPreviewBarCheckItem10.Id = 41;
            this.printPreviewBarCheckItem10.Name = "printPreviewBarCheckItem10";
            this.printPreviewBarCheckItem11.Caption = "CSV Document";
            this.printPreviewBarCheckItem11.Command = PrintingSystemCommand.SendCsv;
            this.printPreviewBarCheckItem11.Enabled = false;
            this.printPreviewBarCheckItem11.GroupIndex = 1;
            this.printPreviewBarCheckItem11.Hint = "CSV Document";
            this.printPreviewBarCheckItem11.Id = 42;
            this.printPreviewBarCheckItem11.Name = "printPreviewBarCheckItem11";
            this.printPreviewBarCheckItem12.Caption = "MHT Document";
            this.printPreviewBarCheckItem12.Command = PrintingSystemCommand.SendMht;
            this.printPreviewBarCheckItem12.Enabled = false;
            this.printPreviewBarCheckItem12.GroupIndex = 1;
            this.printPreviewBarCheckItem12.Hint = "MHT Document";
            this.printPreviewBarCheckItem12.Id = 43;
            this.printPreviewBarCheckItem12.Name = "printPreviewBarCheckItem12";
            this.printPreviewBarCheckItem13.Caption = "Excel Document";
            this.printPreviewBarCheckItem13.Command = PrintingSystemCommand.SendXls;
            this.printPreviewBarCheckItem13.Enabled = false;
            this.printPreviewBarCheckItem13.GroupIndex = 1;
            this.printPreviewBarCheckItem13.Hint = "Excel Document";
            this.printPreviewBarCheckItem13.Id = 44;
            this.printPreviewBarCheckItem13.Name = "printPreviewBarCheckItem13";
            this.printPreviewBarCheckItem14.Caption = "Rich Text Document";
            this.printPreviewBarCheckItem14.Command = PrintingSystemCommand.SendRtf;
            this.printPreviewBarCheckItem14.Enabled = false;
            this.printPreviewBarCheckItem14.GroupIndex = 1;
            this.printPreviewBarCheckItem14.Hint = "Rich Text Document";
            this.printPreviewBarCheckItem14.Id = 45;
            this.printPreviewBarCheckItem14.Name = "printPreviewBarCheckItem14";
            this.printPreviewBarCheckItem15.Caption = "Graphic Document";
            this.printPreviewBarCheckItem15.Command = PrintingSystemCommand.SendGraphic;
            this.printPreviewBarCheckItem15.Enabled = false;
            this.printPreviewBarCheckItem15.GroupIndex = 1;
            this.printPreviewBarCheckItem15.Hint = "Graphic Document";
            this.printPreviewBarCheckItem15.Id = 46;
            this.printPreviewBarCheckItem15.Name = "printPreviewBarCheckItem15";
            this.printControl1.BackColor = Color.Empty;
            this.printControl1.Dock = DockStyle.Fill;
            this.printControl1.ForeColor = Color.Empty;
            this.printControl1.IsMetric = true;
            this.printControl1.Location = new Point(0, 53);
            this.printControl1.Name = "printControl1";
            this.printControl1.Size = new Size(700, 318);
            this.printControl1.TabIndex = 9;
            this.printControl1.TooltipFont = new Font("Tahoma", 8.25f);
            base.Controls.Add(this.printControl1);
            base.Controls.Add(this.panelControl1);
            base.Controls.Add(this.barDockControlLeft);
            base.Controls.Add(this.barDockControlRight);
            base.Controls.Add(this.barDockControlBottom);
            base.Controls.Add(this.barDockControlTop);
            base.Name = "ControlXtraReportViewer";
            base.Size = new Size(700, 396);
            ((ISupportInitialize)this.panelControl1).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((ISupportInitialize)this.printBarManager1).EndInit();
            ((ISupportInitialize)this.printPreviewRepositoryItemComboBox1).EndInit();
            base.ResumeLayout(false);
        }

        #endregion
    }
}
