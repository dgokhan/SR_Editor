/*
<copyright file="BGFieldColor.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Color Field 
    /// </summary>
    [FieldDescriptor(Name = "color", Folder = "Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerColor")]
    public partial class BGFieldColor : BGFieldCachedStructA<Color>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 61;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        public const int SizeOfTheValue = 16;

        /// <inheritdoc />
        protected override int ValueSize => SizeOfTheValue;

        /// <inheritdoc />
        public override string Description => base.Description + ", " + Format;

        public static string Format => BGUtil.Format(" format is [r$g$b$a] (without braces)", S, S, S);


        //for new field
        public BGFieldColor(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldColor(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override bool AreStoredValuesEqual(Color myValue, Color otherValue)
        {
            return Mathf.Approximately(myValue.r, otherValue.r)
                   && Mathf.Approximately(myValue.g, otherValue.g)
                   && Mathf.Approximately(myValue.b, otherValue.b)
                   && Mathf.Approximately(myValue.a, otherValue.a);
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
                    StoreItems[i] = new Color
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

        public static byte[] ValueToBytes(Color value)
        {
            var buffer = new byte[SizeOfTheValue];
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.r), 0, buffer, 0, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.g), 0, buffer, 4, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.b), 0, buffer, 8, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(value.a), 0, buffer, 12, 4);
            return buffer;
        }

        public static Color ValueFromBytes(ArraySegment<byte> segment)
        {
            var result = Color.clear;
            if (segment.Count != SizeOfTheValue) return result;

            var array = segment.Array;
            var offset = segment.Offset;


            result.r = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 0, 4));
            result.g = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 4, 4));
            result.b = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 8, 4));
            result.a = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 12, 4));
            return result;
        }

        public static string ValueToString(Color value)
        {
            return BGUtil.Format("$$$$$$$", BGFieldFloat.ValueToString(value.r), S, BGFieldFloat.ValueToString(value.g), S,
                BGFieldFloat.ValueToString(value.b), S, BGFieldFloat.ValueToString(value.a));
        }

        public static Color ValueFromString(string value)
        {
            if (string.IsNullOrEmpty(value)) return Color.clear;

            var parts = value.Split(S);
            if (parts.Length != 4) throw new BGException("Can not convert $ to color." + Format, value);
            var color = new Color(BGFieldFloat.ValueFromString(parts[0]), BGFieldFloat.ValueFromString(parts[1]), BGFieldFloat.ValueFromString(parts[2]), BGFieldFloat.ValueFromString(parts[3]));
            return color;
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldColor(meta, id, name);
    }
}