using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace SR_Editor.Core
{
    public static class DataExtensions
    {

        public static System.Data.Entity.Core.EntityKey GetEntityKey<T>(this DbContext context, T entity)
    where T : class
        {
            var oc = ((IObjectContextAdapter)context).ObjectContext;
            System.Data.Entity.Core.Objects.ObjectStateEntry ose;
            if (null != entity && oc.ObjectStateManager
                                    .TryGetObjectStateEntry(entity, out ose))
            {
                return ose.EntityKey;
            }
            return null;
        }

        public static System.Data.Entity.Core.EntityKey GetEntityKey<T>(this DbContext context
                                               , DbEntityEntry<T> dbEntityEntry)
            where T : class
        {
            if (dbEntityEntry != null)
            {
                return GetEntityKey(context, dbEntityEntry.Entity);
            }
            return null;
        }
        /*public static IEnumerable<TEntity> Bind<TEntity, TBase>(this IEnumerable<TEntity> source, ObjectSet<TBase> objectSet)
		where TEntity : class, TBase
		where TBase : class
		{
			NETUtility.CheckArgumentNotNull<IEnumerable<TEntity>>(source, "source");
			NETUtility.CheckArgumentNotNull<ObjectSet<TBase>>(objectSet, "objectSet");
			IEnumerable<TEntity> tEntities = 
				from  source
				select objectSet.FindOrAttach<TBase, TEntity>(e);
			return tEntities;
		}*/

        /*public static IEnumerable<TEntity> Bind<TEntity>(this IEnumerable<TEntity> source, ObjectContext context)
		where TEntity : class
		{
			NETUtility.CheckArgumentNotNull<IEnumerable<TEntity>>(source, "source");
			NETUtility.CheckArgumentNotNull<ObjectContext>(context, "context");
			return source.Bind<TEntity, TEntity>(context.CreateObjectSet<TEntity>());
		}*/
        /*
        public static System.Data.Entity.Core.Objects.ObjectContext ToObjectContext(this EditorDbContext ctx)
        {
            return ((IObjectContextAdapter)ctx).ObjectContext;
        }*/

        public static IDisposable CreateConnectionScope(this DbConnection connection)
        {
            NETUtility.CheckArgumentNotNull<DbConnection>(connection, "connection");
            return new DataExtensions.OpenConnectionLifetime(connection);
        }

        public static DbCommand CreateStoreCommand(this Database context, string commandText, params object[] parameters)
        {
            return context.CreateStoreCommand(commandText, CommandType.Text, parameters);
        }

        public static DbCommand CreateStoreCommand(this Database context, string commandText, CommandType commandType, params object[] parameters)
        {
            NETUtility.CheckArgumentNotNull<Database>(context, "context");

            DbCommand value = context.Connection.CreateCommand();
            value.CommandText = commandText;
            value.CommandType = commandType;
            if (null != parameters)
            {
                value.Parameters.AddRange(parameters);
            }
            if (context.CommandTimeout.HasValue)
            {
                value.CommandTimeout = context.CommandTimeout.Value;
            }
            return value;
        }

        public static Expression ExpandInvocations(this Expression expression)
        {
            return DataExtensions.InvocationExpander.Expand(expression);
        }

        public static IQueryable<TElement> ExpandInvocations<TElement>(this IQueryable<TElement> query)
        {
            NETUtility.CheckArgumentNotNull<IQueryable<TElement>>(query, "query");
            IQueryable<TElement> tElements = query.Provider.CreateQuery<TElement>(query.Expression.ExpandInvocations());
            return tElements;
        }

        [MaterializerOptimizedMethod(typeof(DataExtensions.FieldMethodOptimizer))]
        public static T Field<T>(this IDataRecord record, string name)
        {
            NETUtility.CheckArgumentNotNull<IDataRecord>(record, "record");
            NETUtility.CheckArgumentNotNull<string>(name, "name");
            return record.Field<T>(record.GetOrdinal(name));
        }

        [MaterializerOptimizedMethod(typeof(DataExtensions.FieldMethodOptimizer))]
        public static T Field<T>(this IDataRecord record, int ordinal)
        {
            NETUtility.CheckArgumentNotNull<IDataRecord>(record, "record");
            return (T)((record.IsDBNull(ordinal) ? null : record.GetValue(ordinal)));
        }
        /*
        public static TEntity FindOrAttach<TElement, TEntity>(this ObjectSet<TElement> objectSet, TEntity entity)
        where TElement : class
        where TEntity : class, TElement
        {
            ObjectStateEntry objectStateEntry;
            TEntity tEntity;
            NETUtility.CheckArgumentNotNull<ObjectSet<TElement>>(objectSet, "objectSet");
            if (null != entity)
            {
                string str = string.Concat(objectSet.EntitySet.EntityContainer.Name, ".", objectSet.EntitySet.Name);
                EntityKey entityKey = objectSet.Context.CreateEntityKey(str, entity);
                if ((!objectSet.Context.ObjectStateManager.TryGetObjectStateEntry(entityKey, out objectStateEntry) ? true : null == objectStateEntry.Entity))
                {
                    objectSet.Attach((TElement)(object)entity);
                    tEntity = entity;
                }
                else
                {
                    try
                    {
                        tEntity = (TEntity)objectStateEntry.Entity;
                    }
                    catch (InvalidCastException invalidCastException)
                    {
                        throw new InvalidOperationException("Messages.AttachedEntityHasWrongType");
                    }
                }
            }
            else
            {
                tEntity = default(TEntity);
            }
            return tEntity;
        }

        /*public static IEnumerable<TElement> GetTrackedEntities<TElement>(this ObjectSet<TElement> objectSet)
		where TElement : class
		{
			NETUtility.CheckArgumentNotNull<ObjectSet<TElement>>(objectSet, "objectSet");
			IEnumerable<TElement> trackedEntities = objectSet.GetTrackedEntities<TElement>(EntityState.Unchanged | EntityState.Added | EntityState.Deleted | EntityState.Modified);
			return trackedEntities;
		}*/

        /*public static IEnumerable<TElement> GetTrackedEntities<TElement>(this ObjectSet<TElement> objectSet, EntityState state)
		where TElement : class
		{
			NETUtility.CheckArgumentNotNull<ObjectSet<TElement>>(objectSet, "objectSet");
			IEnumerable<TElement> tElements = (
				from  in objectSet.Context.ObjectStateManager.GetObjectStateEntries(state)
				where DataExtensions.IsMemberOfObjectSet<TElement>(objectSet, entry)
				select  into e
				select e.Entity).Cast<TElement>();
			return tElements;
		}*/
        /*
        private static bool IsMemberOfObjectSet<TElement>(ObjectSet<TElement> objectSet, ObjectStateEntry entry)
        where TElement : class
        {
            return (entry.IsRelationship || entry.Entity == null ? false : entry.EntitySet == objectSet.EntitySet);
        }*/

        public static IEnumerable<T> Materialize<T>(this DbDataReader reader)
        {
            return (new Materializer<T>()).Materialize(reader);
        }

        public static IEnumerable<T> Materialize<T>(this DbDataReader reader, Expression<Func<IDataRecord, T>> expression)
        {
            return (new Materializer<T>(expression)).Materialize(reader);
        }

        public static IEnumerable<T> Materialize<T>(this DbCommand command)
        {
            return (new Materializer<T>()).Materialize(command);
        }

        public static IEnumerable<T> Materialize<T>(this DbCommand command, CommandBehavior commandBehavior)
        {
            return (new Materializer<T>()).Materialize(command, commandBehavior);
        }

        public static IEnumerable<T> Materialize<T>(this DbCommand command, Expression<Func<IDataRecord, T>> expression)
        {
            return (new Materializer<T>(expression)).Materialize(command);
        }

        public static IEnumerable<T> Materialize<T>(this DbCommand command, CommandBehavior commandBehavior, Expression<Func<IDataRecord, T>> expression)
        {
            return (new Materializer<T>(expression)).Materialize(command, commandBehavior);
        }

        private class FieldMethodOptimizer : IMaterializerMethodOptimizer
        {
            private readonly static MethodInfo s_fieldOrdinalMethod;

            private readonly static MethodInfo s_fieldNameMethod;

            private readonly static MethodInfo s_isDBNull;

            private readonly static MethodInfo s_getValue;

            static FieldMethodOptimizer()
            {
                Type type = typeof(DataExtensions);
                Type[] typeArray = new Type[] { typeof(IDataRecord), typeof(int) };
                DataExtensions.FieldMethodOptimizer.s_fieldOrdinalMethod = type.GetMethod("Field", typeArray);
                Type type1 = typeof(DataExtensions);
                typeArray = new Type[] { typeof(IDataRecord), typeof(string) };
                DataExtensions.FieldMethodOptimizer.s_fieldNameMethod = type1.GetMethod("Field", typeArray);
                Type type2 = typeof(IDataRecord);
                typeArray = new Type[] { typeof(int) };
                DataExtensions.FieldMethodOptimizer.s_isDBNull = type2.GetMethod("IsDBNull", typeArray);
                Type type3 = typeof(IDataRecord);
                typeArray = new Type[] { typeof(int) };
                DataExtensions.FieldMethodOptimizer.s_getValue = type3.GetMethod("GetValue", typeArray);
            }

            public FieldMethodOptimizer()
            {
            }

            private static DataExtensions.FieldMethodOptimizer.MethodPattern GetMethodPattern(MethodCallExpression methodCall)
            {
                DataExtensions.FieldMethodOptimizer.MethodPattern methodPattern;
                if (methodCall.Method.IsGenericMethod)
                {
                    MethodInfo genericMethodDefinition = methodCall.Method.GetGenericMethodDefinition();
                    if (!(genericMethodDefinition == DataExtensions.FieldMethodOptimizer.s_fieldOrdinalMethod))
                    {
                        methodPattern = (!(genericMethodDefinition == DataExtensions.FieldMethodOptimizer.s_fieldNameMethod) ? DataExtensions.FieldMethodOptimizer.MethodPattern.Unsupported : DataExtensions.FieldMethodOptimizer.MethodPattern.FieldName);
                    }
                    else
                    {
                        methodPattern = DataExtensions.FieldMethodOptimizer.MethodPattern.FieldOrdinal;
                    }
                }
                else
                {
                    methodPattern = DataExtensions.FieldMethodOptimizer.MethodPattern.Unsupported;
                }
                return methodPattern;
            }

            public Expression OptimizeMethodCall(System.Collections.ObjectModel.ReadOnlyCollection<string> fieldNames, ParameterExpression recordParameter, MethodCallExpression methodCall)
            {
                Expression expression;
                Expression expression1;
                bool flag;
                NETUtility.CheckArgumentNotNull<System.Collections.ObjectModel.ReadOnlyCollection<string>>(fieldNames, "fieldNames");
                NETUtility.CheckArgumentNotNull<MethodCallExpression>(methodCall, "methodCall");
                DataExtensions.FieldMethodOptimizer.MethodPattern methodPattern = DataExtensions.FieldMethodOptimizer.GetMethodPattern(methodCall);
                if (methodPattern == DataExtensions.FieldMethodOptimizer.MethodPattern.Unsupported)
                {
                    expression1 = methodCall;
                }
                else if (recordParameter != methodCall.Arguments[0])
                {
                    expression1 = methodCall;
                }
                else if (DataExtensions.FieldMethodOptimizer.TryGetOrdinalExpression(fieldNames, methodCall, methodPattern, out expression))
                {
                    Type type = methodCall.Method.GetGenericArguments().Single<Type>();
                    if (type.IsClass)
                    {
                        flag = true;
                    }
                    else
                    {
                        flag = (!type.IsGenericType ? false : typeof(Nullable<>) == type.GetGenericTypeDefinition());
                    }
                    bool flag1 = flag;
                    Expression item = methodCall.Arguments[0];
                    MethodInfo sGetValue = DataExtensions.FieldMethodOptimizer.s_getValue;
                    Expression[] expressionArray = new Expression[] { expression };
                    Expression expression2 = Expression.Call(item, sGetValue, expressionArray);
                    if (flag1)
                    {
                        Expression item1 = methodCall.Arguments[0];
                        MethodInfo sIsDBNull = DataExtensions.FieldMethodOptimizer.s_isDBNull;
                        expressionArray = new Expression[] { expression };
                        expression2 = Expression.Condition(Expression.Call(item1, sIsDBNull, expressionArray), Expression.Constant(null, typeof(object)), expression2);
                    }
                    expression2 = Expression.Convert(expression2, type);
                    expression1 = expression2;
                }
                else
                {
                    expression1 = methodCall;
                }
                return expression1;
            }

            private static bool TryGetOrdinalExpression(System.Collections.ObjectModel.ReadOnlyCollection<string> fieldNames, MethodCallExpression methodCall, DataExtensions.FieldMethodOptimizer.MethodPattern pattern, out Expression ordinalExpression)
            {
                bool flag;
                ordinalExpression = null;
                if (pattern != DataExtensions.FieldMethodOptimizer.MethodPattern.FieldOrdinal)
                {
                    if (pattern == DataExtensions.FieldMethodOptimizer.MethodPattern.FieldName)
                    {
                        Expression item = methodCall.Arguments[1];
                        if (item.NodeType == ExpressionType.Constant)
                        {
                            string value = (string)((ConstantExpression)item).Value;
                            if (null != value)
                            {
                                int num = 0;
                                while (true)
                                {
                                    if ((num >= fieldNames.Count ? true : value == fieldNames[num]))
                                    {
                                        break;
                                    }
                                    num++;
                                }
                                if (num < fieldNames.Count)
                                {
                                    ordinalExpression = Expression.Constant(num);
                                    flag = true;
                                    return flag;
                                }
                            }
                            else
                            {
                                flag = false;
                                return flag;
                            }
                        }
                    }
                    flag = false;
                }
                else
                {
                    ordinalExpression = methodCall.Arguments[1];
                    flag = true;
                }
                return flag;
            }

            private enum MethodPattern
            {
                Unsupported,
                FieldOrdinal,
                FieldName
            }
        }

        private sealed class InvocationExpander : ExpressionVisitor
        {
            private readonly ParameterExpression _parameter;

            private readonly Expression _expansion;

            private readonly DataExtensions.InvocationExpander _previous;

            private readonly static DataExtensions.InvocationExpander s_singleton;

            static InvocationExpander()
            {
                DataExtensions.InvocationExpander.s_singleton = new DataExtensions.InvocationExpander();
            }

            private InvocationExpander(ParameterExpression parameter, Expression expansion, DataExtensions.InvocationExpander previous)
            {
                NETUtility.CheckArgumentNotNull<ParameterExpression>(parameter, "parameter");
                NETUtility.CheckArgumentNotNull<Expression>(expansion, "expansion");
                NETUtility.CheckArgumentNotNull<DataExtensions.InvocationExpander>(previous, "previous");
                this._parameter = parameter;
                this._expansion = expansion;
                this._previous = previous;
            }

            private InvocationExpander()
            {
            }

            internal static Expression Expand(Expression expression)
            {
                return DataExtensions.InvocationExpander.s_singleton.Visit(expression);
            }

            protected override Expression VisitInvocation(InvocationExpression iv)
            {
                Expression expression;
                if (iv.Expression.NodeType != ExpressionType.Lambda)
                {
                    expression = base.VisitInvocation(iv);
                }
                else
                {
                    LambdaExpression lambdaExpression = (LambdaExpression)iv.Expression;
                    expression = lambdaExpression.Parameters.Zip(iv.Arguments, (ParameterExpression p, Expression e) => new { Parameter = p, Expansion = e }).Aggregate(this, (previous, pair) => new DataExtensions.InvocationExpander(pair.Parameter, pair.Expansion, previous)).Visit(lambdaExpression.Body);
                }
                return expression;
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                Expression expression;
                DataExtensions.InvocationExpander invocationExpander = this;
                while (true)
                {
                    if (null == invocationExpander)
                    {
                        expression = base.VisitParameter(p);
                        break;
                    }
                    else if (invocationExpander._parameter != p)
                    {
                        invocationExpander = invocationExpander._previous;
                    }
                    else
                    {
                        expression = base.Visit(invocationExpander._expansion);
                        break;
                    }
                }
                return expression;
            }
        }

        private class OpenConnectionLifetime : IDisposable
        {
            private readonly DbConnection connection;

            private readonly bool closeOnDispose;

            internal OpenConnectionLifetime(DbConnection connection)
            {
                this.connection = connection;
                this.closeOnDispose = connection.State == ConnectionState.Closed;
                if (this.closeOnDispose)
                {
                    this.connection.Open();
                }
            }

            public void Dispose()
            {
                if ((!this.closeOnDispose ? false : this.connection.State == ConnectionState.Open))
                {
                    this.connection.Close();
                }
                GC.SuppressFinalize(this);
            }
        }

        public static System.Collections.IEnumerable DynamicSqlQuery(this Database database, string sql, params object[] parameters)
        {
            TypeBuilder builder = createTypeBuilder(
                    "MyDynamicAssembly", "MyDynamicModule", "MyDynamicType");

            using (System.Data.IDbCommand command = database.Connection.CreateCommand())
            {
                try
                {
                    database.Connection.Open();
                    command.CommandText = sql;
                    command.CommandTimeout = command.Connection.ConnectionTimeout;
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }

                    using (System.Data.IDataReader reader = command.ExecuteReader())
                    {
                        var schema = reader.GetSchemaTable();

                        foreach (System.Data.DataRow row in schema.Rows)
                        {
                            string name = (string)row["ColumnName"];
                            //var a=row.ItemArray.Select(d=>d.)
                            Type type = (Type)row["DataType"];
                            if (type != typeof(string) && (bool)row.ItemArray[schema.Columns.IndexOf("AllowDbNull")])
                            {
                                type = typeof(Nullable<>).MakeGenericType(type);
                            }
                            createAutoImplementedProperty(builder, name, type);
                        }
                    }
                }
                finally
                {
                    database.Connection.Close();
                    command.Parameters.Clear();
                }
            }

            Type resultType = builder.CreateType();

            return database.SqlQuery(resultType, sql, parameters);
        }

        private static TypeBuilder createTypeBuilder(
            string assemblyName, string moduleName, string typeName)
        {
            TypeBuilder typeBuilder = AppDomain
                .CurrentDomain
                .DefineDynamicAssembly(new AssemblyName(assemblyName),
                                       AssemblyBuilderAccess.Run)
                .DefineDynamicModule(moduleName)
                .DefineType(typeName, TypeAttributes.Public);
            typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            return typeBuilder;
        }

        private static void createAutoImplementedProperty(
            TypeBuilder builder, string propertyName, Type propertyType)
        {
            const string PrivateFieldPrefix = "m_";
            const string GetterPrefix = "get_";
            const string SetterPrefix = "set_";

            // Generate the field.
            FieldBuilder fieldBuilder = builder.DefineField(
                string.Concat(PrivateFieldPrefix, propertyName),
                              propertyType, FieldAttributes.Private);

            // Generate the property
            PropertyBuilder propertyBuilder = builder.DefineProperty(
                propertyName, System.Reflection.PropertyAttributes.HasDefault, propertyType, null);

            // Property getter and setter attributes.
            MethodAttributes propertyMethodAttributes =
                MethodAttributes.Public | MethodAttributes.SpecialName |
                MethodAttributes.HideBySig;

            // Define the getter method.
            MethodBuilder getterMethod = builder.DefineMethod(
                string.Concat(GetterPrefix, propertyName),
                propertyMethodAttributes, propertyType, Type.EmptyTypes);

            // Emit the IL code.
            // ldarg.0
            // ldfld,_field
            // ret
            ILGenerator getterILCode = getterMethod.GetILGenerator();
            getterILCode.Emit(OpCodes.Ldarg_0);
            getterILCode.Emit(OpCodes.Ldfld, fieldBuilder);
            getterILCode.Emit(OpCodes.Ret);

            // Define the setter method.
            MethodBuilder setterMethod = builder.DefineMethod(
                string.Concat(SetterPrefix, propertyName),
                propertyMethodAttributes, null, new Type[] { propertyType });

            // Emit the IL code.
            // ldarg.0
            // ldarg.1
            // stfld,_field
            // ret
            ILGenerator setterILCode = setterMethod.GetILGenerator();
            setterILCode.Emit(OpCodes.Ldarg_0);
            setterILCode.Emit(OpCodes.Ldarg_1);
            setterILCode.Emit(OpCodes.Stfld, fieldBuilder);
            setterILCode.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getterMethod);
            propertyBuilder.SetSetMethod(setterMethod);
        }
    }
}