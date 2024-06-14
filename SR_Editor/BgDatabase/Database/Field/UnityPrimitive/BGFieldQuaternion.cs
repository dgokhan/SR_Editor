/*
<copyright file="BGFieldQuaternion.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// quaternion Field 
    /// </summary>
    [FieldDescriptor(Name = "quaternion", Folder = "Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerQuaternion")]
    public partial class BGFieldQuaternion : BGFieldCachedStructA<Quaternion>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 64;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        public const int SizeOfTheValue = 16;

        /// <inheritdoc />
        protected override int ValueSize => SizeOfTheValue;

        /// <inheritdoc />
        public override string Description => base.Description + ", " + Format;

        public static string Format => BGUtil.Format(" format is [x$y$z$w] (without braces)", S, S, S);

        //for new field
        public BGFieldQuaternion(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldQuaternion(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override bool AreStoredValuesEqual(Quaternion myValue, Quaternion otherValue)
        {
            return Mathf.Approximately(myValue.x, otherValue.x)
                   && Mathf.Approximately(myValue.y, otherValue.y)
                   && Mathf.Approximately(myValue.z, otherValue.z)
                   && Mathf.Approximately(myValue.w, otherValue.w);
        }

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
                    StoreItems[i] = new Quaternion
                    (
                        BitConverter.ToSingle(array, startIndex),
                        BitConverter.ToSingle(array, startIndex + 4),
                        BitConverter.ToSingle(array, startIndex + 8),
                        BitConverter.ToSingle(array, startIndex + 12)
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
        //                                              Static
        //================================================================================================

        public static byte[] ValueToBytes(Quaternion value)
        {
            var buffer = new byte[SizeOfTheValue];
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.x), 0, buffer, 0, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.y), 0, buffer, 4, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.z), 0, buffer, 8, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.w), 0, buffer, 12, 4);
            return buffer;
        }

        public static Quaternion ValueFromBytes(ArraySegment<byte> segment)
        {
            var result = Quaternion.identity;
            if (segment.Count != SizeOfTheValue) return result;

            var array = segment.Array;
            var offset = segment.Offset;


            result.x = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 0, 4));
            result.y = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 4, 4));
            result.z = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 8, 4));
            result.w = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 12, 4));
            return result;
        }

        public static string ValueToString(Quaternion value)
        {
            return BGUtil.Format("$$$$$$$", BGFieldFloat.ValueToString(value.x), S, BGFieldFloat.ValueToString(value.y), S,
                BGFieldFloat.ValueToString(value.z), S, BGFieldFloat.ValueToString(value.w));
        }

        public static Quaternion ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return Quaternion.identity;

            var parts = value.Split(S);
            if (parts.Length != 4) throw new BGException("Can not convert $ to Quaternion." + Format, value);
            return new Quaternion(BGFieldFloat.ValueFromString(parts[0]), BGFieldFloat.ValueFromString(parts[1]), BGFieldFloat.ValueFromString(parts[2]), BGFieldFloat.ValueFromString(parts[3]));
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldQuaternion(meta, id, name);
    }
}