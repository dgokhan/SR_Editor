using DevExpress.XtraBars;
using System;
using System.Collections;

namespace SR_Editor.Core
{
	public class UtilBarManager
	{
		public UtilBarManager()
		{
		}

		public static void InitBarManager(BarManager pBarManager)
		{
			foreach (Bar bar in pBarManager.Bars)
			{
				bar.OptionsBar.UseWholeRow = true;
			}
		}
	}
}