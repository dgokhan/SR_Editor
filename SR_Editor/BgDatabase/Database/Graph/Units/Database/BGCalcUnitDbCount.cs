/*
<copyright file="BGCalcUnitDbCount.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// count database rows node
    /// </summary>
    public class BGCalcUnitDbCount : BGCalcUnitDbMetaBasedA
    {
        public const int Code = 105;

        /// <inheritdoc />
        public override ushort TypeCode => Code;

        /// <inheritdoc />
        public override string Title
        {
            get
            {
                var meta = Meta;
                if (meta == null) return "DB count [ERROR:meta not found]";
                return "DB count [" + meta.Name + "]";
            }
        }

        /// <inheritdoc />
        public override void Definition()
        {
            var meta = Meta;
            if (meta == null) throw new Exception("Meta is not found! id=" + MetaId);

            ValueOutput(BGCalcTypeCodeRegistry.Int, "count", "c", GetValue);
        }

        private object GetValue(BGCalcFlowI flow)
        {
            var meta = MetaCached;
            flow.Context.Events?.AddOnCreate(meta);
            flow.Context.Events?.AddOnDelete(meta);
            return meta.CountEntities;
        }
    }
}