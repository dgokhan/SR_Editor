using DevExpress.XtraEditors;
using SR_Editor.Forms;
//using SR_Editor.Core.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public class UtilConfig
	{
		private static List<ModuleInfoGroup> listModuleInfo;

		public static bool IsLisansAktif;

		//private static List<ModuleLisans> listModuleLisans;

		//private static List<ModulMesaj> listModulMesaj;

		private static List<ParameterInfoGroup> listRightInfo;

		private static List<ParameterInfoGroup> listParameterInfo;

		private static XtraForm mainForm;

		private static XtraForm aktifForm;

		private static ModuleInfo aktifModuleInfo;

		private static List<FormBase> listPasifForm;

		public static XtraForm AktifForm
		{
			get
			{
				return UtilConfig.aktifForm;
			}
			set
			{
				UtilConfig.aktifForm = value;
			}
		}

		public static ModuleInfo AktifModuleInfo
		{
			get
			{
				return UtilConfig.aktifModuleInfo;
			}
			set
			{
				UtilConfig.aktifModuleInfo = value;
			}
		}

		public static List<ModuleInfoGroup> ListModuleInfo
		{
			get
			{
				if (UtilConfig.listModuleInfo == null)
				{
					UtilConfig.listModuleInfo = new List<ModuleInfoGroup>();
				}
				return UtilConfig.listModuleInfo;
			}
			set
			{
				UtilConfig.listModuleInfo = value;
			}
		}

		/*public static List<ModuleLisans> ListModuleLisans
		{
			get
			{
				return UtilConfig.listModuleLisans;
			}
			set
			{
				UtilConfig.listModuleLisans = value;
			}
		}

		public static List<ModulMesaj> ListModulMesaj
		{
			get
			{
				return UtilConfig.listModulMesaj;
			}
			set
			{
				UtilConfig.listModulMesaj = value;
			}
		}*/

		public static List<ParameterInfoGroup> ListParameterInfo
		{
			get
			{
				if (UtilConfig.listParameterInfo == null)
				{
					UtilConfig.listParameterInfo = new List<ParameterInfoGroup>();
				}
				return UtilConfig.listParameterInfo;
			}
			set
			{
				UtilConfig.listParameterInfo = value;
			}
		}

		public static List<FormBase> ListPasifForm
		{
			get
			{
				if (UtilConfig.listPasifForm == null)
				{
					UtilConfig.listPasifForm = new List<FormBase>();
				}
				return UtilConfig.listPasifForm;
			}
			set
			{
				UtilConfig.listPasifForm = value;
			}
		}

		public static List<ParameterInfoGroup> ListRightInfo
		{
			get
			{
				if (UtilConfig.listRightInfo == null)
				{
					UtilConfig.listRightInfo = new List<ParameterInfoGroup>();
				}
				return UtilConfig.listRightInfo;
			}
			set
			{
				UtilConfig.listRightInfo = value;
			}
		}

		public static XtraForm MainForm
		{
			get
			{
				return UtilConfig.mainForm;
			}
			set
			{
				UtilConfig.mainForm = value;
			}
		}

		public static string OLTPConnectionString
		{
			get
			{
                return "";//UtilConnection.SqlConnectionString;
			}
		}

		static UtilConfig()
		{
			UtilConfig.IsLisansAktif = false;
		}

		public UtilConfig()
		{
		}

		public static FormBase GetActiveInstance(string pFormName)
		{
			FormBase formBase = (
				from t in UtilConfig.MainForm.MdiChildren
				where t.Name == pFormName
				select t).FirstOrDefault<System.Windows.Forms.Form>() as FormBase;
			return formBase;
		}

		public static int GetActiveInstanceCount(string pFormName)
		{
			int num = UtilConfig.MainForm.MdiChildren.Count<System.Windows.Forms.Form>((System.Windows.Forms.Form t) => t.Name == pFormName);
			return num;
		}

		public static FormBase GetPasifInstance(string pModuleKey)
		{
			FormBase formBase = UtilConfig.ListPasifForm.Find((FormBase t) => t.Module.FullKey == pModuleKey);
			return formBase;
		}

		public static int GetPasifInstanceCount(string pModuleKey)
		{
			int num = UtilConfig.ListPasifForm.Count<FormBase>((FormBase t) => t.Module.Key == pModuleKey);
			return num;
		}
	}
}