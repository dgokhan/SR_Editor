using DevExpress.XtraLayout;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using System;

namespace SR_Editor.Modules.SqlQuery.Action.Controls
{
    partial class UserControlParamaterAndValue
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private LayoutControl layoutControl1;
        private LayoutControlGroup layoutControlGroup1;
        private LayoutControlItem layoutControlItemParametreType;
        private LayoutControl layoutControlParametreValue;
        private LayoutControlGroup Root;
        private EmptySpaceItem emptySpaceItem1;
        private LayoutControlItem layoutControlItem1;
        private LayoutControlItem layoutControlItemBaşlık;
        private BaseEdit edit;

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
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.textEditBaslik = new DevExpress.XtraEditors.TextEdit();
            this.layoutControlParametreValue = new DevExpress.XtraLayout.LayoutControl();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.emptySpaceItem1 = new DevExpress.XtraLayout.EmptySpaceItem();
            this.lookUpEditParametreTypes = new DevExpress.XtraEditors.LookUpEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItemParametreType = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItemBaşlık = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textEditBaslik.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlParametreValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditParametreTypes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParametreType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemBaşlık)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Controls.Add(this.textEditBaslik);
            this.layoutControl1.Controls.Add(this.layoutControlParametreValue);
            this.layoutControl1.Controls.Add(this.lookUpEditParametreTypes);
            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(277, 139, 250, 350);
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(622, 35);
            this.layoutControl1.TabIndex = 0;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // textEditBaslik
            // 
            this.textEditBaslik.Location = new System.Drawing.Point(98, 7);
            this.textEditBaslik.Name = "textEditBaslik";
            this.textEditBaslik.Size = new System.Drawing.Size(79, 20);
            this.textEditBaslik.StyleController = this.layoutControl1;
            this.textEditBaslik.TabIndex = 6;
            // 
            // layoutControlParametreValue
            // 
            this.layoutControlParametreValue.Location = new System.Drawing.Point(352, 7);
            this.layoutControlParametreValue.Name = "layoutControlParametreValue";
            this.layoutControlParametreValue.Root = this.Root;
            this.layoutControlParametreValue.Size = new System.Drawing.Size(263, 21);
            this.layoutControlParametreValue.TabIndex = 5;
            this.layoutControlParametreValue.Text = "layoutControl2";
            // 
            // Root
            // 
            this.Root.CustomizationFormText = "Root";
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.emptySpaceItem1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.Root.Size = new System.Drawing.Size(263, 21);
            this.Root.TextVisible = false;
            // 
            // emptySpaceItem1
            // 
            this.emptySpaceItem1.AllowHotTrack = false;
            this.emptySpaceItem1.CustomizationFormText = "emptySpaceItem1";
            this.emptySpaceItem1.Location = new System.Drawing.Point(0, 0);
            this.emptySpaceItem1.Name = "emptySpaceItem1";
            this.emptySpaceItem1.Size = new System.Drawing.Size(263, 21);
            this.emptySpaceItem1.TextSize = new System.Drawing.Size(0, 0);
            // 
            // lookUpEditParametreTypes
            // 
            this.lookUpEditParametreTypes.Location = new System.Drawing.Point(266, 7);
            this.lookUpEditParametreTypes.Name = "lookUpEditParametreTypes";
            this.lookUpEditParametreTypes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpEditParametreTypes.Properties.NullText = "";
            this.lookUpEditParametreTypes.Size = new System.Drawing.Size(82, 20);
            this.lookUpEditParametreTypes.StyleController = this.layoutControl1;
            this.lookUpEditParametreTypes.TabIndex = 4;
            this.lookUpEditParametreTypes.EditValueChanged += new System.EventHandler(this.lookUpEditParametreTypes_EditValueChanged);
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItemParametreType,
            this.layoutControlItem1,
            this.layoutControlItemBaşlık});
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 5, 5);
            this.layoutControlGroup1.Size = new System.Drawing.Size(622, 35);
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItemParametreType
            // 
            this.layoutControlItemParametreType.Control = this.lookUpEditParametreTypes;
            this.layoutControlItemParametreType.CustomizationFormText = "layoutControlItemParametreType";
            this.layoutControlItemParametreType.Location = new System.Drawing.Point(174, 0);
            this.layoutControlItemParametreType.Name = "layoutControlItemParametreType";
            this.layoutControlItemParametreType.Size = new System.Drawing.Size(171, 25);
            this.layoutControlItemParametreType.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItemParametreType.TextSize = new System.Drawing.Size(80, 13);
            this.layoutControlItemParametreType.TextToControlDistance = 5;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.layoutControlParametreValue;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(345, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(267, 25);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItemBaşlık
            // 
            this.layoutControlItemBaşlık.Control = this.textEditBaslik;
            this.layoutControlItemBaşlık.CustomizationFormText = "Parametre Başlık";
            this.layoutControlItemBaşlık.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItemBaşlık.Name = "layoutControlItemBaşlık";
            this.layoutControlItemBaşlık.Size = new System.Drawing.Size(174, 25);
            this.layoutControlItemBaşlık.Text = "Parametre Başlık";
            this.layoutControlItemBaşlık.TextSize = new System.Drawing.Size(79, 13);
            // 
            // UserControlParamaterAndValue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "UserControlParamaterAndValue";
            this.Size = new System.Drawing.Size(622, 35);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textEditBaslik.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlParametreValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.emptySpaceItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpEditParametreTypes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemParametreType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItemBaşlık)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public TextEdit textEditBaslik;
        public LookUpEdit lookUpEditParametreTypes;
    }
}
