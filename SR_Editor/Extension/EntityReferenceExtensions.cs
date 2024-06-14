using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SR_Editor.Core
{
	public static class EntityReferenceExtensions
	{
		public static object GetKey<T>(this EntityReference<T> entityReference)
		where T : class, IEntityWithRelationships
		{
			object value;
			NETUtility.CheckArgumentNotNull<EntityReference<T>>(entityReference, "entityReference");
			EntityKey entityKey = entityReference.EntityKey;
			if (!(null == entityKey))
			{
				EntityKeyMember[] entityKeyValues = entityKey.EntityKeyValues;
				if ((int)entityKeyValues.Length != 1)
				{
					throw new InvalidOperationException(/*Messages.SimpleKeyOnly*/);
				}
				value = entityKeyValues[0].Value;
			}
			else
			{
				if (entityReference.GetTargetEntitySet().ElementType.KeyMembers.Count != 1)
				{
					throw new InvalidOperationException(/*Messages.SimpleKeyOnly*/);
				}
				value = null;
			}
			return value;
		}

		public static object GetKey<T>(this EntityReference<T> entityReference, int keyOrdinal)
		where T : class, IEntityWithRelationships
		{
			object value;
			NETUtility.CheckArgumentNotNull<EntityReference<T>>(entityReference, "entityReference");
			if (keyOrdinal < 0)
			{
				throw new ArgumentOutOfRangeException("keyOrdinal");
			}
			EntityKey entityKey = entityReference.EntityKey;
			if (!(null == entityKey))
			{
				if ((int)entityKey.EntityKeyValues.Length <= keyOrdinal)
				{
					throw new ArgumentOutOfRangeException("keyOrdinal");
				}
				value = entityKey.EntityKeyValues[keyOrdinal].Value;
			}
			else
			{
				if (entityReference.GetTargetEntitySet().ElementType.KeyMembers.Count <= keyOrdinal)
				{
					throw new ArgumentOutOfRangeException("keyOrdinal");
				}
				value = null;
			}
			return value;
		}

		public static EntitySet GetTargetEntitySet(this RelatedEnd relatedEnd)
		{
			NETUtility.CheckArgumentNotNull<RelatedEnd>(relatedEnd, "relatedEnd");
			AssociationSet relationshipSet = (AssociationSet)relatedEnd.RelationshipSet;
			if (null == relationshipSet)
			{
				throw new InvalidOperationException(/*Messages.CannotDetermineMetadataForRelatedEnd*/);
			}
			return relationshipSet.AssociationSetEnds[relatedEnd.TargetRoleName].EntitySet;
		}

		public static void SetKey<T>(this EntityReference<T> entityReference, params object[] keyValues)
		where T : class, IEntityWithRelationships
		{
			IEnumerable<string> entityKeyValues;
			int length;
			string str;
			NETUtility.CheckArgumentNotNull<EntityReference<T>>(entityReference, "entityReference");
			if (null == keyValues)
			{
				entityReference.EntityKey = null;
			}
			if (!(null == entityReference.EntityKey))
			{
				EntityKey entityKey = entityReference.EntityKey;
				entityKeyValues = 
					from v in (IEnumerable<EntityKeyMember>)entityKey.EntityKeyValues
					select v.Key;
				length = (int)entityKey.EntityKeyValues.Length;
				str = string.Concat(entityKey.EntityContainerName, ".", entityKey.EntitySetName);
			}
			else
			{
				EntitySet targetEntitySet = entityReference.GetTargetEntitySet();
				entityKeyValues = 
					from m in targetEntitySet.ElementType.KeyMembers
					select m.Name;
				length = targetEntitySet.ElementType.KeyMembers.Count;
				str = string.Concat(targetEntitySet.EntityContainer.Name, ".", targetEntitySet.Name);
			}
			if ((keyValues == null ? false : length != (int)keyValues.Length))
			{
				throw new ArgumentException(/*Messages.UnexpectedKeyCount*/"", "keyValues");
			}
			if ((keyValues == null ? false : !keyValues.Any<object>((object v) => null == v)))
			{
				EntityKey entityKey1 = new EntityKey(str, entityKeyValues.Zip<string, object, EntityKeyMember>((IEnumerable<object>)keyValues, (string name, object value) => new EntityKeyMember(name, value)));
				entityReference.EntityKey = entityKey1;
			}
			else
			{
				entityReference.EntityKey = null;
			}
		}
	}
}