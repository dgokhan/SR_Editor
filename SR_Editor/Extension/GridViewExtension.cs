using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraScheduler;

using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public static class GridViewExtension
	{
		public static int FindRowHandleByRowObject(this GridView view, object row)
		{
			int num;
			if (row != null)
			{
				int num1 = 0;
				while (num1 < view.DataRowCount)
				{
					if (!row.Equals(view.GetRow(num1)))
					{
						num1++;
					}
					else
					{
						num = num1;
						return num;
					}
				}
			}
			num = -2147483648;
			return num;
		}

		public static T GetEntity<T>(this GridView gridView)
		{
			T t;
			if (gridView.IsDataRow(gridView.FocusedRowHandle))
			{
				object row = gridView.GetRow(gridView.FocusedRowHandle);
				if ((row == null ? false : row is T))
				{
					t = (T)row;
					return t;
				}
			}
			t = default(T);
			return t;
		}

		public static void MouseDown(GridView view, MouseEventArgs e, GridHitInfo gridHitInfo)
		{
			gridHitInfo = null;
			GridHitInfo gridHitInfo1 = view.CalcHitInfo(new Point(e.X, e.Y));
			if (gridHitInfo1.RowHandle >= 0 && e.Button == MouseButtons.Left)
			{
				gridHitInfo = gridHitInfo1;
			}
		}

		public static void MouseMove(GridView view, MouseEventArgs e, GridHitInfo gridHitInfo, object listSuruklenen)
		{
			if ((e.Button != MouseButtons.Left ? false : gridHitInfo != null))
			{
				Size dragSize = SystemInformation.DragSize;
				Point hitPoint = gridHitInfo.HitPoint;
				int x = hitPoint.X - dragSize.Width / 2;
				hitPoint = gridHitInfo.HitPoint;
				Rectangle rectangle = new Rectangle(new Point(x, hitPoint.Y - dragSize.Height / 2), dragSize);
				if (!rectangle.Contains(new Point(e.X, e.Y)))
				{
					view.GridControl.DoDragDrop(listSuruklenen, DragDropEffects.Move);
					DXMouseEventArgs.GetMouseArgs(e).Handled = true;
				}
			}
		}
        /*
		public static WeekDays ToWeekDays(this DayOfWeek dayOfWeek)
		{
			WeekDays weekDay;
			switch (dayOfWeek)
			{
				case DayOfWeek.Sunday:
				{
					weekDay = WeekDays.Sunday;
					break;
				}
				case DayOfWeek.Monday:
				{
					weekDay = WeekDays.Monday;
					break;
				}
				case DayOfWeek.Tuesday:
				{
					weekDay = WeekDays.Tuesday;
					break;
				}
				case DayOfWeek.Wednesday:
				{
					weekDay = WeekDays.Wednesday;
					break;
				}
				case DayOfWeek.Thursday:
				{
					weekDay = WeekDays.Thursday;
					break;
				}
				case DayOfWeek.Friday:
				{
					weekDay = WeekDays.Friday;
					break;
				}
				case DayOfWeek.Saturday:
				{
					weekDay = WeekDays.Saturday;
					break;
				}
				default:
				{
					weekDay = WeekDays.Monday;
					break;
				}
			}
			return weekDay;
		}*/
	}
}