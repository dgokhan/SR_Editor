using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraReports.UserDesigner;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SR_Editor.Core.Controls
{
    partial class XtraDesignForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private XRDesignBarManager xrDesignBarManager1;

        private RecentlyUsedItemsComboBox ricbFontName;

        private RepositoryItemComboBox ricbFontSize;

        private DesignBar designBar1;

        private DesignBar designBar2;

        private DesignBar designBar3;

        private DesignBar designBar4;

        private DesignBar designBar5;

        private BarEditItem barEditItem1;

        private BarEditItem barEditItem2;

        private CommandBarItem commandBarItem1;

        private CommandBarItem commandBarItem2;

        private CommandBarItem commandBarItem3;

        private CommandColorBarItem commandColorBarItem1;

        private CommandColorBarItem commandColorBarItem2;

        private CommandBarItem commandBarItem4;

        private CommandBarItem commandBarItem5;

        private CommandBarItem commandBarItem6;

        private CommandBarItem commandBarItem7;

        private CommandBarItem commandBarItem8;

        private CommandBarItem commandBarItem9;

        private CommandBarItem commandBarItem10;

        private CommandBarItem commandBarItem11;

        private CommandBarItem commandBarItem12;

        private CommandBarItem commandBarItem13;

        private CommandBarItem commandBarItem14;

        private CommandBarItem commandBarItem15;

        private CommandBarItem commandBarItem16;

        private CommandBarItem commandBarItem17;

        private CommandBarItem commandBarItem18;

        private CommandBarItem commandBarItem19;

        private CommandBarItem commandBarItem20;

        private CommandBarItem commandBarItem21;

        private CommandBarItem commandBarItem22;

        private CommandBarItem commandBarItem23;

        private CommandBarItem commandBarItem24;

        private CommandBarItem commandBarItem25;

        private CommandBarItem commandBarItem26;

        private CommandBarItem commandBarItem27;

        private CommandBarItem commandBarItem28;

        private CommandBarItem commandBarItem29;

        private CommandBarItem commandBarItem30;

        private CommandBarItem commandBarItem34;

        private CommandBarItem commandBarItem35;

        private CommandBarItem commandBarItem36;

        private CommandBarItem commandBarItem37;

        private CommandBarItem commandBarItem38;

        private BarStaticItem barStaticItem1;

        private BarSubItem barSubItem1;

        private BarSubItem barSubItem2;

        private BarSubItem barSubItem3;

        private BarReportTabButtonsListItem barReportTabButtonsListItem1;

        private BarSubItem barSubItem4;

        private XRBarToolbarsListItem xrBarToolbarsListItem1;

        private BarSubItem barSubItem5;

        private BarDockPanelsListItem barDockPanelsListItem1;

        private BarSubItem barSubItem6;

        private BarSubItem barSubItem7;

        private BarSubItem barSubItem8;

        private BarSubItem barSubItem9;

        private BarSubItem barSubItem10;

        private BarSubItem barSubItem11;

        private BarSubItem barSubItem12;

        private BarSubItem barSubItem13;

        private BarSubItem barSubItem14;

        private CommandBarItem commandBarItem41;

        private CommandBarItem commandBarItem42;

        private CommandBarItem commandBarItem43;

        private XRDesignDockManager xrDesignDockManager1;

        private FieldListDockPanel fieldListDockPanel1;

        private DesignControlContainer fieldListDockPanel1_Container;

        private PropertyGridDockPanel propertyGridDockPanel1;

        private DesignControlContainer propertyGridDockPanel1_Container;

        private ReportExplorerDockPanel reportExplorerDockPanel1;

        private DesignControlContainer reportExplorerDockPanel1_Container;

        private ToolBoxDockPanel toolBoxDockPanel1;

        private DesignControlContainer toolBoxDockPanel1_Container;

        private DesignDockPanel panelContainer2;

        private CommandBarItem commandBarItem44;

        private BarDockControl barDockControlTop;

        private BarDockControl barDockControlBottom;

        private BarDockControl barDockControlLeft;

        private BarDockControl barDockControlRight;

        private BarStaticItem barStaticItemSablonAdi;

        private BarEditItem barEditItemSablonAdi;

        private RepositoryItemTextEdit repositoryItemTextEdit3;

        private BarButtonItem barButtonItemKaydet;

        private RepositoryItemTextEdit repositoryItemTextEdit1;

        private RepositoryItemTextEdit repositoryItemTextEdit2;

        private BarStaticItem barStaticItemOtomatikYazdirText;

        private BarEditItem barEditItemCheckOtomatikYazdir;

        private RepositoryItemCheckEdit repositoryItemCheckEdit1;

        private BarStaticItem barStaticItemYazdirilincaKapansin;

        private BarEditItem barEditItemCheckYazdirilincaKapat;

        private RepositoryItemCheckEdit repositoryItemCheckEdit2;

        private BarCheckItem barCheckItemCheckYazdirincaKapat;

        private Container components = null;


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

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(XtraDesignForm));
            this.xrDesignBarManager1 = new XRDesignBarManager();
            this.designBar1 = new DesignBar();
            this.barSubItem1 = new BarSubItem();
            this.commandBarItem41 = new CommandBarItem();
            this.barSubItem2 = new BarSubItem();
            this.commandBarItem37 = new CommandBarItem();
            this.commandBarItem38 = new CommandBarItem();
            this.commandBarItem34 = new CommandBarItem();
            this.commandBarItem35 = new CommandBarItem();
            this.commandBarItem36 = new CommandBarItem();
            this.commandBarItem42 = new CommandBarItem();
            this.commandBarItem43 = new CommandBarItem();
            this.barSubItem3 = new BarSubItem();
            this.barReportTabButtonsListItem1 = new BarReportTabButtonsListItem();
            this.barSubItem4 = new BarSubItem();
            this.xrBarToolbarsListItem1 = new XRBarToolbarsListItem();
            this.barSubItem5 = new BarSubItem();
            this.barDockPanelsListItem1 = new BarDockPanelsListItem();
            this.barSubItem6 = new BarSubItem();
            this.commandColorBarItem1 = new CommandColorBarItem();
            this.commandColorBarItem2 = new CommandColorBarItem();
            this.barSubItem7 = new BarSubItem();
            this.commandBarItem1 = new CommandBarItem();
            this.commandBarItem2 = new CommandBarItem();
            this.commandBarItem3 = new CommandBarItem();
            this.barSubItem8 = new BarSubItem();
            this.commandBarItem4 = new CommandBarItem();
            this.commandBarItem5 = new CommandBarItem();
            this.commandBarItem6 = new CommandBarItem();
            this.commandBarItem7 = new CommandBarItem();
            this.barSubItem9 = new BarSubItem();
            this.commandBarItem9 = new CommandBarItem();
            this.commandBarItem10 = new CommandBarItem();
            this.commandBarItem11 = new CommandBarItem();
            this.commandBarItem12 = new CommandBarItem();
            this.commandBarItem13 = new CommandBarItem();
            this.commandBarItem14 = new CommandBarItem();
            this.commandBarItem8 = new CommandBarItem();
            this.barSubItem10 = new BarSubItem();
            this.commandBarItem15 = new CommandBarItem();
            this.commandBarItem16 = new CommandBarItem();
            this.commandBarItem17 = new CommandBarItem();
            this.commandBarItem18 = new CommandBarItem();
            this.barSubItem11 = new BarSubItem();
            this.commandBarItem19 = new CommandBarItem();
            this.commandBarItem20 = new CommandBarItem();
            this.commandBarItem21 = new CommandBarItem();
            this.commandBarItem22 = new CommandBarItem();
            this.barSubItem12 = new BarSubItem();
            this.commandBarItem23 = new CommandBarItem();
            this.commandBarItem24 = new CommandBarItem();
            this.commandBarItem25 = new CommandBarItem();
            this.commandBarItem26 = new CommandBarItem();
            this.barSubItem13 = new BarSubItem();
            this.commandBarItem27 = new CommandBarItem();
            this.commandBarItem28 = new CommandBarItem();
            this.barSubItem14 = new BarSubItem();
            this.commandBarItem29 = new CommandBarItem();
            this.commandBarItem30 = new CommandBarItem();
            this.barStaticItemSablonAdi = new BarStaticItem();
            this.barEditItemSablonAdi = new BarEditItem();
            this.repositoryItemTextEdit3 = new RepositoryItemTextEdit();
            this.barStaticItemOtomatikYazdirText = new BarStaticItem();
            this.barEditItemCheckOtomatikYazdir = new BarEditItem();
            this.repositoryItemCheckEdit1 = new RepositoryItemCheckEdit();
            this.barStaticItemYazdirilincaKapansin = new BarStaticItem();
            this.barEditItemCheckYazdirilincaKapat = new BarEditItem();
            this.repositoryItemCheckEdit2 = new RepositoryItemCheckEdit();
            this.barButtonItemKaydet = new BarButtonItem();
            this.designBar2 = new DesignBar();
            this.designBar3 = new DesignBar();
            this.barEditItem2 = new BarEditItem();
            this.ricbFontSize = new RepositoryItemComboBox();
            this.barEditItem1 = new BarEditItem();
            this.ricbFontName = new RecentlyUsedItemsComboBox();
            this.designBar4 = new DesignBar();
            this.designBar5 = new DesignBar();
            this.barStaticItem1 = new BarStaticItem();
            this.barDockControlTop = new BarDockControl();
            this.barDockControlBottom = new BarDockControl();
            this.barDockControlLeft = new BarDockControl();
            this.barDockControlRight = new BarDockControl();
            this.xrDesignDockManager1 = new XRDesignDockManager();
            this.panelContainer2 = new DesignDockPanel();
            this.toolBoxDockPanel1 = new ToolBoxDockPanel();
            this.toolBoxDockPanel1_Container = new DesignControlContainer();
            this.reportExplorerDockPanel1 = new ReportExplorerDockPanel();
            this.reportExplorerDockPanel1_Container = new DesignControlContainer();
            this.fieldListDockPanel1 = new FieldListDockPanel();
            this.fieldListDockPanel1_Container = new DesignControlContainer();
            this.propertyGridDockPanel1 = new PropertyGridDockPanel();
            this.propertyGridDockPanel1_Container = new DesignControlContainer();
            this.barCheckItemCheckYazdirincaKapat = new BarCheckItem();
            this.repositoryItemTextEdit1 = new RepositoryItemTextEdit();
            this.repositoryItemTextEdit2 = new RepositoryItemTextEdit();
            this.commandBarItem44 = new CommandBarItem();
            ((ISupportInitialize)this.xrDesignPanel).BeginInit();
            ((ISupportInitialize)this.xrDesignBarManager1).BeginInit();
            ((ISupportInitialize)this.repositoryItemTextEdit3).BeginInit();
            ((ISupportInitialize)this.repositoryItemCheckEdit1).BeginInit();
            ((ISupportInitialize)this.repositoryItemCheckEdit2).BeginInit();
            ((ISupportInitialize)this.ricbFontSize).BeginInit();
            ((ISupportInitialize)this.ricbFontName).BeginInit();
            ((ISupportInitialize)this.xrDesignDockManager1).BeginInit();
            this.panelContainer2.SuspendLayout();
            this.toolBoxDockPanel1.SuspendLayout();
            this.reportExplorerDockPanel1.SuspendLayout();
            this.fieldListDockPanel1.SuspendLayout();
            this.propertyGridDockPanel1.SuspendLayout();
            ((ISupportInitialize)this.repositoryItemTextEdit1).BeginInit();
            ((ISupportInitialize)this.repositoryItemTextEdit2).BeginInit();
            base.SuspendLayout();
            this.xrDesignPanel.Location = new Point(0, 84);
            this.xrDesignPanel.Size = new Size(844, 491);
            this.xrDesignPanel.ReportStateChanged += new ReportStateEventHandler(this.xrDesignPanel_ReportStateChanged);
            xrDesignBarManager1.Bars.AddRange(new Bar[] { this.designBar1, this.designBar2, this.designBar3, this.designBar4, this.designBar5 });
            this.xrDesignBarManager1.DockControls.Add(this.barDockControlTop);
            this.xrDesignBarManager1.DockControls.Add(this.barDockControlBottom);
            this.xrDesignBarManager1.DockControls.Add(this.barDockControlLeft);
            this.xrDesignBarManager1.DockControls.Add(this.barDockControlRight);
            this.xrDesignBarManager1.DockManager = this.xrDesignDockManager1;
            this.xrDesignBarManager1.FontNameBox = this.ricbFontName;
            this.xrDesignBarManager1.FontNameEdit = this.barEditItem1;
            this.xrDesignBarManager1.FontSizeBox = this.ricbFontSize;
            this.xrDesignBarManager1.FontSizeEdit = this.barEditItem2;
            this.xrDesignBarManager1.Form = this;
            this.xrDesignBarManager1.FormattingToolbar = this.designBar3;
            this.xrDesignBarManager1.HintStaticItem = this.barStaticItem1;
            this.xrDesignBarManager1.ImageStream = xrDesignBarManager1.ImageStream;// (ImageCollectionStreamer)componentResourceManager.GetObject("");
            xrDesignBarManager1.Items.AddRange(new BarItem[] { this.barEditItem1, this.barEditItem2, this.commandBarItem1, this.commandBarItem2, this.commandBarItem3, this.commandColorBarItem1, this.commandColorBarItem2, this.commandBarItem4, this.commandBarItem5, this.commandBarItem6, this.commandBarItem7, this.commandBarItem8, this.commandBarItem9, this.commandBarItem10, this.commandBarItem11, this.commandBarItem12, this.commandBarItem13, this.commandBarItem14, this.commandBarItem15, this.commandBarItem16, this.commandBarItem17, this.commandBarItem18, this.commandBarItem19, this.commandBarItem20, this.commandBarItem21, this.commandBarItem22, this.commandBarItem23, this.commandBarItem24, this.commandBarItem25, this.commandBarItem26, this.commandBarItem27, this.commandBarItem28, this.commandBarItem29, this.commandBarItem30, this.commandBarItem34, this.commandBarItem35, this.commandBarItem36, this.commandBarItem37, this.commandBarItem38, this.barStaticItem1, this.barSubItem1, this.barSubItem2, this.barSubItem3, this.barReportTabButtonsListItem1, this.barSubItem4, this.xrBarToolbarsListItem1, this.barSubItem5, this.barDockPanelsListItem1, this.barSubItem6, this.barSubItem7, this.barSubItem8, this.barSubItem9, this.barSubItem10, this.barSubItem11, this.barSubItem12, this.barSubItem13, this.barSubItem14, this.commandBarItem41, this.commandBarItem42, this.commandBarItem43, this.barStaticItemSablonAdi, this.barEditItemSablonAdi, this.barButtonItemKaydet, this.barStaticItemOtomatikYazdirText, this.barEditItemCheckOtomatikYazdir, this.barStaticItemYazdirilincaKapansin, this.barCheckItemCheckYazdirincaKapat, this.barEditItemCheckYazdirilincaKapat });
            this.xrDesignBarManager1.LayoutToolbar = this.designBar4;
            this.xrDesignBarManager1.MainMenu = this.designBar1;
            this.xrDesignBarManager1.MaxItemId = 77;
            xrDesignBarManager1.RepositoryItems.AddRange(new RepositoryItem[] { this.ricbFontName, this.ricbFontSize, this.repositoryItemTextEdit1, this.repositoryItemTextEdit2, this.repositoryItemTextEdit3, this.repositoryItemCheckEdit1, this.repositoryItemCheckEdit2 });
            this.xrDesignBarManager1.StatusBar = this.designBar5;
            this.xrDesignBarManager1.Toolbar = this.designBar2;
            this.xrDesignBarManager1.XRDesignPanel = base.DesignPanel;
            this.xrDesignBarManager1.ZoomItem = null;
            this.designBar1.BarName = "MainMenu";
            this.designBar1.CanDockStyle = BarCanDockStyle.Top;
            this.designBar1.DockCol = 0;
            this.designBar1.DockRow = 0;
            this.designBar1.DockStyle = BarDockStyle.Top;
            designBar1.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.barSubItem1), new LinkPersistInfo(this.barSubItem2), new LinkPersistInfo(this.barSubItem3), new LinkPersistInfo(this.barSubItem6), new LinkPersistInfo(this.barStaticItemSablonAdi), new LinkPersistInfo(this.barEditItemSablonAdi), new LinkPersistInfo(this.barStaticItemOtomatikYazdirText), new LinkPersistInfo(this.barEditItemCheckOtomatikYazdir), new LinkPersistInfo(this.barStaticItemYazdirilincaKapansin), new LinkPersistInfo(this.barEditItemCheckYazdirilincaKapat), new LinkPersistInfo(this.barButtonItemKaydet) });
            this.designBar1.OptionsBar.AllowQuickCustomization = false;
            this.designBar1.OptionsBar.DisableCustomization = true;
            this.designBar1.OptionsBar.MultiLine = true;
            this.designBar1.OptionsBar.UseWholeRow = true;
            this.designBar1.Text = "Main Menu";
            this.barSubItem1.Caption = "&File";
            this.barSubItem1.Id = 43;
            barSubItem1.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.commandBarItem41, true) });
            this.barSubItem1.Name = "barSubItem1";
            this.commandBarItem41.Caption = "Close";
            this.commandBarItem41.Command = ReportCommand.Exit;
            this.commandBarItem41.Hint = "Close the designer";
            this.commandBarItem41.Id = 62;
            this.commandBarItem41.Name = "commandBarItem41";
            this.barSubItem2.Caption = "&Edit";
            this.barSubItem2.Id = 44;
            barSubItem2.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.commandBarItem37, true), new LinkPersistInfo(this.commandBarItem38), new LinkPersistInfo(this.commandBarItem34, true), new LinkPersistInfo(this.commandBarItem35), new LinkPersistInfo(this.commandBarItem36), new LinkPersistInfo(this.commandBarItem42), new LinkPersistInfo(this.commandBarItem43, true) });
            this.barSubItem2.Name = "barSubItem2";
            this.commandBarItem37.Caption = "&Undo";
            this.commandBarItem37.Command = ReportCommand.Undo;
            this.commandBarItem37.Enabled = false;
            this.commandBarItem37.Hint = "Undo the last operation";
            this.commandBarItem37.Id = 40;
            this.commandBarItem37.ImageIndex = 15;
            this.commandBarItem37.ItemShortcut = new BarShortcut(Keys.RButton | Keys.Back | Keys.LineFeed | Keys.ShiftKey | Keys.Menu | Keys.FinalMode | Keys.B | Keys.H | Keys.J | Keys.P | Keys.R | Keys.X | Keys.Z | Keys.Control);
            this.commandBarItem37.Name = "commandBarItem37";
            this.commandBarItem38.Caption = "&Redo";
            this.commandBarItem38.Command = ReportCommand.Redo;
            this.commandBarItem38.Enabled = false;
            this.commandBarItem38.Hint = "Redo the last operation";
            this.commandBarItem38.Id = 41;
            this.commandBarItem38.ImageIndex = 16;
            this.commandBarItem38.ItemShortcut = new BarShortcut(Keys.LButton | Keys.Back | Keys.Tab | Keys.ShiftKey | Keys.ControlKey | Keys.FinalMode | Keys.HanjaMode | Keys.KanjiMode | Keys.A | Keys.H | Keys.I | Keys.P | Keys.Q | Keys.X | Keys.Y | Keys.Control);
            this.commandBarItem38.Name = "commandBarItem38";
            this.commandBarItem34.Caption = "Cu&t";
            this.commandBarItem34.Command = ReportCommand.Cut;
            this.commandBarItem34.Enabled = false;
            this.commandBarItem34.Hint = "Delete the control and copy it to the clipboard";
            this.commandBarItem34.Id = 37;
            this.commandBarItem34.ImageIndex = 12;
            this.commandBarItem34.ItemShortcut = new BarShortcut(Keys.Back | Keys.ShiftKey | Keys.FinalMode | Keys.H | Keys.P | Keys.X | Keys.Control);
            this.commandBarItem34.Name = "commandBarItem34";
            this.commandBarItem35.Caption = "&Copy";
            this.commandBarItem35.Command = ReportCommand.Copy;
            this.commandBarItem35.Enabled = false;
            this.commandBarItem35.Hint = "Copy the control to the clipboard";
            this.commandBarItem35.Id = 38;
            this.commandBarItem35.ImageIndex = 13;
            this.commandBarItem35.ItemShortcut = new BarShortcut(Keys.LButton | Keys.RButton | Keys.Cancel | Keys.A | Keys.B | Keys.C | Keys.Control);
            this.commandBarItem35.Name = "commandBarItem35";
            this.commandBarItem36.Caption = "&Paste";
            this.commandBarItem36.Command = ReportCommand.Paste;
            this.commandBarItem36.Enabled = false;
            this.commandBarItem36.Hint = "Add the control from the clipboard";
            this.commandBarItem36.Id = 39;
            this.commandBarItem36.ImageIndex = 14;
            this.commandBarItem36.ItemShortcut = new BarShortcut(Keys.RButton | Keys.MButton | Keys.XButton2 | Keys.ShiftKey | Keys.Menu | Keys.Capital | Keys.CapsLock | Keys.B | Keys.D | Keys.F | Keys.P | Keys.R | Keys.T | Keys.V | Keys.Control);
            this.commandBarItem36.Name = "commandBarItem36";
            this.commandBarItem42.Caption = "&Delete";
            this.commandBarItem42.Command = ReportCommand.Delete;
            this.commandBarItem42.Enabled = false;
            this.commandBarItem42.Hint = "Delete the control";
            this.commandBarItem42.Id = 63;
            this.commandBarItem42.Name = "commandBarItem42";
            this.commandBarItem43.Caption = "Select &All";
            this.commandBarItem43.Command = ReportCommand.SelectAll;
            this.commandBarItem43.Enabled = false;
            this.commandBarItem43.Hint = "Select all the controls in the document";
            this.commandBarItem43.Id = 64;
            this.commandBarItem43.ItemShortcut = new BarShortcut(Keys.LButton | Keys.A | Keys.Control);
            this.commandBarItem43.Name = "commandBarItem43";
            this.barSubItem3.Caption = "&View";
            this.barSubItem3.Id = 45;
            barSubItem3.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.barReportTabButtonsListItem1), new LinkPersistInfo(this.barSubItem4, true), new LinkPersistInfo(this.barSubItem5, true) });
            this.barSubItem3.Name = "barSubItem3";
            this.barReportTabButtonsListItem1.Caption = "Tab Buttons";
            this.barReportTabButtonsListItem1.Id = 46;
            this.barReportTabButtonsListItem1.Name = "barReportTabButtonsListItem1";
            this.barSubItem4.Caption = "&Toolbars";
            this.barSubItem4.Id = 47;
            barSubItem4.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.xrBarToolbarsListItem1) });
            this.barSubItem4.Name = "barSubItem4";
            this.xrBarToolbarsListItem1.Caption = "&Toolbars";
            this.xrBarToolbarsListItem1.Id = 48;
            this.xrBarToolbarsListItem1.Name = "xrBarToolbarsListItem1";
            this.barSubItem5.Caption = "&Windows";
            this.barSubItem5.Id = 49;
            barSubItem5.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.barDockPanelsListItem1) });
            this.barSubItem5.Name = "barSubItem5";
            this.barDockPanelsListItem1.Caption = "&Windows";
            this.barDockPanelsListItem1.Id = 50;
            this.barDockPanelsListItem1.Name = "barDockPanelsListItem1";
            this.barDockPanelsListItem1.ShowCustomizationItem = false;
            this.barDockPanelsListItem1.ShowDockPanels = true;
            this.barDockPanelsListItem1.ShowToolbars = false;
            this.barSubItem6.Caption = "Fo&rmat";
            this.barSubItem6.Id = 51;
            barSubItem6.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.commandColorBarItem1), new LinkPersistInfo(this.commandColorBarItem2), new LinkPersistInfo(this.barSubItem7, true), new LinkPersistInfo(this.barSubItem8), new LinkPersistInfo(this.barSubItem9, true), new LinkPersistInfo(this.barSubItem10), new LinkPersistInfo(this.barSubItem11, true), new LinkPersistInfo(this.barSubItem12), new LinkPersistInfo(this.barSubItem13, true), new LinkPersistInfo(this.barSubItem14, true) });
            this.barSubItem6.Name = "barSubItem6";
            this.commandColorBarItem1.ButtonStyle = BarButtonStyle.DropDown;
            this.commandColorBarItem1.Caption = "For&eground Color";
            this.commandColorBarItem1.CloseSubMenuOnClick = false;
            this.commandColorBarItem1.Command = ReportCommand.ForeColor;
            this.commandColorBarItem1.Enabled = false;
            this.commandColorBarItem1.Glyph = commandColorBarItem1.Glyph;
            this.commandColorBarItem1.Hint = "Set the foreground color of the control";
            this.commandColorBarItem1.Id = 5;
            this.commandColorBarItem1.Name = "commandColorBarItem1";
            this.commandColorBarItem2.ButtonStyle = BarButtonStyle.DropDown;
            this.commandColorBarItem2.Caption = "Bac&kground Color";
            this.commandColorBarItem2.CloseSubMenuOnClick = false;
            this.commandColorBarItem2.Command = ReportCommand.BackColor;
            this.commandColorBarItem2.Enabled = false;
            this.commandColorBarItem2.Glyph = commandColorBarItem2.Glyph;
            this.commandColorBarItem2.Hint = "Set the background color of the control";
            this.commandColorBarItem2.Id = 6;
            this.commandColorBarItem2.Name = "commandColorBarItem2";
            this.barSubItem7.Caption = "&Font";
            this.barSubItem7.Id = 52;
            barSubItem7.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.commandBarItem1, true), new LinkPersistInfo(this.commandBarItem2), new LinkPersistInfo(this.commandBarItem3) });
            this.barSubItem7.Name = "barSubItem7";
            this.commandBarItem1.Caption = "&Bold";
            this.commandBarItem1.Command = ReportCommand.FontBold;
            this.commandBarItem1.Enabled = false;
            this.commandBarItem1.Hint = "Make the font bold";
            this.commandBarItem1.Id = 2;
            this.commandBarItem1.ImageIndex = 0;
            this.commandBarItem1.ItemShortcut = new BarShortcut(Keys.RButton | Keys.B | Keys.Control);
            this.commandBarItem1.Name = "commandBarItem1";
            this.commandBarItem2.Caption = "&Italic";
            this.commandBarItem2.Command = ReportCommand.FontItalic;
            this.commandBarItem2.Enabled = false;
            this.commandBarItem2.Hint = "Make the font italic";
            this.commandBarItem2.Id = 3;
            this.commandBarItem2.ImageIndex = 1;
            this.commandBarItem2.ItemShortcut = new BarShortcut(Keys.LButton | Keys.Back | Keys.Tab | Keys.A | Keys.H | Keys.I | Keys.Control);
            this.commandBarItem2.Name = "commandBarItem2";
            this.commandBarItem3.Caption = "&Underline";
            this.commandBarItem3.Command = ReportCommand.FontUnderline;
            this.commandBarItem3.Enabled = false;
            this.commandBarItem3.Hint = "Underline the font";
            this.commandBarItem3.Id = 4;
            this.commandBarItem3.ImageIndex = 2;
            this.commandBarItem3.ItemShortcut = new BarShortcut(Keys.LButton | Keys.MButton | Keys.XButton1 | Keys.ShiftKey | Keys.ControlKey | Keys.Capital | Keys.CapsLock | Keys.KanaMode | Keys.HanguelMode | Keys.HangulMode | Keys.A | Keys.D | Keys.E | Keys.P | Keys.Q | Keys.T | Keys.U | Keys.Control);
            this.commandBarItem3.Name = "commandBarItem3";
            this.barSubItem8.Caption = "&Justify";
            this.barSubItem8.Id = 53;
            barSubItem8.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.commandBarItem4, true), new LinkPersistInfo(this.commandBarItem5), new LinkPersistInfo(this.commandBarItem6), new LinkPersistInfo(this.commandBarItem7) });
            this.barSubItem8.Name = "barSubItem8";
            this.commandBarItem4.Caption = "&Left";
            this.commandBarItem4.Command = ReportCommand.JustifyLeft;
            this.commandBarItem4.Enabled = false;
            this.commandBarItem4.Hint = "Align the control’s text to the left";
            this.commandBarItem4.Id = 7;
            this.commandBarItem4.ImageIndex = 5;
            this.commandBarItem4.Name = "commandBarItem4";
            this.commandBarItem5.Caption = "&Center";
            this.commandBarItem5.Command = ReportCommand.JustifyCenter;
            this.commandBarItem5.Enabled = false;
            this.commandBarItem5.Hint = "Align the control’s text to the center";
            this.commandBarItem5.Id = 8;
            this.commandBarItem5.ImageIndex = 6;
            this.commandBarItem5.Name = "commandBarItem5";
            this.commandBarItem6.Caption = "&Right";
            this.commandBarItem6.Command = ReportCommand.JustifyRight;
            this.commandBarItem6.Enabled = false;
            this.commandBarItem6.Hint = "Align the control’s text to the right";
            this.commandBarItem6.Id = 9;
            this.commandBarItem6.ImageIndex = 7;
            this.commandBarItem6.Name = "commandBarItem6";
            this.commandBarItem7.Caption = "&Justify";
            this.commandBarItem7.Command = ReportCommand.JustifyJustify;
            this.commandBarItem7.Enabled = false;
            this.commandBarItem7.Hint = "Justify the control’s text";
            this.commandBarItem7.Id = 10;
            this.commandBarItem7.ImageIndex = 8;
            this.commandBarItem7.Name = "commandBarItem7";
            this.barSubItem9.Caption = "&Align";
            this.barSubItem9.Id = 54;
            barSubItem9.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.commandBarItem9, true), new LinkPersistInfo(this.commandBarItem10), new LinkPersistInfo(this.commandBarItem11), new LinkPersistInfo(this.commandBarItem12, true), new LinkPersistInfo(this.commandBarItem13), new LinkPersistInfo(this.commandBarItem14), new LinkPersistInfo(this.commandBarItem8, true) });
            this.barSubItem9.Name = "barSubItem9";
            this.commandBarItem9.Caption = "&Lefts";
            this.commandBarItem9.Command = ReportCommand.AlignLeft;
            this.commandBarItem9.Enabled = false;
            this.commandBarItem9.Hint = "Left align the selected controls";
            this.commandBarItem9.Id = 12;
            this.commandBarItem9.ImageIndex = 18;
            this.commandBarItem9.Name = "commandBarItem9";
            this.commandBarItem10.Caption = "&Centers";
            this.commandBarItem10.Command = ReportCommand.AlignVerticalCenters;
            this.commandBarItem10.Enabled = false;
            this.commandBarItem10.Hint = "Align the centers of the selected controls vertically";
            this.commandBarItem10.Id = 13;
            this.commandBarItem10.ImageIndex = 19;
            this.commandBarItem10.Name = "commandBarItem10";
            this.commandBarItem11.Caption = "&Rights";
            this.commandBarItem11.Command = ReportCommand.AlignRight;
            this.commandBarItem11.Enabled = false;
            this.commandBarItem11.Hint = "Right align the selected controls";
            this.commandBarItem11.Id = 14;
            this.commandBarItem11.ImageIndex = 20;
            this.commandBarItem11.Name = "commandBarItem11";
            this.commandBarItem12.Caption = "&Tops";
            this.commandBarItem12.Command = ReportCommand.AlignTop;
            this.commandBarItem12.Enabled = false;
            this.commandBarItem12.Hint = "Align the tops of the selected controls";
            this.commandBarItem12.Id = 15;
            this.commandBarItem12.ImageIndex = 21;
            this.commandBarItem12.Name = "commandBarItem12";
            this.commandBarItem13.Caption = "&Middles";
            this.commandBarItem13.Command = ReportCommand.AlignHorizontalCenters;
            this.commandBarItem13.Enabled = false;
            this.commandBarItem13.Hint = "Align the centers of the selected controls horizontally";
            this.commandBarItem13.Id = 16;
            this.commandBarItem13.ImageIndex = 22;
            this.commandBarItem13.Name = "commandBarItem13";
            this.commandBarItem14.Caption = "&Bottoms";
            this.commandBarItem14.Command = ReportCommand.AlignBottom;
            this.commandBarItem14.Enabled = false;
            this.commandBarItem14.Hint = "Align the bottoms of the selected controls";
            this.commandBarItem14.Id = 17;
            this.commandBarItem14.ImageIndex = 23;
            this.commandBarItem14.Name = "commandBarItem14";
            this.commandBarItem8.Caption = "to &Grid";
            this.commandBarItem8.Command = ReportCommand.AlignToGrid;
            this.commandBarItem8.Enabled = false;
            this.commandBarItem8.Hint = "Align the positions of the selected controls to the grid";
            this.commandBarItem8.Id = 11;
            this.commandBarItem8.ImageIndex = 17;
            this.commandBarItem8.Name = "commandBarItem8";
            this.barSubItem10.Caption = "&Make Same Size";
            this.barSubItem10.Id = 55;
            barSubItem10.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.commandBarItem15, true), new LinkPersistInfo(this.commandBarItem16), new LinkPersistInfo(this.commandBarItem17), new LinkPersistInfo(this.commandBarItem18) });
            this.barSubItem10.Name = "barSubItem10";
            this.commandBarItem15.Caption = "&Width";
            this.commandBarItem15.Command = ReportCommand.SizeToControlWidth;
            this.commandBarItem15.Enabled = false;
            this.commandBarItem15.Hint = "Make the selected controls have the same width";
            this.commandBarItem15.Id = 18;
            this.commandBarItem15.ImageIndex = 24;
            this.commandBarItem15.Name = "commandBarItem15";
            this.commandBarItem16.Caption = "Size to Gri&d";
            this.commandBarItem16.Command = ReportCommand.SizeToGrid;
            this.commandBarItem16.Enabled = false;
            this.commandBarItem16.Hint = "Size the selected controls to the grid";
            this.commandBarItem16.Id = 19;
            this.commandBarItem16.ImageIndex = 25;
            this.commandBarItem16.Name = "commandBarItem16";
            this.commandBarItem17.Caption = "&Height";
            this.commandBarItem17.Command = ReportCommand.SizeToControlHeight;
            this.commandBarItem17.Enabled = false;
            this.commandBarItem17.Hint = "Make the selected controls have the same height";
            this.commandBarItem17.Id = 20;
            this.commandBarItem17.ImageIndex = 26;
            this.commandBarItem17.Name = "commandBarItem17";
            this.commandBarItem18.Caption = "&Both";
            this.commandBarItem18.Command = ReportCommand.SizeToControl;
            this.commandBarItem18.Enabled = false;
            this.commandBarItem18.Hint = "Make the selected controls the same size";
            this.commandBarItem18.Id = 21;
            this.commandBarItem18.ImageIndex = 27;
            this.commandBarItem18.Name = "commandBarItem18";
            this.barSubItem11.Caption = "&Horizontal Spacing";
            this.barSubItem11.Id = 56;
            barSubItem11.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.commandBarItem19, true), new LinkPersistInfo(this.commandBarItem20), new LinkPersistInfo(this.commandBarItem21), new LinkPersistInfo(this.commandBarItem22) });
            this.barSubItem11.Name = "barSubItem11";
            this.commandBarItem19.Caption = "Make &Equal";
            this.commandBarItem19.Command = ReportCommand.HorizSpaceMakeEqual;
            this.commandBarItem19.Enabled = false;
            this.commandBarItem19.Hint = "Make the spacing between the selected controls equal";
            this.commandBarItem19.Id = 22;
            this.commandBarItem19.ImageIndex = 28;
            this.commandBarItem19.Name = "commandBarItem19";
            this.commandBarItem20.Caption = "&Increase";
            this.commandBarItem20.Command = ReportCommand.HorizSpaceIncrease;
            this.commandBarItem20.Enabled = false;
            this.commandBarItem20.Hint = "Increase the spacing between the selected controls";
            this.commandBarItem20.Id = 23;
            this.commandBarItem20.ImageIndex = 29;
            this.commandBarItem20.Name = "commandBarItem20";
            this.commandBarItem21.Caption = "&Decrease";
            this.commandBarItem21.Command = ReportCommand.HorizSpaceDecrease;
            this.commandBarItem21.Enabled = false;
            this.commandBarItem21.Hint = "Decrease the spacing between the selected controls";
            this.commandBarItem21.Id = 24;
            this.commandBarItem21.ImageIndex = 30;
            this.commandBarItem21.Name = "commandBarItem21";
            this.commandBarItem22.Caption = "&Remove";
            this.commandBarItem22.Command = ReportCommand.HorizSpaceConcatenate;
            this.commandBarItem22.Enabled = false;
            this.commandBarItem22.Hint = "Remove the spacing between the selected controls";
            this.commandBarItem22.Id = 25;
            this.commandBarItem22.ImageIndex = 31;
            this.commandBarItem22.Name = "commandBarItem22";
            this.barSubItem12.Caption = "&Vertical Spacing";
            this.barSubItem12.Id = 57;
            barSubItem12.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.commandBarItem23, true), new LinkPersistInfo(this.commandBarItem24), new LinkPersistInfo(this.commandBarItem25), new LinkPersistInfo(this.commandBarItem26) });
            this.barSubItem12.Name = "barSubItem12";
            this.commandBarItem23.Caption = "Make &Equal";
            this.commandBarItem23.Command = ReportCommand.VertSpaceMakeEqual;
            this.commandBarItem23.Enabled = false;
            this.commandBarItem23.Hint = "Make the spacing between the selected controls equal";
            this.commandBarItem23.Id = 26;
            this.commandBarItem23.ImageIndex = 32;
            this.commandBarItem23.Name = "commandBarItem23";
            this.commandBarItem24.Caption = "&Increase";
            this.commandBarItem24.Command = ReportCommand.VertSpaceIncrease;
            this.commandBarItem24.Enabled = false;
            this.commandBarItem24.Hint = "Increase the spacing between the selected controls";
            this.commandBarItem24.Id = 27;
            this.commandBarItem24.ImageIndex = 33;
            this.commandBarItem24.Name = "commandBarItem24";
            this.commandBarItem25.Caption = "&Decrease";
            this.commandBarItem25.Command = ReportCommand.VertSpaceDecrease;
            this.commandBarItem25.Enabled = false;
            this.commandBarItem25.Hint = "Decrease the spacing between the selected controls";
            this.commandBarItem25.Id = 28;
            this.commandBarItem25.ImageIndex = 34;
            this.commandBarItem25.Name = "commandBarItem25";
            this.commandBarItem26.Caption = "&Remove";
            this.commandBarItem26.Command = ReportCommand.VertSpaceConcatenate;
            this.commandBarItem26.Enabled = false;
            this.commandBarItem26.Hint = "Remove the spacing between the selected controls";
            this.commandBarItem26.Id = 29;
            this.commandBarItem26.ImageIndex = 35;
            this.commandBarItem26.Name = "commandBarItem26";
            this.barSubItem13.Caption = "&Center in Form";
            this.barSubItem13.Id = 58;
            barSubItem13.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.commandBarItem27, true), new LinkPersistInfo(this.commandBarItem28) });
            this.barSubItem13.Name = "barSubItem13";
            this.commandBarItem27.Caption = "&Horizontally";
            this.commandBarItem27.Command = ReportCommand.CenterHorizontally;
            this.commandBarItem27.Enabled = false;
            this.commandBarItem27.Hint = "Horizontally center the selected controls within a band";
            this.commandBarItem27.Id = 30;
            this.commandBarItem27.ImageIndex = 36;
            this.commandBarItem27.Name = "commandBarItem27";
            this.commandBarItem28.Caption = "&Vertically";
            this.commandBarItem28.Command = ReportCommand.CenterVertically;
            this.commandBarItem28.Enabled = false;
            this.commandBarItem28.Hint = "Vertically center the selected controls within a band";
            this.commandBarItem28.Id = 31;
            this.commandBarItem28.ImageIndex = 37;
            this.commandBarItem28.Name = "commandBarItem28";
            this.barSubItem14.Caption = "&Order";
            this.barSubItem14.Id = 59;
            barSubItem14.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.commandBarItem29, true), new LinkPersistInfo(this.commandBarItem30) });
            this.barSubItem14.Name = "barSubItem14";
            this.commandBarItem29.Caption = "&Bring to Front";
            this.commandBarItem29.Command = ReportCommand.BringToFront;
            this.commandBarItem29.Enabled = false;
            this.commandBarItem29.Hint = "Bring the selected controls to the front";
            this.commandBarItem29.Id = 32;
            this.commandBarItem29.ImageIndex = 38;
            this.commandBarItem29.Name = "commandBarItem29";
            this.commandBarItem30.Caption = "&Send to Back";
            this.commandBarItem30.Command = ReportCommand.SendToBack;
            this.commandBarItem30.Enabled = false;
            this.commandBarItem30.Hint = "Move the selected controls to the back";
            this.commandBarItem30.Id = 33;
            this.commandBarItem30.ImageIndex = 39;
            this.commandBarItem30.Name = "commandBarItem30";
            this.barStaticItemSablonAdi.Caption = "Şablon Adı : ";
            this.barStaticItemSablonAdi.Id = 67;
            this.barStaticItemSablonAdi.Name = "barStaticItemSablonAdi";
            this.barStaticItemSablonAdi.TextAlignment = StringAlignment.Near;
            this.barEditItemSablonAdi.Caption = "barEditItem3";
            this.barEditItemSablonAdi.Edit = this.repositoryItemTextEdit3;
            this.barEditItemSablonAdi.EditValue = "";
            this.barEditItemSablonAdi.Id = 70;
            this.barEditItemSablonAdi.Name = "barEditItemSablonAdi";
            this.barEditItemSablonAdi.Width = 254;
            this.repositoryItemTextEdit3.AutoHeight = false;
            this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
            this.barStaticItemOtomatikYazdirText.Caption = "Otomatik Yazdir";
            this.barStaticItemOtomatikYazdirText.Id = 72;
            this.barStaticItemOtomatikYazdirText.Name = "barStaticItemOtomatikYazdirText";
            this.barStaticItemOtomatikYazdirText.TextAlignment = StringAlignment.Near;
            this.barEditItemCheckOtomatikYazdir.Edit = this.repositoryItemCheckEdit1;
            this.barEditItemCheckOtomatikYazdir.Id = 73;
            this.barEditItemCheckOtomatikYazdir.Name = "barEditItemCheckOtomatikYazdir";
            this.barEditItemCheckOtomatikYazdir.Width = 15;
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            this.barStaticItemYazdirilincaKapansin.Caption = "Yazdırılınca Kapansin";
            this.barStaticItemYazdirilincaKapansin.Id = 74;
            this.barStaticItemYazdirilincaKapansin.Name = "barStaticItemYazdirilincaKapansin";
            this.barStaticItemYazdirilincaKapansin.TextAlignment = StringAlignment.Near;
            this.barEditItemCheckYazdirilincaKapat.Edit = this.repositoryItemCheckEdit2;
            this.barEditItemCheckYazdirilincaKapat.Id = 76;
            this.barEditItemCheckYazdirilincaKapat.Name = "barEditItemCheckYazdirilincaKapat";
            this.barEditItemCheckYazdirilincaKapat.Width = 15;
            this.repositoryItemCheckEdit2.AutoHeight = false;
            this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
            this.repositoryItemCheckEdit2.NullStyle = StyleIndeterminate.Unchecked;
            this.barButtonItemKaydet.Caption = "Kaydet";
            this.barButtonItemKaydet.Id = 71;
            this.barButtonItemKaydet.Name = "barButtonItemKaydet";
            this.barButtonItemKaydet.ItemClick += new ItemClickEventHandler(this.barButtonItemKaydet_ItemClick);
            this.designBar2.BarName = "ToolBar";
            this.designBar2.CanDockStyle = BarCanDockStyle.Top;
            this.designBar2.DockCol = 0;
            this.designBar2.DockRow = 1;
            this.designBar2.DockStyle = BarDockStyle.Top;
            designBar2.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.commandBarItem34, true), new LinkPersistInfo(this.commandBarItem35), new LinkPersistInfo(this.commandBarItem36), new LinkPersistInfo(this.commandBarItem37, true), new LinkPersistInfo(this.commandBarItem38) });
            this.designBar2.OptionsBar.AllowQuickCustomization = false;
            this.designBar2.OptionsBar.DisableCustomization = true;
            this.designBar2.Text = "Main Toolbar";
            this.designBar3.BarName = "FormattingToolBar";
            this.designBar3.CanDockStyle = BarCanDockStyle.Top;
            this.designBar3.DockCol = 1;
            this.designBar3.DockRow = 1;
            this.designBar3.DockStyle = BarDockStyle.Top;
            designBar3.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.barEditItem2), new LinkPersistInfo(this.commandBarItem1), new LinkPersistInfo(this.commandBarItem2), new LinkPersistInfo(this.commandBarItem3), new LinkPersistInfo(this.commandColorBarItem1, true), new LinkPersistInfo(this.commandColorBarItem2), new LinkPersistInfo(this.commandBarItem4, true), new LinkPersistInfo(this.commandBarItem5), new LinkPersistInfo(this.commandBarItem6), new LinkPersistInfo(this.commandBarItem7), new LinkPersistInfo(this.barEditItem1) });
            this.designBar3.Offset = 143;
            this.designBar3.OptionsBar.AllowQuickCustomization = false;
            this.designBar3.OptionsBar.DisableCustomization = true;
            this.designBar3.Text = "Formatting Toolbar";
            this.barEditItem2.Caption = "Font Size";
            this.barEditItem2.Edit = this.ricbFontSize;
            this.barEditItem2.Hint = "Font Size";
            this.barEditItem2.Id = 1;
            this.barEditItem2.Name = "barEditItem2";
            this.ricbFontSize.AutoHeight = false;
            ricbFontSize.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            ricbFontSize.Items.AddRange(new object[] { (byte)8, (byte)9, (byte)10, (byte)11, (byte)12, (byte)14, (byte)16, (byte)18, (byte)20, (byte)22, (byte)24, (byte)26, (byte)28, (byte)36, (byte)48, (byte)72 });
            this.ricbFontSize.Name = "ricbFontSize";
            this.barEditItem1.Caption = "Font Name";
            this.barEditItem1.Edit = this.ricbFontName;
            this.barEditItem1.Hint = "Font Name";
            this.barEditItem1.Id = 0;
            this.barEditItem1.Name = "barEditItem1";
            this.barEditItem1.Width = 120;
            this.ricbFontName.AutoHeight = false;
            ricbFontName.Buttons.AddRange(new EditorButton[] { new EditorButton(ButtonPredefines.Combo) });
            this.ricbFontName.DropDownRows = 12;
            this.ricbFontName.Name = "ricbFontName";
            this.designBar4.BarName = "LayoutToolBar";
            this.designBar4.CanDockStyle = BarCanDockStyle.Top;
            this.designBar4.DockCol = 0;
            this.designBar4.DockRow = 2;
            this.designBar4.DockStyle = BarDockStyle.Top;
            designBar4.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.commandBarItem8), new LinkPersistInfo(this.commandBarItem9, true), new LinkPersistInfo(this.commandBarItem10), new LinkPersistInfo(this.commandBarItem11), new LinkPersistInfo(this.commandBarItem12, true), new LinkPersistInfo(this.commandBarItem13), new LinkPersistInfo(this.commandBarItem14), new LinkPersistInfo(this.commandBarItem15, true), new LinkPersistInfo(this.commandBarItem16), new LinkPersistInfo(this.commandBarItem17), new LinkPersistInfo(this.commandBarItem18), new LinkPersistInfo(this.commandBarItem19, true), new LinkPersistInfo(this.commandBarItem20), new LinkPersistInfo(this.commandBarItem21), new LinkPersistInfo(this.commandBarItem22), new LinkPersistInfo(this.commandBarItem23, true), new LinkPersistInfo(this.commandBarItem24), new LinkPersistInfo(this.commandBarItem25), new LinkPersistInfo(this.commandBarItem26), new LinkPersistInfo(this.commandBarItem27, true), new LinkPersistInfo(this.commandBarItem28), new LinkPersistInfo(this.commandBarItem29, true), new LinkPersistInfo(this.commandBarItem30) });
            this.designBar4.OptionsBar.AllowQuickCustomization = false;
            this.designBar4.OptionsBar.DisableCustomization = true;
            this.designBar4.Text = "Layout Toolbar";
            this.designBar5.BarName = "StatusBar";
            this.designBar5.CanDockStyle = BarCanDockStyle.Bottom;
            this.designBar5.DockCol = 0;
            this.designBar5.DockRow = 0;
            this.designBar5.DockStyle = BarDockStyle.Bottom;
            designBar5.LinksPersistInfo.AddRange(new LinkPersistInfo[] { new LinkPersistInfo(this.barStaticItem1) });
            this.designBar5.OptionsBar.AllowQuickCustomization = false;
            this.designBar5.OptionsBar.DrawDragBorder = false;
            this.designBar5.OptionsBar.UseWholeRow = true;
            this.designBar5.Text = "Status Bar";
            this.barStaticItem1.AutoSize = BarStaticItemSize.Spring;
            this.barStaticItem1.Id = 42;
            this.barStaticItem1.Name = "barStaticItem1";
            this.barStaticItem1.TextAlignment = StringAlignment.Near;
            this.barStaticItem1.Width = 32;
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = DockStyle.Top;
            this.barDockControlTop.Location = new Point(0, 0);
            this.barDockControlTop.Size = new Size(1044, 84);
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = DockStyle.Bottom;
            this.barDockControlBottom.Location = new Point(0, 575);
            this.barDockControlBottom.Size = new Size(1044, 25);
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = DockStyle.Left;
            this.barDockControlLeft.Location = new Point(0, 84);
            this.barDockControlLeft.Size = new Size(0, 491);
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = DockStyle.Right;
            this.barDockControlRight.Location = new Point(1044, 84);
            this.barDockControlRight.Size = new Size(0, 491);
            this.xrDesignDockManager1.DockingOptions.FloatOnDblClick = false;
            this.xrDesignDockManager1.DockingOptions.ShowAutoHideButton = false;
            this.xrDesignDockManager1.DockingOptions.ShowCloseButton = false;
            this.xrDesignDockManager1.Form = this;
            this.xrDesignDockManager1.ImageStream = xrDesignDockManager1.ImageStream;
            this.xrDesignDockManager1.RootPanels.AddRange(new DockPanel[] { this.panelContainer2 });
            this.xrDesignDockManager1.TopZIndexControls.AddRange(new string[] { "DevExpress.XtraBars.BarDockControl", "System.Windows.Forms.StatusBar" });
            this.xrDesignDockManager1.XRDesignPanel = base.DesignPanel;
            this.panelContainer2.ActiveChild = this.toolBoxDockPanel1;
            this.panelContainer2.Controls.Add(this.reportExplorerDockPanel1);
            this.panelContainer2.Controls.Add(this.fieldListDockPanel1);
            this.panelContainer2.Controls.Add(this.toolBoxDockPanel1);
            this.panelContainer2.Controls.Add(this.propertyGridDockPanel1);
            this.panelContainer2.Dock = DockingStyle.Right;
            this.panelContainer2.FloatVertical = true;
            this.panelContainer2.ID = new Guid("d90a9a1f-b677-42b9-8561-4a921c8cd7e5");
            this.panelContainer2.ImageIndex = 3;
            this.panelContainer2.Location = new Point(844, 84);
            this.panelContainer2.Name = "panelContainer2";
            this.panelContainer2.OriginalSize = new Size(200, 200);
            this.panelContainer2.Size = new Size(200, 491);
            this.panelContainer2.Tabbed = true;
            this.toolBoxDockPanel1.Controls.Add(this.toolBoxDockPanel1_Container);
            this.toolBoxDockPanel1.Dock = DockingStyle.Fill;
            this.toolBoxDockPanel1.ID = new Guid("161a5a1a-d9b9-4f06-9ac4-d0c3e507c54f");
            this.toolBoxDockPanel1.ImageIndex = 3;
            this.toolBoxDockPanel1.Location = new Point(4, 23);
            this.toolBoxDockPanel1.Name = "toolBoxDockPanel1";
            this.toolBoxDockPanel1.OriginalSize = new Size(192, 415);
            this.toolBoxDockPanel1.Size = new Size(192, 436);
            this.toolBoxDockPanel1.Text = "Tool Box";
            this.toolBoxDockPanel1.XRDesignPanel = base.DesignPanel;
            this.toolBoxDockPanel1_Container.Location = new Point(0, 0);
            this.toolBoxDockPanel1_Container.Name = "toolBoxDockPanel1_Container";
            this.toolBoxDockPanel1_Container.Size = new Size(192, 436);
            this.toolBoxDockPanel1_Container.TabIndex = 0;
            this.reportExplorerDockPanel1.Controls.Add(this.reportExplorerDockPanel1_Container);
            this.reportExplorerDockPanel1.Dock = DockingStyle.Fill;
            this.reportExplorerDockPanel1.ID = new Guid("fb3ec6cc-3b9b-4b9c-91cf-cff78c1edbf1");
            this.reportExplorerDockPanel1.ImageIndex = 2;
            this.reportExplorerDockPanel1.Location = new Point(4, 23);
            this.reportExplorerDockPanel1.Name = "reportExplorerDockPanel1";
            this.reportExplorerDockPanel1.OriginalSize = new Size(192, 415);
            this.reportExplorerDockPanel1.Size = new Size(192, 436);
            this.reportExplorerDockPanel1.Text = "Report Explorer";
            this.reportExplorerDockPanel1.XRDesignPanel = base.DesignPanel;
            this.reportExplorerDockPanel1_Container.Location = new Point(0, 0);
            this.reportExplorerDockPanel1_Container.Name = "reportExplorerDockPanel1_Container";
            this.reportExplorerDockPanel1_Container.Size = new Size(192, 436);
            this.reportExplorerDockPanel1_Container.TabIndex = 0;
            this.fieldListDockPanel1.Controls.Add(this.fieldListDockPanel1_Container);
            this.fieldListDockPanel1.Dock = DockingStyle.Fill;
            this.fieldListDockPanel1.ID = new Guid("faf69838-a93f-4114-83e8-d0d09cc5ce95");
            this.fieldListDockPanel1.ImageIndex = 0;
            this.fieldListDockPanel1.Location = new Point(4, 23);
            this.fieldListDockPanel1.Name = "fieldListDockPanel1";
            this.fieldListDockPanel1.OriginalSize = new Size(192, 415);
            this.fieldListDockPanel1.Size = new Size(192, 436);
            this.fieldListDockPanel1.Text = "Field List";
            this.fieldListDockPanel1.XRDesignPanel = base.DesignPanel;
            this.fieldListDockPanel1_Container.Location = new Point(0, 0);
            this.fieldListDockPanel1_Container.Name = "fieldListDockPanel1_Container";
            this.fieldListDockPanel1_Container.Size = new Size(192, 436);
            this.fieldListDockPanel1_Container.TabIndex = 0;
            this.propertyGridDockPanel1.Controls.Add(this.propertyGridDockPanel1_Container);
            this.propertyGridDockPanel1.Dock = DockingStyle.Fill;
            this.propertyGridDockPanel1.FloatVertical = true;
            this.propertyGridDockPanel1.ID = new Guid("b38d12c3-cd06-4dec-b93d-63a0088e495a");
            this.propertyGridDockPanel1.ImageIndex = 1;
            this.propertyGridDockPanel1.Location = new Point(4, 23);
            this.propertyGridDockPanel1.Name = "propertyGridDockPanel1";
            this.propertyGridDockPanel1.OriginalSize = new Size(192, 415);
            this.propertyGridDockPanel1.Size = new Size(192, 436);
            this.propertyGridDockPanel1.Text = "Property Grid";
            this.propertyGridDockPanel1.XRDesignPanel = base.DesignPanel;
            this.propertyGridDockPanel1_Container.Location = new Point(0, 0);
            this.propertyGridDockPanel1_Container.Name = "propertyGridDockPanel1_Container";
            this.propertyGridDockPanel1_Container.Size = new Size(192, 436);
            this.propertyGridDockPanel1_Container.TabIndex = 0;
            this.barCheckItemCheckYazdirincaKapat.Id = 75;
            this.barCheckItemCheckYazdirincaKapat.Name = "barCheckItemCheckYazdirincaKapat";
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            this.commandBarItem44.Caption = "commandBarItem44";
            this.commandBarItem44.Id = 66;
            this.commandBarItem44.Name = "commandBarItem44";
            base.ClientSize = new Size(1044, 600);
            base.Controls.Add(this.panelContainer2);
            base.Controls.Add(this.barDockControlLeft);
            base.Controls.Add(this.barDockControlRight);
            base.Controls.Add(this.barDockControlBottom);
            base.Controls.Add(this.barDockControlTop);
            base.Name = "XtraDesignForm";
            base.Controls.SetChildIndex(this.barDockControlTop, 0);
            base.Controls.SetChildIndex(this.barDockControlBottom, 0);
            base.Controls.SetChildIndex(this.barDockControlRight, 0);
            base.Controls.SetChildIndex(this.barDockControlLeft, 0);
            base.Controls.SetChildIndex(this.panelContainer2, 0);
            base.Controls.SetChildIndex(this.xrDesignPanel, 0);
            ((ISupportInitialize)this.xrDesignPanel).EndInit();
            ((ISupportInitialize)this.xrDesignBarManager1).EndInit();
            ((ISupportInitialize)this.repositoryItemTextEdit3).EndInit();
            ((ISupportInitialize)this.repositoryItemCheckEdit1).EndInit();
            ((ISupportInitialize)this.repositoryItemCheckEdit2).EndInit();
            ((ISupportInitialize)this.ricbFontSize).EndInit();
            ((ISupportInitialize)this.ricbFontName).EndInit();
            ((ISupportInitialize)this.xrDesignDockManager1).EndInit();
            this.panelContainer2.ResumeLayout(false);
            this.toolBoxDockPanel1.ResumeLayout(false);
            this.reportExplorerDockPanel1.ResumeLayout(false);
            this.fieldListDockPanel1.ResumeLayout(false);
            this.propertyGridDockPanel1.ResumeLayout(false);
            ((ISupportInitialize)this.repositoryItemTextEdit1).EndInit();
            ((ISupportInitialize)this.repositoryItemTextEdit2).EndInit();
            base.ResumeLayout(false);
        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
    }
}
