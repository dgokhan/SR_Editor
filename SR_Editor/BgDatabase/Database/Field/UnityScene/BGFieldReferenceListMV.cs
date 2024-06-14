/*
<copyright file="BGFieldReferenceListMV.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;
using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// reference to multiple BGWithId components. Multiple IDs are used
    /// </summary>
    [FieldDescriptor(Name = "objectListMultiValueReference", Folder = "Unity Scene", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerReferenceListMV")]
    public class BGFieldReferenceListMV : BGFieldCachedA<List<BGWithId>, List<BGId>>, BGSceneObjectReferenceI
    {
        /// <inheritdoc />
        public override bool ReadOnly => true;

        public const ushort CodeType = 99;

        /// <inheritdoc />
        public override ushort TypeCode => CodeType;

        public BGFieldReferenceListMV(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldReferenceListMV(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc />
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldReferenceListMV(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override List<BGWithId> this[int entityIndex]
        {
            get
            {
                var list = GetStoredValue(entityIndex);
                if (list == null || list.Count == 0) return null;

                var result = new List<BGWithId>();
                foreach (var item in list)
                {
                    var itemResult = BGWithId.GetAll(item);
                    if (itemResult == null || itemResult.Count == 0) continue;
                    result.AddRange(itemResult);
                }

                return result;
            }
            set { }
        }

        /// <inheritdoc />
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            if (fromEntityIndex == -1 || fromField.IsDeleted) return;
            var index = Meta.FindEntityIndex(toEntityId);
            if (index == -1) return;

            var otherField = (BGFieldReferenceListMV)fromField;
            var otherValue = otherField.GetStoredValue(fromEntityIndex);
            if (otherValue == null || otherValue.Count == 0) ClearValueNoEvent(index);
            else SetStoredValue(index, new List<BGId>(otherValue));
        }

        /// <inheritdoc/>
        public override bool AreStoredValuesEqual(BGField field, int myEntityIndex, int otherEntityIndex)
        {
            if (!(field is BGFieldReferenceListMV typed)) return false;

            var valueList = GetStoredValue(myEntityIndex);
            var valueList2 = typed.GetStoredValue(otherEntityIndex);

            var isEmpty = BGUtil.IsEmpty(valueList);
            var isEmpty2 = BGUtil.IsEmpty(valueList2);

            if (isEmpty && isEmpty2) return true;
            if (isEmpty || isEmpty2) return false;

            if (valueList.Count != valueList2.Count) return false;

            for (var i = 0; i < valueList.Count; i++)
            {
                var myValue = valueList[i];
                var myValue2 = valueList2[i];
                if (myValue != myValue2) return false;
            }

            return true;
        }


        //================================================================================================
        //                                              Synchronization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex)
        {
            var list = GetStoredValue(entityIndex);
            var hasValue = list != null && list.Count > 0;
            var writer = new BGBinaryWriter(hasValue ? BGFieldInt.SizeOfTheValue + (list.Count * BGFieldId.Size) : BGFieldInt.SizeOfTheValue);
            writer.AddArray(() =>
            {
                foreach (var item in list) writer.AddId(item);
            }, hasValue ? list.Count : 0);
            return writer.ToArray();
        }

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            /*
            var reader = new BGBinaryReader(segment);
            List<BGId> list = null;
            reader.ReadArray(() =>
            {
                list = list ?? new List<BGId>();
                list.Add(reader.ReadId());
            });
            */
            
            var count = BGFieldInt.ValueFromBytes(new ArraySegment<byte>(segment.Array, segment.Offset, BGFieldInt.SizeOfTheValue));
            List<BGId> list;
            if (count > 0)
            {
                var startOffset = segment.Offset + BGFieldInt.SizeOfTheValue;
                list = new List<BGId>(count);
                for (var i = 0; i < count; i++) list.Add(new BGId(segment.Array, startOffset + (i * BGFieldId.Size)));
            }
            else list = null;

            StoreItems[entityIndex] = list;
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex)
        {
            var list = GetStoredValue(entityIndex);
            if (list == null || list.Count == 0) return null;
            var builder = new StringBuilder();
            foreach (var item in list)
            {
                if (builder.Length != 0) builder.Append(A);
                builder.Append(item.ToString());
            }

            return builder.ToString();
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            List<BGId> list;
            if (!string.IsNullOrEmpty(value))
            {
                list = new List<BGId>();
                var tokens = value.Split(AA);
                foreach (var token in tokens)
                {
                    if (BGId.TryParse(token, out var id)) list.Add(id);
                }
            }
            else list = null;

            StoreItems[entityIndex] = list;
        }

        /// <summary>
        /// How much array elements the entity with provided index have
        /// </summary>
        public int CountValues(int entityIndex) => this[entityIndex]?.Count ?? 0;
    }
}