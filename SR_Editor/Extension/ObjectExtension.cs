using System;
using System.Runtime.CompilerServices;

namespace SR_Editor.Core
{
	public static class ObjectExtension
	{
		public static bool IsNotNull(this object obj)
		{
			bool flag;
			flag = ((obj == null ? false : obj != DBNull.Value) ? obj.ToString().IsNotNull() : false);
			return flag;
		}

		public static bool IsNull(this object obj)
		{
			return obj == null;
		}
	}
}