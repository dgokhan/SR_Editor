namespace SR_Editor.Forms
{
    using DevExpress.XtraBars;
    using DevExpress.XtraBars.Alerter;
    using DevExpress.XtraBars.ToolbarForm;
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.components = new System.ComponentModel.Container();
            this.barManager = new DevExpress.XtraBars.ToolbarForm.ToolbarFormManager(this.components);
            this.statusBar = new DevExpress.XtraBars.Bar();
            this.barHeaderItemGirisZamani = new DevExpress.XtraBars.BarHeaderItem();
            this.barStaticItemGirisZamani = new DevExpress.XtraBars.BarStaticItem();
            this.barHeaderItemAcikKalmaSuresi = new DevExpress.XtraBars.BarHeaderItem();
            this.barStaticItemAcikKalmaSuresi = new DevExpress.XtraBars.BarStaticItem();
            this.barHeaderItemKurum = new DevExpress.XtraBars.BarHeaderItem();
            this.barStaticItemKurum = new DevExpress.XtraBars.BarStaticItem();
            this.barHeaderItemVersiyon = new DevExpress.XtraBars.BarHeaderItem();
            this.barStaticItemVersiyon = new DevExpress.XtraBars.BarStaticItem();
            this.barHeaderItemBilgisayar = new DevExpress.XtraBars.BarHeaderItem();
            this.barStaticItemBilgisayar = new DevExpress.XtraBars.BarStaticItem();
            this.barHeaderItemIp = new DevExpress.XtraBars.BarHeaderItem();
            this.barStaticItemIp = new DevExpress.XtraBars.BarStaticItem();
            this.topBar = new DevExpress.XtraBars.Bar();
            this.barAndDockingController = new DevExpress.XtraBars.BarAndDockingController(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.hideContainerBottom = new DevExpress.XtraBars.Docking.AutoHideContainer();
            this.panelContainer2 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel5 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel5_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.dockPanel3 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel3_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.dockPanel4 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel4_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.textBox1 = new DevExpress.XtraEditors.MemoEdit();
            this.mainMenuSvgImages = new DevExpress.Utils.SvgImageCollection(this.components);
            this.siNew = new DevExpress.XtraBars.BarSubItem();
            this.iFile = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.iProject = new DevExpress.XtraBars.BarButtonItem();
            this.iBlankSolution = new DevExpress.XtraBars.BarButtonItem();
            this.iOpen = new DevExpress.XtraBars.BarButtonItem();
            this.iClose = new DevExpress.XtraBars.BarButtonItem();
            this.iAddExistingItem = new DevExpress.XtraBars.BarButtonItem();
            this.siAddProject = new DevExpress.XtraBars.BarSubItem();
            this.iNewProject = new DevExpress.XtraBars.BarButtonItem();
            this.iExistingProject = new DevExpress.XtraBars.BarButtonItem();
            this.iOpenSolution = new DevExpress.XtraBars.BarButtonItem();
            this.iCloseSolution = new DevExpress.XtraBars.BarButtonItem();
            this.iPageSetup = new DevExpress.XtraBars.BarButtonItem();
            this.iPrint = new DevExpress.XtraBars.BarButtonItem();
            this.iExit = new DevExpress.XtraBars.BarButtonItem();
            this.siFind = new DevExpress.XtraBars.BarSubItem();
            this.iFind = new DevExpress.XtraBars.BarButtonItem();
            this.iReplace = new DevExpress.XtraBars.BarButtonItem();
            this.iSolutionExplorer = new DevExpress.XtraBars.BarButtonItem();
            this.iProperties = new DevExpress.XtraBars.BarButtonItem();
            this.iToolbox = new DevExpress.XtraBars.BarButtonItem();
            this.iClassView = new DevExpress.XtraBars.BarButtonItem();
            this.iTaskList = new DevExpress.XtraBars.BarButtonItem();
            this.iFindResults = new DevExpress.XtraBars.BarButtonItem();
            this.iOutput = new DevExpress.XtraBars.BarButtonItem();
            this.iLoadLayout = new DevExpress.XtraBars.BarButtonItem();
            this.iSaveLayout = new DevExpress.XtraBars.BarButtonItem();
            this.barDockingMenuItem1 = new DevExpress.XtraBars.BarDockingMenuItem();
            this.iAbout = new DevExpress.XtraBars.BarButtonItem();
            this.barHeaderItem1 = new DevExpress.XtraBars.BarHeaderItem();
            this.barStaticItemBaslik = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItemKullanici = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItemSifreDegistir = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemEkraniKilitle = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemYenidenBaslat = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemHatirlatici = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barToggleSwitchItemCanliMesajIstemiyorum = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemToggleSwitch2 = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem2 = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem25 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem27 = new DevExpress.XtraBars.BarButtonItem();
            this.barHeaderItemAktifBaslik = new DevExpress.XtraBars.BarStaticItem();
            this.barButtonItem11 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem12 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem13 = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryImageComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemImageComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
            this.repositoryItemSearchControl1 = new DevExpress.XtraEditors.Repository.RepositoryItemSearchControl();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemToggleSwitch1 = new DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch();
            this.popupMenu3 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            this.alertControlZorunluCikis = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
            this.alertControl = new DevExpress.XtraBars.Alerter.AlertControl(this.components);
            this.classViewSvgImages = new DevExpress.Utils.SvgImageCollection(this.components);
            this.documentManager = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.fileTypeSvgImages = new DevExpress.Utils.SvgImageCollection(this.components);
            this.toolbarFormControl1 = new DevExpress.XtraBars.ToolbarForm.ToolbarFormControl();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.barStaticItem3 = new DevExpress.XtraBars.BarStaticItem();
            this.barHeaderItem2 = new DevExpress.XtraBars.BarHeaderItem();
            this.timerForm = new System.Windows.Forms.Timer(this.components);
            this.transitionManager = new DevExpress.Utils.Animation.TransitionManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.popupMenu2 = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
            this.hideContainerBottom.SuspendLayout();
            this.panelContainer2.SuspendLayout();
            this.dockPanel5.SuspendLayout();
            this.dockPanel3.SuspendLayout();
            this.dockPanel4.SuspendLayout();
            this.dockPanel4_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textBox1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainMenuSvgImages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryImageComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.classViewSvgImages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileTypeSvgImages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolbarFormControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager
            // 
            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.statusBar,
            this.topBar});
            this.barManager.Categories.AddRange(new DevExpress.XtraBars.BarManagerCategory[] {
            new DevExpress.XtraBars.BarManagerCategory("Built-in Menus", new System.Guid("a984a9d9-f96f-425a-8857-fe4de6df48c2")),
            new DevExpress.XtraBars.BarManagerCategory("File", new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52")),
            new DevExpress.XtraBars.BarManagerCategory("Edit", new System.Guid("ac82dbe7-c530-4aa2-b6de-94a7777426fe")),
            new DevExpress.XtraBars.BarManagerCategory("Standard", new System.Guid("fbaaf85d-943d-4ccd-8517-fc398efe9c7b")),
            new DevExpress.XtraBars.BarManagerCategory("View", new System.Guid("0cb4cc3e-4798-4d61-9457-672bdc2a90d4")),
            new DevExpress.XtraBars.BarManagerCategory("Window", new System.Guid("faa74de1-bd23-44b9-955d-6ba635fa0f01")),
            new DevExpress.XtraBars.BarManagerCategory("Status", new System.Guid("d3532f9f-c716-4c40-8731-d110e1a41e64")),
            new DevExpress.XtraBars.BarManagerCategory("Layouts", new System.Guid("f2b2eae8-5b98-43eb-81aa-d999b20fd3d3")),
            new DevExpress.XtraBars.BarManagerCategory("PaintStyles", new System.Guid("d0a113b2-425b-47f5-a6b5-0aefb1859507"))});
            this.barManager.Controller = this.barAndDockingController;
            this.barManager.DockControls.Add(this.barDockControlTop);
            this.barManager.DockControls.Add(this.barDockControlBottom);
            this.barManager.DockControls.Add(this.barDockControlLeft);
            this.barManager.DockControls.Add(this.barDockControlRight);
            this.barManager.DockManager = this.dockManager;
            this.barManager.Form = this;
            this.barManager.Images = this.mainMenuSvgImages;
            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.siNew,
            this.iOpen,
            this.iClose,
            this.iProject,
            this.iFile,
            this.iBlankSolution,
            this.iAddExistingItem,
            this.siAddProject,
            this.iNewProject,
            this.iExistingProject,
            this.iOpenSolution,
            this.iCloseSolution,
            this.iPageSetup,
            this.iPrint,
            this.iExit,
            this.siFind,
            this.iFind,
            this.iReplace,
            this.iSolutionExplorer,
            this.iProperties,
            this.iToolbox,
            this.iClassView,
            this.iTaskList,
            this.iFindResults,
            this.iOutput,
            this.iLoadLayout,
            this.iSaveLayout,
            this.barDockingMenuItem1,
            this.iAbout,
            this.barButtonItem1,
            this.barHeaderItem1,
            this.barStaticItemBaslik,
            this.barButtonItem2,
            this.barSubItemKullanici,
            this.barButtonItemSifreDegistir,
            this.barButtonItemEkraniKilitle,
            this.barButtonItemYenidenBaslat,
            this.barButtonItemHatirlatici,
            this.barButtonItem5,
            this.barStaticItem2,
            this.barButtonItem3,
            this.barButtonItem4,
            this.barSubItem2,
            this.barButtonItem6,
            this.barButtonItem7,
            this.barButtonItem8,
            this.barButtonItem9,
            this.barButtonItem10,
            this.barHeaderItemGirisZamani,
            this.barStaticItemGirisZamani,
            this.barHeaderItemVersiyon,
            this.barStaticItemVersiyon,
            this.barHeaderItemIp,
            this.barStaticItemIp,
            this.barButtonItem25,
            this.barButtonItem27,
            this.barHeaderItemAktifBaslik,
            this.barToggleSwitchItemCanliMesajIstemiyorum,
            this.barStaticItem1,
            this.barHeaderItemAcikKalmaSuresi,
            this.barStaticItemAcikKalmaSuresi,
            this.barButtonItem11,
            this.barButtonItem12,
            this.barButtonItem13,
            this.barHeaderItemKurum,
            this.barStaticItemKurum,
            this.barHeaderItemBilgisayar,
            this.barStaticItemBilgisayar});
            this.barManager.MaxItemId = 144;
            this.barManager.OptionsLayout.AllowAddNewItems = false;
            this.barManager.OptionsLayout.ResetRecentItems = false;
            this.barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryImageComboBox1,
            this.repositoryItemComboBox1,
            this.repositoryItemImageComboBox1,
            this.repositoryItemSearchControl1,
            this.repositoryItemTextEdit1,
            this.repositoryItemToggleSwitch1,
            this.repositoryItemToggleSwitch2});
            this.barManager.StatusBar = this.statusBar;
            // 
            // statusBar
            // 
            this.statusBar.BarName = "StatusBar";
            this.statusBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.statusBar.DockCol = 0;
            this.statusBar.DockRow = 0;
            this.statusBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.statusBar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barHeaderItemGirisZamani),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemGirisZamani),
            new DevExpress.XtraBars.LinkPersistInfo(this.barHeaderItemAcikKalmaSuresi),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemAcikKalmaSuresi),
            new DevExpress.XtraBars.LinkPersistInfo(this.barHeaderItemKurum),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemKurum),
            new DevExpress.XtraBars.LinkPersistInfo(this.barHeaderItemVersiyon),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemVersiyon),
            new DevExpress.XtraBars.LinkPersistInfo(this.barHeaderItemBilgisayar),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemBilgisayar),
            new DevExpress.XtraBars.LinkPersistInfo(this.barHeaderItemIp),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItemIp)});
            this.statusBar.OptionsBar.AllowQuickCustomization = false;
            this.statusBar.OptionsBar.DrawDragBorder = false;
            this.statusBar.OptionsBar.UseWholeRow = true;
            this.statusBar.Text = "Custom 3";
            // 
            // barHeaderItemGirisZamani
            // 
            this.barHeaderItemGirisZamani.Caption = "Giriş Zamanı:";
            this.barHeaderItemGirisZamani.Id = 111;
            this.barHeaderItemGirisZamani.Name = "barHeaderItemGirisZamani";
            // 
            // barStaticItemGirisZamani
            // 
            this.barStaticItemGirisZamani.Caption = "00.00.00 00:00:00";
            this.barStaticItemGirisZamani.Id = 112;
            this.barStaticItemGirisZamani.Name = "barStaticItemGirisZamani";
            // 
            // barHeaderItemAcikKalmaSuresi
            // 
            this.barHeaderItemAcikKalmaSuresi.Caption = "Açık Kalma Süresi:";
            this.barHeaderItemAcikKalmaSuresi.Id = 133;
            this.barHeaderItemAcikKalmaSuresi.Name = "barHeaderItemAcikKalmaSuresi";
            // 
            // barStaticItemAcikKalmaSuresi
            // 
            this.barStaticItemAcikKalmaSuresi.Caption = "00:00:00";
            this.barStaticItemAcikKalmaSuresi.Id = 134;
            this.barStaticItemAcikKalmaSuresi.Name = "barStaticItemAcikKalmaSuresi";
            // 
            // barHeaderItemKurum
            // 
            this.barHeaderItemKurum.Caption = "Kurum:";
            this.barHeaderItemKurum.Id = 139;
            this.barHeaderItemKurum.Name = "barHeaderItemKurum";
            // 
            // barStaticItemKurum
            // 
            this.barStaticItemKurum.Caption = "Rish Medical";
            this.barStaticItemKurum.Id = 140;
            this.barStaticItemKurum.Name = "barStaticItemKurum";
            // 
            // barHeaderItemVersiyon
            // 
            this.barHeaderItemVersiyon.Caption = "Versiyon:";
            this.barHeaderItemVersiyon.Id = 113;
            this.barHeaderItemVersiyon.Name = "barHeaderItemVersiyon";
            // 
            // barStaticItemVersiyon
            // 
            this.barStaticItemVersiyon.Caption = "1.0.0.0";
            this.barStaticItemVersiyon.Id = 114;
            this.barStaticItemVersiyon.Name = "barStaticItemVersiyon";
            // 
            // barHeaderItemBilgisayar
            // 
            this.barHeaderItemBilgisayar.Caption = "Bilgisayar:";
            this.barHeaderItemBilgisayar.Id = 141;
            this.barHeaderItemBilgisayar.Name = "barHeaderItemBilgisayar";
            // 
            // barStaticItemBilgisayar
            // 
            this.barStaticItemBilgisayar.Caption = "Bilgisayar";
            this.barStaticItemBilgisayar.Id = 142;
            this.barStaticItemBilgisayar.Name = "barStaticItemBilgisayar";
            // 
            // barHeaderItemIp
            // 
            this.barHeaderItemIp.Caption = "Ip Adresi:";
            this.barHeaderItemIp.Id = 115;
            this.barHeaderItemIp.Name = "barHeaderItemIp";
            // 
            // barStaticItemIp
            // 
            this.barStaticItemIp.Caption = "1.1.1.1";
            this.barStaticItemIp.Id = 116;
            this.barStaticItemIp.Name = "barStaticItemIp";
            // 
            // topBar
            // 
            this.topBar.BarName = "TopBar";
            this.topBar.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.topBar.DockCol = 0;
            this.topBar.DockRow = 0;
            this.topBar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.topBar.OptionsBar.AllowQuickCustomization = false;
            this.topBar.OptionsBar.DrawBorder = false;
            this.topBar.OptionsBar.DrawDragBorder = false;
            this.topBar.OptionsBar.UseWholeRow = true;
            this.topBar.Text = "TopBar";
            // 
            // barAndDockingController
            // 
            this.barAndDockingController.PaintStyleName = "Skin";
            this.barAndDockingController.PropertiesBar.AllowLinkLighting = false;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 31);
            this.barDockControlTop.Manager = this.barManager;
            this.barDockControlTop.Size = new System.Drawing.Size(1528, 21);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 638);
            this.barDockControlBottom.Manager = this.barManager;
            this.barDockControlBottom.Size = new System.Drawing.Size(1528, 21);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 52);
            this.barDockControlLeft.Manager = this.barManager;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 586);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1528, 52);
            this.barDockControlRight.Manager = this.barManager;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 586);
            // 
            // dockManager
            // 
            this.dockManager.AutoHideContainers.AddRange(new DevExpress.XtraBars.Docking.AutoHideContainer[] {
            this.hideContainerBottom});
            this.dockManager.Controller = this.barAndDockingController;
            this.dockManager.Form = this;
            this.dockManager.Images = this.mainMenuSvgImages;
            this.dockManager.MenuManager = this.barManager;
            this.dockManager.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // hideContainerBottom
            // 
            this.hideContainerBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.hideContainerBottom.Controls.Add(this.panelContainer2);
            this.hideContainerBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hideContainerBottom.Location = new System.Drawing.Point(0, 611);
            this.hideContainerBottom.Name = "hideContainerBottom";
            this.hideContainerBottom.Size = new System.Drawing.Size(1528, 27);
            this.hideContainerBottom.Visible = false;
            // 
            // panelContainer2
            // 
            this.panelContainer2.ActiveChild = this.dockPanel5;
            this.panelContainer2.Controls.Add(this.dockPanel3);
            this.panelContainer2.Controls.Add(this.dockPanel4);
            this.panelContainer2.Controls.Add(this.dockPanel5);
            this.panelContainer2.Dock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.panelContainer2.FloatSize = new System.Drawing.Size(304, 139);
            this.panelContainer2.ID = new System.Guid("ec7b92c0-cfe1-43c3-9ff0-c24e6320f016");
            this.panelContainer2.Location = new System.Drawing.Point(0, 411);
            this.panelContainer2.Name = "panelContainer2";
            this.panelContainer2.OriginalSize = new System.Drawing.Size(200, 200);
            this.panelContainer2.SavedDock = DevExpress.XtraBars.Docking.DockingStyle.Bottom;
            this.panelContainer2.SavedIndex = 0;
            this.panelContainer2.Size = new System.Drawing.Size(1528, 200);
            this.panelContainer2.Tabbed = true;
            this.panelContainer2.Text = "panelContainer2";
            this.panelContainer2.Visibility = DevExpress.XtraBars.Docking.DockVisibility.AutoHide;
            // 
            // dockPanel5
            // 
            this.dockPanel5.Controls.Add(this.dockPanel5_Container);
            this.dockPanel5.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanel5.FloatSize = new System.Drawing.Size(304, 139);
            this.dockPanel5.ID = new System.Guid("dbdb0ba9-5443-476b-93ad-ec35678d61ef");
            this.dockPanel5.Location = new System.Drawing.Point(3, 47);
            this.dockPanel5.Name = "dockPanel5";
            this.dockPanel5.OriginalSize = new System.Drawing.Size(1520, 172);
            this.dockPanel5.Size = new System.Drawing.Size(1522, 150);
            this.dockPanel5.Text = "Output";
            // 
            // dockPanel5_Container
            // 
            this.dockPanel5_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel5_Container.Name = "dockPanel5_Container";
            this.dockPanel5_Container.Size = new System.Drawing.Size(1522, 150);
            this.dockPanel5_Container.TabIndex = 0;
            // 
            // dockPanel3
            // 
            this.dockPanel3.Controls.Add(this.dockPanel3_Container);
            this.dockPanel3.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanel3.FloatSize = new System.Drawing.Size(304, 139);
            this.dockPanel3.ID = new System.Guid("7351d5e2-6da1-45c0-a5b6-13e4e7d7a56e");
            this.dockPanel3.Location = new System.Drawing.Point(3, 47);
            this.dockPanel3.Name = "dockPanel3";
            this.dockPanel3.OriginalSize = new System.Drawing.Size(1520, 172);
            this.dockPanel3.Size = new System.Drawing.Size(1522, 150);
            this.dockPanel3.TabText = "Task List";
            this.dockPanel3.Text = "Task List - 0 Build Errors";
            // 
            // dockPanel3_Container
            // 
            this.dockPanel3_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel3_Container.Name = "dockPanel3_Container";
            this.dockPanel3_Container.Size = new System.Drawing.Size(1522, 150);
            this.dockPanel3_Container.TabIndex = 0;
            // 
            // dockPanel4
            // 
            this.dockPanel4.Controls.Add(this.dockPanel4_Container);
            this.dockPanel4.Dock = DevExpress.XtraBars.Docking.DockingStyle.Fill;
            this.dockPanel4.FloatSize = new System.Drawing.Size(304, 139);
            this.dockPanel4.ID = new System.Guid("47b3ea95-3900-46d6-b24c-5f3a779b1ae7");
            this.dockPanel4.Location = new System.Drawing.Point(3, 47);
            this.dockPanel4.Name = "dockPanel4";
            this.dockPanel4.OriginalSize = new System.Drawing.Size(1520, 172);
            this.dockPanel4.Size = new System.Drawing.Size(1522, 150);
            this.dockPanel4.Text = "Find Results";
            // 
            // dockPanel4_Container
            // 
            this.dockPanel4_Container.Controls.Add(this.textBox1);
            this.dockPanel4_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel4_Container.Name = "dockPanel4_Container";
            this.dockPanel4_Container.Size = new System.Drawing.Size(1522, 150);
            this.dockPanel4_Container.TabIndex = 0;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.EditValue = "";
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Properties.Appearance.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.textBox1.Properties.Appearance.Options.UseFont = true;
            this.textBox1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.textBox1.Size = new System.Drawing.Size(1522, 150);
            this.textBox1.TabIndex = 0;
            // 
            // siNew
            // 
            this.siNew.Caption = "&New";
            this.siNew.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.siNew.Id = 2;
            this.siNew.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.iFile),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.iProject),
            new DevExpress.XtraBars.LinkPersistInfo(this.iBlankSolution)});
            this.siNew.Name = "siNew";
            // 
            // iFile
            // 
            this.iFile.Caption = "&File...";
            this.iFile.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.iFile.Hint = "New File";
            this.iFile.Id = 6;
            this.iFile.ImageOptions.ImageIndex = 1;
            this.iFile.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N));
            this.iFile.Name = "iFile";
            this.iFile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iNewItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Web Site...";
            this.barButtonItem1.Id = 75;
            this.barButtonItem1.ImageOptions.ImageIndex = 37;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // iProject
            // 
            this.iProject.Caption = "Team &Project...";
            this.iProject.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.iProject.Hint = "New Project";
            this.iProject.Id = 5;
            this.iProject.ImageOptions.ImageIndex = 0;
            this.iProject.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.N));
            this.iProject.Name = "iProject";
            this.iProject.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iNewItemClick);
            // 
            // iBlankSolution
            // 
            this.iBlankSolution.Caption = "&Blank Solution...";
            this.iBlankSolution.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.iBlankSolution.Hint = "Blank Solution";
            this.iBlankSolution.Id = 7;
            this.iBlankSolution.ImageOptions.ImageIndex = 2;
            this.iBlankSolution.Name = "iBlankSolution";
            this.iBlankSolution.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iNewItemClick);
            // 
            // iOpen
            // 
            this.iOpen.Caption = "&Open";
            this.iOpen.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.iOpen.Id = 3;
            this.iOpen.Name = "iOpen";
            // 
            // iClose
            // 
            this.iClose.Caption = "&Close";
            this.iClose.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.iClose.Id = 4;
            this.iClose.Name = "iClose";
            // 
            // iAddExistingItem
            // 
            this.iAddExistingItem.Caption = "Add Existin&g Item...";
            this.iAddExistingItem.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.iAddExistingItem.Id = 9;
            this.iAddExistingItem.ImageOptions.ImageIndex = 4;
            this.iAddExistingItem.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.B));
            this.iAddExistingItem.Name = "iAddExistingItem";
            // 
            // siAddProject
            // 
            this.siAddProject.Caption = "A&dd Project";
            this.siAddProject.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.siAddProject.Id = 10;
            this.siAddProject.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.iNewProject),
            new DevExpress.XtraBars.LinkPersistInfo(this.iExistingProject)});
            this.siAddProject.Name = "siAddProject";
            // 
            // iNewProject
            // 
            this.iNewProject.Caption = "&New Project...";
            this.iNewProject.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.iNewProject.Hint = "New Project";
            this.iNewProject.Id = 11;
            this.iNewProject.Name = "iNewProject";
            this.iNewProject.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iNewItemClick);
            // 
            // iExistingProject
            // 
            this.iExistingProject.Caption = "&Existing Project...";
            this.iExistingProject.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.iExistingProject.Id = 12;
            this.iExistingProject.Name = "iExistingProject";
            // 
            // iOpenSolution
            // 
            this.iOpenSolution.Caption = "Op&en Solution...";
            this.iOpenSolution.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.iOpenSolution.Id = 14;
            this.iOpenSolution.ImageOptions.ImageIndex = 5;
            this.iOpenSolution.Name = "iOpenSolution";
            // 
            // iCloseSolution
            // 
            this.iCloseSolution.Caption = "Close Solu&tion";
            this.iCloseSolution.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.iCloseSolution.Id = 15;
            this.iCloseSolution.ImageOptions.ImageIndex = 6;
            this.iCloseSolution.Name = "iCloseSolution";
            // 
            // iPageSetup
            // 
            this.iPageSetup.Caption = "Page Set&up...";
            this.iPageSetup.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.iPageSetup.Id = 16;
            this.iPageSetup.ImageOptions.ImageIndex = 8;
            this.iPageSetup.Name = "iPageSetup";
            // 
            // iPrint
            // 
            this.iPrint.Caption = "&Print...";
            this.iPrint.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.iPrint.Id = 17;
            this.iPrint.ImageOptions.ImageIndex = 9;
            this.iPrint.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P));
            this.iPrint.Name = "iPrint";
            // 
            // iExit
            // 
            this.iExit.Caption = "E&xit";
            this.iExit.CategoryGuid = new System.Guid("ec880574-4d2a-4f26-8779-903acfad8a52");
            this.iExit.Id = 18;
            this.iExit.Name = "iExit";
            // 
            // siFind
            // 
            this.siFind.Caption = "&Find and Replace";
            this.siFind.CategoryGuid = new System.Guid("ac82dbe7-c530-4aa2-b6de-94a7777426fe");
            this.siFind.Id = 27;
            this.siFind.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.iFind),
            new DevExpress.XtraBars.LinkPersistInfo(this.iReplace)});
            this.siFind.Name = "siFind";
            // 
            // iFind
            // 
            this.iFind.Caption = "&Find";
            this.iFind.CategoryGuid = new System.Guid("ac82dbe7-c530-4aa2-b6de-94a7777426fe");
            this.iFind.Id = 28;
            this.iFind.ImageOptions.ImageIndex = 16;
            this.iFind.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F));
            this.iFind.Name = "iFind";
            // 
            // iReplace
            // 
            this.iReplace.Caption = "R&eplace";
            this.iReplace.CategoryGuid = new System.Guid("ac82dbe7-c530-4aa2-b6de-94a7777426fe");
            this.iReplace.Id = 29;
            this.iReplace.ImageOptions.ImageIndex = 17;
            this.iReplace.ItemShortcut = new DevExpress.XtraBars.BarShortcut((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H));
            this.iReplace.Name = "iReplace";
            // 
            // iSolutionExplorer
            // 
            this.iSolutionExplorer.Caption = "Solution Ex&plorer";
            this.iSolutionExplorer.CategoryGuid = new System.Guid("0cb4cc3e-4798-4d61-9457-672bdc2a90d4");
            this.iSolutionExplorer.Hint = "Solution Explorer";
            this.iSolutionExplorer.Id = 37;
            this.iSolutionExplorer.ImageOptions.ImageIndex = 23;
            this.iSolutionExplorer.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.L));
            this.iSolutionExplorer.Name = "iSolutionExplorer";
            this.iSolutionExplorer.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iSolutionExplorer_ItemClick);
            // 
            // iProperties
            // 
            this.iProperties.Caption = "Properties &Window";
            this.iProperties.CategoryGuid = new System.Guid("0cb4cc3e-4798-4d61-9457-672bdc2a90d4");
            this.iProperties.Hint = "Properties Window";
            this.iProperties.Id = 38;
            this.iProperties.ImageOptions.ImageIndex = 24;
            this.iProperties.ItemShortcut = new DevExpress.XtraBars.BarShortcut(System.Windows.Forms.Keys.F4);
            this.iProperties.Name = "iProperties";
            this.iProperties.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iProperties_ItemClick);
            // 
            // iToolbox
            // 
            this.iToolbox.Caption = "Toolbo&x";
            this.iToolbox.CategoryGuid = new System.Guid("0cb4cc3e-4798-4d61-9457-672bdc2a90d4");
            this.iToolbox.Hint = "Toolbox";
            this.iToolbox.Id = 39;
            this.iToolbox.ImageOptions.ImageIndex = 25;
            this.iToolbox.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.X));
            this.iToolbox.Name = "iToolbox";
            // 
            // iClassView
            // 
            this.iClassView.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.DropDown;
            this.iClassView.Caption = "Cl&ass View";
            this.iClassView.CategoryGuid = new System.Guid("0cb4cc3e-4798-4d61-9457-672bdc2a90d4");
            this.iClassView.Hint = "Class View";
            this.iClassView.Id = 40;
            this.iClassView.ImageOptions.ImageIndex = 26;
            this.iClassView.ItemShortcut = new DevExpress.XtraBars.BarShortcut(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
                | System.Windows.Forms.Keys.C));
            this.iClassView.Name = "iClassView";
            // 
            // iTaskList
            // 
            this.iTaskList.Caption = "Task List";
            this.iTaskList.CategoryGuid = new System.Guid("0cb4cc3e-4798-4d61-9457-672bdc2a90d4");
            this.iTaskList.Hint = "Task List";
            this.iTaskList.Id = 68;
            this.iTaskList.ImageOptions.ImageIndex = 27;
            this.iTaskList.Name = "iTaskList";
            this.iTaskList.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iTaskList_ItemClick);
            // 
            // iFindResults
            // 
            this.iFindResults.Caption = "Find Results";
            this.iFindResults.CategoryGuid = new System.Guid("0cb4cc3e-4798-4d61-9457-672bdc2a90d4");
            this.iFindResults.Hint = "Find Results";
            this.iFindResults.Id = 69;
            this.iFindResults.ImageOptions.ImageIndex = 28;
            this.iFindResults.Name = "iFindResults";
            this.iFindResults.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iFindResults_ItemClick);
            // 
            // iOutput
            // 
            this.iOutput.Caption = "Output";
            this.iOutput.CategoryGuid = new System.Guid("0cb4cc3e-4798-4d61-9457-672bdc2a90d4");
            this.iOutput.Hint = "Output";
            this.iOutput.Id = 70;
            this.iOutput.ImageOptions.ImageIndex = 29;
            this.iOutput.Name = "iOutput";
            this.iOutput.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iOutput_ItemClick);
            // 
            // iLoadLayout
            // 
            this.iLoadLayout.Caption = "&Load Layout...";
            this.iLoadLayout.CategoryGuid = new System.Guid("f2b2eae8-5b98-43eb-81aa-d999b20fd3d3");
            this.iLoadLayout.Hint = "Load Layout";
            this.iLoadLayout.Id = 47;
            this.iLoadLayout.ImageOptions.ImageIndex = 35;
            this.iLoadLayout.Name = "iLoadLayout";
            // 
            // iSaveLayout
            // 
            this.iSaveLayout.Caption = "&Save Layout...";
            this.iSaveLayout.CategoryGuid = new System.Guid("f2b2eae8-5b98-43eb-81aa-d999b20fd3d3");
            this.iSaveLayout.Hint = "Save Layout";
            this.iSaveLayout.Id = 48;
            this.iSaveLayout.ImageOptions.ImageIndex = 34;
            this.iSaveLayout.Name = "iSaveLayout";
            // 
            // barDockingMenuItem1
            // 
            this.barDockingMenuItem1.Caption = "Window";
            this.barDockingMenuItem1.Id = 72;
            this.barDockingMenuItem1.Name = "barDockingMenuItem1";
            // 
            // iAbout
            // 
            this.iAbout.Caption = "&About";
            this.iAbout.Id = 74;
            this.iAbout.ImageOptions.ImageIndex = 36;
            this.iAbout.Name = "iAbout";
            this.iAbout.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.iAbout_ItemClick);
            // 
            // barHeaderItem1
            // 
            this.barHeaderItem1.Caption = "barHeaderItem1";
            this.barHeaderItem1.Id = 77;
            this.barHeaderItem1.Name = "barHeaderItem1";
            // 
            // barStaticItemBaslik
            // 
            this.barStaticItemBaslik.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
            this.barStaticItemBaslik.Caption = "Hastane Adı | ContaSoft XMIES";
            this.barStaticItemBaslik.Id = 78;
            this.barStaticItemBaslik.ItemAppearance.Normal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(124)))), ((int)(((byte)(140)))), ((int)(((byte)(192)))));
            this.barStaticItemBaslik.ItemAppearance.Normal.Options.UseBackColor = true;
            this.barStaticItemBaslik.LeftIndent = 21;
            this.barStaticItemBaslik.Name = "barStaticItemBaslik";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "barButtonItem2";
            this.barButtonItem2.Id = 80;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barSubItemKullanici
            // 
            this.barSubItemKullanici.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barSubItemKullanici.Caption = "ContaAdmin";
            this.barSubItemKullanici.Id = 81;
            this.barSubItemKullanici.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barSubItemKullanici.ItemAppearance.Normal.FontStyleDelta = System.Drawing.FontStyle.Underline;
            this.barSubItemKullanici.ItemAppearance.Normal.Options.UseFont = true;
            this.barSubItemKullanici.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItemSifreDegistir, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItemEkraniKilitle, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItemYenidenBaslat, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItemHatirlatici, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barToggleSwitchItemCanliMesajIstemiyorum, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.barSubItemKullanici.Name = "barSubItemKullanici";
            this.barSubItemKullanici.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            this.barSubItemKullanici.ShowMenuCaption = true;
            // 
            // barButtonItemSifreDegistir
            // 
            this.barButtonItemSifreDegistir.Caption = "Şifre Değiştir";
            this.barButtonItemSifreDegistir.Id = 82;
            this.barButtonItemSifreDegistir.Name = "barButtonItemSifreDegistir";
            // 
            // barButtonItemEkraniKilitle
            // 
            this.barButtonItemEkraniKilitle.Caption = "Ekranı Kilitle";
            this.barButtonItemEkraniKilitle.Id = 83;
            this.barButtonItemEkraniKilitle.Name = "barButtonItemEkraniKilitle";
            this.barButtonItemEkraniKilitle.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.BarButtonItemEkraniKilitle_ItemClick);
            // 
            // barButtonItemYenidenBaslat
            // 
            this.barButtonItemYenidenBaslat.Caption = "Yeniden Başlat";
            this.barButtonItemYenidenBaslat.Id = 84;
            this.barButtonItemYenidenBaslat.Name = "barButtonItemYenidenBaslat";
            // 
            // barButtonItemHatirlatici
            // 
            this.barButtonItemHatirlatici.Caption = "Hatırlatıcı";
            this.barButtonItemHatirlatici.Id = 85;
            this.barButtonItemHatirlatici.Name = "barButtonItemHatirlatici";
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Id = 131;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // barToggleSwitchItemCanliMesajIstemiyorum
            // 
            this.barToggleSwitchItemCanliMesajIstemiyorum.Caption = "Canlı Mesaj İstemiyorum";
            this.barToggleSwitchItemCanliMesajIstemiyorum.Edit = this.repositoryItemToggleSwitch2;
            this.barToggleSwitchItemCanliMesajIstemiyorum.Id = 130;
            this.barToggleSwitchItemCanliMesajIstemiyorum.Name = "barToggleSwitchItemCanliMesajIstemiyorum";
            // 
            // repositoryItemToggleSwitch2
            // 
            this.repositoryItemToggleSwitch2.AutoHeight = false;
            this.repositoryItemToggleSwitch2.Name = "repositoryItemToggleSwitch2";
            this.repositoryItemToggleSwitch2.OffText = "Off";
            this.repositoryItemToggleSwitch2.OnText = "On";
            // 
            // barButtonItem5
            // 
            this.barButtonItem5.Caption = "barButtonItem5";
            this.barButtonItem5.Id = 86;
            this.barButtonItem5.Name = "barButtonItem5";
            // 
            // barStaticItem2
            // 
            this.barStaticItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            this.barStaticItem2.Id = 89;
            this.barStaticItem2.Name = "barStaticItem2";
            // 
            // barButtonItem3
            // 
            this.barButtonItem3.Caption = "barButtonItem3";
            this.barButtonItem3.Id = 91;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // barButtonItem4
            // 
            this.barButtonItem4.Caption = "barButtonItem4";
            this.barButtonItem4.Id = 92;
            this.barButtonItem4.Name = "barButtonItem4";
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "barSubItem2";
            this.barSubItem2.Id = 93;
            this.barSubItem2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem6),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem7),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem8),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem9),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem10)});
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barButtonItem6
            // 
            this.barButtonItem6.Caption = "barButtonItem6";
            this.barButtonItem6.Id = 94;
            this.barButtonItem6.Name = "barButtonItem6";
            // 
            // barButtonItem7
            // 
            this.barButtonItem7.Caption = "barButtonItem7";
            this.barButtonItem7.Id = 95;
            this.barButtonItem7.Name = "barButtonItem7";
            // 
            // barButtonItem8
            // 
            this.barButtonItem8.Caption = "barButtonItem8";
            this.barButtonItem8.Id = 96;
            this.barButtonItem8.Name = "barButtonItem8";
            // 
            // barButtonItem9
            // 
            this.barButtonItem9.Caption = "barButtonItem9";
            this.barButtonItem9.Id = 97;
            this.barButtonItem9.Name = "barButtonItem9";
            // 
            // barButtonItem10
            // 
            this.barButtonItem10.Caption = "barButtonItem10";
            this.barButtonItem10.Id = 98;
            this.barButtonItem10.Name = "barButtonItem10";
            // 
            // barButtonItem25
            // 
            this.barButtonItem25.Caption = "İstatistik ve Analizler";
            this.barButtonItem25.Id = 121;
            this.barButtonItem25.Name = "barButtonItem25";
            // 
            // barButtonItem27
            // 
            this.barButtonItem27.Caption = "Sistem";
            this.barButtonItem27.Id = 123;
            this.barButtonItem27.Name = "barButtonItem27";
            // 
            // barHeaderItemAktifBaslik
            // 
            this.barHeaderItemAktifBaslik.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
            this.barHeaderItemAktifBaslik.Caption = "Hoş Geldiniz";
            this.barHeaderItemAktifBaslik.Id = 129;
            this.barHeaderItemAktifBaslik.ItemAppearance.Normal.FontStyleDelta = System.Drawing.FontStyle.Bold;
            this.barHeaderItemAktifBaslik.ItemAppearance.Normal.Options.UseFont = true;
            this.barHeaderItemAktifBaslik.Name = "barHeaderItemAktifBaslik";
            // 
            // barButtonItem11
            // 
            this.barButtonItem11.Caption = "barButtonItem11";
            this.barButtonItem11.Id = 136;
            this.barButtonItem11.Name = "barButtonItem11";
            // 
            // barButtonItem12
            // 
            this.barButtonItem12.Caption = "barButtonItem12";
            this.barButtonItem12.Id = 137;
            this.barButtonItem12.Name = "barButtonItem12";
            // 
            // barButtonItem13
            // 
            this.barButtonItem13.Caption = "barButtonItem13";
            this.barButtonItem13.Id = 138;
            this.barButtonItem13.Name = "barButtonItem13";
            // 
            // repositoryImageComboBox1
            // 
            this.repositoryImageComboBox1.AllowFocused = false;
            this.repositoryImageComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryImageComboBox1.Name = "repositoryImageComboBox1";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AllowFocused = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "barManager1"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemComboBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repositoryItemComboBox1_KeyDown);
            // 
            // repositoryItemImageComboBox1
            // 
            this.repositoryItemImageComboBox1.AutoHeight = false;
            this.repositoryItemImageComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemImageComboBox1.Name = "repositoryItemImageComboBox1";
            // 
            // repositoryItemSearchControl1
            // 
            this.repositoryItemSearchControl1.AutoHeight = false;
            this.repositoryItemSearchControl1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Repository.ClearButton(),
            new DevExpress.XtraEditors.Repository.SearchButton()});
            this.repositoryItemSearchControl1.Name = "repositoryItemSearchControl1";
            this.repositoryItemSearchControl1.NullValuePrompt = "Search Visual Studio";
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // repositoryItemToggleSwitch1
            // 
            this.repositoryItemToggleSwitch1.AutoHeight = false;
            this.repositoryItemToggleSwitch1.Name = "repositoryItemToggleSwitch1";
            this.repositoryItemToggleSwitch1.OffText = "Off";
            this.repositoryItemToggleSwitch1.OnText = "On";
            // 
            // popupMenu3
            // 
            this.popupMenu3.Manager = this.barManager;
            this.popupMenu3.Name = "popupMenu3";
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem11),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem12),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem13)});
            this.popupMenu1.Manager = this.barManager;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // documentManager
            // 
            this.documentManager.BarAndDockingController = this.barAndDockingController;
            this.documentManager.MdiParent = this;
            this.documentManager.MenuManager = this.barManager;
            this.documentManager.RibbonAndBarsMergeStyle = DevExpress.XtraBars.Docking2010.Views.RibbonAndBarsMergeStyle.Always;
            this.documentManager.ShowToolTips = DevExpress.Utils.DefaultBoolean.True;
            this.documentManager.View = this.tabbedView;
            this.documentManager.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView});
            this.documentManager.DocumentActivate += new DevExpress.XtraBars.Docking2010.Views.DocumentEventHandler(this.DocumentManager_DocumentActivate);
            // 
            // tabbedView
            // 
            this.tabbedView.DocumentProperties.AllowPin = true;
            this.tabbedView.DocumentSelectorProperties.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
            this.tabbedView.DocumentSelectorProperties.DocumentFooterFormat = "{0}\\{1}";
            this.tabbedView.DocumentSelectorProperties.DocumentHeaderFormat = "{0}<br>Source file";
            this.tabbedView.DocumentSelectorProperties.PanelFooterFormat = "";
            this.tabbedView.FloatingDocumentContainer = DevExpress.XtraBars.Docking2010.Views.FloatingDocumentContainer.DocumentsHost;
            // 
            // toolbarFormControl1
            // 
            this.toolbarFormControl1.Location = new System.Drawing.Point(0, 0);
            this.toolbarFormControl1.Manager = this.barManager;
            this.toolbarFormControl1.Name = "toolbarFormControl1";
            this.toolbarFormControl1.Size = new System.Drawing.Size(1528, 31);
            this.toolbarFormControl1.TabIndex = 13;
            this.toolbarFormControl1.TabStop = false;
            this.toolbarFormControl1.TitleItemLinks.Add(this.barStaticItem2);
            this.toolbarFormControl1.TitleItemLinks.Add(this.barSubItemKullanici);
            this.toolbarFormControl1.TitleItemLinks.Add(this.barStaticItemBaslik);
            this.toolbarFormControl1.TitleItemLinks.Add(this.barHeaderItemAktifBaslik);
            this.toolbarFormControl1.ToolbarForm = this;
            // 
            // barStaticItem3
            // 
            this.barStaticItem3.Caption = "00.00.00 00:00:00";
            this.barStaticItem3.Id = 112;
            this.barStaticItem3.Name = "barStaticItem3";
            // 
            // barHeaderItem2
            // 
            this.barHeaderItem2.Caption = "Giriş Zamanı:";
            this.barHeaderItem2.Id = 111;
            this.barHeaderItem2.Name = "barHeaderItem2";
            // 
            // timerForm
            // 
            this.timerForm.Enabled = true;
            this.timerForm.Interval = 1000;
            this.timerForm.Tick += new System.EventHandler(this.TimerForm_Tick);
            // 
            // transitionManager
            // 
            this.transitionManager.FrameCount = 100;
            this.transitionManager.FrameInterval = 1000;
            this.transitionManager.CustomTransition += new DevExpress.Utils.Animation.CustomTransitionEventHandler(this.transitionManager_CustomTransition);
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 2";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barHeaderItemAcikKalmaSuresi)});
            this.bar1.Text = "Custom 2";
            // 
            // popupMenu2
            // 
            this.popupMenu2.Manager = this.barManager;
            this.popupMenu2.Name = "popupMenu2";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1528, 659);
            this.Controls.Add(this.hideContainerBottom);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Controls.Add(this.toolbarFormControl1);
            this.DoubleBuffered = true;
            this.EnableAcrylicAccent = true;
            this.IsMdiContainer = true;
            this.Name = "FormMain";
            this.ShowText = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ContaSoft XMIES";
            this.ToolbarFormControl = this.toolbarFormControl1;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barAndDockingController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
            this.hideContainerBottom.ResumeLayout(false);
            this.panelContainer2.ResumeLayout(false);
            this.dockPanel5.ResumeLayout(false);
            this.dockPanel3.ResumeLayout(false);
            this.dockPanel4.ResumeLayout(false);
            this.dockPanel4_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textBox1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainMenuSvgImages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryImageComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemImageComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSearchControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemToggleSwitch1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.classViewSvgImages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fileTypeSvgImages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.toolbarFormControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraBars.ToolbarForm.ToolbarFormManager barManager;
        private DevExpress.XtraBars.BarSubItem siNew;
        private DevExpress.XtraBars.BarButtonItem iOpen;
        private DevExpress.XtraBars.BarButtonItem iClose;
        private DevExpress.XtraBars.BarButtonItem iProject;
        private DevExpress.XtraBars.BarButtonItem iFile;
        private DevExpress.XtraBars.BarButtonItem iBlankSolution;
        private DevExpress.XtraBars.BarButtonItem iAddExistingItem;
        private DevExpress.XtraBars.BarSubItem siAddProject;
        private DevExpress.XtraBars.BarButtonItem iNewProject;
        private DevExpress.XtraBars.BarButtonItem iExistingProject;
        private DevExpress.XtraBars.BarButtonItem iOpenSolution;
        private DevExpress.XtraBars.BarButtonItem iCloseSolution;
        private DevExpress.XtraBars.BarButtonItem iPageSetup;
        private DevExpress.XtraBars.BarButtonItem iPrint;
        private DevExpress.XtraBars.BarButtonItem iExit;
        private DevExpress.XtraBars.BarSubItem siFind;
        private DevExpress.XtraBars.BarButtonItem iFind;
        private DevExpress.XtraBars.BarButtonItem iReplace;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryImageComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.BarButtonItem iSolutionExplorer;
        private DevExpress.XtraBars.BarButtonItem iProperties;
        private DevExpress.XtraBars.BarButtonItem iToolbox;
        private DevExpress.XtraBars.BarButtonItem iClassView;
        private DevExpress.XtraEditors.MemoEdit textBox1;
        private DevExpress.XtraBars.BarButtonItem iTaskList;
        private DevExpress.XtraBars.BarButtonItem iFindResults;
        private DevExpress.XtraBars.BarButtonItem iOutput;
        private DevExpress.XtraBars.BarButtonItem iLoadLayout;
        private DevExpress.XtraBars.BarButtonItem iSaveLayout;
        private DevExpress.XtraBars.Docking.DockManager dockManager;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel3;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel3_Container;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel4;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel4_Container;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel5;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel5_Container;
        private DevExpress.XtraBars.Docking.DockPanel panelContainer2;
        private DevExpress.XtraBars.BarAndDockingController barAndDockingController;
        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox repositoryItemImageComboBox1;
        public DevExpress.XtraBars.Docking2010.DocumentManager documentManager;
        public DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView;
        private BarDockingMenuItem barDockingMenuItem1;
        private BarButtonItem iAbout;
        private BarButtonItem barButtonItem1;
        private DevExpress.Utils.SvgImageCollection classViewSvgImages;
        private DevExpress.Utils.SvgImageCollection fileTypeSvgImages;
        private DevExpress.Utils.SvgImageCollection mainMenuSvgImages;
        public ToolbarFormControl toolbarFormControl1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSearchControl repositoryItemSearchControl1;
        private BarStaticItem barStaticItemBaslik;
        private BarHeaderItem barHeaderItem1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private BarButtonItem barButtonItem2;
        private BarSubItem barSubItemKullanici;
        private BarButtonItem barButtonItemSifreDegistir;
        private BarButtonItem barButtonItemEkraniKilitle;
        private BarButtonItem barButtonItemYenidenBaslat;
        private BarButtonItem barButtonItemHatirlatici;
        private BarButtonItem barButtonItem5;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch repositoryItemToggleSwitch1;
        private BarStaticItem barStaticItem2;
        private BarButtonItem barButtonItem3;
        private BarButtonItem barButtonItem4;
        private BarSubItem barSubItem2;
        private BarButtonItem barButtonItem6;
        private BarButtonItem barButtonItem7;
        private BarButtonItem barButtonItem8;
        private BarButtonItem barButtonItem9;
        private BarButtonItem barButtonItem10;
        private BarHeaderItem barHeaderItemGirisZamani;
        private BarStaticItem barStaticItemGirisZamani;
        private BarHeaderItem barHeaderItemVersiyon;
        private BarStaticItem barStaticItemVersiyon;
        private BarHeaderItem barHeaderItemIp;
        private BarStaticItem barStaticItemIp;
        private BarButtonItem barButtonItem25;
        private BarButtonItem barButtonItem27;
        private BarStaticItem barHeaderItemAktifBaslik;
        private BarStaticItem barStaticItem1;
        private BarEditItem barToggleSwitchItemCanliMesajIstemiyorum;
        private DevExpress.XtraEditors.Repository.RepositoryItemToggleSwitch repositoryItemToggleSwitch2;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private BarHeaderItem barHeaderItemAcikKalmaSuresi;
        private BarStaticItem barStaticItemAcikKalmaSuresi;
        private BarStaticItem barStaticItem3;
        private BarHeaderItem barHeaderItem2;
        private System.Windows.Forms.Timer timerForm;
        private PopupMenu popupMenu1;
        private BarButtonItem barButtonItem11;
        private BarButtonItem barButtonItem12;
        private BarButtonItem barButtonItem13;
        private AlertControl alertControlZorunluCikis;
        private AlertControl alertControl;
        private BarHeaderItem barHeaderItemKurum;
        private BarStaticItem barStaticItemKurum;
        private BarHeaderItem barHeaderItemBilgisayar;
        private BarStaticItem barStaticItemBilgisayar;
        public DevExpress.Utils.Animation.TransitionManager transitionManager;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private Bar statusBar;
        public Bar topBar;
        private Bar bar1;
        private PopupMenu popupMenu2;
        private DevExpress.XtraBars.Docking.AutoHideContainer hideContainerBottom;
        private PopupMenu popupMenu3;
    }
}