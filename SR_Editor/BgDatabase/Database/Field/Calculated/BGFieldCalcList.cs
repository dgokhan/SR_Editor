/*
<copyright file="BGFieldCalcList.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// calculated list field. 
    /// </summary>
    [FieldDescriptor(Name = "calculatedList", Folder = "Calculated", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerCalcList")]
    public class BGFieldCalcList : BGFieldCalcA<IList>
    {
        public const ushort CodeType = 96;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldCalcList(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected internal BGFieldCalcList(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldCalcList(meta, id, name);

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <inheritdoc/>
        public override BGCalcTypeCode ResultCode => BGCalcTypeCodeRegistry.List;
    }
}