/*
<copyright file="BGFieldEntityName.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field for entity name
    /// </summary>
    [FieldDescriptor(ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerEntityName")]
    public partial class BGFieldEntityName : BGFieldString
    {
        public new const ushort CodeType = 47;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        public const string NameFieldName = "name";

        /// <inheritdoc/>
        public override string Description => "Entity's name";

        private bool nameEmpty;

        /// <summary>
        /// is provided field is name field
        /// </summary>
        public static bool IsName(BGField field) => NameFieldName.Equals(field.Name);

        /// <summary>
        /// is all name fields are  empty
        /// </summary>
        public bool NameEmpty
        {
            get => nameEmpty;
            internal set
            {
                if (nameEmpty == value) return;
                nameEmpty = value;

                if (nameEmpty) StoreClear();
                else base.OnCreate();
            }
        }

        //can we use this method????
        // public override bool EmptyContent => nameEmpty;

        /// <inheritdoc />
        public override string this[int entityIndex]
        {
            get
            {
                if (nameEmpty) return null;
                return base[entityIndex];
            }
            set
            {
                if (nameEmpty) return;
                var oldValue = this[entityIndex];
                if (string.Equals(value, oldValue)) return;

                base[entityIndex] = value;
                Meta.OnEntityNameChange(entityIndex, oldValue, value);
                Meta.ForEachField(field => field.OnNameChange(entityIndex));
            }
        }

        /// <inheritdoc/>
        public override string this[BGId entityId]
        {
            get
            {
                if (nameEmpty) return null;
                return base[entityId];
            }
            set
            {
                if (nameEmpty) return;
                base[entityId] = value;
            }
        }


        public BGFieldEntityName(BGMetaEntity meta, string name) : base(meta, NameFieldName) => nameEmpty = meta.EmptyName;

        internal BGFieldEntityName(BGMetaEntity meta, BGId id, string name) : base(meta, id, NameFieldName) => nameEmpty = meta.EmptyName;

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <summary>
        /// This is faster alternative to standard slower this[BGId id] method
        /// This is used from Binary reader only, so we can skip range check
        /// </summary>
        internal void SetEntityValue(int entityIndex, string value)
        {
            if (nameEmpty) return;
            StoreItems[entityIndex] = value;
        }

        /// <inheritdoc/>
        public override void CopyValue(BGField fromField, BGId fromEntityId, int fromEntityIndex, BGId toEntityId)
        {
            if (nameEmpty) return;
            //copying values usually means batch changes- so it's better to invalidate cache than rebuild it on each copy
            Meta.InvalidateNameCache();
            base.CopyValue(fromField, fromEntityId, fromEntityIndex, toEntityId);
        }

        /// <inheritdoc/>
        public override void ForEachValue(Action<int> action)
        {
            if (nameEmpty) return;
            base.ForEachValue(action);
        }

        /// <inheritdoc/>
        public override byte[] ToBytes(int entityIndex)
        {
            if (nameEmpty) return null;
            return base.ToBytes(entityIndex);
        }

        /// <inheritdoc/>
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (nameEmpty) return;
            base.FromBytes(entityIndex, segment);
        }

        /// <inheritdoc />
        public override void FromBytes(BGBinaryBulkRequestClass request)
        {
            if (nameEmpty) return;
            base.FromBytes(request);
        }

        /// <inheritdoc/>
        public override string ToString(int entityIndex)
        {
            if (nameEmpty) return null;
            return base.ToString(entityIndex);
        }

        /// <inheritdoc/>
        public override void FromString(int entityIndex, string value)
        {
            if (nameEmpty) return;
            base.FromString(entityIndex, value);
        }

        /// <inheritdoc/>
        public override void ClearValues()
        {
            if (nameEmpty) return;
            base.ClearValues();
        }

        /// <inheritdoc/>
        public override void ClearValue(int entityIndex)
        {
            if (nameEmpty) return;
            base.ClearValue(entityIndex);
        }

        /// <inheritdoc/>
        public override void SetStoredValue(int entityIndex, string value)
        {
            if (nameEmpty) return;
            base.SetStoredValue(entityIndex, value);
        }

        /// <inheritdoc/>
        public override bool AreStoredValuesEqual(BGField field, int myEntityIndex, int otherEntityIndex)
        {
            if (nameEmpty)
            {
                if (!(field is BGField<string> typed)) return false;
                if ((field is BGFieldEntityName eName && eName.nameEmpty)) return true;
                return string.IsNullOrEmpty(typed[otherEntityIndex]);
            }

            return base.AreStoredValuesEqual(field, myEntityIndex, otherEntityIndex);
        }

        /// <inheritdoc/>
        public override void MoveEntitiesValues(int fromIndex, int toIndex, int numberOfValues)
        {
            if (nameEmpty) return;
            base.MoveEntitiesValues(fromIndex, toIndex, numberOfValues);
        }

        /// <inheritdoc/>
        public override void Swap(int entityIndex1, int entityIndex2)
        {
            if (nameEmpty) return;
            base.Swap(entityIndex1, entityIndex2);
        }

        /// <inheritdoc/>
        public override void OnEntityAdd(BGEntity entity)
        {
            if (nameEmpty) return;
            base.OnEntityAdd(entity);
        }

        /// <inheritdoc/>
        public override void OnEntityDelete(BGEntity entity)
        {
            if (nameEmpty) return;
            base.OnEntityDelete(entity);
        }

        /// <inheritdoc/>
        public override void OnCreate()
        {
            if (nameEmpty) return;
            base.OnCreate();
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldEntityName(meta, id, name);
    }
}