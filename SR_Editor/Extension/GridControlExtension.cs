using System;
using System.Data;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public static class GridControlExtension
	{
		public static void DragOver(DragEventArgs e)
		{
			if (!e.Data.GetDataPresent(typeof(DataRow)))
			{
				e.Effect = DragDropEffects.None;
			}
			else
			{
				e.Effect = DragDropEffects.Move;
			}
		}
	}
}