/*
<copyright file="BGFieldEnumList.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with list of enums value (underlying type for enum is int)
    /// </summary>
    [FieldDescriptor(Name = "enumList", Folder = "Enum", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerEnumList")]
    public class BGFieldEnumList : BGFieldEnumListA<int>, BGBinaryBulkLoaderClass
    {
        public const ushort CodeType = 11;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //safe-to-use in multi-threaded environment
        //these lists are used during WRITE phase (which can not be multi-threaded) of serialization only!
        private static readonly List<byte> TempList = new List<byte>();

        //these lists are used during READ phase fromString (which can not be multi-threaded)!
        private static readonly List<string> TempStringList = new List<string>();

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldEnumList(BGMetaEntity meta, string name, Type enumType) : base(meta, name, enumType)
        {
        }

        internal BGFieldEnumList(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldEnumList(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(Enum myValue, Enum myValue2) => Equals(myValue, myValue2);

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        public override byte[] ToBytes(int entityIndex)
        {
            var list = this[entityIndex];

            var count = list?.Count ?? 0;
            if (count == 0) return null;

            TempList.Clear();
            TempList.AddRange(BGFieldInt.ValueToBytes(count));

            //do not convert to foreach
            for (var i = 0; i < list.Count; i++)
            {
                var value = list[i];
                TempList.AddRange(BGFieldInt.ValueToBytes(Convert.ToInt32(value)));
            }

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
                    var list = this[entityIndex] ?? new List<Enum>();
                    list.Clear();

                    var cursor = BGFieldInt.SizeOfTheValue;
                    for (var i = 0; i < count; i++)
                    {
                        var value = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(segment.Array, segment.Offset + cursor, BGFieldInt.SizeOfTheValue));
                        cursor += BGFieldInt.SizeOfTheValue;
                        list.Add((Enum)Enum.ToObject(EnumType, value));
                    }

                    this[entityIndex] = list;
                }
            }
        }

        /// <inheritdoc />
        public void FromBytes(BGBinaryBulkRequestClass request)
        {
            var array = request.Array;
            var requests = request.CellRequests;
            var length = requests.Length;
            for (var i = 0; i < length; i++)
            {
                var cellRequest = requests[i];
                var entityIndex = cellRequest.EntityIndex;
                var offset = cellRequest.Offset;
                try
                {
                    var count = (array[offset + 3] << 24) | (array[offset + 2] << 16) | (array[offset + 1] << 8) | array[offset];
                    if (count == 0) StoreItems[entityIndex] = default;
                    else
                    {
                        var list = StoreItems[entityIndex];
                        if (list == null) list = new List<Enum>(count);
                        else
                        {
                            list.Clear();
                            if (list.Capacity < count) list.Capacity = count;
                        }
                        StoreItems[entityIndex] = list;

                        var cursor = BGFieldInt.SizeOfTheValue;
                        for (var j = 0; j < count; j++)
                        {
                            var valueOffset = offset + cursor;
                            var value = (array[valueOffset + 3] << 24) | (array[valueOffset + 2] << 16) | (array[valueOffset + 1] << 8) | array[valueOffset];
                            cursor += BGFieldInt.SizeOfTheValue;
                            list.Add((Enum)Enum.ToObject(EnumType, value));
                        }
                    }
                }
                catch (Exception e)
                {
                    request.OnError?.Invoke(e);
                }
            }
        }

        /// <inheritdoc/>
        public override string ToString(int entityIndex)
        {
            var list = this[entityIndex];
            if (BGUtil.IsEmpty(list)) return null;

            var result = "";
            var separator = StringValueSeparator[0];
            for (var i = 0; i < list.Count; i++)
            {
                var value = list[i];
                var valueString = Enum.GetName(EnumType, value);
                if (string.IsNullOrEmpty(valueString)) continue;
                if (result.Length > 0) result += separator;
                result += valueString;
            }

            return result;
        }

        /// <inheritdoc/>
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) ClearValueNoEvent(entityIndex);
            else
            {
                var separator = StringValueSeparator[0];
                var list = this[entityIndex] ?? new List<Enum>();
                list.Clear();
                TempStringList.Clear();
                BGFieldListString.Split(TempStringList, value, separator, '\\');

                foreach (var literal in TempStringList)
                {
                    if (!Enum.IsDefined(EnumType, literal)) throw new BGException("Invalid enum value $ for enum $, field=$, entity index=$", literal, EnumType.FullName, FullName, entityIndex);
                    list.Add((Enum)Enum.Parse(EnumType, literal));
                }

                this[entityIndex] = list;
            }
        }
    }
}