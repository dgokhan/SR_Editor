using SR_Editor.LookUp;
using System;
using System.Deployment.Application;
using System.Windows.Forms;

namespace SR_Editor.Core
{
	public class UtilVersion
	{
		private static string productName;

		private static string version;

		private static bool isClickOnceDeployment;

		private static System.Version activeVersion;

		public static System.Version ActiveVersion
		{
			get
			{
				if (UtilVersion.activeVersion == null)
				{
					try
					{
						UtilVersion.activeVersion = ApplicationDeployment.CurrentDeployment.CurrentVersion;
						UtilVersion.isClickOnceDeployment = true;
					}
					catch
					{
						UtilVersion.activeVersion = new System.Version(Application.ProductVersion);
						UtilVersion.isClickOnceDeployment = false;
					}
				}
				return UtilVersion.activeVersion;
			}
		}

		public static string ProductName
		{
			get
			{
				if (UtilVersion.productName == "")
				{
					UtilVersion.productName = "SR_Editor";
				}
				return UtilVersion.productName;
			}
		}

		public static string Version
		{
			get
			{
				if (UtilVersion.version == "")
				{
					UtilVersion.version = string.Concat(UtilVersion.ProductName, " ", UtilVersion.ActiveVersion.ToString());
				}
				return UtilVersion.version;
			}
			set
			{
				UtilVersion.version = value;
			}
		}

		static UtilVersion()
		{
			UtilVersion.productName = "";
			UtilVersion.version = "";
			UtilVersion.isClickOnceDeployment = true;
		}

		public UtilVersion()
		{
		}
	}
}