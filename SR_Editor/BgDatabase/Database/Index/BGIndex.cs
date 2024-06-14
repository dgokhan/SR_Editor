/*
<copyright file="BGIndex.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Database index has 1 field for Log(n) range scan
    /// </summary>
    public class BGIndex : BGMetaObject
    {
        public static StringComparison DefaultStringComparison = StringComparison.Ordinal;
        private readonly BGField field;
        private readonly BGIndexTypeEnum indexType;

        public BGMetaEntity Meta => field.Meta;
        public BGField Field => field;

        internal BGIndexTypeEnum IndexType => indexType;

        private BGIndexStorage store;

        /// <summary>
        /// Index name. Must be unique within meta
        /// </summary>
        public override string Name
        {
            set
            {
                if (string.Equals(Name, value)) return;
                Meta.CheckFieldName(value);

                var oldName = Name;
                //value will be checked in base method
                base.Name = value;

                Meta.IndexNameWasChanged(this, oldName);
            }
        }

        public override int Index => Meta.GetIndexIndex(Id);


        /// <summary>
        /// Full name (with meta name)
        /// </summary>
        public string FullName => Meta.Name + "." + Name;

        /// <summary>
        /// meta id
        /// </summary>
        public BGId MetaId => Meta.Id;

        //=================================================================================================================
        //                      Constructors
        //=================================================================================================================
        //for new
        public BGIndex(string name, BGField field) : this(BGId.NewId, name, field)
        {
        }

        //for existing
        private BGIndex(BGId id, string name, BGField field) : base(id, name)
        {
            this.field = field ?? throw new BGException("Field can not be empty");
            indexType = GetKeyType(field);
            field.Meta.Register(this);
        }

        //=================================================================================================================
        //                      Callbacks
        //=================================================================================================================

        public void OnCreate()
        {
        }

        public void OnDelete()
        {
        }

        //=================================================================================================================
        //                      Methods
        //=================================================================================================================
        /// <summary>
        /// Build default storage for full key
        /// </summary>
        public void Build()
        {
            if (store != null) return;
            switch (indexType)
            {
                case BGIndexTypeEnum.Bool:
                    store = new BGIndexStorage<bool>(this, (BGField<bool>)field);
                    break;
                case BGIndexTypeEnum.Byte:
                    store = new BGIndexStorage<byte>(this, (BGField<byte>)field);
                    break;
                case BGIndexTypeEnum.Decimal:
                    store = new BGIndexStorage<decimal>(this, (BGField<decimal>)field);
                    break;
                case BGIndexTypeEnum.Int:
                    store = new BGIndexStorage<int>(this, (BGField<int>)field);
                    break;
                case BGIndexTypeEnum.Long:
                    store = new BGIndexStorage<long>(this, (BGField<long>)field);
                    break;
                case BGIndexTypeEnum.Short:
                    store = new BGIndexStorage<short>(this, (BGField<short>)field);
                    break;
                case BGIndexTypeEnum.String:
                    store = new BGIndexStorage<string>(this, (BGField<string>)field);
                    break;
                case BGIndexTypeEnum.Float:
                    store = new BGIndexStorage<float>(this, (BGField<float>)field);
                    break;
                case BGIndexTypeEnum.Double:
                    store = new BGIndexStorage<double>(this, (BGField<double>)field);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(indexType), $"keyType is illegal {indexType}");
            }
        }
        
        public void MarkDirty() => store?.MarkDirty();

        /// <inheritdoc/>
        public override void Delete()
        {
            if (IsDeleted) return;
            base.Delete();

            Meta.Unregister(this);
            Unload();
        }

        /// <summary>
        /// Find the rows using provided operator with Log(n) efficiency 
        /// </summary>
        public List<BGEntity> FindEntitiesByIndex(BGIndexOperator @operator) => FindEntitiesByIndex<BGEntity>(null, @operator);

        /// <summary>
        /// Find the rows using provided operator with Log(n) efficiency 
        /// </summary>
        public List<T> FindEntitiesByIndex<T>(List<T> result, BGIndexOperator @operator) where T : BGEntity
        {
            Build();
            if (result == null) result = new List<T>();
            else result.Clear();
            @operator.GetResult(result, store);
            return result;
        }

        public int Count
        {
            get
            {
                Build();
                return store.Count;
            }
        }

        public BGRepo Repo => Meta.Repo;

        public T GetMin<T>() where T : IComparable<T>
        {
            Build();
            if(store.Count == 0) throw new Exception("Index is empty, it's not possible to calculate minimum value"); 
            if (!(store is BGIndexStorage<T> castedStore)) throw new Exception($"Wrong generic parameter type, expected type is {indexType}");
            return castedStore.Min;
        }

        public T GetMax<T>() where T : IComparable<T>
        {
            Build();
            if(store.Count == 0) throw new Exception("Index is empty, it's not possible to calculate maximum value"); 
            if (!(store is BGIndexStorage<T> castedStore)) throw new Exception($"Wrong generic parameter type, expected type is {indexType}");
            return castedStore.Max;
        }

        //=================================================================================================================
        //                      Config
        //=================================================================================================================
        /// <inheritdoc/>
        public override string ConfigToString() => null;

        /// <inheritdoc/>
        public override void ConfigFromString(string config)
        {
        }

        /// <inheritdoc/>
        public override byte[] ConfigToBytes() => null;

        /// <inheritdoc/>
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
        }

        //========================================================================================
        //              Binary
        //========================================================================================
        //reconstruct the index from binary stream
        internal static BGIndex FromBinary(BGBinaryReader binder, BGMetaEntity meta)
        {
            var version = binder.ReadInt();
            switch (version)
            {
                case 1:
                case 2:
                {
                    var indexId = binder.ReadId();
                    var keyName = binder.ReadString();
                    var fieldId = binder.ReadId();
                    var field = meta.GetField(fieldId, false);
                    if (field == null) return null;

                    var index = Create(indexId, keyName, field);
                    
                    //v2
                    if (version >= 2)
                    {
                        index.Comment = binder.ReadString();
                        index.ControllerType = binder.ReadString();
                    }
                    
                    return index;
                }
                default:
                {
                    throw new BGException("Can not read key from binary array: unsupported version $", version);
                }
            }
        }

        //convert the index to binary stream
        internal static void ToBinary(BGBinaryWriter builder, BGIndex index)
        {
            //version
            builder.AddInt(2);
            builder.AddId(index.Id);
            builder.AddString(index.Name);
            builder.AddId(index.Field?.Id ?? BGId.Empty);
            
            //v2
            builder.AddString(index.Comment);
            builder.AddString(index.ControllerType);
        }


        //=================================================================================================================
        //                      Factory
        //=================================================================================================================
        /// <summary>
        /// Create a index 
        /// </summary>
        public static BGIndex Create(BGId id, string name, BGField field)
        {
            var index = new BGIndex(id, name, field);
            return index;
        }

        //=================================================================================================================
        //                      Utility
        //=================================================================================================================
        //get field type as enum value
        private static BGIndexTypeEnum GetKeyType(BGField field)
        {
            switch (field)
            {
                case BGFieldBool _:
                    return BGIndexTypeEnum.Bool;
                case BGFieldByte _:
                    return BGIndexTypeEnum.Byte;
                case BGFieldDecimal _:
                    return BGIndexTypeEnum.Decimal;
                case BGFieldInt _:
                    return BGIndexTypeEnum.Int;
                case BGFieldLong _:
                    return BGIndexTypeEnum.Long;
                case BGFieldShort _:
                    return BGIndexTypeEnum.Short;
                case BGFieldString _:
                case BGFieldText _:
                    return BGIndexTypeEnum.String;
                case BGFieldFloat _:
                    return BGIndexTypeEnum.Float;
                case BGFieldDouble _:
                    return BGIndexTypeEnum.Double;
                default:
                    throw new BGException("Field $ can not be used as a index field!", field.Name);
            }
        }

        /// <summary>
        /// Does the provided field is supported as index field?
        /// </summary>
        public static bool IsFieldSupportedAsIndex(BGField field)
        {
            switch (field)
            {
                case BGFieldBool _:
                case BGFieldByte _:
                case BGFieldDecimal _:
                case BGFieldInt _:
                case BGFieldLong _:
                case BGFieldShort _:
                case BGFieldString _:
                case BGFieldText _:
                case BGFieldFloat _:
                case BGFieldDouble _:
                {
                    return true;
                }
                default:
                    return false;
            }
        }

        public override string ToString() => "Index [id:" + Id + ", name:" + Name + ", field:" + field?.Name + "]";

        /// <summary>
        /// Clone this index to provided table
        /// </summary>
        public BGIndex CloneTo(BGMetaEntity meta)
        {
            var cloneField = meta.GetField(field?.Id ?? BGId.Empty, false);
            if (cloneField == null) return null;
            var clone = new BGIndex(Id, Name, cloneField)
            {
                Comment = Comment,
                ControllerType = ControllerType
            };
            return clone;
        }

        /// <summary>
        /// compare indexes parameters
        /// </summary>
        public bool DeepEqual(BGIndex t2)
        {
            if (!string.Equals(ConfigToString(), t2.ConfigToString())) return false;
            if (!string.Equals(Name, t2.Name)) return false;
            if (!string.Equals(Comment, t2.Comment)) return false;
            if (!string.Equals(ControllerType, t2.ControllerType)) return false;
            if (!Equals(field, t2.field)) return false;

            return true;
        }
    }
}