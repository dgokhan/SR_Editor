using Microsoft.JScript;
using Microsoft.JScript.Vsa;
using System;

namespace SR_Editor.Core.Utility
{
	public static class UtilString
	{
		private static VsaEngine vsaEngine;

		static UtilString()
		{
			UtilString.vsaEngine = null;
		}

		public static string ClearMemoEditData(string str)
		{
			str = str.Replace("\r", "");
			str = str.Replace("\n", "");
			str = str.Replace("\t", "");
			return str;
		}

		public static decimal ConvertToDecimal(string str)
		{
			if (UtilString.vsaEngine == null)
			{
				UtilString.vsaEngine = VsaEngine.CreateEngine();
			}
			str = str.Replace(",", ".");
			decimal num = decimal.Parse(Eval.JScriptEvaluate(str, UtilString.vsaEngine).ToString());
			return num;
		}
	}
}