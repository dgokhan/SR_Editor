/*
<copyright file="BGFieldArrayByte.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field with byte array value type
    /// </summary>
    [FieldDescriptor(Name = "byteArray", Folder = "Special", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerArrayByte")]
    public class BGFieldArrayByte : BGFieldCachedStructArrayA<byte>, BGBinaryBulkLoaderClass
    {
        public const ushort CodeType = 1;
        
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        public BGFieldArrayByte(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldArrayByte(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldArrayByte(meta, id, name);

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        public override byte[] ToBytes(int entityIndex)
        {
            var value = this[entityIndex];
            if (value == null || value.Length == 0) return null;
            return value;
        }

        /// <inheritdoc/>
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            var segmentCount = segment.Count;
            if (segmentCount == 0) ClearValueNoEvent(entityIndex);
            else StoreItems[entityIndex] = BGUtil.ToArray(segment);
        }

        /// <inheritdoc />
        public virtual void FromBytes(BGBinaryBulkRequestClass request)
        {
            var array = request.Array;
            var requests = request.CellRequests;
            var length = requests.Length;
            for (var i = 0; i < length; i++)
            {
                var cellRequest = requests[i];
                try
                {
                    var result = new byte[cellRequest.Count];
                    Buffer.BlockCopy(array, cellRequest.Offset, result, 0, cellRequest.Count);
                    StoreItems[cellRequest.EntityIndex] = result;
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
            var value = this[entityIndex];
            if (value == null || value.Length == 0) return null;
            var base64String = Convert.ToBase64String(value);
            if (base64String[0] == '+') base64String = '\'' + base64String;
            return base64String;
        }

        /// <inheritdoc/>
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) ClearValueNoEvent(entityIndex);
            else
            {
                if (value.Length > 1 && value[0] == '\'') value = value.Substring(1);
                StoreItems[entityIndex] = Convert.FromBase64String(value);
            }
        }

        //================================================================================================
        //                                              Misc
        //================================================================================================
        /// <inheritdoc/>
        protected override bool AreEqual(byte myValue, byte myValue2) => myValue == myValue2;
    }
}