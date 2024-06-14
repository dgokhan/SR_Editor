/*
<copyright file="BGCalcUnitGetMeta.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// get table unit
    /// </summary>
    [BGCalcUnitDefinition("Database/Generic/meta/Get meta")]
    public class BGCalcUnitGetMeta : BGCalcUnitWithSource
    {
        public const int Code = 70;
        
        /// <inheritdoc />
        public override ushort TypeCode => Code;
        
        /// <inheritdoc />
        protected override BGCalcTypeCode ObjectTypeCode => BGCalcTypeCodeRegistry.Meta;

        /// <inheritdoc />
        protected override BGObject FetchObjectByName(BGCalcFlowI calcFlowI, string name) => BGRepo.I.GetMeta(name);

        /// <inheritdoc />
        protected override BGObject FetchObjectById(BGCalcFlowI calcFlowI, BGId id) => BGRepo.I.GetMeta(id);

        /// <inheritdoc />
        protected override BGObject FetchObjectByIndex(BGCalcFlowI calcFlowI, int index) => BGRepo.I.GetMeta(index);
    }
}