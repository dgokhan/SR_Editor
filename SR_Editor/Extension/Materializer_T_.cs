using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace SR_Editor.Core
{
	public sealed class Materializer<T>
	{
		private readonly static ParameterExpression s_recordParameter;

		private readonly static MethodInfo s_fieldOfTOrdinalMethod;

		private readonly static MethodInfo s_fieldOfTColumnNameMethod;

		private readonly Expression<Func<IDataRecord, T>> userSuppliedShaper;

        private readonly object syncLock = new object();

		private Func<IDataRecord, T> shaperDelegate;

		private System.Collections.ObjectModel.ReadOnlyCollection<string> fieldNames;

		static Materializer()
		{
			Materializer<T>.s_recordParameter = Expression.Parameter(typeof(IDataRecord), "record");
			Type type = typeof(DataExtensions);
			Type[] typeArray = new Type[] { typeof(IDataRecord), typeof(int) };
			Materializer<T>.s_fieldOfTOrdinalMethod = type.GetMethod("Field", typeArray);
			Type type1 = typeof(DataExtensions);
			typeArray = new Type[] { typeof(IDataRecord), typeof(string) };
			Materializer<T>.s_fieldOfTColumnNameMethod = type1.GetMethod("Field", typeArray);
		}

		public Materializer()
		{
		}

		public Materializer(StructuralType structuralType) : this(Materializer<T>.GetStructuralTypeShaper(structuralType))
		{
		}

		public Materializer(Expression<Func<IDataRecord, T>> shaper)
		{
			this.userSuppliedShaper = shaper;
		}

		private static Expression CreateGetValueCall(Type type, string columnName, int? ordinal)
		{
			MethodInfo methodInfo;
			Expression expression;
			Type[] typeArray;
			if (!ordinal.HasValue)
			{
				MethodInfo sFieldOfTColumnNameMethod = Materializer<T>.s_fieldOfTColumnNameMethod;
				typeArray = new Type[] { type };
				methodInfo = sFieldOfTColumnNameMethod.MakeGenericMethod(typeArray);
				expression = Expression.Constant(columnName);
			}
			else
			{
				MethodInfo sFieldOfTOrdinalMethod = Materializer<T>.s_fieldOfTOrdinalMethod;
				typeArray = new Type[] { type };
				methodInfo = sFieldOfTOrdinalMethod.MakeGenericMethod(typeArray);
				expression = Expression.Constant(ordinal.Value);
			}
			return Expression.Call(methodInfo, Materializer<T>.s_recordParameter, expression);
		}

		private static ConstructorInfo GetDefaultConstructor()
		{
			ConstructorInfo constructor = typeof(T).GetConstructor(Type.EmptyTypes);
			if (null == constructor)
			{
				throw new InvalidOperationException(/*Messages.UnableToCreateDefaultMaterializeDelegate*/);
			}
			return constructor;
		}

		private static Expression<Func<IDataRecord, T>> GetDefaultShaper(System.Collections.ObjectModel.ReadOnlyCollection<string> fieldNames)
		{
			MemberBinding memberBinding;
			ConstructorInfo defaultConstructor = Materializer<T>.GetDefaultConstructor();
			List<MemberBinding> memberBindings = new List<MemberBinding>();
			int num = 0;
			foreach (string fieldName in fieldNames)
			{
				if (Materializer<T>.TryCreateMemberBinding(fieldName, new int?(num), out memberBinding))
				{
					memberBindings.Add(memberBinding);
				}
				num++;
			}
			MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(defaultConstructor), memberBindings);
			ParameterExpression[] sRecordParameter = new ParameterExpression[] { Materializer<T>.s_recordParameter };
			return Expression.Lambda<Func<IDataRecord, T>>(memberInitExpression, sRecordParameter);
		}

		private static System.Collections.ObjectModel.ReadOnlyCollection<string> GetFieldNames(IDataRecord records)
		{
			List<string> strs = new List<string>(records.FieldCount);

            /*var b = from in Enumerable.Range(0, 5) select "a";

            var sad = from in Enumerable.Range(0, records.FieldCount)
                select records.GetName(i);

            strs.AddRange(asd);
            */
            for (int i = 0; i < records.FieldCount; i++)
            {
                strs.Add(records.GetName(i));
            }
            

            return strs.AsReadOnly();


        }

		private static Expression<Func<IDataRecord, T>> GetStructuralTypeShaper(StructuralType structuralType)
		{
			MemberBinding memberBinding;
			ConstructorInfo defaultConstructor = Materializer<T>.GetDefaultConstructor();
			List<MemberBinding> memberBindings = new List<MemberBinding>();
			foreach (EdmProperty edmProperty in structuralType.Members.OfType<EdmProperty>())
			{
				if (Materializer<T>.TryCreateMemberBinding(edmProperty.Name, null, out memberBinding))
				{
					memberBindings.Add(memberBinding);
				}
			}
			MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(defaultConstructor), memberBindings);
			ParameterExpression[] sRecordParameter = new ParameterExpression[] { Materializer<T>.s_recordParameter };
			return Expression.Lambda<Func<IDataRecord, T>>(memberInitExpression, sRecordParameter);
		}

		private void InitializeShaper(IDataRecord record)
		{
			if (null == this.fieldNames)
			{
				//lock (this.syncLock)
				{
					if (null == this.fieldNames)
					{
						System.Collections.ObjectModel.ReadOnlyCollection<string> fieldNames = Materializer<T>.GetFieldNames(record);
						Func<IDataRecord, T> func = Materializer<T>.OptimizingExpressionVisitor.Optimize(fieldNames, this.userSuppliedShaper ?? Materializer<T>.GetDefaultShaper(fieldNames)).Compile();
						this.fieldNames = fieldNames;
						this.shaperDelegate = func;
					}
					else
					{
						this.ValidateFieldNames(record);
					}
				}
			}
			else
			{
				this.ValidateFieldNames(record);
			}
		}

		public IEnumerable<T> Materialize(DbCommand command)
		{
			return this.Materialize(command, CommandBehavior.Default);
		}

		public IEnumerable<T> Materialize(DbCommand command, CommandBehavior commandBehavior)
		{
			NETUtility.CheckArgumentNotNull<DbCommand>(command, "command");
			IDisposable disposable = command.Connection.CreateConnectionScope();
			try
			{
				DbDataReader dbDataReaders = command.ExecuteReader(commandBehavior);
				try
				{
					foreach (T t in this.Materialize(dbDataReaders))
					{
						yield return t;
					}
				}
				finally
				{
					if (dbDataReaders != null)
					{
						((IDisposable)dbDataReaders).Dispose();
					}
				}
			}
			finally
			{
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
		}

		public IEnumerable<T> Materialize(DbDataReader reader)
		{
			NETUtility.CheckArgumentNotNull<DbDataReader>(reader, "reader");
			bool flag = true;
			while (reader.Read())
			{
				if (flag)
				{
					this.InitializeShaper(reader);
					flag = false;
				}
				yield return this.shaperDelegate(reader);
			}
			NETUtility.CheckDBLock();
		}

		private static bool TryCreateMemberBinding(string columnName, int? ordinal, out MemberBinding memberBinding)
		{
			bool flag;
			PropertyInfo property = typeof(T).GetProperty(columnName);
			if (null != property)
			{
				if (((int)property.GetIndexParameters().Length != 0 ? false : property.CanWrite))
				{
					memberBinding = Expression.Bind(property.GetSetMethod(), Materializer<T>.CreateGetValueCall(property.PropertyType, columnName, ordinal));
					flag = true;
					return flag;
				}
			}
			memberBinding = null;
			flag = false;
			return flag;
		}

		private void ValidateFieldNames(IDataRecord record)
		{
			if ((this.fieldNames.Count != record.FieldCount ? true : this.fieldNames.Where<string>((string fieldName, int ordinal) => record.GetName(ordinal) != fieldName).Any<string>()))
			{
				throw new InvalidOperationException(/*Messages.IncompatibleReader*/);
			}
		}

		private class OptimizingExpressionVisitor : ExpressionVisitor
		{
			private readonly System.Collections.ObjectModel.ReadOnlyCollection<string> fieldNames;

			private readonly ParameterExpression recordParameter;

			private OptimizingExpressionVisitor(System.Collections.ObjectModel.ReadOnlyCollection<string> fieldNames, ParameterExpression recordParameter)
			{
				this.fieldNames = fieldNames;
				this.recordParameter = recordParameter;
			}

			internal static Expression<Func<IDataRecord, T>> Optimize(System.Collections.ObjectModel.ReadOnlyCollection<string> fieldNames, Expression<Func<IDataRecord, T>> shaper)
			{
				NETUtility.CheckArgumentNotNull<System.Collections.ObjectModel.ReadOnlyCollection<string>>(fieldNames, "fieldNames");
				NETUtility.CheckArgumentNotNull<Expression<Func<IDataRecord, T>>>(shaper, "shaper");
				Materializer<T>.OptimizingExpressionVisitor optimizingExpressionVisitor = new Materializer<T>.OptimizingExpressionVisitor(fieldNames, shaper.Parameters.Single<ParameterExpression>());
				return (Expression<Func<IDataRecord, T>>)optimizingExpressionVisitor.Visit(shaper);
			}

			protected override Expression VisitMethodCall(MethodCallExpression m)
			{
				Expression expression;
				Expression expression1 = base.VisitMethodCall(m);
				if (expression1.NodeType == ExpressionType.Call)
				{
					m = (MethodCallExpression)expression1;
					MaterializerOptimizedMethodAttribute materializerOptimizedMethodAttribute = m.Method.GetCustomAttributes(typeof(MaterializerOptimizedMethodAttribute), false).Cast<MaterializerOptimizedMethodAttribute>().SingleOrDefault<MaterializerOptimizedMethodAttribute>();
					if (null != materializerOptimizedMethodAttribute)
					{
						expression = materializerOptimizedMethodAttribute.Optimizer.OptimizeMethodCall(this.fieldNames, this.recordParameter, m);
						return expression;
					}
				}
				expression = expression1;
				return expression;
			}
		}
	}
}