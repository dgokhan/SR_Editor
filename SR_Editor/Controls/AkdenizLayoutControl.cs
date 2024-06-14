using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using System;
using System.IO;

namespace SR_Editor.Core.Controls
{
	public class EditorLayoutControl : LayoutControl
	{
		public EditorLayoutControl()
		{
			base.ItemAdded += new EventHandler(this.EditorLayoutControl_ItemAdded);
		}

		public override BaseLayoutItem CreateLayoutItem(LayoutGroup parent)
		{
			BaseLayoutItem padding = base.CreateLayoutItem(parent);
			padding.Padding = new DevExpress.XtraLayout.Utils.Padding(0);
			return padding;
		}

		public byte[] GetByteArrayFromDesign()
		{
			MemoryStream memoryStream = new MemoryStream();
			this.SaveLayoutToStream(memoryStream);
			memoryStream.Position = (long)0;
			return memoryStream.ToArray();
		}

		private void EditorLayoutControl_ItemAdded(object sender, EventArgs e)
		{
			if (sender is LayoutControlGroup)
			{
				(sender as LayoutControlGroup).Padding = new DevExpress.XtraLayout.Utils.Padding(0);
			}
			else if (sender is LayoutControlItem)
			{
				(sender as LayoutControlItem).TextAlignMode = TextAlignModeItem.AutoSize;
			}
		}

		public void RestoreLayoutFromByteArray(byte[] pByteArray)
		{
			MemoryStream memoryStream = new MemoryStream(pByteArray)
			{
				Position = (long)0
			};
			this.RestoreLayoutFromStream(memoryStream);
		}
	}
}