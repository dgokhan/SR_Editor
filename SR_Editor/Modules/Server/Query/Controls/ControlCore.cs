using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils.Controls;

namespace SR_Editor.Modules.Server.Query.Controls
{
    public partial class ControlCore : DevExpress.XtraEditors.XtraUserControl, IXtraResizableControl
    {
        public ControlCore()
        {
            InitializeComponent();

            labelControl1.Text = Guid.NewGuid().ToString();
            labelControl2.Text = Guid.NewGuid().ToString();
        }
    }
}
