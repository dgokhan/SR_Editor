using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SR_Editor.Modules.SqlQuery.Action
{
    partial class FormSqlTanimListesi
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
        private SplitterItem splitterItem1;
        private BindingSource bindingSourceBlokKatUnite;
        private LayoutControlItem layoutControlItem3;
        private RepositoryItemLookUpEdit repositoryItemLookUpEditBinaBirimTipiId;
        private RepositoryItemLookUpEdit repositoryItemLookUpEditBirimOzellikTipiId;
        private SplitterItem splitterItem2;
        private GridControl gridControlSqlTanimListe;
        private GridView gridViewSqlTanimListe;
        private LayoutControlGroup layoutControlGroupSorguListesi;
        private LayoutControlItem layoutControlItem5;
        private BarButtonItem barButtonItemKaydet;
        private BarButtonItem barButtonItemIptal;
        private EmptySpaceItem emptySpaceItem1;
        private BindingSource bindingSourceSqlTanimListe;
        private BarButtonItem barButtonItemKategoriEkle;
        private PopupMenu popupMenuSqlTanimKategori;
        private BarButtonItem barButtonItemSil;
        private BindingSource bindingSourceSqlTanimKategori;
        private TreeList treeListSqlTanimKategori;
        private TreeListColumn colAdi;
        private LayoutControlGroup layoutControlGroup2;
        private LayoutControlItem layoutControlItem1;
        private GridColumn colAd;
        private GridColumn colSql;
        private GridColumn colCreatedDate;
        private PopupMenu popupMenuSqlTanim;
        private BarButtonItem barButtonItemSqlTanimEkle;
        private BarButtonItem barButtonItemSqlTanimSil;

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
            this.layoutControlForm = new DevExpress.XtraLayout.LayoutControl();
            this.treeListSqlTanimKategori = new DevExpress.XtraTreeList.TreeList();
            this.colAdi = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.bindingSourceSqlTanimKategori = new System.Windows.Forms.BindingSource(this.components);
            this.gridControlSqlTanimListe = new DevExpress.XtraGrid.GridControl();
            this.bindingSourceSqlTanimListe = new System.Windows.Forms.BindingSource(this.components);
            this.gridViewSqlTanimListe = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colAd = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSql = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreatedDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItemKaydet = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemIptal = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemKategoriEkle = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemSil = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemSqlTanimEkle = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemSqlTanimSil = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemDuzenle = new DevExpress.XtraBars.BarButtonItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.splitterItem2 = new DevExpress.XtraLayout.SplitterItem();
            this.layoutControlGroupSorguListesi = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.repositoryItemLookUpEditBinaBirimTipiId = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemLookUpEditBirimOzellikTipiId = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.popupMenuSqlTanimKategori = new DevExpress.XtraBars.PopupMenu(this.components);
            this.popupMenuSqlTanim = new DevExpress.XtraBars.PopupMenu(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlForm)).BeginInit();
            this.layoutControlForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeListSqlTanimKategori)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSqlTanimKategori)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlSqlTanimListe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSqlTanimListe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSqlTanimListe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupSorguListesi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditBinaBirimTipiId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditBirimOzellikTipiId)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuSqlTanimKategori)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuSqlTanim)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlForm
            // 
            this.layoutControlForm.Controls.Add(this.treeListSqlTanimKategori);
            this.layoutControlForm.Controls.Add(this.gridControlSqlTanimListe);
            this.layoutControlForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlForm.HiddenItems.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3});
            this.layoutControlForm.Location = new System.Drawing.Point(0, 0);
            this.layoutControlForm.Name = "layoutControlForm";
            this.layoutControlForm.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(547, 258, 250, 350);
            this.layoutControlForm.Root = this.layoutControlGroup1;
            this.layoutControlForm.Size = new System.Drawing.Size(937, 705);
            this.layoutControlForm.TabIndex = 0;
            this.layoutControlForm.Text = "layoutControl1";
            // 
            // treeListSqlTanimKategori
            // 
            this.treeListSqlTanimKategori.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colAdi});
            this.treeListSqlTanimKategori.DataSource = this.bindingSourceSqlTanimKategori;
            this.treeListSqlTanimKategori.KeyFieldName = "Id";
            this.treeListSqlTanimKategori.Location = new System.Drawing.Point(10, 34);
            this.treeListSqlTanimKategori.Name = "treeListSqlTanimKategori";
            this.treeListSqlTanimKategori.OptionsView.ShowIndicator = false;
            this.treeListSqlTanimKategori.ParentFieldName = "EkParentId";
            this.treeListSqlTanimKategori.PreviewFieldName = "Adi";
            this.treeListSqlTanimKategori.Size = new System.Drawing.Size(245, 661);
            this.treeListSqlTanimKategori.TabIndex = 19;
            this.treeListSqlTanimKategori.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.treeListSqlTanimKategori_FocusedNodeChanged);
            this.treeListSqlTanimKategori.DoubleClick += new System.EventHandler(this.treeListSqlTanimKategori_DoubleClick);
            // 
            // colAdi
            // 
            this.colAdi.Caption = "Adı";
            this.colAdi.FieldName = "Name";
            this.colAdi.Name = "colAdi";
            this.colAdi.OptionsColumn.AllowEdit = false;
            this.colAdi.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            this.colAdi.Visible = true;
            this.colAdi.VisibleIndex = 0;
            // 
            // bindingSourceSqlTanimKategori
            // 
            this.bindingSourceSqlTanimKategori.DataSource = typeof(RoyaleSupport.SqlDefinitionCategoryDto);
            // 
            // gridControlSqlTanimListe
            // 
            this.gridControlSqlTanimListe.DataSource = this.bindingSourceSqlTanimListe;
            this.gridControlSqlTanimListe.Location = new System.Drawing.Point(275, 34);
            this.gridControlSqlTanimListe.MainView = this.gridViewSqlTanimListe;
            this.gridControlSqlTanimListe.MenuManager = this.barManager1;
            this.gridControlSqlTanimListe.Name = "gridControlSqlTanimListe";
            this.gridControlSqlTanimListe.Size = new System.Drawing.Size(652, 661);
            this.gridControlSqlTanimListe.TabIndex = 8;
            this.gridControlSqlTanimListe.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewSqlTanimListe});
            // 
            // bindingSourceSqlTanimListe
            // 
            this.bindingSourceSqlTanimListe.DataSource = typeof(RoyaleSupport.SqlDefinitionParameterDto);
            // 
            // gridViewSqlTanimListe
            // 
            this.gridViewSqlTanimListe.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colAd,
            this.colSql,
            this.colCreatedDate});
            this.gridViewSqlTanimListe.GridControl = this.gridControlSqlTanimListe;
            this.gridViewSqlTanimListe.Name = "gridViewSqlTanimListe";
            this.gridViewSqlTanimListe.OptionsBehavior.Editable = false;
            this.gridViewSqlTanimListe.OptionsView.ShowAutoFilterRow = true;
            this.gridViewSqlTanimListe.OptionsView.ShowFooter = true;
            this.gridViewSqlTanimListe.OptionsView.ShowGroupPanel = false;
            this.gridViewSqlTanimListe.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewSqlTanimListe_FocusedRowChanged);
            this.gridViewSqlTanimListe.DoubleClick += new System.EventHandler(this.gridViewSqlTanimListe_DoubleClick);
            // 
            // colAd
            // 
            this.colAd.Caption = "Adı";
            this.colAd.FieldName = "Name";
            this.colAd.Name = "colAd";
            this.colAd.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.colAd.Visible = true;
            this.colAd.VisibleIndex = 0;
            this.colAd.Width = 259;
            // 
            // colSql
            // 
            this.colSql.Caption = "Açıklama";
            this.colSql.FieldName = "Description";
            this.colSql.Name = "colSql";
            this.colSql.Visible = true;
            this.colSql.VisibleIndex = 1;
            this.colSql.Width = 268;
            // 
            // colCreatedDate
            // 
            this.colCreatedDate.Caption = "Oluşturulma Tarihi";
            this.colCreatedDate.FieldName = "CreatedOn";
            this.colCreatedDate.Name = "colCreatedDate";
            this.colCreatedDate.Visible = true;
            this.colCreatedDate.VisibleIndex = 2;
            this.colCreatedDate.Width = 112;
            // 
            // barManager1
            // 
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItemKaydet,
            this.barButtonItemIptal,
            this.barButtonItemKategoriEkle,
            this.barButtonItemSil,
            this.barButtonItemSqlTanimEkle,
            this.barButtonItemSqlTanimSil,
            this.barButtonItemDuzenle});
            this.barManager1.MaxItemId = 27;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(937, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 705);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(937, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 705);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(937, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 705);
            // 
            // barButtonItemKaydet
            // 
            this.barButtonItemKaydet.Id = 25;
            this.barButtonItemKaydet.Name = "barButtonItemKaydet";
            // 
            // barButtonItemIptal
            // 
            this.barButtonItemIptal.Caption = "İptal";
            this.barButtonItemIptal.Id = 20;
            this.barButtonItemIptal.Name = "barButtonItemIptal";
            this.barButtonItemIptal.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemIptal_ItemClick);
            // 
            // barButtonItemKategoriEkle
            // 
            this.barButtonItemKategoriEkle.Caption = "Ekle";
            this.barButtonItemKategoriEkle.Id = 21;
            this.barButtonItemKategoriEkle.Name = "barButtonItemKategoriEkle";
            this.barButtonItemKategoriEkle.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemKategoriEkle_ItemClick);
            // 
            // barButtonItemSil
            // 
            this.barButtonItemSil.Caption = "Sil";
            this.barButtonItemSil.Id = 22;
            this.barButtonItemSil.Name = "barButtonItemSil";
            this.barButtonItemSil.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSil_ItemClick);
            // 
            // barButtonItemSqlTanimEkle
            // 
            this.barButtonItemSqlTanimEkle.Caption = "Ekle";
            this.barButtonItemSqlTanimEkle.Id = 23;
            this.barButtonItemSqlTanimEkle.Name = "barButtonItemSqlTanimEkle";
            this.barButtonItemSqlTanimEkle.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSqlTanimEkle_ItemClick);
            // 
            // barButtonItemSqlTanimSil
            // 
            this.barButtonItemSqlTanimSil.Caption = "Sil";
            this.barButtonItemSqlTanimSil.Id = 24;
            this.barButtonItemSqlTanimSil.Name = "barButtonItemSqlTanimSil";
            this.barButtonItemSqlTanimSil.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSqlTanimSil_ItemClick);
            // 
            // barButtonItemDuzenle
            // 
            this.barButtonItemDuzenle.Caption = "Düzenle";
            this.barButtonItemDuzenle.Id = 26;
            this.barButtonItemDuzenle.Name = "barButtonItemDuzenle";
            this.barButtonItemDuzenle.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDuzenle_ItemClick);
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
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Root";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.splitterItem2,
            this.layoutControlGroupSorguListesi,
            this.layoutControlGroup2});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroup1.Size = new System.Drawing.Size(937, 705);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // splitterItem2
            // 
            this.splitterItem2.AllowHotTrack = true;
            this.splitterItem2.CustomizationFormText = "splitterItem2";
            this.splitterItem2.Location = new System.Drawing.Point(255, 0);
            this.splitterItem2.Name = "splitterItem2";
            this.splitterItem2.Size = new System.Drawing.Size(10, 695);
            // 
            // layoutControlGroupSorguListesi
            // 
            this.layoutControlGroupSorguListesi.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlGroupSorguListesi.AppearanceGroup.Options.UseFont = true;
            this.layoutControlGroupSorguListesi.CustomizationFormText = "SQL  Kayıt";
            this.layoutControlGroupSorguListesi.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem5});
            this.layoutControlGroupSorguListesi.Location = new System.Drawing.Point(265, 0);
            this.layoutControlGroupSorguListesi.Name = "layoutControlGroupSorguListesi";
            this.layoutControlGroupSorguListesi.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroupSorguListesi.Size = new System.Drawing.Size(662, 695);
            this.layoutControlGroupSorguListesi.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 5, 2);
            this.layoutControlGroupSorguListesi.Text = "Sorgu Listesi";
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.gridControlSqlTanimListe;
            this.layoutControlItem5.CustomizationFormText = "layoutControlItem5";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(656, 665);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlGroup2.AppearanceGroup.Options.UseFont = true;
            this.layoutControlGroup2.CustomizationFormText = "Bina Birimleri";
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.Size = new System.Drawing.Size(255, 695);
            this.layoutControlGroup2.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 5, 2);
            this.layoutControlGroup2.Text = "Sorgu Grubu";
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.treeListSqlTanimKategori;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(249, 665);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // repositoryItemLookUpEditBinaBirimTipiId
            // 
            this.repositoryItemLookUpEditBinaBirimTipiId.AutoHeight = false;
            this.repositoryItemLookUpEditBinaBirimTipiId.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEditBinaBirimTipiId.Name = "repositoryItemLookUpEditBinaBirimTipiId";
            this.repositoryItemLookUpEditBinaBirimTipiId.NullText = "";
            // 
            // repositoryItemLookUpEditBirimOzellikTipiId
            // 
            this.repositoryItemLookUpEditBirimOzellikTipiId.AutoHeight = false;
            this.repositoryItemLookUpEditBirimOzellikTipiId.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEditBirimOzellikTipiId.Name = "repositoryItemLookUpEditBirimOzellikTipiId";
            this.repositoryItemLookUpEditBirimOzellikTipiId.NullText = "";
            // 
            // splitterItem1
            // 
            this.splitterItem1.AllowHotTrack = true;
            this.splitterItem1.CustomizationFormText = "splitterItem1";
            this.splitterItem1.Location = new System.Drawing.Point(236, 0);
            this.splitterItem1.Name = "splitterItem1";
            this.splitterItem1.Size = new System.Drawing.Size(6, 445);
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
            // popupMenuSqlTanimKategori
            // 
            this.popupMenuSqlTanimKategori.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemKategoriEkle),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemSil)});
            this.popupMenuSqlTanimKategori.Manager = this.barManager1;
            this.popupMenuSqlTanimKategori.Name = "popupMenuSqlTanimKategori";
            // 
            // popupMenuSqlTanim
            // 
            this.popupMenuSqlTanim.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemSqlTanimEkle),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemDuzenle),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemSqlTanimSil)});
            this.popupMenuSqlTanim.Manager = this.barManager1;
            this.popupMenuSqlTanim.Name = "popupMenuSqlTanim";
            // 
            // FormSqlTanimListesi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(937, 705);
            this.Controls.Add(this.layoutControlForm);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Name = "FormSqlTanimListesi";
            this.Text = "FormSqlTanimListesi";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlForm)).EndInit();
            this.layoutControlForm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeListSqlTanimKategori)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSqlTanimKategori)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlSqlTanimListe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSqlTanimListe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSqlTanimListe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupSorguListesi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditBinaBirimTipiId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditBirimOzellikTipiId)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuSqlTanimKategori)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.popupMenuSqlTanim)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BarButtonItem barButtonItemDuzenle;
    }
}