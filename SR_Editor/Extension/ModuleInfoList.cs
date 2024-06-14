using System;
using System.Collections.ObjectModel;

namespace SR_Editor.Core
{
	public class ModuleInfoList : KeyedCollection<string, ModuleInfo>
	{
		public ModuleInfoList()
		{
		}

		protected override string GetKeyForItem(ModuleInfo item)
		{
			return item.FullKey;
		}
	}
}