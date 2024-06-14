/*
<copyright file="BGFieldCalcInt.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// calculated int field. 
    /// </summary>
    [FieldDescriptor(Name = "calculatedInt", Folder = "Calculated", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerCalcInt")]
    public class BGFieldCalcInt : BGFieldCalcA<int>
    {
        public const ushort CodeType = 5;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldCalcInt(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected internal BGFieldCalcInt(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldCalcInt(meta, id, name);

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <inheritdoc/>
        public override BGCalcTypeCode ResultCode => BGCalcTypeCodeRegistry.Int;
    }
}