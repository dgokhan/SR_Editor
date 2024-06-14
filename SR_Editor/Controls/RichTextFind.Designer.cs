namespace SR_Editor.Core.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;
    partial class RichTextFind
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private RichTextBox rtb;
        private CheckBox chWholeword;
        private TextBox txtFind;
        private Button btnFindNext;
        private CheckBox chCase;
        private Button btnCancel;
        private Label label1;

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
            this.chWholeword = new CheckBox();
            this.label1 = new Label();
            this.btnFindNext = new Button();
            this.btnCancel = new Button();
            this.txtFind = new TextBox();
            this.chCase = new CheckBox();
            this.SuspendLayout();
            this.chWholeword.Location = new Point(8, 32);
            this.chWholeword.Name = "chWholeword";
            this.chWholeword.Size = new Size(164, 16);
            this.chWholeword.TabIndex = 1;
            this.chWholeword.Text = "Match whole word only";
            this.label1.Location = new Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(60, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Find what:";
            this.btnFindNext.FlatStyle = FlatStyle.Popup;
            this.btnFindNext.Location = new Point(248, 4);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new Size(92, 24);
            this.btnFindNext.TabIndex = 3;
            this.btnFindNext.Text = "&Find Next";
            this.btnFindNext.Click += new EventHandler(this.btnFindNext_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.FlatStyle = FlatStyle.Popup;
            this.btnCancel.Location = new Point(248, 36);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(92, 24);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.txtFind.Location = new Point(80, 4);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new Size(156, 20);
            this.txtFind.TabIndex = 0;
            this.txtFind.TextChanged += new EventHandler(this.txtFind_TextChanged);
            this.chCase.Location = new Point(8, 56);
            this.chCase.Name = "chCase";
            this.chCase.Size = new Size(164, 16);
            this.chCase.TabIndex = 2;
            this.chCase.Text = "Match case";
            this.AcceptButton = (IButtonControl)this.btnFindNext;
            this.AutoScaleBaseSize = new Size(5, 13);
            this.CancelButton = (IButtonControl)this.btnCancel;
            this.ClientSize = new Size(351, 82);
            this.Controls.Add((Control)this.btnCancel);
            this.Controls.Add((Control)this.btnFindNext);
            this.Controls.Add((Control)this.chCase);
            this.Controls.Add((Control)this.chWholeword);
            this.Controls.Add((Control)this.txtFind);
            this.Controls.Add((Control)this.label1);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "RichTextFind";
            this.StartPosition = FormStartPosition.Manual;
            this.Text = "Find ";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}