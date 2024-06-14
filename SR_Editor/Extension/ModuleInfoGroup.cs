using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SR_Editor.Core
{
	public class ModuleInfoGroup : SystemInfo
	{
		private string imageName = "";

		private string assemblyName = "";

		private string nameSpace = "";

		private bool isMenuVisible = true;

		private bool isCommonModule = false;

		public string AssemblyName
		{
			get
			{
				string str;
				str = ((this.assemblyName != "" ? true : base.ParentItem == null) ? this.assemblyName : ((ModuleInfoGroup)base.ParentItem).AssemblyName);
				return str;
			}
			set
			{
				this.assemblyName = value;
			}
		}

		public override string FullKey
		{
			get
			{
				string str;
				str = (base.IsGroup ? this.NameSpace : string.Concat(((ModuleInfoGroup)base.ParentItem).NameSpace, ".", base.Key));
				return str;
			}
		}

		public string ImageName
		{
			get
			{
				return this.imageName;
			}
			set
			{
				this.imageName = value;
			}
		}

		public bool IsCommonModule
		{
			get
			{
				return this.isCommonModule;
			}
			set
			{
				this.isCommonModule = value;
			}
		}

		public bool IsMenuVisible
		{
			get
			{
				return this.isMenuVisible;
			}
			set
			{
				this.isMenuVisible = value;
			}
		}

		public ModuleInfo this[string Key]
		{
			get
			{
				ModuleInfo moduleInfo = (ModuleInfo)UtilConfig.ListModuleInfo.Find((ModuleInfoGroup t) => t.FullKey == string.Concat(this.FullKey, ".", Key));
				return moduleInfo;
			}
		}

		public string NameSpace
		{
			get
			{
				string str;
				str = ((this.nameSpace != "" ? true : base.ParentItem == null) ? this.nameSpace : ((ModuleInfoGroup)base.ParentItem).NameSpace);
				return str;
			}
			set
			{
				this.nameSpace = value;
			}
		}

		public ModuleInfoGroup()
		{
			base.IsGroup = true;
		}

		public ModuleInfoGroup Add(ModuleInfoGroup item)
		{
			item.ParentItem = this;
			UtilConfig.ListModuleInfo.Add(item);
			return item;
		}

		public virtual void InitParameter()
		{
		}

		public virtual void InitRight()
		{
		}
	}
}