/*
<copyright file="BGFieldVector3.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Vector3 Field 
    /// </summary>
    [FieldDescriptor(Name = "vector3", Folder = "Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerVector3")]
    public partial class BGFieldVector3 : BGFieldCachedStructA<Vector3>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 69;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        public const int SizeOfTheValue = 12;

        /// <inheritdoc />
        protected override int ValueSize => SizeOfTheValue;

        /// <inheritdoc />
        public override string Description => base.Description + ", " + Format;

        public static string Format => BGUtil.Format(" format is [x$y$z] (without braces)", S, S);

        //for new field
        public BGFieldVector3(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldVector3(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override bool AreStoredValuesEqual(Vector3 myValue, Vector3 otherValue) => AreValuesEqual(myValue, otherValue);

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
                    StoreItems[i] = new Vector3
                    (
                        BitConverter.ToSingle(array, startIndex),
                        BitConverter.ToSingle(array, startIndex + 4),
                        BitConverter.ToSingle(array, startIndex + 8)
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldVector3(meta, id, name);

        //================================================================================================
        //                                              Static
        //================================================================================================

        public static byte[] ValueToBytes(Vector3 value)
        {
            var buffer = new byte[SizeOfTheValue];
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.x), 0, buffer, 0, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.y), 0, buffer, 4, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.z), 0, buffer, 8, 4);
            return buffer;
        }

        public static Vector3 ValueFromBytes(ArraySegment<byte> segment)
        {
            var result = Vector3.zero;
            if (segment.Count != SizeOfTheValue) return result;

            var array = segment.Array;
            var offset = segment.Offset;


            result.x = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 0, 4));
            result.y = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 4, 4));
            result.z = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 8, 4));
            return result;
        }

        public static string ValueToString(Vector3 value)
        {
            return BGUtil.Format("$$$$$", BGFieldFloat.ValueToString(value.x), S, BGFieldFloat.ValueToString(value.y), S, BGFieldFloat.ValueToString(value.z));
        }

        public static Vector3 ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return Vector3.zero;

            var parts = value.Split(S);
            if (parts.Length != 3) throw new BGException("Can not convert $ to Vector3." + Format, value);
            return new Vector3(BGFieldFloat.ValueFromString(parts[0]), BGFieldFloat.ValueFromString(parts[1]), BGFieldFloat.ValueFromString(parts[2]));
        }

        public static bool AreValuesEqual(Vector3 myValue, Vector3 otherValue)
        {
            return Mathf.Approximately(myValue.x, otherValue.x)
                   && Mathf.Approximately(myValue.y, otherValue.y)
                   && Mathf.Approximately(myValue.z, otherValue.z);
        }
    }
}