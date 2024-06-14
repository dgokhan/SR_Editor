using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// nullable byte Field 
    /// </summary>
    [FieldDescriptor(Name = "byte?", Folder = "Primitive Nullable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerByteNullable")]
    public class BGFieldByteNullable : BGFieldCachedStructNullableA<byte>
    {
        public const ushort CodeType = 105;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldByteNullable(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldByteNullable(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        /// <inheritdoc/>
        protected override int ValueSize => BGFieldByte.SizeOfTheValue;

        /// <inheritdoc/>
        public override bool CanBeUsedAsKey => true;

        //================================================================================================
        //                                              Serialization
        //================================================================================================
        /// <inheritdoc/>
        protected override byte[] ValueToBytes(byte value) => BGFieldByte.ValueToBytes(value);

        /// <inheritdoc/>
        protected override byte ValueFromBytes(ArraySegment<byte> segment) => BGFieldByte.ValueFromBytes(segment);
        protected override byte ValueFromBytes(byte[] array, int offset) => array[offset];
        /// <inheritdoc/>
        protected override string ValueToString(byte value) => BGFieldByte.ValueToString(value);

        /// <inheritdoc/>
        protected override byte? ValueFromString(string value)
        {
            try
            {
                return BGFieldByte.ValueFromString(value);
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
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldByteNullable(meta, id, name);
    }
}