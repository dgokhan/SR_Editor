/*
<copyright file="BGFieldText.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// text (multiline string) Field 
    /// </summary>
    [FieldDescriptor(Name = "text", Folder = "Primitive", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerText")]
    public partial class BGFieldText : BGFieldStringA
    {
        public const ushort CodeType = 35;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //for new field
        public BGFieldText(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing field
        internal BGFieldText(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldText(meta, id, name);
    }
}