/*
<copyright file="BGFieldCalcString.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// calculated string field. 
    /// </summary>
    [FieldDescriptor(Name = "calculatedString", Folder = "Calculated", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerCalcString")]
    public class BGFieldCalcString : BGFieldCalcA<string>
    {
        public const ushort CodeType = 7;
        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldCalcString(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected internal BGFieldCalcString(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldCalcString(meta, id, name);

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <inheritdoc/>
        public override BGCalcTypeCode ResultCode => BGCalcTypeCodeRegistry.String;
    }
}