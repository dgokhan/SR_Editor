using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SR_Editor.Core
{
	public static class NETUtility
	{
		internal static void CheckArgumentNotNull<T>(T argumentValue, string argumentName)
		where T : class
		{
			if (null == argumentValue)
			{
				throw new ArgumentNullException(argumentName);
			}
		}

		internal static void CheckDBLock()
		{
		}

		public static IEnumerable<TResult> ForEach<TResult>(this IEnumerable<TResult> source, Action<TResult> action)
		{
			NETUtility.CheckArgumentNotNull<IEnumerable<TResult>>(source, "source");
			NETUtility.CheckArgumentNotNull<Action<TResult>>(action, "action");
			return NETUtility.ForEachIterator<TResult>(source, action);
		}

		private static IEnumerable<TResult> ForEachIterator<TResult>(IEnumerable<TResult> source, Action<TResult> action)
		{
			foreach (TResult tResult in source)
			{
				action(tResult);
				yield return tResult;
			}
		}

		public static bool IsGenericAssignableFrom(this Type toType, Type fromType, out Type[] genericArguments)
		{
			bool flag;
			NETUtility.CheckArgumentNotNull<Type>(toType, "toType");
			NETUtility.CheckArgumentNotNull<Type>(fromType, "fromType");
			if ((!toType.IsGenericTypeDefinition ? false : !fromType.IsGenericTypeDefinition))
			{
				if (!toType.IsInterface)
				{
					while (fromType != null)
					{
						if ((!fromType.IsGenericType ? true : !(fromType.GetGenericTypeDefinition() == toType)))
						{
							fromType = fromType.BaseType;
						}
						else
						{
							genericArguments = fromType.GetGenericArguments();
							flag = true;
							return flag;
						}
					}
				}
				else
				{
					Type[] interfaces = fromType.GetInterfaces();
					int num = 0;
					while (num < (int)interfaces.Length)
					{
						Type type = interfaces[num];
						if ((!type.IsGenericType ? true : !(type.GetGenericTypeDefinition() == toType)))
						{
							num++;
						}
						else
						{
							genericArguments = type.GetGenericArguments();
							flag = true;
							return flag;
						}
					}
				}
				genericArguments = null;
				flag = false;
			}
			else
			{
				genericArguments = null;
				flag = false;
			}
			return flag;
		}
	}
}