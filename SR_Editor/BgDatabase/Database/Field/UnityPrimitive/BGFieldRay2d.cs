/*
<copyright file="BGFieldRay2d.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// ray2d Field 
    /// </summary>
    [FieldDescriptor(Name = "ray2d", Folder = "Unity Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerRay2d")]
    public partial class BGFieldRay2d : BGFieldCachedStructA<Ray2D>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 66;
        
        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        private const int Size = 16;

        /// <inheritdoc />
        protected override int ValueSize => Size;

        /// <inheritdoc />
        public override string Description => base.Description + ", " + Format;

        public static string Format => BGUtil.Format(" format is [origin.x$origin.y$direction.x$direction.y] (without braces)", S, S, S);

        //for new field
        public BGFieldRay2d(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldRay2d(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc />
        protected override bool AreStoredValuesEqual(Ray2D myValue, Ray2D otherValue)
        {
            return BGFieldVector2.AreValuesEqual(myValue.origin, otherValue.origin)
                   && BGFieldVector2.AreValuesEqual(myValue.direction, otherValue.direction);
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

            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(direction.x), 0, buffer, 8, 4);
            Buffer.BlockCopy(BGFieldFloat.ValueToBytes(direction.y), 0, buffer, 12, 4);

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


                var origin = Vector2.zero;
                origin.x = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 0, 4));
                origin.y = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 4, 4));

                var direction = Vector2.zero;
                direction.x = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 8, 4));
                direction.y = BGFieldFloat.ValueFromBytes(new ArraySegment<byte>(array, offset + 12, 4));

                this[entityIndex] = new Ray2D(origin, direction);
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
                    var origin = Vector2.zero;
                    origin.x = BitConverter.ToSingle(array, startIndex);
                    origin.y = BitConverter.ToSingle(array, startIndex + 4);

                    var direction = Vector2.zero;
                    direction.x = BitConverter.ToSingle(array, startIndex + 8);
                    direction.y = BitConverter.ToSingle(array, startIndex + 12);

                    StoreItems[i] = new Ray2D
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
            return BGUtil.Format("$$$$$$$", BGFieldFloat.ValueToString(origin.x), S, BGFieldFloat.ValueToString(origin.y), S,
                BGFieldFloat.ValueToString(direction.x), S, BGFieldFloat.ValueToString(direction.y));
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) ClearValue(entityIndex);
            else
            {
                var parts = value.Split(S);
                if (parts.Length != 4) throw new BGException("Can not convert $ to Ray2D. Should be [origin.x,origin.y,direction.x,direction.y] (without braces)", value);
                this[entityIndex] = new Ray2D(
                    new Vector2(BGFieldFloat.ValueFromString(parts[0]), BGFieldFloat.ValueFromString(parts[1])),
                    new Vector2(BGFieldFloat.ValueFromString(parts[2]), BGFieldFloat.ValueFromString(parts[3])));
            }
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldRay2d(meta, id, name);
    }
}