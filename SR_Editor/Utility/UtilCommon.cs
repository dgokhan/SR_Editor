using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Menu;
using Microsoft.Win32;
//using SR_Editor.Core.Turkcelestirme;
using SR_Editor.Core.Utility;
using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public class UtilCommon
	{
		public UtilCommon()
		{
		}

		public static object GetRegistryValue(string pRegistryGroup, string pRegistryKey)
		{
			object obj;
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(pRegistryGroup, false);
			if (registryKey == null)
			{
				obj = null;
			}
			else
			{
				object value = registryKey.GetValue(pRegistryKey);
				registryKey.Close();
				obj = value;
			}
			return obj;
		}

		public static void Init(GridControl pGridControl, QBaseEntity pQBaseEntity, PopupMenu pPopUpMenu)
		{
			pGridControl.FindForm();
			//pGridControl.Tag = new DataControlExport(pQBaseEntity, pPopUpMenu, pGridControl.Views[0], "");
			pGridControl.MouseUp += new MouseEventHandler(UtilCommon.pDataControl_MouseUp);
		}

		public static void InitPopupMenu(BaseEdit pControl, PopupMenu pMenu)
		{
			pControl.Tag = pMenu;
			pControl.MouseUp += new MouseEventHandler(UtilCommon.pControl_MouseUp);
		}

		public static void InitPopupMenu(GridView pGridView, PopupMenu pMenu)
		{
			try
			{
				pGridView.Tag = pMenu;
				UtilCommon.LoadTasarim(pGridView);
				pGridView.PopupMenuShowing -= new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(UtilCommon.pGridView_PopupMenuShowing);
				pGridView.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(UtilCommon.pGridView_PopupMenuShowing);
			}
			catch
			{
			}
		}

		public static void InitPopupMenu(BandedGridView pGridView, PopupMenu pMenu)
		{
			try
			{
				pGridView.Tag = pMenu;
				UtilCommon.LoadTasarim(pGridView);
				pGridView.PopupMenuShowing -= new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(UtilCommon.pGridView_PopupMenuShowing);
				pGridView.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(UtilCommon.pGridView_PopupMenuShowing);
			}
			catch
			{
			}
		}

		public static void InitPopupMenu(GridControl pControl, PopupMenu pMenu)
		{
			pControl.Tag = pMenu;
			pControl.MouseUp += new MouseEventHandler(UtilCommon.pControl_MouseUp);
		}

		public static void InitPopupMenu(TreeList pControl, PopupMenu pMenu)
		{
			try
			{
				pControl.Tag = pMenu;
				pControl.MouseUp += new MouseEventHandler(UtilCommon.pControl_MouseUp);
				UtilCommon.LoadTasarim(pControl);
				pControl.PopupMenuShowing -= new DevExpress.XtraTreeList.PopupMenuShowingEventHandler(UtilCommon.pControl_PopupMenuShowing);
				pControl.PopupMenuShowing += new DevExpress.XtraTreeList.PopupMenuShowingEventHandler(UtilCommon.pControl_PopupMenuShowing);
			}
			catch
			{
			}
		}

		public static void InitRight(BaseEdit pBaseEdit, bool pIsRight, EnumRightOptions pRightOptions)
		{
			if (pRightOptions == EnumRightOptions.Readonly)
			{
				UtilCommon.SetReadOnly(pBaseEdit, !pIsRight);
			}
		}

		public static void InitRight(BarItem pBarItem, bool pIsRight, EnumRightOptions pRightOptions)
		{
			if ((pRightOptions == EnumRightOptions.Readonly ? true : pRightOptions == EnumRightOptions.Enable))
			{
				pBarItem.Enabled = pIsRight;
			}
		}

		public static void InitShortcut(ModuleInfo pModuleInfo, BarItem pBarItem, EnumShortcut pEnumShortcut)
		{
			if (pEnumShortcut == EnumShortcut.Save)
			{
				UtilCommon.InitShortcut(pModuleInfo, pBarItem, Keys.LButton | Keys.RButton | Keys.Cancel | Keys.ShiftKey | Keys.ControlKey | Keys.Menu | Keys.Pause | Keys.A | Keys.B | Keys.C | Keys.P | Keys.Q | Keys.R | Keys.S | Keys.Control);
			}
			else if (pEnumShortcut == EnumShortcut.Save)
			{
				UtilCommon.InitShortcut(pModuleInfo, pBarItem, Keys.Back | Keys.ShiftKey | Keys.FinalMode | Keys.H | Keys.P | Keys.X | Keys.Control);
			}
			else if (pEnumShortcut == EnumShortcut.List)
			{
				UtilCommon.InitShortcut(pModuleInfo, pBarItem, Keys.MButton | Keys.Back | Keys.Clear | Keys.D | Keys.H | Keys.L | Keys.Control);
			}
		}

		public static void InitShortcut(ModuleInfo pModuleInfo, BarItem pBarItem, Keys pKeys)
		{
			pBarItem.ItemShortcut = new BarShortcut(pKeys);
		}

		private static void LoadTasarim(GridView pGridView)
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(string.Concat(UtilConfig.AktifModuleInfo.FullKey, ".", pGridView.Name));
			bool flag = false;
			if (registryKey != null)
			{
				pGridView.RestoreLayoutFromRegistry(string.Concat(UtilConfig.AktifModuleInfo.FullKey, ".", pGridView.Name));
				flag = true;
			}
			//UtilFormTasarim.GridViewTasarimiYukle(pGridView, UtilConfig.AktifModuleInfo.FullKey, flag);
		}

		private static void LoadTasarim(TreeList pTreeList)
		{
			if (Registry.CurrentUser.OpenSubKey(string.Concat(UtilConfig.AktifModuleInfo.FullKey, ".", pTreeList.Name)) == null)
			{
				//UtilFormTasarim.TreeListTasarimiYukle(pTreeList, UtilConfig.AktifModuleInfo.FullKey);
			}
			else
			{
				pTreeList.RestoreLayoutFromRegistry(string.Concat(UtilConfig.AktifModuleInfo.FullKey, ".", pTreeList.Name));
			}
		}

		private static void pControl_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (sender is BaseEdit)
				{
					((sender as BaseEdit).Tag as PopupMenu).ShowPopup((sender as BaseEdit).PointToScreen(new Point(e.X, e.Y)));
				}
				if (sender is GridControl)
				{
					((sender as GridControl).Tag as PopupMenu).ShowPopup((sender as GridControl).PointToScreen(new Point(e.X, e.Y)));
				}
				if (sender is TreeList)
				{
					((sender as TreeList).Tag as PopupMenu).ShowPopup((sender as TreeList).PointToScreen(new Point(e.X, e.Y)));
				}
			}
		}

		private static void pControl_PopupMenuShowing(object sender, DevExpress.XtraTreeList.PopupMenuShowingEventArgs e)
		{
			if (e.Menu.MenuType == TreeListMenuType.Column)
			{
				//(new TurkceTreeListMenuIslemleri(e)).MenuHazirla();
			}
			else if (e.Menu.MenuType == TreeListMenuType.User)
			{
				var x = (sender as BaseView);
				if (x == null)
					return;

				PopupMenu tag = x.Tag as PopupMenu;
				if (tag != null)
				{
					tag.ShowPopup(Control.MousePosition);
				}
			}
		}

		private static void pDataControl_MouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				if (sender is BaseEdit)
				{
					//((sender as BaseEdit).Tag as DataControlExport).PopUpMenu.ShowPopup((sender as BaseEdit).PointToScreen(new Point(e.X, e.Y)));
				}
				if (sender is GridControl)
				{
					//((sender as GridControl).Tag as DataControlExport).PopUpMenu.ShowPopup((sender as GridControl).PointToScreen(new Point(e.X, e.Y)));
				}
				if (sender is TreeList)
				{
					//((sender as TreeList).Tag as DataControlExport).PopUpMenu.ShowPopup((sender as TreeList).PointToScreen(new Point(e.X, e.Y)));
				}
			}
		}

		private static void pGridView_PopupMenuShowing(object sender, DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventArgs e)
		{
			PopupMenu tag;
			if (e.MenuType == GridMenuType.Column)
			{
				//(new TurkceGridMenuIslemleri(e)).MenuHazirla();
			}
			else if (!(e.MenuType != GridMenuType.Row ? true : !(sender as GridView).IsDataRow(e.HitInfo.RowHandle)))
			{
				tag = (sender as BaseView).Tag as PopupMenu;
				if (tag != null)
				{
					tag.ShowPopup(Control.MousePosition);
				}
			}
			else if (e.MenuType == GridMenuType.User)
			{
				tag = (sender as BaseView).Tag as PopupMenu;
				if (tag != null)
				{
					tag.ShowPopup(Control.MousePosition);
				}
			}
		}

		public static void SetReadOnly(BaseEdit pBaseEdit, bool pIsReadOnly)
		{
			pBaseEdit.Properties.ReadOnly = pIsReadOnly;
		}

		public static void SetReadOnly(GridView pGridView, bool pIsReadOnly)
		{
			pGridView.OptionsBehavior.Editable = !pIsReadOnly;
		}

		public static void SetReadOnly(TreeList pTreeList, bool pIsReadOnly)
		{
			pTreeList.OptionsBehavior.Editable = !pIsReadOnly;
		}

		public static void SetReadOnly(LayoutControlGroup control, bool pIsReadOnly)
		{
			for (int i = 0; i < control.Items.Count; i++)
			{
				if (control.Items[i] is LayoutControlItem)
				{
					if ((((LayoutControlItem)control.Items[i]).Control == null ? false : ((LayoutControlItem)control.Items[i]).Control is BaseEdit))
					{
						((BaseEdit)((LayoutControlItem)control.Items[i]).Control).Properties.ReadOnly = pIsReadOnly;
					}
				}
			}
		}

		public static void SetReadOnly(LayoutControl control, bool pIsReadOnly)
		{
			for (int i = 0; i < control.Items.Count; i++)
			{
				if (control.Items[i] is LayoutControlItem)
				{
					if ((((LayoutControlItem)control.Items[i]).Control == null ? false : ((LayoutControlItem)control.Items[i]).Control is BaseEdit))
					{
						((BaseEdit)((LayoutControlItem)control.Items[i]).Control).Properties.ReadOnly = pIsReadOnly;
					}
				}
			}
		}

		public static void SetRegistryValue(string pRegistryGroup, string pRegistryKey, object pValue)
		{
			RegistryKey registryKey = Registry.CurrentUser.OpenSubKey(pRegistryGroup, true);
			if (registryKey == null)
			{
				registryKey = Registry.CurrentUser.CreateSubKey(pRegistryGroup);
			}
			registryKey.SetValue(pRegistryKey, pValue);
			registryKey.Close();
		}
	}
}