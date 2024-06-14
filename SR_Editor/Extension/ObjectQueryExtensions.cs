using System;
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SR_Editor.Core
{
	public static class ObjectQueryExtensions
	{
		public static ObjectQuery<T> AsObjectQuery<T>(this IQueryable<T> source)
		{
			return source as ObjectQuery<T>;
		}

		public static IQueryable<T> Include<T>(this IQueryable<T> source, string path)
		{
			return source.ToObjectQuery<T>("source").Include(path);
		}

		public static IQueryable<T> SetMergeOption<T>(this IQueryable<T> source, MergeOption mergeOption)
		{
			ObjectQuery<T> objectQuery = source.ToObjectQuery<T>("source");
			objectQuery.MergeOption = mergeOption;
			return objectQuery;
		}

		public static IBindingList ToBindingList<T>(this IQueryable<T> source)
		{
			NETUtility.CheckArgumentNotNull<IQueryable<T>>(source, "source");
			IListSource listSource = source as IListSource;
			if (null == listSource)
			{
				throw new ArgumentException(""/*Messages.UnableToGetBindingList*/, "source");
			}
			IBindingList list = listSource.GetList() as IBindingList;
			if (null == list)
			{
				throw new ArgumentException(""/*Messages.UnableToGetBindingList*/, "source");
			}
			return list;
		}

		private static ObjectQuery<T> ToObjectQuery<T>(this IQueryable<T> source, string argumentName)
		{
			NETUtility.CheckArgumentNotNull<IQueryable<T>>(source, "source");
			ObjectQuery<T> ts = source as ObjectQuery<T>;
			if (null == ts)
			{
				CultureInfo currentCulture = CultureInfo.CurrentCulture;
				string operationRequiresObjectQuery = "Messages.OperationRequiresObjectQuery";
				object[] objArray = new object[] { argumentName };
				throw new ArgumentException(string.Format(currentCulture, operationRequiresObjectQuery, objArray));
			}
			return ts;
		}

		public static string ToTraceString<T>(this IQueryable<T> source)
		{
			return source.ToObjectQuery<T>("source").ToTraceString();
		}
	}
}