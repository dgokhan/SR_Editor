namespace SR_Editor.Core.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    partial class RichTextReplace
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
        private Label label2;
        private TextBox txtReplace;
        private Button btnReplaceAll;
        private Button btnReplace;
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
            this.btnReplace = new Button();
            this.label1 = new Label();
            this.label2 = new Label();
            this.chCase = new CheckBox();
            this.txtFind = new TextBox();
            this.chWholeword = new CheckBox();
            this.btnReplaceAll = new Button();
            this.btnCancel = new Button();
            this.btnFindNext = new Button();
            this.txtReplace = new TextBox();
            this.SuspendLayout();
            this.btnReplace.FlatStyle = FlatStyle.Popup;
            this.btnReplace.Location = new Point(256, 32);
            this.btnReplace.Name = "btnReplace";
            this.btnReplace.Size = new Size(92, 24);
            this.btnReplace.TabIndex = 5;
            this.btnReplace.Text = "&Replace";
            this.btnReplace.Click += new EventHandler(this.btnReplace_Click);
            this.label1.Location = new Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find what:";
            this.label2.Location = new Point(8, 36);
            this.label2.Name = "label2";
            this.label2.Size = new Size(88, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Replace with:";
            this.chCase.Location = new Point(8, 88);
            this.chCase.Name = "chCase";
            this.chCase.Size = new Size(164, 16);
            this.chCase.TabIndex = 3;
            this.chCase.Text = "Match case";
            this.txtFind.Location = new Point(88, 4);
            this.txtFind.Name = "txtFind";
            this.txtFind.Size = new Size(156, 20);
            this.txtFind.TabIndex = 0;
            this.txtFind.TextChanged += new EventHandler(this.txtFind_TextChanged);
            this.chWholeword.Location = new Point(8, 64);
            this.chWholeword.Name = "chWholeword";
            this.chWholeword.Size = new Size(164, 16);
            this.chWholeword.TabIndex = 2;
            this.chWholeword.Text = "Match whole word only";
            this.btnReplaceAll.FlatStyle = FlatStyle.Popup;
            this.btnReplaceAll.Location = new Point(256, 60);
            this.btnReplaceAll.Name = "btnReplaceAll";
            this.btnReplaceAll.Size = new Size(92, 24);
            this.btnReplaceAll.TabIndex = 6;
            this.btnReplaceAll.Text = "Replace A&ll";
            this.btnReplaceAll.Click += new EventHandler(this.btnReplaceAll_Click);
            this.btnCancel.DialogResult = DialogResult.Cancel;
            this.btnCancel.FlatStyle = FlatStyle.Popup;
            this.btnCancel.Location = new Point(256, 92);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new Size(92, 24);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnFindNext.FlatStyle = FlatStyle.Popup;
            this.btnFindNext.Location = new Point(256, 4);
            this.btnFindNext.Name = "btnFindNext";
            this.btnFindNext.Size = new Size(92, 24);
            this.btnFindNext.TabIndex = 4;
            this.btnFindNext.Text = "&Find Next";
            this.btnFindNext.Click += new EventHandler(this.btnFindNext_Click);
            this.txtReplace.Location = new Point(88, 32);
            this.txtReplace.Name = "txtReplace";
            this.txtReplace.Size = new Size(156, 20);
            this.txtReplace.TabIndex = 1;
            this.AcceptButton = (IButtonControl)this.btnFindNext;
            this.AutoScaleBaseSize = new Size(5, 13);
            this.CancelButton = (IButtonControl)this.btnCancel;
            this.ClientSize = new Size(359, 130);
            this.Controls.Add((Control)this.btnReplaceAll);
            this.Controls.Add((Control)this.btnReplace);
            this.Controls.Add((Control)this.txtReplace);
            this.Controls.Add((Control)this.label2);
            this.Controls.Add((Control)this.btnCancel);
            this.Controls.Add((Control)this.btnFindNext);
            this.Controls.Add((Control)this.chCase);
            this.Controls.Add((Control)this.chWholeword);
            this.Controls.Add((Control)this.txtFind);
            this.Controls.Add((Control)this.label1);
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "RichTextReplace";
            this.StartPosition = FormStartPosition.Manual;
            this.Text = "Replace";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}