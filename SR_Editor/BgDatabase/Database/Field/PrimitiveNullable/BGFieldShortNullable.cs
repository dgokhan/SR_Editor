using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nullable short Field 
    /// </summary>
    [FieldDescriptor(Name = "short?", Folder = "Primitive Nullable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerShortNullable")]
    public class BGFieldShortNullable : BGFieldCachedStructNullableA<short>
    {
        public const ushort CodeType = 106;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldShortNullable(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldShortNullable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldShort.SizeOfTheValue;

        /// <inheritdoc/>
        public override bool CanBeUsedAsKey => true;

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(short value) => BGFieldShort.ValueToBytes(value);

        /// <inheritdoc/>
        protected override short ValueFromBytes(ArraySegment<byte> segment) => BGFieldShort.ValueFromBytes(segment);

        protected override short ValueFromBytes(byte[] array, int offset) => (short)((array[offset + 1] << 8) | array[offset]);
        /// <inheritdoc/>
        protected override string ValueToString(short value) => BGFieldShort.ValueToString(value);

        /// <inheritdoc/>
        protected override short? ValueFromString(string value)
        {
            try
            {
                return BGFieldShort.ValueFromString(value);
            }
            catch
            {
                return null;
            }
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldShortNullable(meta, id, name);
    }
}