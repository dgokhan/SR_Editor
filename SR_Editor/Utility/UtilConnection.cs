using SR_Editor.Core.EF;
using SR_Editor.Core.EditorApplication;
using System;
using System.Data.Common;
using System.Data.EntityClient;

namespace SR_Editor.Core
{
	public class UtilConnection
	{
		public static string SqlConnectionString;

		public static string WindowsConnectionString;

		static UtilConnection()
		{
			UtilConnection.SqlConnectionString = "Data Source=Server;Initial Catalog=DB;User ID=USR;Password=PASS;Max Pool Size=500";
			UtilConnection.WindowsConnectionString = "Data Source=Server;Initial Catalog=DB;Persist Security Info=True;Integrated Security=True;MultipleActiveResultSets=True";
		}

		public UtilConnection()
		{
		}

		public static string GetEntityConnectionString(string pModelName, Hastane hastane)
		{
			string sqlConnectionString;
			if (hastane == null)
			{
				if (Editor.Core.EditorApplication.EditorApplication.SqlConnectionString.IsNull())
				{
					Editor.Core.EditorApplication.EditorApplication.SetSqlConnectionString();
				}
				sqlConnectionString = Editor.Core.EditorApplication.EditorApplication.SqlConnectionString;
			}
			else
			{
				sqlConnectionString = UtilConnection.SqlConnectionString.Replace("Server", hastane.ServerIP).Replace("DB", hastane.DatabaseName).Replace("USR", hastane.UserName).Replace("PASS", hastane.Password);
			}
			string str = "res://*/MODEL.csdl|res://*/MODEL.ssdl|res://*/MODEL.msl".Replace("MODEL", pModelName);
			EntityConnectionStringBuilder entityConnectionStringBuilder = new EntityConnectionStringBuilder()
			{
				Metadata = str,
				Provider = "System.Data.SqlClient",
				ProviderConnectionString = sqlConnectionString
			};
			return entityConnectionStringBuilder.ConnectionString;
		}

		public static string GetEntityConnectionStringWeb(string pModelName, Hastane hastane)
		{
			string sqlConnectionString;
			if (hastane == null)
			{
				if (Editor.Core.EditorApplication.EditorApplication.SqlConnectionString.IsNull())
				{
					Editor.Core.EditorApplication.EditorApplication.SetSqlConnectionString();
				}
				sqlConnectionString = Editor.Core.EditorApplication.EditorApplication.SqlConnectionString;
			}
			else
			{
				sqlConnectionString = UtilConnection.SqlConnectionString.Replace("Server", hastane.ServerIP).Replace("DB", hastane.DatabaseName).Replace("USR", hastane.EkDecryptedDBUserName).Replace("PASS", hastane.EkDecryptedDBPassword);
			}
			string str = "res://*/MODEL.csdl|res://*/MODEL.ssdl|res://*/MODEL.msl".Replace("MODEL", pModelName);
			EntityConnectionStringBuilder entityConnectionStringBuilder = new EntityConnectionStringBuilder()
			{
				Metadata = str,
				Provider = "System.Data.SqlClient",
				ProviderConnectionString = sqlConnectionString
			};
			return entityConnectionStringBuilder.ConnectionString;
		}
	}
}