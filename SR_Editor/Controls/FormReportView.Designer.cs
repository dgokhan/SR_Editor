using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SR_Editor.Core.Controls
{
    partial class FormReportView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        private PanelControl panelViewer;

        private PanelControl panelInstanceParent;

        private PanelControl panelInstance;

        private SaveFileDialog saveCRFileDialog;

        private BarManager barManagerReport;

        private Bar bar1;

        private BarButtonItem barButtonItemYenile;

        private BarDockControl barDockControlTop;

        private BarDockControl barDockControlBottom;

        private BarDockControl barDockControlLeft;

        private BarDockControl barDockControlRight;

        private BarButtonItem barButtonItemYazdir;

        private BarButtonItem barButtonItemTasarim;

        private BarButtonItem barButtonItemOrajinalTasarim;

        private BarButtonItem barButtonItemMailGonder;

        private BarStaticItem barStaticItem1;

        private BarEditItem barEditItemDigerTasarimlar;

        private RepositoryItemLookUpEdit repositoryItemLookUpEditDigerTasarimlar;


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
            this.panelViewer = new DevExpress.XtraEditors.PanelControl();
            this.panelInstanceParent = new DevExpress.XtraEditors.PanelControl();
            this.panelInstance = new DevExpress.XtraEditors.PanelControl();
            this.saveCRFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.barManagerReport = new DevExpress.XtraBars.BarManager();
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonItemYenile = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemYazdir = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemMailGonder = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemTasarim = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItemOrajinalTasarim = new DevExpress.XtraBars.BarButtonItem();
            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
            this.barEditItemDigerTasarimlar = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemLookUpEditDigerTasarimlar = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelInstanceParent)).BeginInit();
            this.panelInstanceParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelInstance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerReport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditDigerTasarimlar)).BeginInit();
            this.SuspendLayout();
            // 
            // panelViewer
            // 
            this.panelViewer.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Flat;
            this.panelViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelViewer.Location = new System.Drawing.Point(3, 71);
            this.panelViewer.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelViewer.LookAndFeel.UseWindowsXPTheme = true;
            this.panelViewer.Name = "panelViewer";
            this.panelViewer.Size = new System.Drawing.Size(1008, 552);
            this.panelViewer.TabIndex = 2;
            // 
            // panelInstanceParent
            // 
            this.panelInstanceParent.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.panelInstanceParent.Appearance.Options.UseBackColor = true;
            this.panelInstanceParent.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelInstanceParent.Controls.Add(this.panelInstance);
            this.panelInstanceParent.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelInstanceParent.Location = new System.Drawing.Point(3, 31);
            this.panelInstanceParent.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelInstanceParent.LookAndFeel.UseWindowsXPTheme = true;
            this.panelInstanceParent.Name = "panelInstanceParent";
            this.panelInstanceParent.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.panelInstanceParent.Size = new System.Drawing.Size(1008, 40);
            this.panelInstanceParent.TabIndex = 7;
            // 
            // panelInstance
            // 
            this.panelInstance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInstance.Location = new System.Drawing.Point(0, 3);
            this.panelInstance.Name = "panelInstance";
            this.panelInstance.Size = new System.Drawing.Size(1008, 37);
            this.panelInstance.TabIndex = 4;
            // 
            // barManagerReport
            // 
            this.barManagerReport.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1});
            this.barManagerReport.DockControls.Add(this.barDockControlTop);
            this.barManagerReport.DockControls.Add(this.barDockControlBottom);
            this.barManagerReport.DockControls.Add(this.barDockControlLeft);
            this.barManagerReport.DockControls.Add(this.barDockControlRight);
            this.barManagerReport.Form = this;
            this.barManagerReport.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItemYenile,
            this.barButtonItemYazdir,
            this.barButtonItemTasarim,
            this.barButtonItemOrajinalTasarim,
            this.barButtonItemMailGonder,
            this.barEditItemDigerTasarimlar,
            this.barStaticItem1});
            this.barManagerReport.MaxItemId = 7;
            this.barManagerReport.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEditDigerTasarimlar});
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 0;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemYenile),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemYazdir),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.barButtonItemMailGonder, false),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItemTasarim),
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.barButtonItemOrajinalTasarim, false),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barEditItemDigerTasarimlar)});
            this.bar1.Text = "Tools";
            // 
            // barButtonItemYenile
            // 
            this.barButtonItemYenile.Caption = "Yenile";
            this.barButtonItemYenile.Id = 0;
            this.barButtonItemYenile.Name = "barButtonItemYenile";
            this.barButtonItemYenile.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemYenile_ItemClick);
            // 
            // barButtonItemYazdir
            // 
            this.barButtonItemYazdir.Caption = "Yazdır";
            this.barButtonItemYazdir.Id = 1;
            this.barButtonItemYazdir.Name = "barButtonItemYazdir";
            this.barButtonItemYazdir.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemYazdir_ItemClick);
            // 
            // barButtonItemMailGonder
            // 
            this.barButtonItemMailGonder.Caption = "Mail Gönder";
            this.barButtonItemMailGonder.Id = 4;
            this.barButtonItemMailGonder.Name = "barButtonItemMailGonder";
            this.barButtonItemMailGonder.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemMailGonder_ItemClick);
            // 
            // barButtonItemTasarim
            // 
            this.barButtonItemTasarim.Caption = "Tasarımı Değiştir";
            this.barButtonItemTasarim.Id = 2;
            this.barButtonItemTasarim.Name = "barButtonItemTasarim";
            this.barButtonItemTasarim.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItemTasarim.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemTasarim_ItemClick);
            // 
            // barButtonItemOrajinalTasarim
            // 
            this.barButtonItemOrajinalTasarim.Caption = "Orjinal Tasarıma Dön";
            this.barButtonItemOrajinalTasarim.Id = 3;
            this.barButtonItemOrajinalTasarim.Name = "barButtonItemOrajinalTasarim";
            this.barButtonItemOrajinalTasarim.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            this.barButtonItemOrajinalTasarim.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemOrajinalTasarim_ItemClick);
            // 
            // barStaticItem1
            // 
            this.barStaticItem1.Caption = "Diğer Tasarımlar :";
            this.barStaticItem1.Id = 6;
            this.barStaticItem1.Name = "barStaticItem1";
            // 
            // barEditItemDigerTasarimlar
            // 
            this.barEditItemDigerTasarimlar.Caption = "barEditItem1";
            this.barEditItemDigerTasarimlar.Edit = this.repositoryItemLookUpEditDigerTasarimlar;
            this.barEditItemDigerTasarimlar.EditWidth = 234;
            this.barEditItemDigerTasarimlar.Id = 5;
            this.barEditItemDigerTasarimlar.Name = "barEditItemDigerTasarimlar";
            this.barEditItemDigerTasarimlar.EditValueChanged += new System.EventHandler(this.barEditItemDigerTasarimlar_EditValueChanged);
            // 
            // repositoryItemLookUpEditDigerTasarimlar
            // 
            this.repositoryItemLookUpEditDigerTasarimlar.AutoHeight = false;
            this.repositoryItemLookUpEditDigerTasarimlar.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEditDigerTasarimlar.Name = "repositoryItemLookUpEditDigerTasarimlar";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(3, 3);
            this.barDockControlTop.Manager = this.barManagerReport;
            this.barDockControlTop.Size = new System.Drawing.Size(1008, 28);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(3, 623);
            this.barDockControlBottom.Manager = this.barManagerReport;
            this.barDockControlBottom.Size = new System.Drawing.Size(1008, 0);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(3, 31);
            this.barDockControlLeft.Manager = this.barManagerReport;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 592);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1011, 31);
            this.barDockControlRight.Manager = this.barManagerReport;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 592);
            // 
            // FormReportView
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Control;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 626);
            this.Controls.Add(this.panelViewer);
            this.Controls.Add(this.panelInstanceParent);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "Caramel";
            this.LookAndFeel.UseDefaultLookAndFeel = false;
            this.Name = "FormReportView";
            this.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ReportViewForm";
            this.Shown += new System.EventHandler(this.FormReportView_Shown);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormReportView_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.panelViewer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelInstanceParent)).EndInit();
            this.panelInstanceParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelInstance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerReport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditDigerTasarimlar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}