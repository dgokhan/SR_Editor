/*
<copyright file="BGFieldCodedBool.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// bool programmable field
    /// </summary>
    [FieldDescriptor(Name = "programmableBool", Folder = "Programmable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerCodedBool")]
    public class BGFieldCodedBool : BGFieldCodedA<bool>
    {
        public const ushort CodeType = 100;
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldCodedBool(BGMetaEntity meta, string name, Type delegateType) : base(meta, name, delegateType)
        {
        }

        protected internal BGFieldCodedBool(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldCodedBool(meta, id, name);

    }
}