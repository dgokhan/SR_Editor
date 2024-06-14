/*
<copyright file="BGFieldCalcAction.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// action field 
    /// </summary>
    [FieldDescriptor(Name = "action", Folder = "Calculated", ManagerType = "BansheeGz.BGDatabase.Editor.BGFieldManagerCalcAction")]
    public class BGFieldCalcAction : BGFieldCalcA<BGFieldCalcActionValue>
    {
        public const ushort CodeType = 2;

        /// <inheritdoc/>
        public override ushort TypeCode => CodeType;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldCalcAction(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        protected internal BGFieldCalcAction(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }

        //================================================================================================
        //                                              Factory
        //================================================================================================
        /// <inheritdoc/>
        protected override Func<BGMetaEntity, BGId, string, BGField> CreateFieldFactory() => (meta, id, name) => new BGFieldCalcAction(meta, id, name);

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <inheritdoc/>
        public override BGCalcTypeCode ResultCode => BGCalcTypeCodeRegistry.CalcAction;

        /// <inheritdoc/>
        protected override BGFieldCalcActionValue Convert(BGEntity entity, BGFieldCalcValue value)
        {
            BGCalcGraph graphToExecute = null;
            if (value?.Graph != null) graphToExecute = value.Graph;
            else if (Graph != null) graphToExecute = Graph;

            if (graphToExecute == null) return new BGFieldCalcActionValue();

            return new BGFieldCalcActionValue(() =>
            {
                var context = BGCalcFlowContext.Get();
                try
                {
                    context.Graph = graphToExecute;
                    context.CurrentEntity = entity;
                    context.VarsOverrides = value;
                    context.GraphType = BGCalcGraphTypeEnum.Action;
                    graphToExecute.Execute(context);
                }
                finally
                {
                    BGCalcFlowContext.Return(context);
                }
            });
        }
    }
}