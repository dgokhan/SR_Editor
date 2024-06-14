using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections;

namespace SR_Editor.Core
{
	public static class UtilGrid
	{
		public static void SetGridSummaryItemByCount(GridView gridView)
		{
			if (gridView.Columns.Count > 0)
			{
				int num = 0;
				while (num < gridView.Columns.Count)
				{
					if (!gridView.Columns[num].Visible)
					{
						num++;
					}
					else
					{
						UtilGrid.SetGridSummaryItemByCount(gridView, num);
						return;
					}
				}
			}
		}

		public static void SetGridSummaryItemByCount(GridView gridView, GridColumn gridColumn)
		{
			if (gridColumn.Visible)
			{
				UtilGrid.SetGridSummaryItemByCount(gridView, gridColumn.AbsoluteIndex);
			}
		}

		public static void SetGridSummaryItemByCount(GridView gridView, string ColumName)
		{
			if ((gridView.Columns.ColumnByName(ColumName) == null ? false : gridView.Columns.ColumnByName(ColumName).Visible))
			{
				UtilGrid.SetGridSummaryItemByCount(gridView, gridView.Columns.ColumnByName(ColumName).AbsoluteIndex);
			}
		}

		public static void SetGridSummaryItemByCount(GridView gridView, int columnIndex)
		{
			if (!gridView.OptionsView.ShowFooter)
			{
				gridView.OptionsView.ShowFooter = true;
			}
			if (gridView.Columns.Count > columnIndex)
			{
				gridView.Columns[columnIndex].SummaryItem.FieldName = gridView.Columns[columnIndex].FieldName;
				gridView.Columns[columnIndex].SummaryItem.DisplayFormat = "Toplam {0} kayıt.";
				gridView.Columns[columnIndex].SummaryItem.SummaryType = SummaryItemType.Count;
			}
		}

		public static void ValidateActiveControl(GridControl pGridControl)
		{
			pGridControl.FocusedView.PostEditor();
			pGridControl.FocusedView.UpdateCurrentRow();
		}
	}
}