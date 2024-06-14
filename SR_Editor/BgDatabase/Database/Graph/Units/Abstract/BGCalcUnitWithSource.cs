/*
<copyright file="BGCalcUnitWithSource.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract class for node with database member source
    /// </summary>
    public abstract class BGCalcUnitWithSource : BGCalcUnit
    {
        public static readonly byte SourceVarId = 1;

        public Action OnSourceChange;

        public const string InIndex = "a";
        public const string InId = "b";
        public const string InName = "c";
        public const string InObject = "d";
        public const string OutIndex = "k";
        public const string OutId = "l";
        public const string OutName = "m";
        public const string OutObject = "n";

        private BGCalcValueInput indexInput;
        private BGCalcValueInput idInput;
        private BGCalcValueInput nameInput;
        private BGCalcValueInput objInput;

        public BGCalcValueInput IndexInput => indexInput;

        public BGCalcValueInput IdInput => idInput;

        public BGCalcValueInput NameInput => nameInput;

        public BGCalcValueInput DbObjectInput => objInput;

        /// <summary>
        /// source value
        /// </summary>
        public BGCalcUnitSourceEnum Source
        {
            get
            {
                var calcVar = GetVar(SourceVarId);
                return (BGCalcUnitSourceEnum)calcVar.Value;
            }
            set
            {
                if (Source == value) return;
                GetVar(SourceVarId).Value = value;
            }
        }

        /// <inheritdoc />
        public override void Definition()
        {
            //in
            var sourceVar = GetOrAddVar(SourceVarId, BGCalcTypeCodeRegistry.EntitySource);
            sourceVar.OnValueChange += OnSourceChanged;
            AddRefPort();

            //out
            ValueOutput(ObjectTypeCode, ObjectTypeCode.Name, OutObject, GetObject);
            ValueOutput(BGCalcTypeCodeRegistry.Int, "index", OutIndex, GetIndex);
            ValueOutput(BGCalcTypeCodeRegistry.BGId, "id", OutId, GetId);
            ValueOutput(BGCalcTypeCodeRegistry.String, "name", OutName, GetName);
        }

        /// <inheritdoc />
        public override string GetPublicVarLabel(byte varId) => varId == SourceVarId ? "source" : null;

        private BGObject FetchObject(BGCalcFlowI flow)
        {
            var sourceValue = Source;
            switch (sourceValue)
            {
                case BGCalcUnitSourceEnum.DB_Object:
                    return (BGObject)flow.GetValue(objInput);
                case BGCalcUnitSourceEnum.Index:
                    return FetchObjectByIndex(flow, flow.GetValue<int>(indexInput));
                case BGCalcUnitSourceEnum.Id:
                    return FetchObjectById(flow, flow.GetValue<BGId>(idInput));
                case BGCalcUnitSourceEnum.Name:
                    return FetchObjectByName(flow, flow.GetValue<string>(nameInput));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private object GetObject(BGCalcFlowI flow) => FetchObject(flow);

        private int GetIndex(BGCalcFlowI flow)
        {
            var obj = FetchObject(flow);
            if (obj == null) throw new Exception("Can not get object index cause object is not found!");
            return ((BGIndexableI)obj).Index;
        }

        private BGId GetId(BGCalcFlowI flow)
        {
            var obj = FetchObject(flow);
            if (obj == null) throw new Exception("Can not get object Id cause object is not found!");
            return obj.Id;
        }

        private string GetName(BGCalcFlowI flow)
        {
            var obj = FetchObject(flow);
            if (obj == null) throw new Exception("Can not get object name cause object is not found!");
            return ((BGObjectWithNameI)obj).Name;
        }

        private void OnSourceChanged()
        {
            Remove(indexInput);
            Remove(idInput);
            Remove(nameInput);
            Remove(objInput);
            indexInput = null;
            idInput = null;
            nameInput = null;
            objInput = null;
            AddRefPort();

            OnSourceChange?.Invoke();
        }

        protected void AddRefPort()
        {
            var sourceValue = Source;
            switch (sourceValue)
            {
                case BGCalcUnitSourceEnum.DB_Object:
                    objInput = ValueInput(ObjectTypeCode, ObjectTypeCode.Name, InObject);
                    break;
                case BGCalcUnitSourceEnum.Index:
                    indexInput = ValueInput(BGCalcTypeCodeRegistry.Int, "index", InIndex);
                    break;
                case BGCalcUnitSourceEnum.Id:
                    idInput = ValueInput(BGCalcTypeCodeRegistry.BGId, "id", InId);
                    break;
                case BGCalcUnitSourceEnum.Name:
                    nameInput = ValueInput(BGCalcTypeCodeRegistry.String, "name", InName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void Remove(BGCalcValueInput port)
        {
            if (port == null) return;
            if (port.IsConnected) port.DisconnectAll();
            RemovePort(port);
        }

        /// <summary>
        /// target database member type code
        /// </summary>
        protected abstract BGCalcTypeCode ObjectTypeCode { get; }

        /// <summary>
        /// find database member by name
        /// </summary>
        protected abstract BGObject FetchObjectByName(BGCalcFlowI flow, string name);

        /// <summary>
        /// find database member by ID
        /// </summary>
        protected abstract BGObject FetchObjectById(BGCalcFlowI flow, BGId id);

        /// <summary>
        /// find database member by index
        /// </summary>
        protected abstract BGObject FetchObjectByIndex(BGCalcFlowI flow, int index);
    }
}