using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Specialized;
using System.Reflection;

namespace SR_Editor.Core
{
	public class UtilCompilerInternal
	{
		private CompilerResults compilerResults;

		private object instanceObject;

		public ArrayList AssemblyList;

		public UtilCompilerInternal()
		{
			this.AssemblyList = new ArrayList();
		}

		public CompilerResults ComplileCode(string code)
		{
			CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider();
			CompilerParameters compilerParameter = new CompilerParameters();
			foreach (string assemblyList in this.AssemblyList)
			{
				compilerParameter.ReferencedAssemblies.Add(assemblyList);
			}
			compilerParameter.GenerateInMemory = true;
			string[] str = new string[] { code };
			this.compilerResults = cSharpCodeProvider.CompileAssemblyFromSource(compilerParameter, str);
			if (this.compilerResults.Errors.HasErrors)
			{
				string str1 = "";
				for (int i = 0; i < this.compilerResults.Errors.Count; i++)
				{
					string str2 = str1;
					str = new string[] { str2, "\r\n Line: ", null, null, null };
					int line = this.compilerResults.Errors[i].Line;
					str[2] = line.ToString();
					str[3] = " - ";
					str[4] = this.compilerResults.Errors[i].ErrorText;
					str1 = string.Concat(str);
				}
				throw new Exception(str1);
			}
			return this.compilerResults;
		}

		public object CreateInstance(string instanceName)
		{
			this.instanceObject = this.compilerResults.CompiledAssembly.CreateInstance(instanceName);
			if (this.instanceObject == null)
			{
				throw new Exception(string.Concat(instanceName, " not loaded"));
			}
			return this.instanceObject;
		}

		public object ExecuteMember(string memberName, object[] codeParams)
		{
			object obj;
			obj = (!(this.instanceObject.GetType().GetMethod(memberName) != null) ? null : this.instanceObject.GetType().InvokeMember(memberName, BindingFlags.InvokeMethod, null, this.instanceObject, codeParams));
			return obj;
		}
	}
}