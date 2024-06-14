/*
<copyright file="BGFieldBounds.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// bounds Field 
    /// </summary>
    [FieldDescriptor(Name = "bounds", Folder = "Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerBounds")]
    public partial class BGFieldBounds : BGFieldCachedStructA<Bounds>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 60;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        private const int Size = 24;

        /// <inheritdoc />
        protected override int ValueSize => Size;

        /// <inheritdoc />
        public override string Description => base.Description + ", " + Format;

        public static string Format => BGUtil.Format(" format is [center.x$center.y$center.z$extends.x$extends.y$extends.z] (without braces)", S, S, S, S, S);

        //for new field
        public BGFieldBounds(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldBounds(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override bool AreStoredValuesEqual(Bounds myValue, Bounds otherValue)
        {
            return BGFieldVector3.AreValuesEqual(myValue.center, otherValue.center)
                   && BGFieldVector3.AreValuesEqual(myValue.extents, otherValue.extents);
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex)
        {
            var value = this[entityIndex];
            var center = value.center;
            var extends = value.extents;


            var buffer = new byte[Size];

            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(center.x), 0, buffer, 0, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(center.y), 0, buffer, 4, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(center.z), 0, buffer, 8, 4);

            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(extends.x), 0, buffer, 12, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(extends.y), 0, buffer, 16, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(extends.z), 0, buffer, 20, 4);

            return buffer;
        }

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count != Size) ClearValueNoEvent(entityIndex);
            else
            {
                var array = segment.Array;
                var offset = segment.Offset;

                var center = Vector3.zero;
                center.x = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 0, 4));
                center.y = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 4, 4));
                center.z = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 8, 4));

                var extends = Vector3.zero;
                extends.x = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 12, 4));
                extends.y = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 16, 4));
                extends.z = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 20, 4));

                this[entityIndex] = new Bounds
                {
                    center = center,
                    extents = extends
                };
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
                    var center = Vector3.zero;
                    center.x = BitConverter.ToSingle(array, startIndex);
                    center.y = BitConverter.ToSingle(array, startIndex + 4);
                    center.z = BitConverter.ToSingle(array, startIndex + 8);

                    var extends = Vector3.zero;
                    extends.x = BitConverter.ToSingle(array, startIndex + 12);
                    extends.y = BitConverter.ToSingle(array, startIndex + 16);
                    extends.z = BitConverter.ToSingle(array, startIndex + 20);

                    StoreItems[i] = new Bounds
                    {
                        center = center,
                        extents = extends
                    };
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
            var center = value.center;
            var extends = value.extents;
            return BGUtil.Format("$$$$$$$$$$$", BGFieldFloat.ValueToString(center.x), S, BGFieldFloat.ValueToString(center.y), S, BGFieldFloat.ValueToString(center.z), S,
                BGFieldFloat.ValueToString(extends.x), S, BGFieldFloat.ValueToString(extends.y), S, BGFieldFloat.ValueToString(extends.z));
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) ClearValueNoEvent(entityIndex);
            else
            {
                var parts = value.Split(S);
                if (parts.Length != 6) throw new BGException("Can not convert $ to Bounds." + Format, value);
                this[entityIndex] = new Bounds
                {
                    center = new Vector3(BGFieldFloat.ValueFromString(parts[0]), BGFieldFloat.ValueFromString(parts[1]), BGFieldFloat.ValueFromString(parts[2])),
                    extents = new Vector3(BGFieldFloat.ValueFromString(parts[3]), BGFieldFloat.ValueFromString(parts[4]), BGFieldFloat.ValueFromString(parts[5]))
                };
            }
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldBounds(meta, id, name);
    }
}