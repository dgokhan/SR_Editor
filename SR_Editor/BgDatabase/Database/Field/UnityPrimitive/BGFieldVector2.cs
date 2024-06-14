/*
<copyright file="BGFieldVector2.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Vector2 Field 
    /// </summary>
    [FieldDescriptor(Name = "vector2", Folder = "Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerVector2")]
    public partial class BGFieldVector2 : BGFieldCachedStructA<Vector2>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 68;
        
        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        public const int SizeOfTheValue = 8;

        /// <inheritdoc />
        protected override int ValueSize => SizeOfTheValue;

        /// <inheritdoc />
        public override string Description => base.Description + ", " + Format;

        public static string Format => BGUtil.Format(" format is [x$y] (without braces)", S);

        //for new field
        public BGFieldVector2(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldVector2(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override bool AreStoredValuesEqual(Vector2 myValue, Vector2 otherValue) => AreValuesEqual(myValue, otherValue);

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex) => ValueToBytes(this[entityIndex]);

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment) => this[entityIndex] = ValueFromBytes(segment);

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
                    var startIndex = offset + (SizeOfTheValue * i);
                    StoreItems[i] = new Vector2
                    (
                        BitConverter.ToSingle(array, startIndex),
                        BitConverter.ToSingle(array, startIndex + 4)
                    );
                }
            }
            else
            {
                for (var i = 0; i < entitiesCount; i++) FromBytes(i, new ArraySegment<byte>(array, offset + (SizeOfTheValue * i), SizeOfTheValue));
            }
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex) => ValueToString(this[entityIndex]);

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value) => this[entityIndex] = ValueFromString(value);

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldVector2(meta, id, name);

        //================================================================================================
        //                                              Static
        //================================================================================================
        public static byte[] ValueToBytes(Vector2 value)
        {
            var buffer = new byte[SizeOfTheValue];
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.x), 0, buffer, 0, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.y), 0, buffer, 4, 4);
            return buffer;
        }

        public static Vector2 ValueFromBytes(ArraySegment<byte> segment)
        {
            var result = Vector2.zero;
            if (segment.Count != SizeOfTheValue) return result;

            var array = segment.Array;
            var offset = segment.Offset;

            result.x = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 0, 4));
            result.y = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 4, 4));
            return result;
        }

        public static string ValueToString(Vector2 value)
        {
            return BGUtil.Format("$$$", BGFieldFloat.ValueToString(value.x), S, BGFieldFloat.ValueToString(value.y));
        }

        public static Vector2 ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return Vector2.zero;

            var parts = value.Split(S);
            if (parts.Length != 2) throw new BGException("Can not convert $ to Vector2. " + Format, value, S);
            return new Vector2(BGFieldFloat.ValueFromString(parts[0]), BGFieldFloat.ValueFromString(parts[1]));
        }

        public static bool AreValuesEqual(Vector2 myValue, Vector2 otherValue)
        {
            return Mathf.Approximately(myValue.x, otherValue.x)
                   && Mathf.Approximately(myValue.y, otherValue.y);
        }
    }
}