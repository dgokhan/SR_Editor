/*
<copyright file="BGCalcUnitWithInPortsCount.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public abstract class BGCalcUnitWithInPortsCount : BGCalcUnit, BGCalcUnitInitializable
    {
        public const byte CountVarId = 1;
        private const int MaxCount = 250;
        public event Action OnCountChange;
        protected readonly List<BGCalcValueInput> inputs = new List<BGCalcValueInput>();
        private BGCalcValueOutput outValue;

        /// <summary>
        /// number of items (250 items maximum)
        /// </summary>
        public int Count
        {
            get
            {
                var value = (int)(byte)GetVar(CountVarId).Value;
                var clamp = Mathf.Clamp(value, Min, MaxCount);
                return clamp;
            }
            set
            {
                var clamp = Mathf.Clamp(value, Min, MaxCount);
                GetVar(CountVarId).Value = (byte)clamp;
            }
        }

        protected virtual int Min => 0;
        
        /// <inheritdoc />
        public override string GetPublicVarLabel(byte varId) => CountVarId == varId ? "count" : null;
        
        /// <summary>
        /// initialize unit with default items count
        /// </summary>
        public void Init()
        {
            var sourceVar = BGCalcVarLite.Create(this, CountVarId, BGCalcTypeCodeRegistry.Byte);
            sourceVar.Value = (byte)2;
        }
        
        public BGCalcValueInput GetInput(int index) => inputs[index];

        /// <inheritdoc />
        public override void Definition() => Rebuild();

        private void Rebuild()
        {
            var var = GetVar(CountVarId);
            var.OnValueChange -= Rebuild;
            var.OnValueChange += Rebuild;

            var count = Count;
            if (count != inputs.Count)
            {
                if (count > inputs.Count)
                    for (var i = inputs.Count; i < count; i++)
                        inputs.Add(ValueInput(InPortType, i + "", i + ""));
                else
                    for (var i = inputs.Count - 1; i >= count; i--)
                    {
                        var toRemove = inputs[i];
                        toRemove.DisconnectAll();
                        RemovePort(toRemove);
                        inputs.RemoveAt(i);
                    }
            }

            if (outValue == null) outValue = CreateOutputPort();

            OnCountChange?.Invoke();
        }

        protected abstract BGCalcValueOutput CreateOutputPort();

        protected abstract BGCalcTypeCode InPortType
        {
            get;
        }
    }
}