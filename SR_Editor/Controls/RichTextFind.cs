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
    public partial class RichTextFind : Form
    {
        public RichTextFind(RichTextBox r, Rectangle rec)
        {
            this.rtb = r;
            this.rtb.SelectionStart = 0;
            this.InitializeComponent();
            this.Location = new Point(rec.X + (rec.Width - this.Width) / 2, rec.Y + (rec.Height - this.Height) / 2);
            this.txtFind_TextChanged((object)null, (EventArgs)null);
        }

        private void btnFindNext_Click(object sender, EventArgs e)
        {
            RichTextBoxFinds options = RichTextBoxFinds.None;
            if (this.chWholeword.Checked)
                options |= RichTextBoxFinds.WholeWord;
            if (this.chCase.Checked)
                options |= RichTextBoxFinds.MatchCase;
            if (this.rtb.Find(this.txtFind.Text, this.rtb.SelectionStart + this.rtb.SelectionLength, this.rtb.MaxLength, options) != -1)
                return;
            int num = (int)UtilMessage.Show(EnumUtilMessage.ArananKayitBulunamadi, null, "Aranan kayıt bulunamadı.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            this.btnFindNext.Enabled = this.txtFind.Text != "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
