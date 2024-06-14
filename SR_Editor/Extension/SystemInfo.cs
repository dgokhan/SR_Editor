using SR_Editor.Core.Utility;
using System;

namespace SR_Editor.Core
{
	public class SystemInfo
	{
		private string key = "";

		private string caption = "";

		private string lisansKey = "";

		private bool isGroup = true;

		private SystemInfo parentItem;

		public string Caption
		{
			get
			{
				return this.caption;
			}
			set
			{
				this.caption = value;
			}
		}

		public string CeviriCaption
		{
			get
			{
                return this.caption;//UtilLanguage.GetCeviriStr(this.caption, false);
			}
		}

		public virtual string FullKey
		{
			get
			{
				string str;
				str = (this.ParentKey == null ? this.Key : string.Concat(this.ParentKey, ".", this.Key));
				return str;
			}
		}

		public bool IsGroup
		{
			get
			{
				return this.isGroup;
			}
			set
			{
				this.isGroup = value;
			}
		}

		public string Key
		{
			get
			{
				return this.key;
			}
			set
			{
				this.key = value;
			}
		}

		public string LisansKey
		{
			get
			{
				string str;
				str = ((this.lisansKey != "" ? true : this.ParentItem == null) ? this.lisansKey : ((ModuleInfoGroup)this.ParentItem).LisansKey);
				return str;
			}
			set
			{
				this.lisansKey = value;
			}
		}

		public SystemInfo ParentItem
		{
			get
			{
				return this.parentItem;
			}
			set
			{
				this.parentItem = value;
			}
		}

		public string ParentKey
		{
			get
			{
				string fullKey;
				if (this.parentItem == null)
				{
					fullKey = null;
				}
				else
				{
					fullKey = this.parentItem.FullKey;
				}
				return fullKey;
			}
		}

		public SystemInfo()
		{
		}
	}
}