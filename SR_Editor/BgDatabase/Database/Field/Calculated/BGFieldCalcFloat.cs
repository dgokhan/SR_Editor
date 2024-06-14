/*
<copyright file="BGFieldCalcFloat.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// calculated float field. 
    /// </summary>
    [FieldDescriptor(Name = "calculatedFloat", Folder = "Calculated", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerCalcFloat")]
    public class BGFieldCalcFloat : BGFieldCalcA<float>
    {
        public const ushort CodeType = 4;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldCalcFloat(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected internal BGFieldCalcFloat(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldCalcFloat(meta, id, name);

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <inheritdoc/>
        public override BGCalcTypeCode ResultCode => BGCalcTypeCodeRegistry.Float;
    }
}