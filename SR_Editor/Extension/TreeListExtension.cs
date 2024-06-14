using DevExpress.XtraTreeList;
using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public static class TreeListExtension
	{
		public static void DragOver(object sender, DragEventArgs e, TreeList treeList, Point p)
		{
			treeList = sender as TreeList;
			p = treeList.PointToClient(new Point(e.X, e.Y));
		}

		public static T GetEntity<T>(this TreeList treeList)
		{
			T t;
			if (treeList.FocusedNode != null)
			{
				object dataRecordByNode = treeList.GetDataRecordByNode(treeList.FocusedNode);
				if ((dataRecordByNode == null ? false : dataRecordByNode is T))
				{
					t = (T)dataRecordByNode;
					return t;
				}
			}
			t = default(T);
			return t;
		}
	}
}