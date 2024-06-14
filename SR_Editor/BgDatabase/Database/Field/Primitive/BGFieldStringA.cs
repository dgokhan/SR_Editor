/*
<copyright file="BGFieldStringA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract string Field 
    /// </summary>
    public abstract partial class BGFieldStringA : BGFieldCachedClassA<string>, BGBinaryBulkLoaderClass
    {
        /// <inheritdoc/>
        public override bool CanBeUsedAsKey => true;


        protected BGFieldStringA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected BGFieldStringA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc/>
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            if (fromEntityIndex == -1 || fromField.IsDeleted) return;
            var index = Meta.FindEntityIndex(toEntityId);
            if (index == -1) return;

            var otherField = (BGField<string>)fromField;
            StoreSet(index, otherField[fromEntityIndex]);
        }

        /// <inheritdoc/>
        public override string this[int entityIndex]
        {
            set
            {
                if (events.On)
                {
                    var oldValue = this[entityIndex];
                    if (string.Equals(oldValue, value)) return;
                    var entity = Meta[entityIndex];
                    FireBeforeValueChanged(entity, oldValue, value);
                    StoreSet(entityIndex, value);
                    FireValueChanged(entity, oldValue, value);
                }
                else StoreSet(entityIndex, value);
            }
        }

        /// <inheritdoc/>
        public override void ForEachValue(Action<int> action)
        {
            //this is copy/paste from Store.ForEachValue to get rid of empty strings
            var count = StoreCount;
            for (var i = 0; i < count; i++)
            {
                var item = StoreItems[i];
                if (string.IsNullOrEmpty(item)) continue;
                action(i);
            }
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex) => ValueToBytes(this[entityIndex]);

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment) => this[entityIndex] = ValueFromBytes(segment);

        /// <inheritdoc />
        public virtual void FromBytes(BGBinaryBulkRequestClass request)
        {
            var array = request.Array;
            var requests = request.CellRequests;
            var length = requests.Length;
            var encoding = Encoding.UTF8;
            for (var i = 0; i < length; i++)
            {
                var cellRequest = requests[i];
                try
                {
                    StoreItems[cellRequest.EntityIndex] = encoding.GetString(array, cellRequest.Offset, cellRequest.Count);
                }
                catch (Exception e)
                {
                    request.OnError?.Invoke(e);
                }
            }
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex) => ValueToString(this[entityIndex]);

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value) => this[entityIndex] = ValueFromString(value);


        //================================================================================================
        //                                              Static
        //================================================================================================

        public static byte[] ValueToBytes(string value) => string.IsNullOrEmpty(value) ? null : Encoding.UTF8.GetBytes(value);

        public static string ValueFromBytes(ArraySegment<byte> segment) => segment.Count == 0 ? null : Encoding.UTF8.GetString(segment.Array, segment.Offset, segment.Count);

        public static string ValueToString(string value) => value ?? "";

        public static string ValueFromString(string value) => value;
    }
}