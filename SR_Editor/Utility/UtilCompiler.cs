//using SR_Editor.Core.Exceptions;
using System;
using System.Collections;

namespace SR_Editor.Core
{
	public class UtilCompiler
	{
		private const string PRE_STR = "using System;\r\n\t\t\tusing System.Runtime.InteropServices;\r\n\t\t\t\r\n            namespace SR_Editor.Core\r\n\t\t\t{";

		private const string END_STR = "}";

		private UtilCompilerInternal EditorCompilerInternal;

		private static UtilCompiler instance;

		public static UtilCompiler Instance
		{
			get
			{
				if (UtilCompiler.instance == null)
				{
					UtilCompiler.instance = new UtilCompiler();
				}
				return UtilCompiler.instance;
			}
		}

		public UtilCompiler()
		{
			this.EditorCompilerInternal = new UtilCompilerInternal();
			this.EditorCompilerInternal.AssemblyList.Add("System.dll");
		}

		public void ComplileCode(string code, int assemblyVersion)
		{
			string[] str = new string[] { "public class CompileHelper", assemblyVersion.ToString(), " {", code, "}" };
			code = string.Concat(str);
			this.EditorCompilerInternal.ComplileCode(this.GetCodeStr(code));
		}

		public void CreateInstance(int assemblyVersion)
		{
			this.EditorCompilerInternal.CreateInstance(string.Format("Editor.Core.CompileHelper{0}", assemblyVersion));
		}

		public object ExecuteMember(string methodName)
		{
			return this.EditorCompilerInternal.ExecuteMember(methodName, null);
		}

		public object ExecuteMember(string methodName, object[] parameters)
		{
			return this.EditorCompilerInternal.ExecuteMember(methodName, parameters);
		}

		private string GetCodeStr(string code)
		{
			return string.Concat("using System;\r\n\t\t\tusing System.Runtime.InteropServices;\r\n\t\t\t\r\n            namespace SR_Editor.Core\r\n\t\t\t{", code, "}");
		}

		public decimal GetFormulDeger(string pFormul)
		{
			decimal num;
			decimal num1 = new decimal(0);
			if (!pFormul.IsBos())
			{
				try
				{
					string str = string.Concat("public decimal GetDeger(){ try{return Convert.ToDecimal(", pFormul.Replace(",", "."), ");}catch{return 0;}}");
					this.ComplileCode(str, 1);
					this.CreateInstance(1);
					object obj = this.ExecuteMember("GetDeger");
					if ((obj == null || obj == DBNull.Value ? false : Convert.ToDecimal(obj) >= new decimal(0)))
					{
						num1 = Convert.ToDecimal(obj);
					}
				}
				catch (Exception exception)
				{
					throw new Exception(string.Concat("Anlaşma tanımlarına yanlış formül girilmiş(", pFormul, ")\nLütfen kontrol ediniz."), exception);
				}
				num = num1;
			}
			else
			{
				num = num1;
			}
			return num;
		}
	}
}