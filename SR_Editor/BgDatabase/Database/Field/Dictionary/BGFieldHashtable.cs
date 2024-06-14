/*
<copyright file="BGFieldHashtable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// hashtable field
    /// </summary>
    [FieldDescriptor(Name = "hashtable", Folder = "Dictionary", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerHashtable")]
    public class BGFieldHashtable : BGFieldCachedClassA<Hashtable>, BGFieldWithCustomConfigI
    {
        public const ushort CodeType = 8;
        public override ushort TypeCode => CodeType;

        public const string DefaultDelegateMetaName = "m";
        public const string DefaultDelegateKeyFieldName = "k";
        public const string DefaultDelegateValueFieldName = "v";

        private const char EQ = '=';

        //---- static
        //safe-to-use in multi-threaded environment
        //these lists are used during WRITE phase (which can not be multi-threaded) of serialization only! 
        private static readonly List<byte> TempList = new List<byte>();
        private static readonly List<byte> KeysList = new List<byte>();
        private static readonly List<byte> ValuesList = new List<byte>();

        //---- fields
        private string keyDelegateType;
        private byte[] keyDelegateConfig;
        private string valueDelegateType;
        private byte[] valueDelegateConfig;

        private BGRepo repoDelegate;
        private BGMetaRow metaDelegate;
        private BGField keyDelegate;
        private BGField valueDelegate;


        private BGRepo RepoDelegate => repoDelegate ?? (repoDelegate = new BGRepo());

        private BGMetaRow MetaDelegate
        {
            get
            {
                if (metaDelegate != null) return metaDelegate;
                metaDelegate = new BGMetaRow(RepoDelegate, DefaultDelegateMetaName);
                //default single entity
                metaDelegate.NewEntity();
                return metaDelegate;
            }
        }

        private BGField KeyDelegate
        {
            get
            {
                if (keyDelegate != null) return keyDelegate;
                if (MetaDelegate.CountFields > 1) keyDelegate = MetaDelegate.GetField(DefaultDelegateKeyFieldName, false);
                if (keyDelegate == null) keyDelegate = Create(MetaDelegate, keyDelegateType, DefaultDelegateKeyFieldName, keyDelegateConfig);
                return keyDelegate;
            }
        }

        private BGField ValueDelegate
        {
            get
            {
                if (valueDelegate != null) return valueDelegate;
                if (MetaDelegate.CountFields > 1) valueDelegate = MetaDelegate.GetField(DefaultDelegateValueFieldName, false);
                if (valueDelegate == null) valueDelegate = Create(MetaDelegate, valueDelegateType, DefaultDelegateValueFieldName, valueDelegateConfig);
                return valueDelegate;
            }
        }

        public string KeyDelegateType => keyDelegateType;

        public string ValueDelegateType => valueDelegateType;

        public override string Description => "Field [hashtable(key=" + KeyDelegate.ValueType.FullName + ",value=" + ValueDelegate.ValueType.FullName + ")]";

        //------ constructors
        //for new field
        public BGFieldHashtable(BGMetaEntity meta, string name, Type keyFieldType, Type valueFieldType) : base(meta, name)
        {
            BGException e = null;
            if (keyFieldType == null) e = new BGException("keyFieldType can not be null");
            else if (valueFieldType == null) e = new BGException("valueFieldType can not be null");
            else if (!IsFieldSupportedAsKey(keyFieldType)) e = new BGException("$ field is not supported as key field", keyFieldType.FullName);
            else if (!IsFieldSupportedAsValue(valueFieldType)) e = new BGException("$ field is not supported as value field", valueFieldType.FullName);
            else
            {
                //only fields with default constructor is supported 
                var types = new[] { typeof(BGMetaEntity), typeof(string) };
                var keyConstructor = keyFieldType.GetConstructor(types);
                if (keyConstructor == null)
                    e = new BGException("$ field is not supported by this constructor, cause it requires custom configuration." +
                                        "Use BGFieldHashtable(BGMetaEntity meta, string name, BGField keyField, BGField valueField)", keyFieldType.FullName);
                else
                {
                    var valueConstructor = valueFieldType.GetConstructor(types);
                    if (valueConstructor == null)
                        e = new BGException("$ field is not supported by this constructor, cause it requires custom configuration." +
                                            "Use BGFieldHashtable(BGMetaEntity meta, string name, BGField keyField, BGField valueField)", valueFieldType.FullName);
                    else
                    {
                        keyDelegateType = keyFieldType.AssemblyQualifiedName;
                        valueDelegateType = valueFieldType.AssemblyQualifiedName;
                    }
                }
            }

            if (e != null)
            {
                Unregister();
                throw e;
            }
        }

        public BGFieldHashtable(BGMetaEntity meta, string name, BGField keyField, BGField valueField) : base(meta, name)
        {
            BGException e = null;
            if (keyField == null) e = new BGException("keyField can not be null");
            else if (valueField == null) e = new BGException("valueField can not be null");
            else if (!IsFieldSupportedAsKey(keyField.GetType())) e = new BGException("$ field is not supported as key field", keyField.GetType().FullName);
            else if (!IsFieldSupportedAsValue(valueField.GetType())) e = new BGException("$ field is not supported as value field", valueField.GetType().FullName);
            else
            {
                keyDelegateType = keyField.GetType().AssemblyQualifiedName;
                keyDelegateConfig = keyField.ConfigToBytes();
                valueDelegateType = valueField.GetType().AssemblyQualifiedName;
                valueDelegateConfig = valueField.ConfigToBytes();
            }

            if (e != null)
            {
                Unregister();
                throw e;
            }
        }

        //for existing field
        protected internal BGFieldHashtable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldHashtable(meta, id, name);

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        public override byte[] ToBytes(int entityIndex)
        {
            var value = this[entityIndex];
            if (value == null || value.Count == 0) return null;

            var keyDelegate = KeyDelegate;
            var valueDelegate = ValueDelegate;

            // var KeysList = new List<byte>(keyDelegate.ConstantSize > 0 ? keyDelegate.ConstantSize * value.Count : value.Count * 4);
            // var ValuesList = new List<byte>(valueDelegate.ConstantSize > 0 ? valueDelegate.ConstantSize * value.Count : value.Count * 4);
            KeysList.Clear();
            ValuesList.Clear();
            TempList.Clear();

            var count = 0;
            foreach (DictionaryEntry entry in value)
            {
                var entryKey = entry.Key;
                var entryValue = entry.Value;

                if (!IsSupported(keyDelegate, valueDelegate, entryKey, entryValue)) continue;

                byte[] serializedKey;
                byte[] serializedValue;

                try
                {
                    //----- key
                    serializedKey = ToBytes(keyDelegate, entryKey);
                    //----- value
                    serializedValue = ToBytes(valueDelegate, entryValue);
                }
                catch
                {
                    continue;
                }

                if (serializedKey == null || serializedValue == null) continue;
                count++;
                KeysList.AddRange(serializedKey);
                ValuesList.AddRange(serializedValue);
            }

            if (count == 0) return null;

            // var TempList = new List<byte>(4 + KeysList.Count + ValuesList.Count);
            TempList.AddRange(BGFieldInt.ValueToBytes(count));
            TempList.AddRange(KeysList);
            TempList.AddRange(ValuesList);

            KeysList.Clear();
            ValuesList.Clear();
            var result = TempList.ToArray();
            TempList.Clear();

            return result;
        }

        /// <inheritdoc/>
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count < 4) ClearValueNoEvent(entityIndex);
            else
            {
                var count = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(segment.Array, segment.Offset, BGFieldInt.SizeOfTheValue));
                if (count == 0) ClearValueNoEvent(entityIndex);
                else
                {
                    var hashtable = this[entityIndex];
                    if (hashtable == null) this[entityIndex] = hashtable = new Hashtable();
                    else hashtable.Clear();

                    var keyDelegate = KeyDelegate;
                    var valueDelegate = ValueDelegate;

                    var keys = new object[count];
                    var values = new object[count];

                    var cursor = segment.Offset + BGFieldInt.SizeOfTheValue;
                    //----------keys
                    for (var i = 0; i < count; i++) keys[i] = FromBytes(ref cursor, keyDelegate, segment.Array);

                    //----------values
                    for (var i = 0; i < count; i++) values[i] = FromBytes(ref cursor, valueDelegate, segment.Array);

                    //---- final pass
                    for (var i = 0; i < count; i++)
                    {
                        var key = keys[i];
                        var value = values[i];
                        if (key == null || value == null) continue;
                        hashtable[key] = value;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override string ToString(int entityIndex)
        {
            var hashtable = this[entityIndex];
            if (hashtable == null || hashtable.Count == 0) return null;

            var keyDelegate = KeyDelegate;
            var valueDelegate = ValueDelegate;

            var result = "";
            foreach (DictionaryEntry entry in hashtable)
            {
                var entryKey = entry.Key;
                var entryValue = entry.Value;
                if (!IsSupported(keyDelegate, valueDelegate, entryKey, entryValue)) continue;

                string keyAsString;
                string valueAsString;
                try
                {
                    keyAsString = ToString(keyDelegate, entryKey);
                    valueAsString = ToString(valueDelegate, entryValue);
                }
                catch
                {
                    continue;
                }

                if (keyAsString == null || valueAsString == null) continue;
                if (result.Length != 0) result += A;
                result += keyAsString + EQ + valueAsString;
            }

            return result;
        }

        /// <inheritdoc/>
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) ClearValueNoEvent(entityIndex);
            else
            {
                var list = new List<string>();
                BGFieldListString.Split(list, value, A, '\\', true);
                if (list.Count == 0) ClearValueNoEvent(entityIndex);
                else
                {
                    var keyDelegate = KeyDelegate;
                    var valueDelegate = ValueDelegate;

                    var hashtable = new Hashtable();
                    var tokenList = new List<string>();
                    for (var i = 0; i < list.Count; i++)
                    {
                        var token = list[i];
                        tokenList.Clear();
                        BGFieldListString.Split(tokenList, token, EQ, '\\');
                        if (tokenList.Count == 2)
                        {
                            object keyV;
                            object valueV;
                            var keyAsString = tokenList[0];
                            var valueAsString = tokenList[1];
                            try
                            {
                                keyV = FromString(keyDelegate, keyAsString);
                                valueV = FromString(valueDelegate, valueAsString);
                            }
                            catch
                            {
                                continue;
                            }

                            if (keyV == null || valueV == null) continue;

                            hashtable[keyV] = valueV;
                        }
                    }

                    if (hashtable.Count == 0) ClearValueNoEvent(entityIndex);
                    else this[entityIndex] = hashtable;
                }
            }
        }


        public static string ToString(BGField delegateField, object value)
        {
            delegateField.ClearValue(0);
            delegateField.SetValue(0, value);
            return EscapeString(delegateField.ToString(0));
        }

        public static object FromString(BGField delegateField, string value)
        {
            delegateField.ClearValue(0);
            delegateField.FromString(0, value);
            return delegateField.GetValue(0);
        }

        private static string EscapeString(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            //replace all special chars, like escaping char, equal char, separating key=value and | char, separating pairs key1=value1|key2=value2
            return value.Replace(@"\", @"\\").Replace("" + EQ, @"\" + EQ).Replace("" + A, @"\" + A);
        }

        public static byte[] ToBytes(BGField delegateField, object value)
        {
            delegateField.ClearValue(0);
            delegateField.SetValue(0, value);
            if (delegateField.ConstantSize > 0) return delegateField.ToBytes(0);

            //not constant size
            var valBytes = delegateField.ToBytes(0);
            if (valBytes == null || valBytes.Length == 0) return BGFieldInt.ValueToBytes(0);

            var ret = new byte[valBytes.Length + BGFieldInt.SizeOfTheValue];
            var sizeBytes = BGFieldInt.ValueToBytes(valBytes.Length);
            sizeBytes.CopyTo(ret, 0);
            valBytes.CopyTo(ret, BGFieldInt.SizeOfTheValue);
            return ret;
        }

        public static object FromBytes(ref int cursor, BGField delegateField, byte[] array)
        {
            delegateField.ClearValue(0);
            var constantSize = delegateField.ConstantSize;
            if (constantSize > 0)
            {
                //constant size
                delegateField.FromBytes(0, new ArraySegment<byte>(array, cursor, constantSize));
                cursor += constantSize;
            }
            else
            {
                //not constant size
                var length = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(array, cursor, BGFieldInt.SizeOfTheValue));
                cursor += BGFieldInt.SizeOfTheValue;

                delegateField.FromBytes(0, new ArraySegment<byte>(array, cursor, length));
                cursor += length;
            }

            return delegateField.GetValue(0);
        }

        //================================================================================================
        //                                              Configuration
        //================================================================================================
        /// <inheritdoc />
        public override string ConfigToString()
        {
            return JsonUtility.ToJson(new JsonConfig
            {
                KeyType = keyDelegateType, KeyConfig = KeyDelegate.ConfigToString(),
                ValueType = valueDelegateType, ValueConfig = ValueDelegate.ConfigToString()
            });
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var jsonSettings = JsonUtility.FromJson<JsonConfig>(config);
            keyDelegateType = jsonSettings.KeyType;
            valueDelegateType = jsonSettings.ValueType;

            KeyDelegate.ConfigFromString(jsonSettings.KeyConfig);
            ValueDelegate.ConfigFromString(jsonSettings.ValueConfig);
        }

        [Serializable]
        private struct JsonConfig
        {
            public string KeyType;
            public string KeyConfig;
            public string ValueType;
            public string ValueConfig;
        }

        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(64);
            //version
            writer.AddInt(1);
            //keyType
            writer.AddString(keyDelegateType);
            //valueType
            writer.AddString(valueDelegateType);

            writer.AddByteArray(keyDelegateConfig);
            writer.AddByteArray(valueDelegateConfig);

            return writer.ToArray();
        }

        /// <inheritdoc />
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    keyDelegateType = reader.ReadString();
                    valueDelegateType = reader.ReadString();
                    keyDelegateConfig = BGUtil.ToArray(reader.ReadByteArray());
                    valueDelegateConfig = BGUtil.ToArray(reader.ReadByteArray());
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        //================================================================================================
        //                                              Support
        //================================================================================================
        //safe-to-use in multi-threaded environment 
        private static List<Type> allKeyFields;
        private static List<Type> allValueFields;

        //used by test code only?
        public static List<Type> AllKeyFields
        {
            get
            {
                if (allKeyFields != null) return allKeyFields;
                allKeyFields = FieldTypes.FindAll(type =>
                {
                    if (!BGUtil.HasAttribute<FieldDescriptor>(type, false)) return false;
                    if (string.IsNullOrEmpty(BGUtil.GetAttribute<FieldDescriptor>(type).Name)) return false;
                    return IsFieldSupportedAsKey(type);
                });
                return allKeyFields;
            }
        }

        //used by test code only?
        public static List<Type> AllValueFields
        {
            get
            {
                if (allValueFields != null) return allValueFields;
                allValueFields = FieldTypes.FindAll(type =>
                {
                    if (!BGUtil.HasAttribute<FieldDescriptor>(type, false)) return false;
                    if (string.IsNullOrEmpty(BGUtil.GetAttribute<FieldDescriptor>(type).Name)) return false;
                    return IsFieldSupportedAsValue(type);
                });
                return allValueFields;
            }
        }

        private static readonly HashSet<Type> SupportedKeyFields = new HashSet<Type>()
        {
            typeof(BGFieldBool),
            typeof(BGFieldByte),
            typeof(BGFieldGuid),
            typeof(BGFieldInt),
            typeof(BGFieldLong),
            typeof(BGFieldShort),
            typeof(BGFieldString),
            typeof(BGFieldText),
            typeof(BGFieldId),
            typeof(BGFieldEnum),
            typeof(BGFieldEnumByte),
            typeof(BGFieldEnumShort)
        };

        public static bool IsFieldSupportedAsKey(Type fieldType) => SupportedKeyFields.Contains(fieldType);

        //this is a bad method, cause supported fields should be enumerated, rather than excluding from all fields
        public static bool IsFieldSupportedAsValue(Type fieldType)
        {
            //should be field
            if (!typeof(BGField).IsAssignableFrom(fieldType)) return false;
            //no assets
            //no relations
            if (typeof(BGAbstractRelationI).IsAssignableFrom(fieldType)) return false;
            //no references
            //no nullable structs (cause null values are not supported)
            if (typeof(BGStructNullableI).IsAssignableFrom(fieldType)) return false;
            //no hashmap
            if (typeof(BGFieldHashtable) == fieldType) return false;
            //no lists
            // if (typeof(BGListI).IsAssignableFrom(fieldType)) return false;
            //no enum list
            if (typeof(BGFieldEnumList) == fieldType) return false;
            //no arrays (do not pass QA tests because of empty array)
            if (typeof(BGArrayI).IsAssignableFrom(fieldType)) return false;


            //no class fields
            //no animationCurve
            if (fieldType.FullName.StartsWith("BansheeGz.BGDatabase.BGFieldAnimationCurve")) return false;
            if (typeof(BGFieldGradient) == fieldType) return false;

            //no meta ref
            if (typeof(BGFieldMetaReference) == fieldType) return false;

            return true;
        }

        public static bool IsSupported(BGField keyDelegateField, BGField valueDelegateField, object key, object value) => IsSupported(keyDelegateField, key) && IsSupported(valueDelegateField, value);

        public static bool IsSupported(BGField delegateField, object value)
        {
            if (value == null) return false;

            if (delegateField.ValueType == value.GetType()) return true;

            switch (delegateField)
            {
                case BGFieldEnumI _:
                    return true;
                case BGStructNullableI _:
                {
                    var genericArguments = delegateField.ValueType.GetGenericArguments();
                    var genericType = genericArguments.Length > 0 ? genericArguments[0] : null;
                    if (genericType == value.GetType()) return true;
                    break;
                }
            }

            return false;
        }

        /// <inheritdoc/>
        public override bool AreStoredValuesEqual(BGField field, int myEntityIndex, int otherEntityIndex)
        {
            if (!(field is BGFieldHashtable typed)) return false;

            var valueList = this[myEntityIndex];
            var valueList2 = typed[otherEntityIndex];

            var isEmpty = IsEmpty(valueList);
            var isEmpty2 = IsEmpty(valueList2);

            if (isEmpty && isEmpty2) return true;
            if (isEmpty || isEmpty2) return false;

            if (valueList.Count != valueList2.Count) return false;

            var valueDelegate = ValueDelegate;
            var valueDelegate2 = typed.ValueDelegate;

            if (valueDelegate.GetType() != valueDelegate2.GetType()) return false;

            foreach (DictionaryEntry entry in valueList)
            {
                var value2 = valueList2[entry.Key];
                if (entry.Value == null && value2 == null) continue;
                if (entry.Value == null || value2 == null) return false;

                valueDelegate.SetValue(0, entry.Value);
                valueDelegate2.SetValue(0, value2);
                if (!valueDelegate.AreStoredValuesEqual(valueDelegate2, 0, 0)) return false;

//                if (!Equals(entry.Value, value2)) return false; <-- this does not work!
            }

            return true;
        }

        //================================================================================================
        //                                              Misc
        //================================================================================================
        public static bool IsEmpty(Hashtable list) => list == null || list.Count == 0;

        private static BGField Create(BGMetaEntity meta, string type, string name, byte[] config)
        {
            return BGField.Create(meta, type, BGId.NewId, name, config == null ? new ArraySegment<byte>(Array.Empty<byte>()) : new ArraySegment<byte>(config), false, null, null, false);
        }

        /// <inheritdoc/>
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            if (fromEntityIndex == -1 || fromField.IsDeleted) return;
            var index = Meta.FindEntityIndex(toEntityId);
            if (index == -1) return;

            var otherField = (BGFieldHashtable)fromField;
            var otherValue = otherField[fromEntityIndex];
            if (otherValue == null || otherValue.Count == 0) ClearValueNoEvent(index);
            else
            {
                var keyDelegate = KeyDelegate;
                var valueDelegate = ValueDelegate;

                var keyIsValue = keyDelegate.ValueType.IsValueType;
                var keyIsCloneable = !keyIsValue && typeof(ICloneable).IsAssignableFrom(keyDelegate.ValueType);
                var valueIsValue = valueDelegate.ValueType.IsValueType;
                var valueIsCloneable = !valueIsValue && typeof(ICloneable).IsAssignableFrom(valueDelegate.ValueType);

                var result = new Hashtable(otherValue.Count);
                foreach (DictionaryEntry entry in otherValue) result[Clone(keyIsValue, keyIsCloneable, entry.Key)] = Clone(valueIsValue, valueIsCloneable, entry.Value);

                StoreSet(index, result);
            }
        }

        private static object Clone(bool isValue, bool isCloneable, object obj)
        {
            //is this method correct?
            object clone;
            if (isValue) clone = obj;
            else if (isCloneable) clone = ((ICloneable)obj).Clone();
            else clone = BGUtil.Clone(obj);
            return clone;
        }

        public void CreateKeyValueDelegates(out BGField key, out BGField value)
        {
            var repo = new BGRepo();
            var meta = new BGMetaRow(repo, DefaultDelegateMetaName);
            meta.NewEntity();
            key = Create(meta, keyDelegateType, DefaultDelegateKeyFieldName, keyDelegateConfig);
            value = Create(meta, valueDelegateType, DefaultDelegateValueFieldName, valueDelegateConfig);
        }
    }
}