using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Controls;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Container;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Layout;
using DevExpress.XtraLayout;
using DevExpress.XtraPivotGrid;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Columns;
using SR_Editor.Core;
using SR_Editor.LookUp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SR_Editor.Core.Utility
{
	public class UtilLanguage
	{
		public UtilLanguage()
		{
		}

        public static string GetCeviriStr(string pMesaj, bool v)
        {
            return pMesaj;
        }

        /*

		private static void CeviriYap(object control, string pKey)
		{
			DilCeviriEntity aktifDilCeviriEntity = DilCeviri.GetAktifDilCeviriEntity(pKey);
			string ceviri = null;
			string ikinciCeviri = null;
			if (Editor.Core.EditorApplication.EditorApplication.LanguageId != Editor.Core.EditorApplication.EditorApplication.SecondLanguageId)
			{
				if (Editor.Core.EditorApplication.EditorApplication.SecondLanguageId == 1)
				{
					ikinciCeviri = pKey;
				}
				else if ((!aktifDilCeviriEntity.IsDolu() ? false : aktifDilCeviriEntity.IkinciCeviri.IsDolu()))
				{
					ikinciCeviri = aktifDilCeviriEntity.IkinciCeviri;
				}
			}
			if ((!aktifDilCeviriEntity.IsDolu() || !aktifDilCeviriEntity.Ceviri.IsDolu() ? false : Editor.Core.EditorApplication.EditorApplication.LanguageId != 1))
			{
				ceviri = aktifDilCeviriEntity.Ceviri;
			}
			if (control is System.Windows.Forms.Form)
			{
				if (ceviri.IsDolu())
				{
					(control as System.Windows.Forms.Form).Text = ceviri;
				}
			}
			else if (control is LayoutControlItem)
			{
				if (Editor.Core.EditorApplication.EditorApplication.LanguageId == 2)
				{
					(control as LayoutControlItem).TextLocation = Locations.Right;
				}
				if (ceviri.IsDolu())
				{
					(control as LayoutControlItem).Text = ceviri;
				}
				if (ikinciCeviri.IsDolu())
				{
					(control as LayoutControlItem).OptionsToolTip.ToolTip = ikinciCeviri;
				}
			}
			else if (control is LayoutControlGroup)
			{
				if (Editor.Core.EditorApplication.EditorApplication.LanguageId == 2)
				{
					(control as LayoutControlGroup).TextLocation = Locations.Right;
				}
				if (ceviri.IsDolu())
				{
					(control as LayoutControlGroup).Text = ceviri;
				}
				if (ikinciCeviri.IsDolu())
				{
					(control as LayoutControlGroup).OptionsToolTip.ToolTip = ikinciCeviri;
				}
			}
			else if (control is BarItem)
			{
				if (ceviri.IsDolu())
				{
					(control as BarItem).Caption = ceviri;
				}
			}
			else if (control is GridColumn)
			{
				if (ceviri.IsDolu())
				{
					(control as GridColumn).Caption = ceviri;
				}
				if (ikinciCeviri.IsDolu())
				{
					(control as GridColumn).ToolTip = ikinciCeviri;
				}
			}
			else if (control is ChartTitle)
			{
				if (ceviri.IsDolu())
				{
					(control as ChartTitle).Text = ceviri;
				}
			}
			else if (control is PivotGridField)
			{
				if (ceviri.IsDolu())
				{
					(control as PivotGridField).Caption = ceviri;
				}
			}
			else if (control is TreeListColumn)
			{
				if (ceviri.IsDolu())
				{
					(control as TreeListColumn).Caption = ceviri;
				}
			}
			else if (control is GridBand)
			{
				if (ceviri.IsDolu())
				{
					(control as GridBand).Caption = ceviri;
				}
				if (ikinciCeviri.IsDolu())
				{
					(control as GridBand).ToolTip = ikinciCeviri;
				}
			}
			else if (control is GroupControl)
			{
				if (ceviri.IsDolu())
				{
					(control as GroupControl).Text = ceviri;
				}
			}
			else if (control is LabelControl)
			{
				if (ceviri.IsDolu())
				{
					(control as LabelControl).Text = ceviri;
				}
				if (ikinciCeviri.IsDolu())
				{
					(control as LabelControl).ToolTip = ikinciCeviri;
				}
			}
			else if (control is HyperLinkEdit)
			{
				if (ceviri.IsDolu())
				{
					(control as HyperLinkEdit).Text = ceviri;
				}
				if (ikinciCeviri.IsDolu())
				{
					(control as HyperLinkEdit).ToolTip = ikinciCeviri;
				}
			}
			else if (control is Label)
			{
				if (ceviri.IsDolu())
				{
					(control as Label).Text = ceviri;
				}
			}
			else if (control is Button)
			{
				if (ceviri.IsDolu())
				{
					(control as Button).Text = ceviri;
				}
			}
			else if (control is SimpleButton)
			{
				if (ceviri.IsDolu())
				{
					(control as SimpleButton).Text = ceviri;
				}
			}
			else if (control is CheckEdit)
			{
				if (ceviri.IsDolu())
				{
					(control as CheckEdit).Text = ceviri;
				}
				if (ikinciCeviri.IsDolu())
				{
					(control as CheckEdit).ToolTip = ikinciCeviri;
				}
			}
			else if (control is RadioGroupItem)
			{
				if (ceviri.IsDolu())
				{
					(control as RadioGroupItem).Description = ceviri;
				}
			}
			else if (!(!(control is NavBarGroupControlContainer) ? true : !(control as NavBarGroupControlContainer).OwnerGroup.IsDolu()))
			{
				if (ceviri.IsDolu())
				{
					(control as NavBarGroupControlContainer).OwnerGroup.Caption = ceviri;
				}
			}
			else if (control is DockPanel)
			{
				if (ceviri.IsDolu())
				{
					(control as DockPanel).Text = ceviri;
				}
			}
			else if (control is ToolStripMenuItem)
			{
				if (ceviri.IsDolu())
				{
					(control as ToolStripMenuItem).Text = ceviri;
				}
			}
			else if (control is RadioGroupItem)
			{
				if (ceviri.IsDolu())
				{
					(control as RadioGroupItem).Description = ceviri;
				}
			}
			else if (control is CheckedListBoxItem)
			{
				if (ceviri.IsDolu())
				{
					(control as CheckedListBoxItem).Description = ceviri;
				}
			}
			else if (control is GridView)
			{
				if (ceviri.IsDolu())
				{
					(control as GridView).GroupPanelText = ceviri;
				}
			}
			else if (control is ImageComboBoxItem)
			{
				if (ceviri.IsDolu())
				{
					(control as ImageComboBoxItem).Description = ceviri;
				}
			}
			else if (control is LookUpColumnInfo)
			{
				if (ceviri.IsDolu())
				{
					(control as LookUpColumnInfo).Caption = ceviri;
				}
			}
		}

		public static void CeviriYukle()
		{
			if ((Editor.Core.EditorApplication.EditorApplication.LanguageId != 1 ? true : Editor.Core.EditorApplication.EditorApplication.SecondLanguageId != 1))
			{
				foreach (DilReferansCeviri byDilTipiId in CoreEntities.Instance.DilReferansCeviriQuery.GetByDilTipiId(Editor.Core.EditorApplication.EditorApplication.LanguageId, Editor.Core.EditorApplication.EditorApplication.SecondLanguageId))
				{
					List<DilCeviriEntity> dilCeviriEntities = DilCeviri.listDilReferansCeviri;
					DilCeviriEntity dilCeviriEntity = new DilCeviriEntity()
					{
						DilReferansKey = byDilTipiId.DilReferansKey.Replace(" ", "").Replace("\n", ""),
						Ceviri = byDilTipiId.Ceviri,
						IkinciCeviri = byDilTipiId.EkIkinciCeviri
					};
					dilCeviriEntities.Add(dilCeviriEntity);
				}
				List<DilMesajCeviri> dilMesajCeviris = CoreEntities.Instance.DilMesajCeviriQuery.GetByDilTipiId(Editor.Core.EditorApplication.EditorApplication.LanguageId);
				DilCeviri.listDilMesajCeviriEntity.Clear();
				foreach (DilMesajCeviri dilMesajCevirus in dilMesajCeviris)
				{
					List<DilMesajCeviriEntity> dilMesajCeviriEntities = DilCeviri.listDilMesajCeviriEntity;
					DilMesajCeviriEntity dilMesajCeviriEntity = new DilMesajCeviriEntity()
					{
						MesajTipiId = dilMesajCevirus.MesajTipiId,
						MesajBaslik = dilMesajCevirus.MesajBaslik,
						MesajIcerik = dilMesajCevirus.MesajIcerik,
						IsMesajBaslikSabit = dilMesajCevirus.IsMesajBaslikSabit.IsBosIse(false),
						IsMesajIcerikSabit = dilMesajCevirus.IsMesajIcerikSabit.IsBosIse(false)
					};
					dilMesajCeviriEntities.Add(dilMesajCeviriEntity);
				}
			}
		}

		public static void FormCeviriYap(System.Windows.Forms.Form aktifForm)
		{
			BarItem item = null;
			Control control = null;
			bool flag = false;
			UtilLanguage.CeviriYap(aktifForm, aktifForm.Text);
			foreach (Control control1 in aktifForm.Controls)
			{
				if (control1 is LayoutControl)
				{
					UtilLanguage.LayoutControlItemCeviriYap(control1 as LayoutControl);
				}
				else if (!(flag ? true : !(control1 is BarDockControl)))
				{
					if (((control1 as BarDockControl).Manager == null ? false : (control1 as BarDockControl).Manager.Items != null))
					{
						foreach (BarItem barItem in (control1 as BarDockControl).Manager.Items)
						{
							UtilLanguage.CeviriYap(barItem, barItem.Caption);
						}
						flag = true;
					}
				}
				else if (control1 is RibbonControl)
				{
					foreach (BarItem item in (control1 as RibbonControl).Manager.Items)
					{
						UtilLanguage.CeviriYap(item, item.Caption);
					}
				}
				else if (control1 is AutoHideContainer)
				{
					foreach (Control control2 in (control1 as AutoHideContainer).Controls)
					{
						if (control2 is DockPanel)
						{
							UtilLanguage.CeviriYap(control2 as DockPanel, (control2 as DockPanel).Text);
							foreach (Control control3 in control2.Controls)
							{
								if (!(control3 is LayoutControl))
								{
									UtilLanguage.FormControlCeviriYap(control3);
								}
								else
								{
									UtilLanguage.LayoutControlItemCeviriYap(control3 as LayoutControl);
								}
							}
						}
					}
				}
				else if (control1 is DockPanel)
				{
					UtilLanguage.CeviriYap(control1 as DockPanel, (control1 as DockPanel).Text);
					foreach (Control control4 in control1.Controls)
					{
						if (!(control4 is LayoutControl))
						{
							UtilLanguage.FormControlCeviriYap(control4);
						}
						else
						{
							UtilLanguage.LayoutControlItemCeviriYap(control4 as LayoutControl);
						}
					}
				}
				else if (control1 is PanelControl)
				{
					foreach (Control control in control1.Controls)
					{
						if (!(control is LayoutControl))
						{
							UtilLanguage.FormControlCeviriYap(control);
						}
						else
						{
							UtilLanguage.LayoutControlItemCeviriYap(control as LayoutControl);
						}
					}
				}
			}
		}

		private static void FormControlCeviriYap(Control pcontrol)
		{
			GridColumn column = null;
			if (!(!(pcontrol is GridControl) || !(pcontrol as GridControl).Views.IsDolu() ? true : (pcontrol as GridControl).Views.Count <= 0))
			{
				for (int i = 0; i < (pcontrol as GridControl).Views.Count; i++)
				{
					if ((pcontrol as GridControl).Views[i] is GridView)
					{
						UtilLanguage.CeviriYap((pcontrol as GridControl).Views[i] as GridView, ((pcontrol as GridControl).Views[i] as GridView).GroupPanelText);
						foreach (GridColumn gridColumn in ((pcontrol as GridControl).Views[i] as GridView).Columns)
						{
							UtilLanguage.CeviriYap(gridColumn, gridColumn.Caption.IsBosIse(gridColumn.ToString()));
						}
					}
					if ((pcontrol as GridControl).Views[i] is BandedGridView)
					{
						foreach (GridBand band in ((pcontrol as GridControl).Views[i] as BandedGridView).Bands)
						{
							UtilLanguage.CeviriYap(band, band.Caption);
							if (band.Children != null)
							{
								foreach (GridBand child in band.Children)
								{
									UtilLanguage.CeviriYap(child, child.Caption);
									if (child.Children != null)
									{
										foreach (GridBand gridBand in child.Children)
										{
											UtilLanguage.CeviriYap(gridBand, gridBand.Caption);
										}
									}
								}
							}
						}
					}
					else if ((pcontrol as GridControl).Views[i] is LayoutView)
					{
						foreach (LayoutViewColumn layoutViewColumn in ((pcontrol as GridControl).Views[i] as LayoutView).Columns)
						{
							UtilLanguage.CeviriYap(layoutViewColumn, layoutViewColumn.Caption);
						}
					}
				}
				foreach (RepositoryItem repositoryItem in (pcontrol as GridControl).RepositoryItems)
				{
					if (repositoryItem is RepositoryItemRadioGroup)
					{
						foreach (RadioGroupItem item in (repositoryItem as RepositoryItemRadioGroup).Items)
						{
							UtilLanguage.CeviriYap(item, item.Description);
						}
					}
				}
			}
			else if (pcontrol is PivotGridControl)
			{
				foreach (PivotGridField field in (pcontrol as PivotGridControl).Fields)
				{
					UtilLanguage.CeviriYap(field, field.Caption);
				}
			}
			else if (pcontrol is ChartControl)
			{
				if ((pcontrol as ChartControl).Titles != null)
				{
					foreach (ChartTitle title in (pcontrol as ChartControl).Titles)
					{
						UtilLanguage.CeviriYap(title, title.Text);
					}
				}
			}
			else if (!(!(pcontrol is SearchLookUpEdit) ? true : (pcontrol as SearchLookUpEdit).Properties.View == null))
			{
				foreach (GridColumn column1 in (pcontrol as SearchLookUpEdit).Properties.View.Columns)
				{
					UtilLanguage.CeviriYap(column1, column1.Caption);
				}
			}
			else if (!(!(pcontrol is GridLookUpEdit) ? true : (pcontrol as GridLookUpEdit).Properties.View == null))
			{
				foreach (GridColumn column in (pcontrol as GridLookUpEdit).Properties.View.Columns)
				{
					UtilLanguage.CeviriYap(column, column.Caption);
				}
			}
			else if (!(!(pcontrol is LookUpEdit) ? true : (pcontrol as LookUpEdit).Properties.Columns == null))
			{
				foreach (LookUpColumnInfo lookUpColumnInfo in (pcontrol as LookUpEdit).Properties.Columns)
				{
					UtilLanguage.CeviriYap(lookUpColumnInfo, lookUpColumnInfo.Caption);
				}
			}
			else if (pcontrol is TreeList)
			{
				foreach (TreeListColumn treeListColumn in (pcontrol as TreeList).Columns)
				{
					UtilLanguage.CeviriYap(treeListColumn, treeListColumn.Caption);
				}
			}
			else if (pcontrol is RadioGroup)
			{
				foreach (RadioGroupItem radioGroupItem in (pcontrol as RadioGroup).Properties.Items)
				{
					UtilLanguage.CeviriYap(radioGroupItem, radioGroupItem.Description);
				}
			}
			else if (pcontrol is GroupControl)
			{
				UtilLanguage.CeviriYap(pcontrol as GroupControl, (pcontrol as GroupControl).Text);
			}
			else if (pcontrol is Label)
			{
				UtilLanguage.CeviriYap(pcontrol as Label, (pcontrol as Label).Text);
			}
			else if (pcontrol is LabelControl)
			{
				UtilLanguage.CeviriYap(pcontrol as LabelControl, (pcontrol as LabelControl).Text);
			}
			else if (pcontrol is Button)
			{
				UtilLanguage.CeviriYap(pcontrol as Button, (pcontrol as Button).Text);
			}
			else if (pcontrol is SimpleButton)
			{
				UtilLanguage.CeviriYap(pcontrol as SimpleButton, (pcontrol as SimpleButton).Text);
			}
			else if (pcontrol is CheckEdit)
			{
				UtilLanguage.CeviriYap(pcontrol as CheckEdit, (pcontrol as CheckEdit).Text);
			}
			else if (!(!(pcontrol is NavBarGroupControlContainer) ? true : !(pcontrol as NavBarGroupControlContainer).OwnerGroup.IsDolu()))
			{
				UtilLanguage.CeviriYap(pcontrol as NavBarGroupControlContainer, (pcontrol as NavBarGroupControlContainer).OwnerGroup.Caption);
			}
			else if (pcontrol is LayoutControl)
			{
				UtilLanguage.LayoutControlItemCeviriYap(pcontrol as LayoutControl);
			}
			else if (pcontrol is UserControl)
			{
				UtilLanguage.UserControlCeviriYap(pcontrol as UserControl);
			}
			else if (pcontrol is ContextMenuStrip)
			{
				foreach (ToolStripMenuItem toolStripMenuItem in (pcontrol as ContextMenuStrip).Items)
				{
					UtilLanguage.CeviriYap(toolStripMenuItem, toolStripMenuItem.Text);
				}
			}
			else if (pcontrol is CheckedListBoxControl)
			{
				foreach (CheckedListBoxItem checkedListBoxItem in (pcontrol as CheckedListBoxControl).Items)
				{
					UtilLanguage.CeviriYap(checkedListBoxItem, checkedListBoxItem.Description);
				}
			}
			else if (pcontrol is ImageComboBoxEdit)
			{
				foreach (ImageComboBoxItem imageComboBoxItem in (pcontrol as ImageComboBoxEdit).Properties.Items)
				{
					UtilLanguage.CeviriYap(imageComboBoxItem, imageComboBoxItem.Description);
				}
			}
			if (((!pcontrol.Controls.IsDolu() || !(pcontrol is NavBarControl)) && !(pcontrol is NavBarGroupControlContainer) && !(pcontrol is NavBarGroupControlContainerWrapper) && !(pcontrol is Panel) && !(pcontrol is PanelControl) ? pcontrol is GroupControl : true))
			{
				foreach (Control control in pcontrol.Controls)
				{
					UtilLanguage.FormControlCeviriYap(control);
				}
			}
		}

		public static string GetCeviriStr(string pStr, bool pInsertReferans)
		{
			return pStr.CeviriYap();
		}

		private static void LayoutControlItemCeviriYap(LayoutControl pLayoutControl)
		{
			foreach (BaseLayoutItem item in pLayoutControl.Items)
			{
				if (item is LayoutControlItem)
				{
					UtilLanguage.CeviriYap(item as LayoutControlItem, (item as LayoutControlItem).Text);
					if ((item as LayoutControlItem).Control != null)
					{
						UtilLanguage.FormControlCeviriYap((item as LayoutControlItem).Control);
					}
				}
				else if (item is LayoutControlGroup)
				{
					UtilLanguage.CeviriYap(item as LayoutControlGroup, (item as LayoutControlGroup).Text);
				}
			}
		}

		public static void ManuelCeviriYap(Control pControl)
		{
			if (Editor.Core.EditorApplication.EditorApplication.LanguageId != 1)
			{
				UtilLanguage.FormControlCeviriYap(pControl);
			}
		}

		public static void UserControlCeviriYap(UserControl pUserControl)
		{
			bool flag = false;
			foreach (Control control in pUserControl.Controls)
			{
				if (control is LayoutControl)
				{
					UtilLanguage.LayoutControlItemCeviriYap(control as LayoutControl);
				}
				else if ((flag ? false : control is BarDockControl))
				{
					foreach (BarItem item in (control as BarDockControl).Manager.Items)
					{
						UtilLanguage.CeviriYap(item, item.Caption);
					}
					flag = true;
				}
			}
		}
        */
    }
}