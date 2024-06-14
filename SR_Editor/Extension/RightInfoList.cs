using System;
using System.Collections.ObjectModel;

namespace SR_Editor.Core
{
	public class RightInfoList : KeyedCollection<string, RightInfo>
	{
		public RightInfoList()
		{
		}

		protected override string GetKeyForItem(RightInfo item)
		{
			return item.Key;
		}
	}
}