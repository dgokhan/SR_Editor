using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace SR_Editor.Core
{
	public static class CheckedListBoxControlExtension
	{
		public static string ToQueryString(this CheckedListBoxControl pCheckedList)
		{
			string str = "";
			if (pCheckedList.CheckedItemsCount > 0)
			{
				foreach (CheckedListBoxItem checkedItem in (IEnumerable)pCheckedList.CheckedItems)
				{
					if (checkedItem.Value.IsNotNull())
					{
						if (checkedItem.Value is string)
						{
							str = string.Concat(str, "'");
						}
						str = string.Concat(str, checkedItem.Value.ToString());
						if (checkedItem.Value is string)
						{
							str = string.Concat(str, "'");
						}
						str = string.Concat(str, ",");
					}
				}
				str = str.TrimEnd(new char[] { ',' });
			}
			return str;
		}
	}
}