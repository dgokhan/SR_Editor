/*
<copyright file="BGFieldRay.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// ray Field 
    /// </summary>
    [FieldDescriptor(Name = "ray", Folder = "Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerRay")]
    public partial class BGFieldRay : BGFieldCachedStructA<Ray>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 65;
        
        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        private const int Size = 24;

        /// <inheritdoc />
        protected override int ValueSize => Size;

        /// <inheritdoc />
        public override string Description => base.Description + ", " + Format;

        public static string Format => BGUtil.Format(" format is [origin.x$origin.y$origin.z$direction.x$direction.y$direction.z] (without braces)", S, S, S, S, S);

        //for new field
        public BGFieldRay(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldRay(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override bool AreStoredValuesEqual(Ray myValue, Ray otherValue)
        {
            return BGFieldVector3.AreValuesEqual(myValue.origin, otherValue.origin)
                   && BGFieldVector3.AreValuesEqual(myValue.direction, otherValue.direction);
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex)
        {
            var value = this[entityIndex];
            var origin = value.origin;
            var direction = value.direction;


            var buffer = new byte[Size];

            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(origin.x), 0, buffer, 0, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(origin.y), 0, buffer, 4, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(origin.z), 0, buffer, 8, 4);

            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(direction.x), 0, buffer, 12, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(direction.y), 0, buffer, 16, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(direction.z), 0, buffer, 20, 4);

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


                var origin = Vector3.zero;
                origin.x = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 0, 4));
                origin.y = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 4, 4));
                origin.z = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 8, 4));

                var direction = Vector3.zero;
                direction.x = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 12, 4));
                direction.y = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 16, 4));
                direction.z = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 20, 4));

                this[entityIndex] = new Ray(origin, direction);
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
                    var origin = Vector3.zero;
                    origin.x = BitConverter.ToSingle(array, startIndex);
                    origin.y = BitConverter.ToSingle(array, startIndex + 4);
                    origin.z = BitConverter.ToSingle(array, startIndex + 8);

                    var direction = Vector3.zero;
                    direction.x = BitConverter.ToSingle(array, startIndex + 12);
                    direction.y = BitConverter.ToSingle(array, startIndex + 16);
                    direction.z = BitConverter.ToSingle(array, startIndex + 20);

                    StoreItems[i] = new Ray
                    {
                        origin = origin,
                        direction = direction
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
            var origin = value.origin;
            var direction = value.direction;
            return BGUtil.Format("$$$$$$$$$$$", BGFieldFloat.ValueToString(origin.x), S, BGFieldFloat.ValueToString(origin.y), S, BGFieldFloat.ValueToString(origin.z), S,
                BGFieldFloat.ValueToString(direction.x), S, BGFieldFloat.ValueToString(direction.y), S, BGFieldFloat.ValueToString(direction.z));
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) ClearValue(entityIndex);
            else
            {
                var parts = value.Split(S);
                if (parts.Length != 6) throw new BGException("Can not convert $ to Ray" + Format, value);
                this[entityIndex] = new Ray(
                    new Vector3(BGFieldFloat.ValueFromString(parts[0]), BGFieldFloat.ValueFromString(parts[1]), BGFieldFloat.ValueFromString(parts[2])),
                    new Vector3(BGFieldFloat.ValueFromString(parts[3]), BGFieldFloat.ValueFromString(parts[4]), BGFieldFloat.ValueFromString(parts[5])));
            }
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldRay(meta, id, name);
    }
}