/*
<copyright file="BGFieldCodedInt.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// int programmable field
    /// </summary>
    [FieldDescriptor(Name = "programmableInt", Folder = "Programmable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerCodedInt")]
    public class BGFieldCodedInt : BGFieldCodedA<int>
    {
        public const ushort CodeType = 102;
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldCodedInt(BGMetaEntity meta, string name, Type delegateType) : base(meta, name, delegateType)
        {
        }

        protected internal BGFieldCodedInt(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldCodedInt(meta, id, name);
    }
}