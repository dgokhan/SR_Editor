/*
<copyright file="BGFieldCodedFloat.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// float programmable field
    /// </summary>
    [FieldDescriptor(Name = "programmableFloat", Folder = "Programmable", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerCodedFloat")]
    public class BGFieldCodedFloat : BGFieldCodedA<float>
    {
        public const ushort CodeType = 101;
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldCodedFloat(BGMetaEntity meta, string name, Type delegateType) : base(meta, name, delegateType)
        {
        }

        protected internal BGFieldCodedFloat(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldCodedFloat(meta, id, name);
    }
}