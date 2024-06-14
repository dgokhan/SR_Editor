using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public class UtilParameters
	{
		public System.Windows.Forms.DialogResult DialogResult = System.Windows.Forms.DialogResult.None;

		private ParameterList paramList;

		public int Count
		{
			get
			{
				return this.ParamList.Count;
			}
		}

		public object this[string key]
		{
			get
			{
				object obj = null;
				if (this.ParamList.ContainsKey(key))
				{
					this.ParamList.TryGetValue(key, out obj);
				}
				return obj;
			}
			set
			{
				if (!this.ParamList.ContainsKey(key))
				{
					this.ParamList.Add(key, value);
				}
				else
				{
					this.ParamList[key] = value;
				}
			}
		}

		public ParameterList ParamList
		{
			get
			{
				if (this.paramList == null)
				{
					this.paramList = new ParameterList();
				}
				return this.paramList;
			}
			set
			{
				this.paramList = value;
			}
		}

		public UtilParameters()
		{
		}

		public void Add(string key, object value)
		{
			if (!this.ParamList.ContainsKey(key))
			{
				this.ParamList.Add(key, value);
			}
		}

		public void Clear()
		{
			this.ParamList.Clear();
		}

		public bool Contains(string key)
		{
			return this.ParamList.ContainsKey(key);
		}

		public void Remove(string key)
		{
			this.ParamList.Remove(key);
		}
	}
}