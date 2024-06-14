/*
<copyright file="BGFieldRect.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Rect Field 
    /// </summary>
    [FieldDescriptor(Name = "rect", Folder = "Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerRect")]
    public partial class BGFieldRect : BGFieldCachedStructA<Rect>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 67;
        
        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        private const int Size = 16;

        /// <inheritdoc />
        protected override int ValueSize => Size;

        /// <inheritdoc />
        public override string Description => base.Description + ", " + Format;

        public static string Format => BGUtil.Format("format is [x$y$width$height] (without braces)", S, S, S);

        //for new field
        public BGFieldRect(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldRect(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override bool AreStoredValuesEqual(Rect myValue, Rect otherValue)
        {
            return Mathf.Approximately(myValue.x, otherValue.x)
                   && Mathf.Approximately(myValue.y, otherValue.y)
                   && Mathf.Approximately(myValue.width, otherValue.width)
                   && Mathf.Approximately(myValue.height, otherValue.height);
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex)
        {
            var value = this[entityIndex];

            var buffer = new byte[Size];

            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.x), 0, buffer, 0, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.y), 0, buffer, 4, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.width), 0, buffer, 8, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.height), 0, buffer, 12, 4);

            return buffer;
        }

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count != Size) ClearValue(entityIndex);
            else
            {
                var array = segment.Array;
                var offset = segment.Offset;


                var result = Rect.zero;
                result.x = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 0, 4));
                result.y = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 4, 4));
                result.width = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 8, 4));
                result.height = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 12, 4));

                this[entityIndex] = result;
            }
        }

        /// <inheritdoc />
        public void FromBytes(BGBinaryBulkRequestStruct request)
        {
            var array = request.Array;
            var offset = request.Offset;
            var entitiesCount = request.EntitiesCount;
            if (BitConverter.IsLittleEndian)
            {
                for (var i = 0; i < entitiesCount; i++)
                {
                    var startIndex = offset + (Size * i);
                    var result = Rect.zero;
                    result.x = BitConverter.ToSingle(array, startIndex);
                    result.y = BitConverter.ToSingle(array, startIndex + 4);
                    result.width = BitConverter.ToSingle(array, startIndex + 8);
                    result.height = BitConverter.ToSingle(array, startIndex + 12);

                    StoreItems[i] = result;
                }
            }
            else
            {
                for (var i = 0; i < entitiesCount; i++) FromBytes(i, new ArraySegment<byte>(array, offset + (Size * i), Size));
            }
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex)
        {
            var value = this[entityIndex];
            return BGUtil.Format("$$$$$$$", BGFieldFloat.ValueToString(value.x), S, BGFieldFloat.ValueToString(value.y), S,
                BGFieldFloat.ValueToString(value.width), S, BGFieldFloat.ValueToString(value.height));
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) ClearValue(entityIndex);
            else
            {
                var parts = value.Split(S);
                if (parts.Length != 4) throw new BGException("Can not convert $ to Rect." + Format, value);

                this[entityIndex] = new Rect(BGFieldFloat.ValueFromString(parts[0]), BGFieldFloat.ValueFromString(parts[1]),
                    BGFieldFloat.ValueFromString(parts[2]), BGFieldFloat.ValueFromString(parts[3]));
            }
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldRect(meta, id, name);
    }
}