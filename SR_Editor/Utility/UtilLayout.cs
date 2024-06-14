using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using System;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace SR_Editor.Core
{
	public class UtilLayout
	{
		public UtilLayout()
		{
		}

		public static void SetControlsReadOnly(Control control, bool pReadOnly)
		{
			foreach (Control control1 in control.Controls)
			{
				if (control1 is BaseEdit)
				{
					((BaseEdit)control1).Properties.ReadOnly = pReadOnly;
				}
				if (control1.Controls.Count > 0)
				{
					UtilLayout.SetControlsReadOnly(control1, pReadOnly);
				}
			}
		}

		public static void SetReadOnly(LayoutControlGroup control)
		{
			for (int i = 0; i < control.Items.Count; i++)
			{
				if (control.Items[i] is LayoutControlItem)
				{
					if ((((LayoutControlItem)control.Items[i]).Control == null ? false : ((LayoutControlItem)control.Items[i]).Control is BaseEdit))
					{
						((BaseEdit)((LayoutControlItem)control.Items[i]).Control).Properties.ReadOnly = true;
					}
				}
			}
		}

		public static void SetReadOnly(LayoutControlGroup control, bool pReadOnly)
		{
			for (int i = 0; i < control.Items.Count; i++)
			{
				if (control.Items[i] is LayoutControlItem)
				{
					if ((((LayoutControlItem)control.Items[i]).Control == null ? false : ((LayoutControlItem)control.Items[i]).Control is BaseEdit))
					{
						((BaseEdit)((LayoutControlItem)control.Items[i]).Control).Properties.ReadOnly = pReadOnly;
					}
				}
			}
		}

		public static void SetReadOnly(LayoutControl control, bool pReadOnly)
		{
			for (int i = 0; i < control.Items.Count; i++)
			{
				if (control.Items[i] is LayoutControlItem)
				{
					if ((((LayoutControlItem)control.Items[i]).Control == null ? false : ((LayoutControlItem)control.Items[i]).Control is BaseEdit))
					{
						((BaseEdit)((LayoutControlItem)control.Items[i]).Control).Properties.ReadOnly = pReadOnly;
					}
				}
			}
		}
	}
}