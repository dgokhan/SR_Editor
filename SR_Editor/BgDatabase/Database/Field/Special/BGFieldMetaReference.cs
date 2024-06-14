using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field for referencing metasBGId
    /// </summary>
    [FieldDescriptor(Name = "metaReference", Folder = "Special", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerMetaReference")]
    public class BGFieldMetaReference : BGFieldCachedA<BGMetaEntity, BGId>, BGBinaryBulkLoaderStruct
    {
        public const ushort CodeType = 108;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Fields
        //================================================================================================

        /// <inheritdoc />
        public override int ConstantSize => BGFieldId.Size;

        //================================================================================================
        //                                              Constructors
        //================================================================================================

        public BGFieldMetaReference(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldMetaReference(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }


        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldMetaReference(meta, id, name);

        //================================================================================================
        //                                              Value
        //================================================================================================
        /// <inheritdoc />
        public override BGMetaEntity this[int entityIndex]
        {
            get
            {
                //micro-optimization code copied from base class
                if (entityIndex >= StoreCount) ThrowIndexOutOfBoundOnRead(entityIndex);
                var storeValue = StoreItems[entityIndex];
                if (storeValue.IsEmpty) return null;
                return Meta.Repo.GetMeta(storeValue);
            }
            set
            {
                if (events.On)
                {
                    var oldStoredValue = StoreItems[entityIndex];
                    if ((value == null && oldStoredValue.IsEmpty) || (value != null && value.Id == oldStoredValue)) return;
                    var oldValue = this[entityIndex];
                    var entity = Meta[entityIndex];
                    FireBeforeValueChanged(entity, oldValue, value);
                    StoreSet(entityIndex, value?.Id ?? BGId.Empty);
                    FireValueChanged(entity, oldValue, value);
                }
                else StoreSet(entityIndex, value?.Id ?? BGId.Empty);
            }
        }

        //================================================================================================
        //                                              Configuration
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(4);
            //version
            writer.AddInt(1);

            return writer.ToArray();
        }

        /// <inheritdoc />
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc />
        public override byte[] ToBytes(int entityIndex) => BGFieldId.ValueToBytes(StoreItems[entityIndex]);

        /// <inheritdoc />
        public override void FromBytes(int entityIndex, ArraySegment<byte> segment) => StoreItems[entityIndex] = BGFieldId.ValueFromBytes(segment);

        /// <inheritdoc />
        public void FromBytes(BGBinaryBulkRequestStruct request)
        {
            var array = request.Array;
            var offset = request.Offset;
            var entitiesCount = request.EntitiesCount;
            for (var i = 0; i < entitiesCount; i++) StoreItems[i] = new BGId(array, offset + (BGFieldId.Size * i));
        }

        /// <inheritdoc />
        public override string ToString(int entityIndex)
        {
            var id = StoreItems[entityIndex];
            if (id.IsEmpty) return null;
            var meta = this[entityIndex];
            if (meta == null) return id.ToString();
            return meta.Name + "_" + id;
        }

        /// <inheritdoc />
        public override void FromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) StoreItems[entityIndex] = BGId.Empty;
            else
            {
                var separator = value.LastIndexOf('_');
                if (separator == -1 || separator >= value.Length - 2) StoreItems[entityIndex] = new BGId(value);
                else
                {
                    var id = value.Substring(separator + 1, value.Length - separator - 1);
                    StoreItems[entityIndex] = new BGId(id);
                }
            }
        }
    }
}