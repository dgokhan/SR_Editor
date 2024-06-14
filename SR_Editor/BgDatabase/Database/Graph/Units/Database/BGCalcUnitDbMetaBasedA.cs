/*
<copyright file="BGCalcUnitDbMetaBasedA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract table-based unit
    /// </summary>
    public abstract class BGCalcUnitDbMetaBasedA : BGCalcUnit
    {
        public static readonly byte MetaIdVarId = 1;

        private BGMetaEntity metaCached;

        //cached table for faster operations
        protected BGMetaEntity MetaCached
        {
            get
            {
                if (metaCached == null || metaCached.IsDeleted) metaCached = Meta;
                if (metaCached == null) throw new Exception($"Can not get a meta with Id={MetaId}!");
                return metaCached;
            }
        }

        /// <summary>
        /// table variable
        /// </summary>
        public BGCalcVarLite MetaVar => GetVar(MetaIdVarId);

        /// <summary>
        /// table 
        /// </summary>
        public BGMetaEntity Meta => BGRepo.I.GetMeta(MetaId);

        /// <summary>
        /// table ID
        /// </summary>
        public BGId MetaId => (BGId)MetaVar.Value;


        /// <summary>
        /// init unit with provided table ID 
        /// </summary>
        public virtual void Init(BGId metaId)
        {
            GetVars()?.Variables.Clear();
            var idVar = BGCalcVarLite.Create(this, MetaIdVarId, BGCalcTypeCodeRegistry.BGId);
            idVar.Value = metaId;
        }
    }
}