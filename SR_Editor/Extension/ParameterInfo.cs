using System;
using System.Collections.Generic;

namespace SR_Editor.Core
{
	public class ParameterInfo : ParameterInfoGroup
	{
		private string description = "";

		private object defaultValue = null;

		private EnumDegerTipi degerTipi;

		private List<ModuleInfo> listModule;

		private ParameterValue parameterValue;

		public object DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
			set
			{
				this.defaultValue = value;
			}
		}

		public EnumDegerTipi DegerTipi
		{
			get
			{
				return this.degerTipi;
			}
			set
			{
				this.degerTipi = value;
			}
		}

		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}

		public List<ModuleInfo> ListModule
		{
			get
			{
				if (this.listModule == null)
				{
					this.listModule = new List<ModuleInfo>();
				}
				return this.listModule;
			}
			set
			{
				this.listModule = value;
				if ((this.listModule == null ? false : this.listModule.Count > 0))
				{
					this.SetModuleInfo();
				}
			}
		}

		public ParameterValue ParameterValue
		{
			get
			{
				bool flag;
				if (this.parameterValue == null)
				{
					flag = false;
				}
				else
				{
					flag = (this.parameterValue == null ? true : this.parameterValue.IsStaticValue);
				}
				if (!flag)
				{
					this.parameterValue = this.GetValue();
				}
				return this.parameterValue;
			}
			set
			{
				this.parameterValue = value;
			}
		}

		public ParameterInfo()
		{
			base.IsGroup = false;
		}

		public virtual ParameterValue GetValue()
		{
			return null;
		}

		protected virtual void SetModuleInfo()
		{
			foreach (ModuleInfo moduleInfo in this.listModule)
			{
				moduleInfo.AddParameter(this);
			}
		}
	}
}