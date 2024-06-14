namespace SR_Editor.Modules.SqlQuery.Action
{
    using DevExpress.Utils;
    using DevExpress.XtraBars;
    using DevExpress.XtraEditors;
    using DevExpress.XtraEditors.DXErrorProvider;
    using DevExpress.XtraLayout;
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using RoyaleSupport;
    partial class FormSqlTanimKategori
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private LayoutControl layoutControlForm;
        private LayoutControlGroup layoutControlGroupBransTanim2;
        private PanelControl panelControl1;
        private StandaloneBarDockControl standaloneBarDockControl1;
        private LayoutControlItem layoutControlItemButonlar;
        private BarManager barManager1;
        private Bar bar1;
        private BarButtonItem barButtonItemTamam;
        private BarButtonItem barButtonItemIptal;
        private BarDockControl barDockControlTop;
        private BarDockControl barDockControlBottom;
        private BarDockControl barDockControlLeft;
        private BarDockControl barDockControlRight;
        private TextEdit textEditAdi;
        private LayoutControlGroup layoutControlGroupTetkikTanim;
        private LayoutControlItem layoutControlItemAdi;
        private EmptySpaceItem emptySpaceItem1;
        private BindingSource bindingSourceSqlTanimKategori;

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
            this.lookUpEditKategoriTipi = new DevExpress.XtraEditors.LookUpEdit();
            this.bindingSourceSqlTanimKategori = new System.Windows.Forms.BindingSource(this.components);
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonItemTamam = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemIptal = new DevExpress.XtraBars.BarButtonItem();
            this.standaloneBarDockControl1 = new DevExpress.XtraBars.StandaloneBarDockControl();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.textEditAdi = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlGroupBransTanim2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemButonlar = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroupTetkikTanim = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemAdi = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlForm)).BeginInit();
            this.layoutControlForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditKategoriTipi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSqlTanimKategori)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditAdi.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBransTanim2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemButonlar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupTetkikTanim)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAdi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlForm
            // 
            this.layoutControlForm.Controls.Add(this.lookUpEditKategoriTipi);
            this.layoutControlForm.Controls.Add(this.panelControl1);
            this.layoutControlForm.Controls.Add(this.textEditAdi);
            this.layoutControlForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControlForm.Location = new System.Drawing.Point(0, 0);
            this.layoutControlForm.Name = "layoutControlForm";
            this.layoutControlForm.Root = this.layoutControlGroupBransTanim2;
            this.layoutControlForm.Size = new System.Drawing.Size(284, 212);
            this.layoutControlForm.TabIndex = 0;
            this.layoutControlForm.Text = "layoutControl1";
            // 
            // lookUpEditKategoriTipi
            // 
            this.lookUpEditKategoriTipi.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.bindingSourceSqlTanimKategori, "Type", true));
            this.lookUpEditKategoriTipi.Location = new System.Drawing.Point(91, 95);
            this.lookUpEditKategoriTipi.MenuManager = this.barManager1;
            this.lookUpEditKategoriTipi.Name = "lookUpEditKategoriTipi";
            this.lookUpEditKategoriTipi.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditKategoriTipi.Size = new System.Drawing.Size(178, 20);
            this.lookUpEditKategoriTipi.StyleController = this.layoutControlForm;
            this.lookUpEditKategoriTipi.TabIndex = 4;
            // 
            // bindingSourceSqlTanimKategori
            // 
            this.bindingSourceSqlTanimKategori.DataSource = typeof(RoyaleSupport.SqlDefinitionCategoryDto);
            this.bindingSourceSqlTanimKategori.CurrentChanged += new System.EventHandler(this.bindingSourceSqlTanimKategori_CurrentChanged);
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
            this.barButtonItemTamam,
            this.barButtonItemIptal});
            this.barManager1.MaxItemId = 2;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Standalone;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemTamam),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemIptal)});
            this.bar1.StandaloneBarDockControl = this.standaloneBarDockControl1;
            this.bar1.Text = "Tools";
            // 
            // barButtonItemTamam
            // 
            this.barButtonItemTamam.Caption = "Tamam";
            this.barButtonItemTamam.Id = 0;
            this.barButtonItemTamam.Name = "barButtonItemTamam";
            this.barButtonItemTamam.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemTamam_ItemClick);
            // 
            // barButtonItemIptal
            // 
            this.barButtonItemIptal.Caption = "İptal";
            this.barButtonItemIptal.Id = 1;
            this.barButtonItemIptal.Name = "barButtonItemIptal";
            this.barButtonItemIptal.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemIptal_ItemClick);
            // 
            // standaloneBarDockControl1
            // 
            this.standaloneBarDockControl1.CausesValidation = false;
            this.standaloneBarDockControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.standaloneBarDockControl1.Location = new System.Drawing.Point(2, 2);
            this.standaloneBarDockControl1.Manager = this.barManager1;
            this.standaloneBarDockControl1.MinimumSize = new System.Drawing.Size(0, 25);
            this.standaloneBarDockControl1.Name = "standaloneBarDockControl1";
            this.standaloneBarDockControl1.Size = new System.Drawing.Size(266, 25);
            this.standaloneBarDockControl1.Text = "standaloneBarDockControl1";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(284, 0);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 212);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(284, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 212);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(284, 0);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 212);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.standaloneBarDockControl1);
            this.panelControl1.Location = new System.Drawing.Point(7, 7);
            this.panelControl1.MinimumSize = new System.Drawing.Size(0, 29);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(270, 29);
            this.panelControl1.TabIndex = 2;
            // 
            // textEditAdi
            // 
            this.textEditAdi.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceSqlTanimKategori, "Name", true));
            this.textEditAdi.EnterMoveNextControl = true;
            this.textEditAdi.Location = new System.Drawing.Point(91, 71);
            this.textEditAdi.Name = "textEditAdi";
            this.textEditAdi.Size = new System.Drawing.Size(178, 20);
            this.textEditAdi.StyleController = this.layoutControlForm;
            this.textEditAdi.TabIndex = 1;
            // 
            // layoutControlGroupBransTanim2
            // 
            this.layoutControlGroupBransTanim2.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroupBransTanim2.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroupBransTanim2.GroupBordersVisible = false;
            this.layoutControlGroupBransTanim2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemButonlar,
            this.layoutControlGroupTetkikTanim});
            this.layoutControlGroupBransTanim2.Name = "layoutControlGroupBransTanim2";
            this.layoutControlGroupBransTanim2.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroupBransTanim2.Size = new System.Drawing.Size(284, 212);
            this.layoutControlGroupBransTanim2.TextVisible = false;
            // 
            // layoutControlItemButonlar
            // 
            this.layoutControlItemButonlar.Control = this.panelControl1;
            this.layoutControlItemButonlar.CustomizationFormText = "layoutControlItemButonlar";
            this.layoutControlItemButonlar.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemButonlar.MaxSize = new System.Drawing.Size(0, 32);
            this.layoutControlItemButonlar.MinSize = new System.Drawing.Size(104, 32);
            this.layoutControlItemButonlar.Name = "layoutControlItemButonlar";
            this.layoutControlItemButonlar.Size = new System.Drawing.Size(274, 32);
            this.layoutControlItemButonlar.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItemButonlar.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemButonlar.TextVisible = false;
            // 
            // layoutControlGroupTetkikTanim
            // 
            this.layoutControlGroupTetkikTanim.AppearanceGroup.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.layoutControlGroupTetkikTanim.AppearanceGroup.Options.UseFont = true;
            this.layoutControlGroupTetkikTanim.CustomizationFormText = "Tetkik Tanım";
            this.layoutControlGroupTetkikTanim.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemAdi,
            this.emptySpaceItem1,
            this.layoutControlItem1});
            this.layoutControlGroupTetkikTanim.Location = new System.Drawing.Point(0, 32);
            this.layoutControlGroupTetkikTanim.Name = "layoutControlGroupTetkikTanim";
            this.layoutControlGroupTetkikTanim.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroupTetkikTanim.Size = new System.Drawing.Size(274, 170);
            this.layoutControlGroupTetkikTanim.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 5, 2);
            this.layoutControlGroupTetkikTanim.Text = "Kategori Tanım";
            // 
            // layoutControlItemAdi
            // 
            this.layoutControlItemAdi.Control = this.textEditAdi;
            this.layoutControlItemAdi.CustomizationFormText = "Adı";
            this.layoutControlItemAdi.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemAdi.Name = "layoutControlItemAdi";
            this.layoutControlItemAdi.Size = new System.Drawing.Size(258, 24);
            this.layoutControlItemAdi.Text = "Kategori Adı  ";
            this.layoutControlItemAdi.TextSize = new System.Drawing.Size(64, 13);
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 48);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(258, 82);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.lookUpEditKategoriTipi;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(258, 24);
            this.layoutControlItem1.Text = "Kategori Tipi";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(64, 13);
            // 
            // FormSqlTanimKategori
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 212);
            this.Controls.Add(this.layoutControlForm);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IsDialog = true;
            this.Name = "FormSqlTanimKategori";
            this.Text = "FormSqlTanimKategori";
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlForm)).EndInit();
            this.layoutControlForm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditKategoriTipi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSqlTanimKategori)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEditAdi.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupBransTanim2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemButonlar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroupTetkikTanim)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemAdi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LookUpEdit lookUpEditKategoriTipi;
        private LayoutControlItem layoutControlItem1;
    }
}