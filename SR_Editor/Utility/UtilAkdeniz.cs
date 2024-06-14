using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace SR_Editor.Core.Utility
{
	public static class UtilEditor
	{
		public static string GetApplicationFolder()
		{
			string location = Assembly.GetExecutingAssembly().Location;
			location = location.Replace("\\Editor.exe", "");
			location = location.Replace("\\bin\\Release", "");
			location = location.Replace("\\bin\\Debug", "");
			location = location.Replace(string.Concat("\\", Assembly.GetExecutingAssembly().ManifestModule.Name), "");
			return location;
		}

		public static string GetLocalIp()
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
			string str = "";
			IPAddress[] pAddressArray = hostAddresses;
			for (int i = 0; i < (int)pAddressArray.Length; i++)
			{
				IPAddress pAddress = pAddressArray[i];
				if (pAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					str = pAddress.ToString();
				}
			}
			return str;
		}
	}
}