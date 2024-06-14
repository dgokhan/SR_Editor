/*
<copyright file="BGFieldListString.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with list of strings value
    /// </summary>
    [FieldDescriptor(Name = "listString", Folder = "List/Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerListString")]
    public partial class BGFieldListString : BGFieldCachedClassListA<string>, BGBinaryBulkLoaderClass
    {
        public const ushort CodeType = 19;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //safe-to-use in multi-threaded environment
        //these lists are used during WRITE phase (which can not be multi-threaded) of serialization only!
        private static readonly List<byte> TempList = new List<byte>();

        //for new field
        public BGFieldListString(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldListString(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

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
                var valueBytes = BGFieldStringA.ValueToBytes(value);
                if (valueBytes == null) TempList.AddRange(BGFieldInt.ValueToBytes(0));
                else
                {
                    TempList.AddRange(BGFieldInt.ValueToBytes(valueBytes.Length));
                    TempList.AddRange(valueBytes);
                }
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
                    var list = this[entityIndex] ?? new List<string>();
                    list.Clear();

                    var cursor = BGFieldInt.SizeOfTheValue;
                    for (var i = 0; i < count; i++)
                    {
                        var length = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(segment.Array, segment.Offset + cursor, BGFieldInt.SizeOfTheValue));
                        cursor += BGFieldInt.SizeOfTheValue;
                        if (length == 0) list.Add("");
                        else
                        {
                            list.Add(Encoding.UTF8.GetString(segment.Array, segment.Offset + cursor, length));
                            cursor += length;
                        }
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
            var encoding = Encoding.UTF8;
            for (var i = 0; i < length; i++)
            {
                var cellRequest = requests[i];
                var entityIndex = cellRequest.EntityIndex;
                var offset = cellRequest.Offset;
                try
                {
                    var count = (array[offset + 3] << 24) | (array[offset + 2] << 16) | (array[offset + 1] << 8) | array[offset];
                    if (count == 0) StoreItems[entityIndex] = null;
                    else
                    {
                        var list = StoreItems[entityIndex];
                        if (list == null) list = new List<string>(count);
                        else
                        {
                            list.Clear();
                            if (list.Capacity < count) list.Capacity = count;
                        }
                        StoreItems[entityIndex] = list;

                        var cursor = offset + BGFieldInt.SizeOfTheValue;
                        for (var j = 0; j < count; j++)
                        {
                            var valueLength =  (array[cursor + 3] << 24) | (array[cursor + 2] << 16) | (array[cursor + 1] << 8) | array[cursor];
                            cursor += BGFieldInt.SizeOfTheValue;
                            if (valueLength == 0) list.Add("");
                            else
                            {
                                list.Add(encoding.GetString(array, cursor, valueLength));
                                cursor += valueLength;
                            }
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
                if (string.IsNullOrEmpty(value)) continue;
                if (result.Length > 0) result += separator;
                result += value.Replace(@"\", @"\\").Replace("" + separator, @"\" + separator);
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
                var list = this[entityIndex] ?? new List<string>();
                list.Clear();

                Split(list, value, separator, '\\');
                this[entityIndex] = list;
            }
        }

        //original here https://stackoverflow.com/questions/667803/what-is-the-best-algorithm-for-arbitrary-delimiter-escape-character-processing
        public static void Split(List<string> list, string text, char delimiter, char escapeChar, bool keepEscape = false)
        {
            var currentlyEscaped = false;
            var fragment = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (currentlyEscaped)
                {
                    if (keepEscape) fragment.Append(escapeChar);
                    fragment.Append(c);
                    currentlyEscaped = false;
                }
                else
                {
                    if (c == delimiter)
                    {
                        if (fragment.Length > 0)
                        {
                            list.Add(fragment.ToString());
                            fragment.Remove(0, fragment.Length);
                        }
                    }
                    else if (c == escapeChar) currentlyEscaped = true;
                    else fragment.Append(c);
                }
            }

            if (fragment.Length > 0) list.Add(fragment.ToString());
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldListString(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(string myValue, string myValue2) => string.Equals(myValue, myValue2);
    }
}