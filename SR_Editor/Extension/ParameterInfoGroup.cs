using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SR_Editor.Core
{
	public class ParameterInfoGroup
	{
		private string key = "";

		private string caption = "";

		private bool isGroup = true;

		private int order = 99;

		private ParameterInfoGroup parentItem;

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

		public string FullKey
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

		public object this[string Key]
		{
			get
			{
				ParameterInfo parameterInfo = null;
				parameterInfo = (!(this is RightInfoGroup) ? (ParameterInfo)UtilConfig.ListParameterInfo.Find((ParameterInfoGroup t) => t.FullKey == string.Concat(this.FullKey, ".", Key)) : (ParameterInfo)UtilConfig.ListRightInfo.Find((ParameterInfoGroup t) => t.FullKey == string.Concat(this.FullKey, ".", Key)));
				return ((parameterInfo == null ? true : parameterInfo.ParameterValue == null) ? null : parameterInfo.ParameterValue.Value);
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

		public int Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}

		public ParameterInfoGroup ParentItem
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

		public ParameterInfoGroup()
		{
			this.IsGroup = true;
		}

		public ParameterInfoGroup Add(ParameterInfoGroup item)
		{
			item.ParentItem = this;
			if ((item is RightInfoGroup ? false : !(item is RightInfo)))
			{
				UtilConfig.ListParameterInfo.Add(item);
			}
			else
			{
				UtilConfig.ListRightInfo.Add(item);
			}
			return item;
		}
	}
}