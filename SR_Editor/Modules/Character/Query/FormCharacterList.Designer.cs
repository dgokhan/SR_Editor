
namespace SR_Editor.Modules.Character.Query
{
    partial class FormCharacterList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCharacterList));
            this.editorLayoutControl1 = new SR_Editor.Core.Controls.EditorLayoutControl();
            this.gridControlKarakterler = new DevExpress.XtraGrid.GridControl();
            this.bindingSourceKarakterler = new System.Windows.Forms.BindingSource(this.components);
            this.gridViewKarakterler = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colServer_name = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colChar_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colJob = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEditJob = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lookUpJobTipiBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.colPlaytime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLevel = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colGold = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLast_play = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colServer_id = new DevExpress.XtraGrid.Columns.GridColumn();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.editorLayoutControl2 = new SR_Editor.Core.Controls.EditorLayoutControl();
            this.simpleButtonSorgula = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.textEditAccountName = new System.Windows.Forms.TextBox();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.editorLayoutControl1)).BeginInit();
            this.editorLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlKarakterler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceKarakterler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewKarakterler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditJob)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpJobTipiBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editorLayoutControl2)).BeginInit();
            this.editorLayoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            this.SuspendLayout();
            // 
            // editorLayoutControl1
            // 
            this.editorLayoutControl1.Controls.Add(this.gridControlKarakterler);
            this.editorLayoutControl1.Controls.Add(this.groupControl1);
            this.editorLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorLayoutControl1.Location = new System.Drawing.Point(0, 0);
            this.editorLayoutControl1.Name = "editorLayoutControl1";
            this.editorLayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(1448, 419, 650, 400);
            this.editorLayoutControl1.Root = this.Root;
            this.editorLayoutControl1.Size = new System.Drawing.Size(1455, 649);
            this.editorLayoutControl1.TabIndex = 0;
            this.editorLayoutControl1.Text = "editorLayoutControl1";
            // 
            // gridControlKarakterler
            // 
            this.gridControlKarakterler.DataSource = this.bindingSourceKarakterler;
            this.gridControlKarakterler.Location = new System.Drawing.Point(333, 0);
            this.gridControlKarakterler.MainView = this.gridViewKarakterler;
            this.gridControlKarakterler.Name = "gridControlKarakterler";
            this.gridControlKarakterler.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEditJob});
            this.gridControlKarakterler.Size = new System.Drawing.Size(1122, 649);
            this.gridControlKarakterler.TabIndex = 5;
            this.gridControlKarakterler.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewKarakterler});
            // 
            // bindingSourceKarakterler
            // 
            this.bindingSourceKarakterler.DataSource = typeof(RoyaleSupport.GameAccountCharacterDto);
            // 
            // gridViewKarakterler
            // 
            this.gridViewKarakterler.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colServer_name,
            this.colChar_id,
            this.colName,
            this.colJob,
            this.colPlaytime,
            this.colLevel,
            this.colGold,
            this.colLast_play,
            this.colStatus,
            this.colServer_id});
            this.gridViewKarakterler.GridControl = this.gridControlKarakterler;
            this.gridViewKarakterler.Name = "gridViewKarakterler";
            this.gridViewKarakterler.OptionsView.ShowAutoFilterRow = true;
            this.gridViewKarakterler.OptionsView.ShowGroupPanel = false;
            this.gridViewKarakterler.DoubleClick += new System.EventHandler(this.gridViewKarakterler_DoubleClick_1);
            // 
            // colServer_name
            // 
            this.colServer_name.Caption = "Server Name";
            this.colServer_name.FieldName = "Server_name";
            this.colServer_name.Name = "colServer_name";
            this.colServer_name.OptionsColumn.AllowEdit = false;
            this.colServer_name.OptionsColumn.AllowMove = false;
            this.colServer_name.Visible = true;
            this.colServer_name.VisibleIndex = 0;
            // 
            // colChar_id
            // 
            this.colChar_id.Caption = "CharId";
            this.colChar_id.FieldName = "Char_id";
            this.colChar_id.Name = "colChar_id";
            this.colChar_id.OptionsColumn.AllowEdit = false;
            this.colChar_id.OptionsColumn.AllowMove = false;
            this.colChar_id.Visible = true;
            this.colChar_id.VisibleIndex = 1;
            // 
            // colName
            // 
            this.colName.FieldName = "Name";
            this.colName.Name = "colName";
            this.colName.OptionsColumn.AllowEdit = false;
            this.colName.OptionsColumn.AllowMove = false;
            this.colName.Visible = true;
            this.colName.VisibleIndex = 2;
            // 
            // colJob
            // 
            this.colJob.ColumnEdit = this.repositoryItemLookUpEditJob;
            this.colJob.FieldName = "Job";
            this.colJob.Name = "colJob";
            this.colJob.OptionsColumn.AllowEdit = false;
            this.colJob.OptionsColumn.AllowMove = false;
            this.colJob.Visible = true;
            this.colJob.VisibleIndex = 3;
            // 
            // repositoryItemLookUpEditJob
            // 
            this.repositoryItemLookUpEditJob.AutoHeight = false;
            this.repositoryItemLookUpEditJob.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEditJob.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Id", "Name1"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Aciklama", "Name2")});
            this.repositoryItemLookUpEditJob.DataSource = this.lookUpJobTipiBindingSource;
            this.repositoryItemLookUpEditJob.DisplayMember = "Aciklama";
            this.repositoryItemLookUpEditJob.KeyMember = "Id";
            this.repositoryItemLookUpEditJob.Name = "repositoryItemLookUpEditJob";
            this.repositoryItemLookUpEditJob.ValueMember = "Id";
            // 
            // lookUpJobTipiBindingSource
            // 
            this.lookUpJobTipiBindingSource.DataSource = typeof(SR_Editor.LookUp.LookUpJobTipi);
            // 
            // colPlaytime
            // 
            this.colPlaytime.FieldName = "Playtime";
            this.colPlaytime.Name = "colPlaytime";
            this.colPlaytime.OptionsColumn.AllowEdit = false;
            this.colPlaytime.OptionsColumn.AllowMove = false;
            this.colPlaytime.Visible = true;
            this.colPlaytime.VisibleIndex = 4;
            // 
            // colLevel
            // 
            this.colLevel.FieldName = "Level";
            this.colLevel.Name = "colLevel";
            this.colLevel.OptionsColumn.AllowEdit = false;
            this.colLevel.OptionsColumn.AllowMove = false;
            this.colLevel.Visible = true;
            this.colLevel.VisibleIndex = 5;
            // 
            // colGold
            // 
            this.colGold.FieldName = "Gold";
            this.colGold.Name = "colGold";
            this.colGold.OptionsColumn.AllowEdit = false;
            this.colGold.OptionsColumn.AllowMove = false;
            this.colGold.Visible = true;
            this.colGold.VisibleIndex = 6;
            // 
            // colLast_play
            // 
            this.colLast_play.Caption = "Last Play";
            this.colLast_play.DisplayFormat.FormatString = "d";
            this.colLast_play.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colLast_play.FieldName = "Last_play";
            this.colLast_play.Name = "colLast_play";
            this.colLast_play.OptionsColumn.AllowEdit = false;
            this.colLast_play.OptionsColumn.AllowMove = false;
            this.colLast_play.Visible = true;
            this.colLast_play.VisibleIndex = 7;
            // 
            // colStatus
            // 
            this.colStatus.FieldName = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.OptionsColumn.AllowEdit = false;
            this.colStatus.OptionsColumn.AllowMove = false;
            // 
            // colServer_id
            // 
            this.colServer_id.FieldName = "Server_id";
            this.colServer_id.Name = "colServer_id";
            this.colServer_id.OptionsColumn.AllowEdit = false;
            this.colServer_id.OptionsColumn.AllowMove = false;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.editorLayoutControl2);
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.MinimumSize = new System.Drawing.Size(0, 100);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Padding = new System.Windows.Forms.Padding(5);
            this.groupControl1.Size = new System.Drawing.Size(333, 112);
            this.groupControl1.TabIndex = 4;
            this.groupControl1.Text = "Sorgu";
            // 
            // editorLayoutControl2
            // 
            this.editorLayoutControl2.Controls.Add(this.textEditAccountName);
            this.editorLayoutControl2.Controls.Add(this.simpleButtonSorgula);
            this.editorLayoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorLayoutControl2.Location = new System.Drawing.Point(7, 28);
            this.editorLayoutControl2.Name = "editorLayoutControl2";
            this.editorLayoutControl2.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(983, 544, 650, 400);
            this.editorLayoutControl2.Root = this.layoutControlGroup1;
            this.editorLayoutControl2.Size = new System.Drawing.Size(319, 77);
            this.editorLayoutControl2.TabIndex = 0;
            this.editorLayoutControl2.Text = "editorLayoutControl2";
            // 
            // simpleButtonSorgula
            // 
            this.simpleButtonSorgula.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButtonSorgula.ImageOptions.Image")));
            this.simpleButtonSorgula.Location = new System.Drawing.Point(0, 20);
            this.simpleButtonSorgula.Name = "simpleButtonSorgula";
            this.simpleButtonSorgula.Size = new System.Drawing.Size(319, 36);
            this.simpleButtonSorgula.StyleController = this.editorLayoutControl2;
            this.simpleButtonSorgula.TabIndex = 5;
            this.simpleButtonSorgula.Text = "Sorgula";
            this.simpleButtonSorgula.Click += new System.EventHandler(this.simpleButtonSorgula_Click);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem3,
            this.layoutControlItem5});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(319, 77);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.simpleButtonSorgula;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 20);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItem3.Size = new System.Drawing.Size(319, 57);
            this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextToControlDistance = 0;
            this.layoutControlItem3.TextVisible = false;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4,
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(1455, 649);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.groupControl1;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem4.MaxSize = new System.Drawing.Size(333, 112);
            this.layoutControlItem4.MinSize = new System.Drawing.Size(333, 112);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItem4.Size = new System.Drawing.Size(333, 649);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem4.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem4.TextToControlDistance = 0;
            this.layoutControlItem4.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControlKarakterler;
            this.layoutControlItem1.Location = new System.Drawing.Point(333, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItem1.Size = new System.Drawing.Size(1122, 649);
            this.layoutControlItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // textEditAccountName
            // 
            this.textEditAccountName.Location = new System.Drawing.Point(64, 0);
            this.textEditAccountName.Name = "textEditAccountName";
            this.textEditAccountName.Size = new System.Drawing.Size(255, 20);
            this.textEditAccountName.TabIndex = 6;
            this.textEditAccountName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEditAccountName_KeyDown);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.textEditAccountName;
            this.layoutControlItem5.CustomizationFormText = "Karakter Adı";
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItem5.Size = new System.Drawing.Size(319, 20);
            this.layoutControlItem5.Text = "Karakter Adı";
            this.layoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(59, 13);
            this.layoutControlItem5.TextToControlDistance = 5;
            // 
            // FormCharacterList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1455, 649);
            this.Controls.Add(this.editorLayoutControl1);
            this.Name = "FormCharacterList";
            this.Text = "FormCharacter";
            this.Shown += new System.EventHandler(this.FormAccount_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.editorLayoutControl1)).EndInit();
            this.editorLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlKarakterler)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceKarakterler)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewKarakterler)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEditJob)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpJobTipiBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editorLayoutControl2)).EndInit();
            this.editorLayoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Core.Controls.EditorLayoutControl editorLayoutControl1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private Core.Controls.EditorLayoutControl editorLayoutControl2;
        private DevExpress.XtraEditors.SimpleButton simpleButtonSorgula;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private System.Windows.Forms.BindingSource bindingSourceKarakterler;
        private System.Windows.Forms.BindingSource lookUpJobTipiBindingSource;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraGrid.GridControl gridControlKarakterler;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewKarakterler;
        private DevExpress.XtraGrid.Columns.GridColumn colServer_name;
        private DevExpress.XtraGrid.Columns.GridColumn colChar_id;
        private DevExpress.XtraGrid.Columns.GridColumn colName;
        private DevExpress.XtraGrid.Columns.GridColumn colJob;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEditJob;
        private DevExpress.XtraGrid.Columns.GridColumn colPlaytime;
        private DevExpress.XtraGrid.Columns.GridColumn colLevel;
        private DevExpress.XtraGrid.Columns.GridColumn colGold;
        private DevExpress.XtraGrid.Columns.GridColumn colLast_play;
        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
        private DevExpress.XtraGrid.Columns.GridColumn colServer_id;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private System.Windows.Forms.TextBox textEditAccountName;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
    }
}