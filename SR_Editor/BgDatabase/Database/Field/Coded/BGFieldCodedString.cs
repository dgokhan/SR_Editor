/*
<copyright file="BGFieldCodedString.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// string programmable field
    /// </summary>
    [FieldDescriptor(Name = "programmableString", Folder = "Programmable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerCodedString")]
    public class BGFieldCodedString : BGFieldCodedA<string>
    {
        public const ushort CodeType = 103;
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldCodedString(BGMetaEntity meta, string name, Type delegateType) : base(meta, name, delegateType)
        {
        }

        protected internal BGFieldCodedString(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldCodedString(meta, id, name);
    }
}