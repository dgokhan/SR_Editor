/*
<copyright file="BGFieldString.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// stirng Field 
    /// </summary>
    [FieldDescriptor(Name = "string", Folder = "Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerString")]
    public partial class BGFieldString : BGFieldStringA
    {
        public const ushort CodeType = 34;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldString(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        protected internal BGFieldString(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldString(meta, id, name);
    }
}