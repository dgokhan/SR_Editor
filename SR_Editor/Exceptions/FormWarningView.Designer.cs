using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace SR_Editor.Core.Exceptions
{
    partial class FormWarningView
    {
        /// <summary>
        /// Required designer variable.
        private System.ComponentModel.IContainer components = null;
        private Panel panelTop;
        internal Label lblMesaj;
        private SimpleButton simpleButtonTamam;

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
            this.panelTop = new Panel();
            this.lblMesaj = new Label();
            this.simpleButtonTamam = new SimpleButton();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            this.panelTop.BackColor = SystemColors.Window;
            this.panelTop.Controls.Add((Control)this.lblMesaj);
            this.panelTop.Dock = DockStyle.Top;
            this.panelTop.Location = new Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new Size(542, 135);
            this.panelTop.TabIndex = 2;
            this.lblMesaj.Font = new Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte)0);
            this.lblMesaj.Location = new Point(12, 13);
            this.lblMesaj.Name = "lblMesaj";
            this.lblMesaj.Size = new Size(518, 106);
            this.lblMesaj.TabIndex = 0;
            this.lblMesaj.Text = "Hata";
            this.simpleButtonTamam.Location = new Point(478, 141);
            this.simpleButtonTamam.Name = "simpleButtonTamam";
            this.simpleButtonTamam.Size = new Size(52, 23);
            this.simpleButtonTamam.TabIndex = 13;
            this.simpleButtonTamam.Text = "Tamam";
            this.simpleButtonTamam.Click += new EventHandler(this.simpleButtonTamam_Click);
            this.AcceptButton = (IButtonControl)this.simpleButtonTamam;
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(542, 173);
            this.Controls.Add((Control)this.simpleButtonTamam);
            this.Controls.Add((Control)this.panelTop);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.IsDialog = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = nameof(FormWarningView);
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.Text = nameof(FormWarningView);
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion
    }
}