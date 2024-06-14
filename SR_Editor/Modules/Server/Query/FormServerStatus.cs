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
using DevExpress.Utils.Extensions;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using SR_Editor.Modules.Server.Query.Controls;

namespace SR_Editor.Modules.Server.Query
{
    public partial class FormServerStatus : DevExpress.XtraEditors.XtraForm
    {
        public FormServerStatus()
        {
            InitializeComponent();

            var serverAuth = new ControlServer();
            serverAuth.layoutControlGroup.BeginUpdate();
            //serverAuth.Parent = editorLayoutControlServer;
            {
                LayoutControlItem item = serverAuth.layoutControlGroup.AddItem("PhItem", new ControlCore(), serverAuth.layoutControlGroup, InsertType.Top);
                item.TextVisible = false;
            }
            {
                //LayoutControlItem item = serverAuth.layoutControlGroup.AddItem("PhItem2", new ControlCore(), serverAuth.layoutControlGroup, InsertType.Top);
                //item.TextVisible = false;
                //item.Move(serverAuth.layoutControlGroup, InsertType.Top);
            }
            serverAuth.layoutControlGroup.EndUpdate();
            //serverAuth.layoutControlGroup.AddItem("Core2", new ControlCore(), InsertType.Top);
            serverAuth.layoutControlGroup.Invalidate();
            serverAuth.layoutControlGroup.Update();
            serverAuth.layoutControlGroup.BestFit();
            serverAuth.Invalidate();
            xtraScrollableControl2.AddControl(serverAuth);
            xtraScrollableControl2.Invalidate();

        }
    }
}