using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Field for referencing meta from tables partition table
    /// </summary>
    [FieldDescriptor(ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerPartitionMetaReference")]
    public class BGFieldPartitionMetaReference : BGFieldMetaReference
    {
        public override ushort TypeCode => 0;

        public BGFieldPartitionMetaReference(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        internal BGFieldPartitionMetaReference(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldPartitionMetaReference(meta, id, name);
    }
}