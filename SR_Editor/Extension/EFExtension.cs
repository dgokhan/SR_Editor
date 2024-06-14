using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace SR_Editor.Core
{
	public static class EFExtension
	{
		private readonly static List<SelfReferencesTracking> _tracking;

		static EFExtension()
		{
			EFExtension._tracking = new List<SelfReferencesTracking>();
		}

		public static void AttachToContext(this ObjectContext pObjectContext, EntityObject pEntityObject, string pEntitySetName, string pKeyName, object pKeyValue)
		{
			if ((pEntityObject.EntityKey == null ? true : pEntityObject.EntityState == EntityState.Detached))
			{
				pEntityObject.EntityKey = new EntityKey(pEntitySetName, pKeyName, pKeyValue);
				ObjectStateEntry objectStateEntry = null;
				if (pObjectContext.ObjectStateManager.TryGetObjectStateEntry(pEntityObject.EntityKey, out objectStateEntry))
				{
					pObjectContext.Detach(objectStateEntry.Entity);
				}
				pObjectContext.Attach(pEntityObject);
			}
		}

		public static EntityObject Clone(this EntityObject entityObject)
		{
			object entityKey = entityObject.GetType().GetConstructor(new Type[0]).Invoke(new object[0]);
			List<SelfReferencesTracking> selfReferencesTrackings = EFExtension._tracking;
			SelfReferencesTracking selfReferencesTracking = new SelfReferencesTracking()
			{
				EntitySetName = entityObject.EntityKey.EntitySetName,
				OriginalKeys = entityObject.EntityKey,
				NewEntityObject = (EntityObject)entityKey
			};
			selfReferencesTrackings.Add(selfReferencesTracking);
			PropertyInfo[] properties = entityObject.GetType().GetProperties();
			for (int i = 0; i < (int)properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				try
				{
					object value = propertyInfo.GetValue(entityObject, null);
					PropertyInfo propertyInfo1 = propertyInfo;
					if ((
						from x in entityObject.EntityKey.EntityKeyValues
						where x.Key == propertyInfo1.Name
						select x).Count<EntityKeyMember>() == 0)
					{
						if ((!(propertyInfo.PropertyType != typeof(EntityKey)) || !(propertyInfo.PropertyType != typeof(EntityState)) ? false : propertyInfo.PropertyType != typeof(EntityReference<>)))
						{
							if (propertyInfo.GetCustomAttributes(typeof(EdmRelationshipNavigationPropertyAttribute), false).Count<object>() != 1)
							{
								if (!(value.GetType().BaseType.Name == "EntityReference"))
								{
									propertyInfo.SetValue(entityKey, propertyInfo.GetValue(entityObject, null), null);
								}
								else
								{
									((EntityReference)propertyInfo.GetValue(entityKey, null)).EntityKey = ((EntityReference)propertyInfo.GetValue(entityObject, null)).EntityKey;
								}
							}
							else if (value.GetType() == entityObject.GetType())
							{
								EntityObject value1 = (EntityObject)propertyInfo.GetValue(entityObject, null);
								EntityObject newEntityObject = null;
								foreach (SelfReferencesTracking selfReferencesTracking1 in 
									from x in EFExtension._tracking
									where x.EntitySetName == value1.EntityKey.EntitySetName
									select x)
								{
									EntityKeyMember[] entityKeyValues = value1.EntityKey.EntityKeyValues;
									for (int j = 0; j < (int)entityKeyValues.Length; j++)
									{
										EntityKeyMember entityKeyMember = entityKeyValues[j];
										EntityKeyMember[] entityKeyMemberArray = selfReferencesTracking1.OriginalKeys.EntityKeyValues;
										int num = 0;
										while (num < (int)entityKeyMemberArray.Length)
										{
											EntityKeyMember entityKeyMember1 = entityKeyMemberArray[num];
											if (newEntityObject != null)
											{
												break;
											}
											else
											{
												if ((entityKeyMember1.Key != entityKeyMember.Key ? false : entityKeyMember1.Value == entityKeyMember.Value))
												{
													newEntityObject = selfReferencesTracking1.NewEntityObject;
												}
												num++;
											}
										}
									}
								}
								propertyInfo.SetValue(entityKey, newEntityObject, null);
							}
							else if (value.GetType().IsGenericType)
							{
								if (!value.GetType().GetGenericArguments().First<Type>().FullName.Equals(entityObject.GetType().FullName))
								{
									RelatedEnd relatedEnd = (RelatedEnd)propertyInfo.GetValue(entityObject, null);
									if (!relatedEnd.IsLoaded)
									{
										relatedEnd.Load();
									}
									Type type = typeof(EntityCollection<>);
									Type[] genericArguments = new Type[] { propertyInfo.PropertyType.GetGenericArguments()[0] };
									object obj = Activator.CreateInstance(type.MakeGenericType(genericArguments));
									foreach (object obj1 in relatedEnd)
									{
										MethodInfo method = obj.GetType().GetMethod("Add");
										object[] objArray = new object[] { ((EntityObject)obj1).Clone() };
										method.Invoke(obj, objArray);
									}
									propertyInfo.SetValue(entityKey, obj, null);
								}
							}
						}
					}
				}
				catch (InvalidCastException invalidCastException)
				{
					Debug.WriteLine(invalidCastException.Message);
					goto Label0;
				}
				catch (Exception exception)
				{
					Debug.WriteLine(exception.Message);
					goto Label0;
				}
            }
            Label0:
            return (EntityObject)entityKey;
		}

		public static EntityObject CloneSimple(this EntityObject entityObject)
		{
			bool flag;
			object obj = entityObject.GetType().GetConstructor(new Type[0]).Invoke(new object[0]);
			PropertyInfo[] properties = entityObject.GetType().GetProperties();
			for (int i = 0; i < (int)properties.Length; i++)
			{
				PropertyInfo propertyInfo = properties[i];
				try
				{
					object value = propertyInfo.GetValue(entityObject, null);
					PropertyInfo propertyInfo1 = propertyInfo;
					if (entityObject.EntityKey == null || entityObject.EntityKey != null && entityObject.EntityKey.EntityKeyValues == null)
					{
						flag = false;
					}
					else
					{
						flag = (!(entityObject.EntityKey != null) || entityObject.EntityKey.EntityKeyValues == null ? true : (
							from x in entityObject.EntityKey.EntityKeyValues
							where x.Key == propertyInfo1.Name
							select x).Count<EntityKeyMember>() != 0);
					}
					if (!flag)
					{
						if ((!(propertyInfo.PropertyType != typeof(EntityKey)) || !(propertyInfo.PropertyType != typeof(EntityReference<>)) || !(propertyInfo.PropertyType != typeof(EntityState)) || !(propertyInfo.PropertyType != typeof(EntityReference<>)) || value == null || value == DBNull.Value ? false : value.GetType().BaseType.Name != "EntityReference"))
						{
							if (propertyInfo.GetCustomAttributes(typeof(EdmRelationshipNavigationPropertyAttribute), false).Count<object>() == 0)
							{
								propertyInfo.SetValue(obj, value, null);
							}
						}
					}
				}
				catch (InvalidCastException invalidCastException)
				{
					Debug.WriteLine(invalidCastException.Message);
				}
				catch (Exception exception)
				{
					Debug.WriteLine(exception.Message);
				}
			}
			return (EntityObject)obj;
		}

		public static void DiscardChanges(this ObjectContext pObjectContext)
		{
			foreach (ObjectStateEntry objectStateEntry in pObjectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified))
			{
				EFExtension.DiscardChanges(objectStateEntry);
			}
		}

		public static void DiscardChanges(this ObjectContext pObjectContext, Type pType)
		{
			foreach (ObjectStateEntry objectStateEntry in pObjectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Added | EntityState.Deleted | EntityState.Modified))
			{
				if (objectStateEntry.Entity.GetType() == pType)
				{
					EFExtension.DiscardChanges(objectStateEntry);
				}
			}
		}

		public static void DiscardChanges(this ObjectContext pObjectContext, IEntityWithKey pEntity)
		{
			EFExtension.DiscardChanges(pObjectContext.ObjectStateManager.GetObjectStateEntry(pEntity.EntityKey));
		}

		private static void DiscardChanges(ObjectStateEntry pEntry)
		{
			EntityState state = pEntry.State;
			if (state == EntityState.Added)
			{
				pEntry.ChangeState(EntityState.Detached);
			}
			else if (state == EntityState.Deleted)
			{
				pEntry.ChangeState(EntityState.Unchanged);
			}
			else if (state == EntityState.Modified)
			{
				for (int i = 0; i < pEntry.OriginalValues.FieldCount; i++)
				{
					try
					{
						pEntry.CurrentValues.SetValue(i, pEntry.OriginalValues[i]);
					}
					catch (Exception exception)
					{
					}
				}
				pEntry.AcceptChanges();
			}
		}

		public static string GetTableName<T>(this ObjectContext context)
		where T : class
		{
			string traceString = context.CreateObjectSet<T>().ToTraceString();
			Match match = (new Regex("FROM (?<table>.*) AS")).Match(traceString);
			return match.Groups["table"].Value;
		}
	}
}