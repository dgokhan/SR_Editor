namespace SR_Editor.Forms
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.panelControlLogo = new DevExpress.XtraEditors.PanelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.groupControlLogin = new DevExpress.XtraEditors.GroupControl();
            this.editorLayoutControl1 = new SR_Editor.Core.Controls.EditorLayoutControl();
            this.simpleButtonLogin = new DevExpress.XtraEditors.SimpleButton();
            this.textEditPassword = new DevExpress.XtraEditors.TextEdit();
            this.textEditUsername = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemUsername = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.layoutControlItemPassword = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemLogin = new DevExpress.XtraLayout.LayoutControlItem();
            this.emptySpaceItem2 = new DevExpress.XtraLayout.EmptySpaceItem();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlLogo)).BeginInit();
            this.panelControlLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlLogin)).BeginInit();
            this.groupControlLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.editorLayoutControl1)).BeginInit();
            this.editorLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditUsername.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUsername)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlLogo
            // 
            this.panelControlLogo.Controls.Add(this.pictureEdit1);
            this.panelControlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlLogo.Location = new System.Drawing.Point(0, 0);
            this.panelControlLogo.Name = "panelControlLogo";
            this.panelControlLogo.Size = new System.Drawing.Size(740, 228);
            this.panelControlLogo.TabIndex = 0;
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(2, 2);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pictureEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.pictureEdit1.Properties.ReadOnly = true;
            this.pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.pictureEdit1.Properties.ShowMenu = false;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.pictureEdit1.ShowToolTips = false;
            this.pictureEdit1.Size = new System.Drawing.Size(736, 224);
            this.pictureEdit1.TabIndex = 0;
            // 
            // panelControl2
            // 
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 388);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(740, 120);
            this.panelControl2.TabIndex = 1;
            // 
            // panelControl3
            // 
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControl3.Location = new System.Drawing.Point(0, 228);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(185, 160);
            this.panelControl3.TabIndex = 2;
            // 
            // panelControl4
            // 
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl4.Location = new System.Drawing.Point(555, 228);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(185, 160);
            this.panelControl4.TabIndex = 3;
            // 
            // groupControlLogin
            // 
            this.groupControlLogin.Controls.Add(this.editorLayoutControl1);
            this.groupControlLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControlLogin.Location = new System.Drawing.Point(185, 228);
            this.groupControlLogin.Name = "groupControlLogin";
            this.groupControlLogin.Padding = new System.Windows.Forms.Padding(10);
            this.groupControlLogin.Size = new System.Drawing.Size(370, 160);
            this.groupControlLogin.TabIndex = 4;
            this.groupControlLogin.Text = "Kullanıcı Bilgileri";
            // 
            // editorLayoutControl1
            // 
            this.editorLayoutControl1.Controls.Add(this.simpleButtonLogin);
            this.editorLayoutControl1.Controls.Add(this.textEditPassword);
            this.editorLayoutControl1.Controls.Add(this.textEditUsername);
            this.editorLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editorLayoutControl1.Location = new System.Drawing.Point(12, 33);
            this.editorLayoutControl1.Name = "editorLayoutControl1";
            this.editorLayoutControl1.Root = this.Root;
            this.editorLayoutControl1.Size = new System.Drawing.Size(346, 115);
            this.editorLayoutControl1.TabIndex = 0;
            this.editorLayoutControl1.Text = "editorLayoutControl1";
            // 
            // simpleButtonLogin
            // 
            this.simpleButtonLogin.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("simpleButtonLogin.ImageOptions.SvgImage")));
            this.simpleButtonLogin.Location = new System.Drawing.Point(173, 79);
            this.simpleButtonLogin.Name = "simpleButtonLogin";
            this.simpleButtonLogin.Size = new System.Drawing.Size(173, 36);
            this.simpleButtonLogin.StyleController = this.editorLayoutControl1;
            this.simpleButtonLogin.TabIndex = 6;
            this.simpleButtonLogin.Text = "Giriş Yap";
            this.simpleButtonLogin.Click += new System.EventHandler(this.simpleButtonLogin_Click);
            // 
            // textEditPassword
            // 
            this.textEditPassword.EditValue = "";
            this.textEditPassword.Location = new System.Drawing.Point(70, 35);
            this.textEditPassword.Name = "textEditPassword";
            this.textEditPassword.Properties.PasswordChar = '*';
            this.textEditPassword.Size = new System.Drawing.Size(271, 20);
            this.textEditPassword.StyleController = this.editorLayoutControl1;
            this.textEditPassword.TabIndex = 5;
            this.textEditPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEditPassword_KeyDown);
            // 
            // textEditUsername
            // 
            this.textEditUsername.EditValue = "";
            this.textEditUsername.Location = new System.Drawing.Point(70, 5);
            this.textEditUsername.Name = "textEditUsername";
            this.textEditUsername.Size = new System.Drawing.Size(271, 20);
            this.textEditUsername.StyleController = this.editorLayoutControl1;
            this.textEditUsername.TabIndex = 4;
            this.textEditUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textEditUsername_KeyDown);
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemUsername,
            this.emptySpaceItem1,
            this.layoutControlItemPassword,
            this.layoutControlItemLogin,
            this.emptySpaceItem2});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(346, 115);
            this.Root.TextVisible = false;
            // 
            // layoutControlItemUsername
            // 
            this.layoutControlItemUsername.Control = this.textEditUsername;
            this.layoutControlItemUsername.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemUsername.Name = "layoutControlItemUsername";
            this.layoutControlItemUsername.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItemUsername.Size = new System.Drawing.Size(346, 30);
            this.layoutControlItemUsername.Text = "Kullanıcı Adı";
            this.layoutControlItemUsername.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItemUsername.TextSize = new System.Drawing.Size(60, 20);
            this.layoutControlItemUsername.TextToControlDistance = 5;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 60);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(173, 55);
            this.emptySpaceItem1.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // layoutControlItemPassword
            // 
            this.layoutControlItemPassword.Control = this.textEditPassword;
            this.layoutControlItemPassword.Location = new System.Drawing.Point(0, 30);
            this.layoutControlItemPassword.Name = "layoutControlItemPassword";
            this.layoutControlItemPassword.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlItemPassword.Size = new System.Drawing.Size(346, 30);
            this.layoutControlItemPassword.Text = "Şifre";
            this.layoutControlItemPassword.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItemPassword.TextSize = new System.Drawing.Size(60, 20);
            this.layoutControlItemPassword.TextToControlDistance = 5;
            // 
            // layoutControlItemLogin
            // 
            this.layoutControlItemLogin.Control = this.simpleButtonLogin;
            this.layoutControlItemLogin.Location = new System.Drawing.Point(173, 79);
            this.layoutControlItemLogin.Name = "layoutControlItemLogin";
            this.layoutControlItemLogin.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlItemLogin.Size = new System.Drawing.Size(173, 36);
            this.layoutControlItemLogin.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItemLogin.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItemLogin.TextToControlDistance = 0;
            this.layoutControlItemLogin.TextVisible = false;
            // 
            // emptySpaceItem2
            // 
            this.emptySpaceItem2.AllowHotTrack = false;
            this.emptySpaceItem2.Location = new System.Drawing.Point(173, 60);
            this.emptySpaceItem2.Name = "emptySpaceItem2";
            this.emptySpaceItem2.Size = new System.Drawing.Size(173, 19);
            this.emptySpaceItem2.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.emptySpaceItem2.TextSize = new System.Drawing.Size(0, 0);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 508);
            this.Controls.Add(this.groupControlLogin);
            this.Controls.Add(this.panelControl4);
            this.Controls.Add(this.panelControl3);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControlLogo);
            this.Name = "FormLogin";
            this.Text = "Royale Support - Giriş";
            this.Load += new System.EventHandler(this.FormLogin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControlLogo)).EndInit();
            this.panelControlLogo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControlLogin)).EndInit();
            this.groupControlLogin.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.editorLayoutControl1)).EndInit();
            this.editorLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEditPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEditUsername.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemUsername)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControlLogo;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.GroupControl groupControlLogin;
        private Core.Controls.EditorLayoutControl editorLayoutControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButtonLogin;
        private DevExpress.XtraEditors.TextEdit textEditPassword;
        private DevExpress.XtraEditors.TextEdit textEditUsername;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemUsername;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemPassword;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItemLogin;
        private DevExpress.XtraLayout.EmptySpaceItem emptySpaceItem2;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
    }
}