namespace SR_Editor.Modules.SqlQuery.Action
{
    using DevExpress.Data;
    using DevExpress.Utils;
    using DevExpress.Utils.Menu;
    using DevExpress.XtraBars;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.Controls;
    using DevExpress.XtraEditors.DXErrorProvider;
    using DevExpress.XtraEditors.Mask;
    using DevExpress.XtraEditors.Repository;
    using DevExpress.XtraGrid;
    using DevExpress.XtraGrid.Columns;
    using DevExpress.XtraGrid.Views.Base;
    using DevExpress.XtraGrid.Views.Grid;
    using DevExpress.XtraLayout;
    using SR_Editor.Utility;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    partial class FormSqlTanimCalistir
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private LayoutControl layoutControlForm;
        private PanelControl panelControl1;
        private LayoutControlGroup layoutControlGroup1;
        private BarManager barManager1;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private BindingSource bindingSourceBlokKatUnite;
        private LayoutControlItem layoutControlItem3;
        private RepositoryItemLookUpEdit repositoryItemLookUpEditBinaBirimTipiId;
        private RepositoryItemLookUpEdit repositoryItemLookUpEditBirimOzellikTipiId;
        private GridControl gridControlParametreler;
        private GridView gridViewParametreler;
        private LayoutControlGroup layoutControlGroup3;
        private LayoutControlItem layoutControlItem5;
        private PanelControl panelControl2;
        private LayoutControlItem layoutControlItem6;
        private StandaloneBarDockControl standaloneBarDockControl1;
        private Bar bar1;
        private BarButtonItem barButtonItemKaydet;
        private BarButtonItem barButtonItemIptal;
        private EmptySpaceItem emptySpaceItem1;
        private TextEdit textEditBaslik;
        private LayoutControlItem layoutControlItem8;
        private BindingSource bindingSourceSqlTanimListe;
        private BindingSource bindingSourceSqlEkran;
        private XtraScrollableControl xtraScrollableControlParametreler;
        private LookUpEdit lookUpEditSqlTanimKategori;
        private LayoutControlItem layoutControlItemSqlTanimKategori;
        private BarButtonItem barButtonItemCalistir;
        private LayoutControlGroup layoutControlGroup2;
        private BarButtonItem barButtonItemParametreOlustur;
        private SimpleSeparator simpleSeparator1;
        private LayoutControlGroup layoutControlGroup4;
        private LayoutControlItem layoutControlItem9;
        private SplitterItem splitterItem4;
        private BarButtonItem barButtonItemMailAt;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSqlTanimCalistir));
            this.layoutControlForm = new DevExpress.XtraLayout.LayoutControl();
            this.xtraScrollableControlKosullar = new DevExpress.XtraEditors.XtraScrollableControl();
            this.simpleButtonKosulEkle = new DevExpress.XtraEditors.SimpleButton();
            this.richEditControlSorgu = new DevExpress.XtraRichEdit.RichEditControl();
            this.bindingSourceSqlEkran = new System.Windows.Forms.BindingSource(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonItemKaydet = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemCalistir = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemIptal = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemParametreOlustur = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemKosullariTestEt = new DevExpress.XtraBars.BarButtonItem();
            this.standaloneBarDockControl1 = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItemMailAt = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemHesabiSorgula = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemKarakteriSorgula = new DevExpress.XtraBars.BarButtonItem();
            this.lookUpEditServer = new DevExpress.XtraEditors.LookUpEdit();
            this.lookUpEditVeritabani = new DevExpress.XtraEditors.LookUpEdit();
            this.textEditAciklama = new DevExpress.XtraEditors.TextEdit();
            this.lookUpEditSqlTanimKategori = new DevExpress.XtraEditors.LookUpEdit();
            this.xtraScrollableControlParametreler = new DevExpress.XtraEditors.XtraScrollableControl();
            this.textEditBaslik = new DevExpress.XtraEditors.TextEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.gridControlParametreler = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemHesabiSorgula = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemKarakteriSorgula = new System.Windows.Forms.ToolStripMenuItem();
            this.gridViewParametreler = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup5 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.splitterItem4 = new DevExpress.XtraLayout.SplitterItem();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemSqlTanimKategori = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.simpleSeparator1 = new DevExpress.XtraLayout.SimpleSeparator();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemServer = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.bindingSourceShard = new System.Windows.Forms.BindingSource(this.components);
            this.repositoryItemLookUpEditBinaBirimTipiId = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemLookUpEditBirimOzellikTipiId = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.bindingSourceSqlTanimListe = new System.Windows.Forms.BindingSource(this.components);
            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlForm)).BeginInit();
            this.layoutControlForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSqlEkran)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditServer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditVeritabani.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditAciklama.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditSqlTanimKategori.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditBaslik.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlParametreler)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewParametreler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSqlTanimKategori)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemServer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceShard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditBinaBirimTipiId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditBirimOzellikTipiId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSqlTanimListe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlForm
            // 
            this.layoutControlForm.Controls.Add(this.xtraScrollableControlKosullar);
            this.layoutControlForm.Controls.Add(this.simpleButtonKosulEkle);
            this.layoutControlForm.Controls.Add(this.richEditControlSorgu);
            this.layoutControlForm.Controls.Add(this.lookUpEditServer);
            this.layoutControlForm.Controls.Add(this.lookUpEditVeritabani);
            this.layoutControlForm.Controls.Add(this.textEditAciklama);
            this.layoutControlForm.Controls.Add(this.lookUpEditSqlTanimKategori);
            this.layoutControlForm.Controls.Add(this.xtraScrollableControlParametreler);
            this.layoutControlForm.Controls.Add(this.textEditBaslik);
            this.layoutControlForm.Controls.Add(this.panelControl2);
            this.layoutControlForm.Controls.Add(this.gridControlParametreler);
            this.layoutControlForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlForm.HiddenItems.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.layoutControlGroup5,
            this.layoutControlItem4});
            this.layoutControlForm.Location = new System.Drawing.Point(0, 0);
            this.layoutControlForm.Name = "layoutControlForm";
            this.layoutControlForm.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(453, 258, 250, 350);
            this.layoutControlForm.Root = this.layoutControlGroup1;
            this.layoutControlForm.Size = new System.Drawing.Size(1284, 699);
            this.layoutControlForm.TabIndex = 0;
            this.layoutControlForm.Text = "layoutControl1";
            // 
            // xtraScrollableControlKosullar
            // 
            this.xtraScrollableControlKosullar.Location = new System.Drawing.Point(765, 66);
            this.xtraScrollableControlKosullar.Name = "xtraScrollableControlKosullar";
            this.xtraScrollableControlKosullar.Size = new System.Drawing.Size(572, 403);
            this.xtraScrollableControlKosullar.TabIndex = 25;
            // 
            // simpleButtonKosulEkle
            // 
            this.simpleButtonKosulEkle.Location = new System.Drawing.Point(765, 40);
            this.simpleButtonKosulEkle.Name = "simpleButtonKosulEkle";
            this.simpleButtonKosulEkle.Size = new System.Drawing.Size(572, 22);
            this.simpleButtonKosulEkle.StyleController = this.layoutControlForm;
            this.simpleButtonKosulEkle.TabIndex = 24;
            this.simpleButtonKosulEkle.Text = "Koşul Ekle";
            this.simpleButtonKosulEkle.Click += new System.EventHandler(this.simpleButtonKosulEkle_Click);
            // 
            // richEditControlSorgu
            // 
            this.richEditControlSorgu.ActiveViewType = DevExpress.XtraRichEdit.RichEditViewType.Simple;
            this.richEditControlSorgu.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceSqlEkran, "Sql", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.richEditControlSorgu.LayoutUnit = DevExpress.XtraRichEdit.DocumentLayoutUnit.Pixel;
            this.richEditControlSorgu.Location = new System.Drawing.Point(123, 140);
            this.richEditControlSorgu.MenuManager = this.barManager1;
            this.richEditControlSorgu.Name = "richEditControlSorgu";
            this.richEditControlSorgu.Options.HorizontalRuler.Visibility = DevExpress.XtraRichEdit.RichEditRulerVisibility.Hidden;
            this.richEditControlSorgu.Size = new System.Drawing.Size(1218, 160);
            this.richEditControlSorgu.TabIndex = 23;
            this.richEditControlSorgu.RtfTextChanged += new System.EventHandler(this.richEditControl1_RtfTextChanged);
            // 
            // bindingSourceSqlEkran
            // 
            this.bindingSourceSqlEkran.DataSource = typeof(RoyaleSupport.SqlDefinitionDto);
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.DockControls.Add(this.standaloneBarDockControl1);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItemKaydet,
            this.barButtonItemIptal,
            this.barButtonItemCalistir,
            this.barButtonItemParametreOlustur,
            this.barButtonItemMailAt,
            this.barButtonItem1,
            this.barButtonItemKosullariTestEt,
            this.barButtonItemHesabiSorgula,
            this.barButtonItemKarakteriSorgula});
            this.barManager1.MaxItemId = 31;
            // 
            // bar1
            // 
            this.bar1.BarName = "Custom 2";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItemKaydet, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItemCalistir, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItemIptal, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItemParametreOlustur, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.barButtonItemKosullariTestEt, "", false, true, false, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
            this.bar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            this.bar1.Text = "Custom 2";
            // 
            // barButtonItemKaydet
            // 
            this.barButtonItemKaydet.Caption = "Kaydet";
            this.barButtonItemKaydet.Id = 19;
            this.barButtonItemKaydet.Name = "barButtonItemKaydet";
            this.barButtonItemKaydet.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemKaydet_ItemClick);
            // 
            // barButtonItemCalistir
            // 
            this.barButtonItemCalistir.Caption = "Çalıştır";
            this.barButtonItemCalistir.Id = 23;
            this.barButtonItemCalistir.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItemCalistir.ImageOptions.Image")));
            this.barButtonItemCalistir.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItemCalistir.ImageOptions.LargeImage")));
            this.barButtonItemCalistir.Name = "barButtonItemCalistir";
            this.barButtonItemCalistir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemCalistir_ItemClick);
            // 
            // barButtonItemIptal
            // 
            this.barButtonItemIptal.Caption = "İptal";
            this.barButtonItemIptal.Id = 20;
            this.barButtonItemIptal.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barButtonItemIptal.ImageOptions.Image")));
            this.barButtonItemIptal.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItemIptal.ImageOptions.LargeImage")));
            this.barButtonItemIptal.Name = "barButtonItemIptal";
            this.barButtonItemIptal.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemIptal_ItemClick);
            // 
            // barButtonItemParametreOlustur
            // 
            this.barButtonItemParametreOlustur.Caption = "Parametre Oluştur";
            this.barButtonItemParametreOlustur.Id = 25;
            this.barButtonItemParametreOlustur.Name = "barButtonItemParametreOlustur";
            this.barButtonItemParametreOlustur.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemParametreOlustur_ItemClick);
            // 
            // barButtonItemKosullariTestEt
            // 
            this.barButtonItemKosullariTestEt.Caption = "Koşulları Test Et";
            this.barButtonItemKosullariTestEt.Id = 28;
            this.barButtonItemKosullariTestEt.Name = "barButtonItemKosullariTestEt";
            this.barButtonItemKosullariTestEt.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemKosullariTestEt_ItemClick);
            // 
            // standaloneBarDockControl1
            // 
            this.standaloneBarDockControl1.CausesValidation = false;
            this.standaloneBarDockControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.standaloneBarDockControl1.Location = new System.Drawing.Point(2, 2);
            this.standaloneBarDockControl1.Manager = this.barManager1;
            this.standaloneBarDockControl1.MaximumSize = new System.Drawing.Size(0, 25);
            this.standaloneBarDockControl1.MinimumSize = new System.Drawing.Size(0, 25);
            this.standaloneBarDockControl1.Name = "standaloneBarDockControl1";
            this.standaloneBarDockControl1.Size = new System.Drawing.Size(524, 25);
            this.standaloneBarDockControl1.Text = "standaloneBarDockControl1";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(1284, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 699);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(1284, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 699);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1284, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 699);
            // 
            // barButtonItemMailAt
            // 
            this.barButtonItemMailAt.Caption = "Mail At";
            this.barButtonItemMailAt.Id = 26;
            this.barButtonItemMailAt.Name = "barButtonItemMailAt";
            this.barButtonItemMailAt.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItemMailAt.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemMailAt_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "barButtonItem1";
            this.barButtonItem1.Id = 27;
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItemHesabiSorgula
            // 
            this.barButtonItemHesabiSorgula.Caption = "Hesabı Sorgula";
            this.barButtonItemHesabiSorgula.Id = 29;
            this.barButtonItemHesabiSorgula.Name = "barButtonItemHesabiSorgula";
            // 
            // barButtonItemKarakteriSorgula
            // 
            this.barButtonItemKarakteriSorgula.Caption = "Karakteri Sorgula";
            this.barButtonItemKarakteriSorgula.Id = 30;
            this.barButtonItemKarakteriSorgula.Name = "barButtonItemKarakteriSorgula";
            // 
            // lookUpEditServer
            // 
            this.lookUpEditServer.Location = new System.Drawing.Point(132, 92);
            this.lookUpEditServer.MenuManager = this.barManager1;
            this.lookUpEditServer.Name = "lookUpEditServer";
            this.lookUpEditServer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditServer.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("ID", "ID", 13, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name", 40, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.lookUpEditServer.Size = new System.Drawing.Size(395, 20);
            this.lookUpEditServer.StyleController = this.layoutControlForm;
            this.lookUpEditServer.TabIndex = 22;
            this.lookUpEditServer.EditValueChanged += new System.EventHandler(this.lookUpEditServer_EditValueChanged);
            // 
            // lookUpEditVeritabani
            // 
            this.lookUpEditVeritabani.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bindingSourceSqlEkran, "DatabaseType", true));
            this.lookUpEditVeritabani.Location = new System.Drawing.Point(132, 116);
            this.lookUpEditVeritabani.MenuManager = this.barManager1;
            this.lookUpEditVeritabani.Name = "lookUpEditVeritabani";
            this.lookUpEditVeritabani.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditVeritabani.Properties.DisplayMember = "Aciklama";
            this.lookUpEditVeritabani.Properties.ReadOnly = true;
            this.lookUpEditVeritabani.Properties.ValueMember = "Id";
            this.lookUpEditVeritabani.Size = new System.Drawing.Size(395, 20);
            this.lookUpEditVeritabani.StyleController = this.layoutControlForm;
            this.lookUpEditVeritabani.TabIndex = 21;
            // 
            // textEditAciklama
            // 
            this.textEditAciklama.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bindingSourceSqlEkran, "Description", true));
            this.textEditAciklama.Location = new System.Drawing.Point(132, 140);
            this.textEditAciklama.MenuManager = this.barManager1;
            this.textEditAciklama.Name = "textEditAciklama";
            this.textEditAciklama.Properties.ReadOnly = true;
            this.textEditAciklama.Size = new System.Drawing.Size(395, 20);
            this.textEditAciklama.StyleController = this.layoutControlForm;
            this.textEditAciklama.TabIndex = 20;
            // 
            // lookUpEditSqlTanimKategori
            // 
            this.lookUpEditSqlTanimKategori.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bindingSourceSqlEkran, "SqlDefinitionCategoryId", true));
            this.lookUpEditSqlTanimKategori.Location = new System.Drawing.Point(132, 68);
            this.lookUpEditSqlTanimKategori.MenuManager = this.barManager1;
            this.lookUpEditSqlTanimKategori.Name = "lookUpEditSqlTanimKategori";
            this.lookUpEditSqlTanimKategori.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditSqlTanimKategori.Properties.DisplayMember = "Adi";
            this.lookUpEditSqlTanimKategori.Properties.ReadOnly = true;
            this.lookUpEditSqlTanimKategori.Properties.ValueMember = "Id";
            this.lookUpEditSqlTanimKategori.Size = new System.Drawing.Size(196, 20);
            this.lookUpEditSqlTanimKategori.StyleController = this.layoutControlForm;
            this.lookUpEditSqlTanimKategori.TabIndex = 19;
            // 
            // xtraScrollableControlParametreler
            // 
            this.xtraScrollableControlParametreler.Location = new System.Drawing.Point(547, 36);
            this.xtraScrollableControlParametreler.Name = "xtraScrollableControlParametreler";
            this.xtraScrollableControlParametreler.Size = new System.Drawing.Size(722, 135);
            this.xtraScrollableControlParametreler.TabIndex = 18;
            // 
            // textEditBaslik
            // 
            this.textEditBaslik.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bindingSourceSqlEkran, "Name", true));
            this.textEditBaslik.Location = new System.Drawing.Point(332, 68);
            this.textEditBaslik.MenuManager = this.barManager1;
            this.textEditBaslik.Name = "textEditBaslik";
            this.textEditBaslik.Properties.ReadOnly = true;
            this.textEditBaslik.Size = new System.Drawing.Size(195, 20);
            this.textEditBaslik.StyleController = this.layoutControlForm;
            this.textEditBaslik.TabIndex = 16;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.standaloneBarDockControl1);
            this.panelControl2.Location = new System.Drawing.Point(7, 7);
            this.panelControl2.MaximumSize = new System.Drawing.Size(0, 29);
            this.panelControl2.MinimumSize = new System.Drawing.Size(0, 29);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(528, 29);
            this.panelControl2.TabIndex = 10;
            // 
            // gridControlParametreler
            // 
            this.gridControlParametreler.ContextMenuStrip = this.contextMenuStrip1;
            this.gridControlParametreler.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gridControlParametreler.Location = new System.Drawing.Point(10, 207);
            this.gridControlParametreler.MainView = this.gridViewParametreler;
            this.gridControlParametreler.MenuManager = this.barManager1;
            this.gridControlParametreler.Name = "gridControlParametreler";
            this.gridControlParametreler.Size = new System.Drawing.Size(1264, 482);
            this.gridControlParametreler.TabIndex = 8;
            this.gridControlParametreler.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewParametreler});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemHesabiSorgula,
            this.toolStripMenuItemKarakteriSorgula});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 70);
            this.contextMenuStrip1.Text = "İşlemler";
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
            this.contextMenuStrip1.Click += new System.EventHandler(this.contextMenuStrip1_Click);
            // 
            // toolStripMenuItemHesabiSorgula
            // 
            this.toolStripMenuItemHesabiSorgula.Name = "toolStripMenuItemHesabiSorgula";
            this.toolStripMenuItemHesabiSorgula.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemHesabiSorgula.Tag = "Hesap";
            this.toolStripMenuItemHesabiSorgula.Text = "Hesabı Sorgula";
            // 
            // toolStripMenuItemKarakteriSorgula
            // 
            this.toolStripMenuItemKarakteriSorgula.Name = "toolStripMenuItemKarakteriSorgula";
            this.toolStripMenuItemKarakteriSorgula.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItemKarakteriSorgula.Tag = "Karakter";
            this.toolStripMenuItemKarakteriSorgula.Text = "Karakteri Sorgula";
            // 
            // gridViewParametreler
            // 
            this.gridViewParametreler.GridControl = this.gridControlParametreler;
            this.gridViewParametreler.Name = "gridViewParametreler";
            this.gridViewParametreler.OptionsBehavior.Editable = false;
            this.gridViewParametreler.OptionsSelection.InvertSelection = true;
            this.gridViewParametreler.OptionsView.ColumnAutoWidth = false;
            this.gridViewParametreler.OptionsView.ShowAutoFilterRow = true;
            this.gridViewParametreler.OptionsView.ShowFooter = true;
            this.gridViewParametreler.OptionsView.ShowGroupPanel = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 108);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(655, 108);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlGroup5
            // 
            this.layoutControlGroup5.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem7,
            this.layoutControlItem10});
            this.layoutControlGroup5.Location = new System.Drawing.Point(746, 0);
            this.layoutControlGroup5.Name = "layoutControlGroup5";
            this.layoutControlGroup5.Size = new System.Drawing.Size(600, 478);
            this.layoutControlGroup5.Text = "Koşullar";
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.simpleButtonKosulEkle;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(576, 26);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.xtraScrollableControlKosullar;
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 26);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(576, 407);
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.richEditControlSorgu;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(1330, 164);
            this.layoutControlItem4.Text = "Sorgu";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(105, 13);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Root";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem6,
            this.layoutControlGroup2,
            this.splitterItem4,
            this.layoutControlGroup3,
            this.layoutControlGroup4});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1284, 699);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.panelControl2;
            this.layoutControlItem6.CustomizationFormText = "layoutControlItem6";
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem6.MaxSize = new System.Drawing.Size(0, 29);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(24, 29);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(532, 29);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlGroup2.AppearanceGroup.Options.UseFont = true;
            this.layoutControlGroup2.CustomizationFormText = "Sorgu Sonucu";
            this.layoutControlGroup2.ExpandButtonVisible = true;
            this.layoutControlGroup2.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 176);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.Size = new System.Drawing.Size(1274, 513);
            this.layoutControlGroup2.Text = "Sorgu Sonucu";
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.gridControlParametreler;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(1268, 486);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // splitterItem4
            // 
            this.splitterItem4.AllowHotTrack = true;
            this.splitterItem4.CustomizationFormText = "splitterItem4";
            this.splitterItem4.Location = new System.Drawing.Point(0, 166);
            this.splitterItem4.Name = "splitterItem4";
            this.splitterItem4.ResizeMode = DevExpress.XtraLayout.SplitterItemResizeMode.AllSiblings;
            this.splitterItem4.Size = new System.Drawing.Size(532, 10);
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlGroup3.AppearanceGroup.Options.UseFont = true;
            this.layoutControlGroup3.CustomizationFormText = "SQL  Kayıt";
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemSqlTanimKategori,
            this.layoutControlItem8,
            this.simpleSeparator1,
            this.layoutControlItem1,
            this.layoutControlItemServer,
            this.layoutControlItem2});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 29);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroup3.Size = new System.Drawing.Size(532, 137);
            this.layoutControlGroup3.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 5, 2);
            this.layoutControlGroup3.Text = "Sorgu Bilgileri";
            // 
            // layoutControlItemSqlTanimKategori
            // 
            this.layoutControlItemSqlTanimKategori.Control = this.lookUpEditSqlTanimKategori;
            this.layoutControlItemSqlTanimKategori.CustomizationFormText = "Kategori";
            this.layoutControlItemSqlTanimKategori.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemSqlTanimKategori.Name = "layoutControlItemSqlTanimKategori";
            this.layoutControlItemSqlTanimKategori.Size = new System.Drawing.Size(317, 24);
            this.layoutControlItemSqlTanimKategori.Text = " Kategori - Sorgu Adı  ";
            this.layoutControlItemSqlTanimKategori.TextSize = new System.Drawing.Size(105, 13);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.textEditBaslik;
            this.layoutControlItem8.CustomizationFormText = "Kayıt Başlık";
            this.layoutControlItem8.Location = new System.Drawing.Point(317, 0);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(199, 24);
            this.layoutControlItem8.Text = "Kayıt Başlık";
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // simpleSeparator1
            // 
            this.simpleSeparator1.AllowHotTrack = false;
            this.simpleSeparator1.CustomizationFormText = "simpleSeparator1";
            this.simpleSeparator1.Location = new System.Drawing.Point(0, 96);
            this.simpleSeparator1.Name = "simpleSeparator1";
            this.simpleSeparator1.Size = new System.Drawing.Size(516, 1);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.textEditAciklama;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(516, 24);
            this.layoutControlItem1.Text = "Açıklama";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(105, 13);
            // 
            // layoutControlItemServer
            // 
            this.layoutControlItemServer.Control = this.lookUpEditServer;
            this.layoutControlItemServer.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItemServer.Name = "layoutControlItemServer";
            this.layoutControlItemServer.Size = new System.Drawing.Size(516, 24);
            this.layoutControlItemServer.Text = "Server";
            this.layoutControlItemServer.TextSize = new System.Drawing.Size(105, 13);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.lookUpEditVeritabani;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(516, 24);
            this.layoutControlItem2.Text = "Veritabanı";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(105, 13);
            // 
            // layoutControlGroup4
            // 
            this.layoutControlGroup4.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlGroup4.AppearanceGroup.Options.UseFont = true;
            this.layoutControlGroup4.CustomizationFormText = "Parametre Tipleri";
            this.layoutControlGroup4.HeaderButtonsLocation = DevExpress.Utils.GroupElementLocation.AfterText;
            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem9});
            this.layoutControlGroup4.Location = new System.Drawing.Point(532, 0);
            this.layoutControlGroup4.Name = "layoutControlGroup4";
            this.layoutControlGroup4.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroup4.Size = new System.Drawing.Size(742, 176);
            this.layoutControlGroup4.Text = "Parametre Tipleri";
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.xtraScrollableControlParametreler;
            this.layoutControlItem9.CustomizationFormText = "layoutControlItem9";
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(726, 139);
            this.layoutControlItem9.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem9.TextVisible = false;
            // 
            // bindingSourceShard
            // 
            this.bindingSourceShard.DataSource = typeof(RoyaleSupport.ShardDto);
            // 
            // repositoryItemLookUpEditBinaBirimTipiId
            // 
            this.repositoryItemLookUpEditBinaBirimTipiId.AutoHeight = false;
            this.repositoryItemLookUpEditBinaBirimTipiId.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEditBinaBirimTipiId.Name = "repositoryItemLookUpEditBinaBirimTipiId";
            // 
            // repositoryItemLookUpEditBirimOzellikTipiId
            // 
            this.repositoryItemLookUpEditBirimOzellikTipiId.AutoHeight = false;
            this.repositoryItemLookUpEditBirimOzellikTipiId.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEditBirimOzellikTipiId.Name = "repositoryItemLookUpEditBirimOzellikTipiId";
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(601, 45);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // bindingSourceSqlTanimListe
            // 
            this.bindingSourceSqlTanimListe.DataSource = typeof(RoyaleSupport.SqlDefinitionDto);
            // 
            // popupMenu1
            // 
            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemHesabiSorgula),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemKarakteriSorgula)});
            this.popupMenu1.Manager = this.barManager1;
            this.popupMenu1.Name = "popupMenu1";
            // 
            // FormSqlTanimCalistir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 699);
            this.Controls.Add(this.layoutControlForm);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FormSqlTanimCalistir";
            this.Text = "FormSqlTanim";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlForm)).EndInit();
            this.layoutControlForm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSqlEkran)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditServer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditVeritabani.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditAciklama.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditSqlTanimKategori.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditBaslik.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlParametreler)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewParametreler)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemSqlTanimKategori)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.simpleSeparator1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemServer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceShard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditBinaBirimTipiId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditBirimOzellikTipiId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSqlTanimListe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextEdit textEditAciklama;
        private LayoutControlItem layoutControlItem1;
        private LookUpEdit lookUpEditServer;
        private LookUpEdit lookUpEditVeritabani;
        private LayoutControlItem layoutControlItemServer;
        private LayoutControlItem layoutControlItem2;
        private BindingSource bindingSourceShard;
        private DevExpress.XtraRichEdit.RichEditControl richEditControlSorgu;
        private LayoutControlItem layoutControlItem4;
        private BarButtonItem barButtonItem1;
        private LayoutControlGroup layoutControlGroup5;
        private SimpleButton simpleButtonKosulEkle;
        private LayoutControlItem layoutControlItem7;
        private XtraScrollableControl xtraScrollableControlKosullar;
        private LayoutControlItem layoutControlItem10;
        private BarButtonItem barButtonItemKosullariTestEt;
        private BarButtonItem barButtonItemHesabiSorgula;
        private BarButtonItem barButtonItemKarakteriSorgula;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem toolStripMenuItemHesabiSorgula;
        private ToolStripMenuItem toolStripMenuItemKarakteriSorgula;
        private PopupMenu popupMenu1;
    }
}