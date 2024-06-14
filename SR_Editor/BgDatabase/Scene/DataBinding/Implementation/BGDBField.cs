/*
<copyright file="BGDBField.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// implementation class for the field binder
    /// </summary>
    [Serializable]
    public class BGDBField : BGDBA
    {
        //-------------------- serializable
        //field
        [SerializeField] private string metaIdString;
        [SerializeField] private string entityIdString;
        [SerializeField] private string fieldIdString;
        [SerializeField] private short functionCode;
        [SerializeField] private string functionClass;

        //-------------------- Non serializable
        [NonSerialized] private FieldValueProvider valueProvider;
        [NonSerialized] private readonly FieldBindSourceProvider sourceProvider;
        [NonSerialized] private BGFBFuntion function;

        /// <summary>
        /// helper class (for the source provider)
        /// </summary>
        public FieldBindSourceProvider BindSourceProvider
        {
            get
            {
                if (sourceProvider.IsObsolete) sourceProvider.Build();
                return sourceProvider;
            }
        }

        /// <summary>
        /// Table ID as string
        /// </summary>
        public string MetaIdString
        {
            get => metaIdString;
            set => metaIdString = value;
        }

        /// <summary>
        /// Entity ID as string
        /// </summary>
        public string EntityIdString
        {
            get => entityIdString;
            set => entityIdString = value;
        }

        /// <summary>
        /// field ID as string
        /// </summary>
        public string FieldIdString
        {
            get => fieldIdString;
            set => fieldIdString = value;
        }

        public short FunctionCode
        {
            get => functionCode;
            set => functionCode = value;
        }

        public string FunctionClass
        {
            get => functionClass;
            set => functionClass = value;
        }

        public BGFBFuntion Function
        {
            get
            {
                if (functionCode == 0) return null;
                if (function != null)
                {
                    if (functionCode == BGFBFuntionToString.Code && function is BGFBFuntionToString) return function;
                    if (functionCode == BGFBFuntion.CustomCode && function.GetType().FullName == functionClass) return function;
                }

                function = null;
                switch (functionCode)
                {
                    case BGFBFuntionToString.Code:
                    {
                        return function = new BGFBFuntionToString();
                    }
                    case BGFBFuntion.CustomCode:
                    {
                        try
                        {
                            return function = (BGFBFuntion)Activator.CreateInstance(BGUtil.GetType(functionClass));
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                            break;
                        }
                    }
                }

                return null;
            }
        }


        /// <summary>
        /// Custom value provider (if defined in settings)
        /// </summary>
        public FieldValueProvider ValueProvider
        {
            get
            {
                if (valueProvider == null && IsUsingSpecialField) valueProvider = BGLocalizationUglyHacks.DataBindingInitValueProvider(fieldIdString);
                return valueProvider;
            }
        }

        /// <summary>
        /// Source table
        /// </summary>
        public BGMetaEntity Meta => BindSourceProvider.SourceMeta;

        /// <summary>
        /// Source field
        /// </summary>
        public BGField Field => BindSourceProvider.TargetField;

        /// <summary>
        /// Target field/property type
        /// </summary>
        public Type FieldType
        {
            get
            {
                if (IsUsingSpecialField)
                {
                    var provider = ValueProvider;
                    return provider == null ? null : ValueProvider.GetValueType(BindSourceProvider.TargetMeta);
                }

                var field = Field;
                return field?.ValueType;
            }
        }

        /// <summary>
        /// Source row
        /// </summary>
        public BGEntity Entity => BindSourceProvider.SourceEntity;

        /// <summary>
        /// Is special field is used (like $locale)
        /// </summary>
        public bool IsUsingSpecialField => !string.IsNullOrEmpty(fieldIdString) && fieldIdString[0] == '$';


        /// <inheritdoc/>
        public override object ValueToBind
        {
            get
            {
                error = null;
                EnsureTarget();
                if (error != null) return null;

                var valueToBind = GetValue();

                //auto-convert
                if (functionCode != 0)
                {
                    var func = Function;
                    if (func != null) valueToBind = func.Convert(Field, Entity, valueToBind);
                }

                return valueToBind;
            }
        }

        public override object GetValue()
        {
            var buildSourceProvider = BindSourceProvider;
            if (error != null) return null;

            object value;
            if (IsUsingSpecialField) value = ValueProvider.GetValue(buildSourceProvider.TargetEntity);
            else value = buildSourceProvider.TargetField.GetValue(buildSourceProvider.TargetEntity.Index);

            return value;
        }

        /*private object GetAutoConvertedValue(object valueToBind)
        {
            var targetType = IsTargetProperty ? targetProperty.PropertyType : targetField.FieldType;
            if (targetType != typeof(string)) return valueToBind;
            if (valueToBind is string) return valueToBind;
            return valueToBind.ToString();
        }*/


        public BGDBField()
        {
            sourceProvider = new FieldBindSourceProvider(this);
        }


        /// <inheritdoc/>
        public override string ReverseBind()
        {
            if (IsUsingSpecialField) return null;
            var field = BindSourceProvider.TargetField;
            if (field != null && !field.StoredValueIsTheSameAsValueType) return null;


            try
            {
                if (field == null)
                {
                    error = "Can not find field with id " + fieldIdString;
                    return error;
                }

                var entity = BindSourceProvider.TargetEntity;
                if (entity == null)
                {
                    error = "Can not find entity with id " + entityIdString;
                    return error;
                }

                if (entity.IsDeleted)
                {
                    error = "Entity is deleted. id " + entityIdString;
                    return error;
                }

                var valueToAssign = IsTargetProperty ? targetProperty.GetValue(target, null) : targetField.GetValue(target);
                if (valueToAssign != null && !field.ValueType.IsInstanceOfType(valueToAssign))
                {
                    error = "Object of type " + valueToAssign.GetType().FullName + " is not compatible with field " + field.FullName + " value type " + field.ValueType.FullName;
                    return error;
                }

                field.SetValue(entity.Index, valueToAssign);
                error = null;
            }
            catch (Exception e)
            {
                error = e.Message;
            }

            return error;
        }

        /// <inheritdoc/>
        public override int AddFieldsListeners(Action action)
        {
            RemoveFieldsListeners();
            var fieldPath = BindSourceProvider.FieldPath;
            var entity = Entity;
            if (entity == null) return 0;
            if (fieldPath != null && fieldPath.Relations != null)
                foreach (var relationSingle in fieldPath.Relations)
                {
                    if (entity.MetaId != relationSingle.MetaId) return eventHandlers.Count;

                    //relation field
                    eventHandlers.Add(new FieldEventHandler(entity.MetaId, relationSingle.Id, entity.Id, () => RelationAction(action)));
                    entity = relationSingle[entity.Index];
                    if (entity == null) return eventHandlers.Count;
                }

            //target field
            var field = Field;
            if (field == null) return eventHandlers.Count;
            eventHandlers.Add(new FieldEventHandler(field.MetaId, field.Id, entity.Id, action));
            return eventHandlers.Count;
        }

        private void RelationAction(Action action)
        {
            action();
            // RemoveFieldsListeners();
            AddFieldsListeners(action);
        }

        /// <summary>
        /// Source database field path (using single relations)
        /// </summary>
        public class FieldPath
        {
            private readonly BGField targetField;
            private readonly List<BGFieldRelationSingle> relations;
            private readonly string fieldNameOverride;

            /// <summary>
            /// Source field
            /// </summary>
            public BGField TargetField => targetField;

            /// <summary>
            /// Relations is any
            /// </summary>
            public List<BGFieldRelationSingle> Relations => relations;

            /// <summary>
            /// Convert source field to dotted delimited names path
            /// </summary>
            public string PathAsNames
            {
                get
                {
                    string result = null;
                    if (relations != null && relations.Count != 0)
                        foreach (var relation in relations)
                            result = result == null ? relation.Name : result + '.' + relation.Name;

                    if (!string.IsNullOrEmpty(fieldNameOverride)) result = result == null ? fieldNameOverride : result + '.' + fieldNameOverride;
                    else
                    {
                        result = targetField == null
                            ? null
                            : result == null
                                ? targetField.Name
                                : result + '.' + targetField.Name;
                    }

                    return result;
                }
            }

            /// <summary>
            /// Convert source field to IDs path
            /// </summary>
            public string PathAsIds
            {
                get
                {
                    string result = null;
                    var useSpecialField = !string.IsNullOrEmpty(fieldNameOverride);
                    var hasRelations = relations != null && relations.Count != 0;

                    //special field
                    if (useSpecialField) result = fieldNameOverride + (hasRelations ? "$" : "");

                    //relations
                    if (hasRelations)
                        foreach (var relation in relations)
                            result = result == null ? relation.Id.ToString() : result + relation.Id.ToString();

                    //target field
                    if (!useSpecialField)
                    {
                        result = targetField == null
                            ? null
                            : result == null
                                ? targetField.Id.ToString()
                                : result + targetField.Id.ToString();
                    }

                    return result;
                }
            }

            /// <summary>
            /// Custom value provider ( for special fields, like $locale )
            /// this is used from Editor- no need to optimize 
            /// </summary>
            public FieldValueProvider ValueProvider => UseSpecial ? BGLocalizationUglyHacks.DataBindingInitValueProvider(fieldNameOverride) : null;

            /// <summary>
            /// Is special field is used
            /// </summary>
            public bool UseSpecial => !string.IsNullOrEmpty(fieldNameOverride) && fieldNameOverride[0] == '$';

            public FieldPath(BGField targetField, List<BGFieldRelationSingle> relations)
            {
                this.targetField = targetField;
                this.relations = relations;
            }

            public FieldPath(string fieldNameOverride, List<BGFieldRelationSingle> relations)
            {
                this.fieldNameOverride = fieldNameOverride;
                this.relations = relations;
            }

            /// <summary>
            /// Get final row using root source entity as a source 
            /// </summary>
            public BGEntity GetTargetEntity(BGEntity sourceEntity)
            {
                if (sourceEntity == null) return null;
                // if (!string.IsNullOrEmpty(fieldNameOverride)) return sourceEntity;

                var entity = sourceEntity;
                if (relations != null)
                    foreach (var relationSingle in relations)
                    {
                        if (entity.MetaId != relationSingle.MetaId) return null;

                        entity = relationSingle[entity.Index];
                        if (entity == null) return null;
                    }

                return entity;
            }

            /// <summary>
            /// Get final meta using root source meta as a source 
            /// </summary>
            public BGMetaEntity GetTargetMeta(BGMetaEntity sourceMeta)
            {
                if (sourceMeta == null) return null;
                var meta = sourceMeta;
                if (relations != null)
                    foreach (var relationSingle in relations)
                    {
                        if (meta.Id != relationSingle.MetaId) return null;

                        meta = relationSingle.To;
                        if (meta == null) return null;
                    }

                return meta;
            }

            //=========================================== Equality members
            public override string ToString() => PathAsNames ?? "";

            protected bool Equals(FieldPath other)
            {
                if (fieldNameOverride != null && Equals(fieldNameOverride, other.fieldNameOverride)) return true;
                var e1 = Equals(targetField, other.targetField);
                var e2 = AreEquals(relations, other.relations);
                return e1 && e2;
            }

            private bool AreEquals(List<BGFieldRelationSingle> r1, List<BGFieldRelationSingle> r2)
            {
                var empty1 = r1 == null || r1.Count == 0;
                var empty2 = r2 == null || r2.Count == 0;
                if (empty1 && empty2) return true;
                if (empty1 || empty2) return false;
                if (r1.Count != r2.Count) return false;
                for (var i = 0; i < r1.Count; i++)
                {
                    var rel1 = r1[i];
                    var rel2 = r2[i];
                    if (!Equals(rel1, rel2)) return false;
                }

                return true;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((FieldPath)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = targetField != null ? targetField.GetHashCode() : 0;
                    hashCode = (hashCode * 397) ^ (relations != null ? relations.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (fieldNameOverride != null ? fieldNameOverride.GetHashCode() : 0);
                    return hashCode;
                }
            }

            public static bool operator ==(FieldPath left, FieldPath right)
            {
                return Equals(left, right);
            }

            public static bool operator !=(FieldPath left, FieldPath right)
            {
                return !Equals(left, right);
            }
        }

        /// <summary>
        /// Data container for source information
        /// </summary>
        public class FieldBindSourceProvider
        {
            private string error;

            private readonly BGDBField source;
            private string sourceFieldIdString;
            private string sourceMetaIdString;
            private string sourceEntityIdString;

            private BGMetaEntity sourceMeta;
            private BGEntity sourceEntity;
            private FieldPath fieldPath;

            /// <summary>
            /// Source table
            /// </summary>
            public BGMetaEntity SourceMeta => sourceMeta;

            /// <summary>
            /// Source row
            /// </summary>
            public BGEntity SourceEntity => sourceEntity;

            /// <summary>
            /// Final source field
            /// </summary>
            public BGField TargetField => fieldPath == null ? null : fieldPath.TargetField;

            /// <summary>
            /// Final target meta
            /// </summary>
            public BGMetaEntity TargetMeta => fieldPath == null ? sourceMeta : fieldPath.GetTargetMeta(sourceMeta);


            /// <summary>
            /// Final source entity
            /// </summary>
            public BGEntity TargetEntity => fieldPath == null ? null : fieldPath.GetTargetEntity(SourceEntity);

            /// <summary>
            /// Source path for the final field
            /// </summary>
            public FieldPath FieldPath => fieldPath;


            public FieldBindSourceProvider(BGDBField source)
            {
                this.source = source;
            }

            /// <summary>
            /// Initialize internal state using BGDBField object configuration
            /// </summary>
            public void Build()
            {
                error = null;
                sourceMeta = null;
                sourceEntity = null;
                fieldPath = null;

                sourceFieldIdString = source.fieldIdString;
                sourceMetaIdString = source.metaIdString;
                sourceEntityIdString = source.entityIdString;

                //meta
                if (SetError(string.IsNullOrEmpty(sourceMetaIdString), "meta not set")) return;
                sourceMeta = BGRepo.I[BGId.Parse(sourceMetaIdString)];
                if (SetError(sourceMeta == null, "can not find meta with id " + sourceMetaIdString)) return;

                BuildField();

                BuildEntity();
            }

            //resolve the row
            private void BuildEntity()
            {
                //source entity
                if (SetError(string.IsNullOrEmpty(sourceEntityIdString), "entity is not set")) return;
                var entityId = BGId.Parse(sourceEntityIdString);
                if (SetError(entityId.IsEmpty, "entity is not set")) return;
                sourceEntity = sourceMeta.GetEntity(entityId);
                if (SetError(sourceEntity == null, "can not find entity with id " + sourceEntityIdString)) return;

                //target entity
                if (SetError(fieldPath == null, "source field not set")) return;
                var targetEntity = fieldPath.GetTargetEntity(sourceEntity);
                if (SetError(targetEntity == null, "source entity can not be found")) return;
            }

            //resolve the field
            private void BuildField()
            {
                //field
                if (SetError(string.IsNullOrEmpty(sourceFieldIdString), "field not set")) return;

                //special field
                var fieldIdString = sourceFieldIdString;
                var isUsingSpecialField = source.IsUsingSpecialField;
                string specialField = null;
                if (isUsingSpecialField)
                {
                    var endOfSpecialField = fieldIdString.IndexOf('$', 1);
                    if (endOfSpecialField == -1 || fieldIdString.Length <= endOfSpecialField + 1)
                    {
                        fieldPath = new FieldPath(fieldIdString, null);
                        return;
                    }

                    specialField = fieldIdString.Substring(0, endOfSpecialField);
                    fieldIdString = fieldIdString.Substring(endOfSpecialField + 1);
                }

                if (SetError(fieldIdString.Length % 22 != 0, "field has invalid id")) return;

                //relations
                var targetMeta = sourceMeta;
                BGField targetField = null;
                List<BGFieldRelationSingle> relations = null;
                for (var i = 0; i < fieldIdString.Length; i += 22)
                {
                    var currentId = fieldIdString.Substring(i, 22);
                    if (!BGId.TryParse(currentId, out var id))
                    {
                        SetError("invalid field id " + sourceFieldIdString);
                        return;
                    }

                    var currentField = targetMeta.GetField(id, false);
                    if (SetError(currentField == null, "field can not be found, id= " + currentId)) return;

                    if (i + 22 != fieldIdString.Length || isUsingSpecialField)
                    {
                        if (!(currentField is BGFieldRelationSingle relation))
                        {
                            SetError("invalid field id: path field is not relation " + currentId);
                            return;
                        }

                        relations = relations ?? new List<BGFieldRelationSingle>();
                        relations.Add(relation);
                        targetMeta = relation.To;
                    }
                    else
                        //last
                        targetField = currentField;
                }

                if (isUsingSpecialField) fieldPath = new FieldPath(specialField, relations);
                else
                {
                    if (SetError(targetField == null, "source field can not be found " + sourceFieldIdString)) return;
                    fieldPath = new FieldPath(targetField, relations);
                }
            }

            //set configuration error if condition is true 
            private bool SetError(bool condition, string message)
            {
                if (!condition) return false;
                SetError(message);
                return true;
            }

            //set configuration error 
            private void SetError(string message)
            {
                var errorMessage = message == null ? null : '[' + message + ']';
                error = errorMessage;
                source.error = errorMessage;
            }

            /// <summary>
            /// is internal state is obsolete
            /// </summary>
            public bool IsObsolete
            {
                get
                {
                    if (!string.Equals(sourceFieldIdString, source.fieldIdString)
                        || !string.Equals(sourceMetaIdString, source.metaIdString)
                        || !string.Equals(sourceEntityIdString, source.entityIdString)) return true;
                    var meta = SourceMeta;
                    if (meta == null || meta.IsDeleted) return true;
                    var sourceEntity = SourceEntity;
                    if (sourceEntity == null || sourceEntity.IsDeleted || sourceEntity.Meta.IsDeleted) return true;

                    var targetField = TargetField;
                    if (targetField == null || targetField.IsDeleted || targetField.Meta.IsDeleted) return true;

                    var targetEntity = TargetEntity;
                    if (targetEntity == null || targetEntity.IsDeleted || targetEntity.Meta.IsDeleted) return true;

                    return false;
                }
            }

            /// <inheritdoc/>
            public override string ToString()
            {
                if (!string.IsNullOrEmpty(error)) return error;
                var result = "";
                result += sourceMeta.Name + '@';
                result += fieldPath.ToString();
                var targetEntity = TargetEntity;
                result += '@' + (targetEntity == null ? "[entity not found]" : targetEntity.Name);
                return result;
            }
        }

        /// <summary>
        /// Custom field value provider ( for example for localization $locale field)
        /// </summary>
        public interface FieldValueProvider
        {
            /// <summary>
            /// Get the final value  
            /// </summary>
            object GetValue(BGEntity entity);

            /// <summary>
            /// factory method 
            /// </summary>
            FieldValueProvider Create();

            /// <summary>
            /// get value type 
            /// </summary>
            Type GetValueType(BGMetaEntity meta);
        }

        public void AssignFunction(BGFBFuntion function)
        {
            if (function == null)
            {
                FunctionCode = 0;
                FunctionClass = null;
            }
            else
            {
                if (function is BGFBFuntionToString)
                {
                    FunctionCode = BGFBFuntionToString.Code;
                    FunctionClass = null;
                }
                else
                {
                    FunctionCode = BGFBFuntion.CustomCode;
                    FunctionClass = function.GetType().FullName;
                }
            }

        }
    }
}