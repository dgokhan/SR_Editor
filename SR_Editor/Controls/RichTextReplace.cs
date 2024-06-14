using SR_Editor.LookUp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SR_Editor.Core.Controls
{
    public partial class RichTextReplace : Form
    {

        public RichTextReplace(RichTextBox r, Rectangle rec)
        {
            this.rtb = r;
            this.InitializeComponent();
            this.txtFind.Text = this.rtb.SelectedText;
            this.Location = new Point(rec.X + (rec.Width - this.Width) / 2, rec.Y + (rec.Height - this.Height) / 2);
            this.txtFind_TextChanged((object)null, (EventArgs)null);
        }

        protected RichTextBoxFinds FindsOptions
        {
            get
            {
                RichTextBoxFinds richTextBoxFinds = RichTextBoxFinds.None;
                if (this.chWholeword.Checked)
                    richTextBoxFinds |= RichTextBoxFinds.WholeWord;
                if (this.chCase.Checked)
                    richTextBoxFinds |= RichTextBoxFinds.MatchCase;
                return richTextBoxFinds;
            }
        }

        protected void MessageNotFound(int p)
        {
            if (p != -1)
                return;
            int num = (int)UtilMessage.Show(EnumUtilMessage.ArananKayitBulunamadi, null, "Aranan kayıt bulunamadı.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        protected int Find()
        {
            return this.rtb.Find(this.txtFind.Text, this.rtb.SelectionStart + this.rtb.SelectionLength, this.rtb.MaxLength, this.FindsOptions);
        }

        protected int FindForReplace()
        {
            return this.rtb.Find(this.txtFind.Text, this.rtb.SelectionStart, this.rtb.MaxLength, this.FindsOptions);
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            this.MessageNotFound(this.Find());
        }

        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            this.btnFindNext.Enabled = this.txtFind.Text != "";
            this.btnReplace.Enabled = this.btnFindNext.Enabled;
            this.btnReplaceAll.Enabled = this.btnFindNext.Enabled;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            if (this.FindForReplace() != -1)
                this.rtb.SelectedText = this.txtReplace.Text;
            else
                this.MessageNotFound(-1);
        }

        private void btnReplaceAll_Click(object sender, EventArgs e)
        {
            int p = -1;
            int num = 0;
            this.rtb.SelectionStart = 0;
            this.rtb.SelectionLength = 0;
            while (num != -1)
            {
                num = this.Find();
                if (num != -1)
                {
                    ++p;
                    this.rtb.SelectedText = this.txtReplace.Text;
                }
            }
            this.MessageNotFound(p);
        }
    }
}
