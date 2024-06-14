using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SR_Editor.Core;
using DevExpress.XtraEditors;
using SR_Editor.LookUp;

namespace SR_Editor.Modules.SqlQuery.Action.Controls
{
    public partial class UserControlParamaterAndValue : UserControl
    {
        public UserControlParamaterAndValue()
        {
            InitializeComponent();
        }

        private string ParameterAdi { get; set; }

        public byte DbType { get; private set; }

        public string Baslik
        {
            get
            {
                return this.textEditBaslik.EditValue == null ? string.Empty : this.textEditBaslik.EditValue.ToString();
            }
            set
            {
                this.textEditBaslik.EditValue = (object)value;
            }
        }

        public object Value
        {
            get
            {
                return this.edit != null ? this.edit.EditValue : (object)null;
            }
        }

        public UserControlParamaterAndValue(
          string parametreAdi,
          string baslik,
          bool readOnly = false,
          byte lookupSelectedValue = 0)
        {
            this.InitializeComponent();
            this.ParameterAdi = parametreAdi;
            this.Baslik = baslik;
            UtilLookUp.InitLookupEdit(this.lookUpEditParametreTypes);
            this.lookUpEditParametreTypes.Properties.DataSource = (object)SqlParametreTipi.Liste;
            if (lookupSelectedValue.IsNotNull())
                this.lookUpEditParametreTypes.EditValue = (object)lookupSelectedValue;
            this.layoutControlItemParametreType.Text = this.ParameterAdi;

            if(readOnly)
            {
                layoutControlItemBaşlık.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlItemParametreType.Text = this.Baslik;
                this.lookUpEditParametreTypes.ReadOnly = true;
            }
        }

        private void lookUpEditParametreTypes_EditValueChanged(object sender, EventArgs e)
        {
            object editValue = ((BaseEdit)sender).EditValue;
            if (editValue == null)
                return;
            if (editValue.Equals((object)(byte)1))
            {
                this.layoutControlParametreValue.Controls.Clear();
                TextEdit textEdit = new TextEdit();
                textEdit.Location = new Point(0, 0);
                textEdit.Name = "textEdit" + this.ParameterAdi;
                textEdit.Dock = DockStyle.Fill;
                this.edit = (BaseEdit)textEdit;
                textEdit.MinimumSize = new Size(300, 0);
                this.layoutControlParametreValue.Controls.Add((Control)this.edit);
                this.DbType = (byte)1;
            }
            else if (editValue.Equals((object)(byte)2))
            {
                this.layoutControlParametreValue.Controls.Clear();
                DateEdit dateEdit = new DateEdit();
                dateEdit.Location = new Point(0, 0);
                dateEdit.Properties.Mask.EditMask = "dd-MM-yyyy HH:mm:ss";
                dateEdit.Properties.DisplayFormat.FormatString = "dd-MM-yyyy HH:mm:ss";
                dateEdit.Properties.EditFormat.FormatString = "dd-MM-yyyy HH:mm:ss";
                dateEdit.Name = "dateEdit" + this.ParameterAdi;
                dateEdit.Dock = DockStyle.Fill;
                dateEdit.MinimumSize = new Size(300, 0);
                this.edit = (BaseEdit)dateEdit;
                this.layoutControlParametreValue.Controls.Add((Control)this.edit);
                this.DbType = (byte)2;
            }
            else if(editValue.Equals((object)(byte)3))
            {
                this.layoutControlParametreValue.Controls.Clear();
                CheckEdit checkEdit = new CheckEdit();
                checkEdit.Location = new Point(0, 0);
                checkEdit.Dock = DockStyle.Fill;
                checkEdit.Name = "checkEdit" + this.ParameterAdi;
                this.edit = (BaseEdit)checkEdit;
                this.layoutControlParametreValue.Controls.Add((Control)this.edit);
                this.DbType = (byte)3;
            }
            else if (editValue.Equals((object)(byte)4))
            {
                this.layoutControlParametreValue.Controls.Clear();
                TextEdit textEdit = new TextEdit();
                textEdit.Location = new Point(0, 0);
                textEdit.Name = "textEdit" + this.ParameterAdi;
                textEdit.Dock = DockStyle.Fill;
                this.edit = (BaseEdit)textEdit;
                textEdit.MinimumSize = new Size(300, 0);
                this.layoutControlParametreValue.Controls.Add((Control)this.edit);
                this.DbType = (byte)4;
            }
        }
    }
}
