/*
<copyright file="BGCalcUnitDbRowBasedA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract row-based unit
    /// </summary>
    public abstract class BGCalcUnitDbRowBasedA : BGCalcUnitDbMetaBasedA
    {
        public static readonly byte SourceVarId = 2;

        public Action OnSourceChange;

        public const string IndexId = "a";
        public const string IdId = "b";
        public const string NameId = "c";
        public const string EntityId = "d";
        private BGCalcValueInput indexInput;
        private BGCalcValueInput idInput;
        private BGCalcValueInput nameInput;
        private BGCalcValueInput entityInput;

        public BGCalcValueInput IndexInput => indexInput;

        public BGCalcValueInput IdInput => idInput;

        public BGCalcValueInput NameInput => nameInput;

        public BGCalcValueInput EntityInput => entityInput;

        /// <summary>
        /// Row's source
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
                OnSourceChange?.Invoke();
            }
        }

        /// <inheritdoc />
        public override string Title
        {
            get
            {
                var meta = Meta;
                if (meta == null) return $"DB {Operation} [ERROR:meta not found]";
                return $"DB {Operation} [" + meta.Name + "]";
            }
        }

        /// <summary>
        /// operation name
        /// </summary>
        protected abstract string Operation { get; }

        /// <inheritdoc />
        public override void Definition()
        {
            var meta = Meta;
            if (meta == null) throw new Exception("Meta is not found! id=" + MetaId);

            AddRefPort(meta);

            ValueOutput(new BGCalcTypeCodeEntityRuntime(Meta), "entity", "e", GetEntity);
            ValueOutput(BGCalcTypeCodeRegistry.Int, "index", "f", flow => GetEntity(flow).Index);
            ValueOutput(BGCalcTypeCodeRegistry.BGId, "id", "g", flow => GetEntity(flow).Id);

            GetVar(SourceVarId).OnValueChange += OnSourceChanged;
        }

        /// <inheritdoc />
        public override void Init(BGId metaId)
        {
            base.Init(metaId);

            var sourceVar = BGCalcVarLite.Create(this, SourceVarId, BGCalcTypeCodeRegistry.EntitySource);
            sourceVar.Value = BGCalcUnitSourceEnum.DB_Object;
        }

        /// <inheritdoc />
        public override string GetPublicVarLabel(byte varId) => varId == SourceVarId ? "source" : null;

        private void OnSourceChanged()
        {
            var meta = Meta;
            if (meta == null) throw new Exception("Meta is not found! id=" + MetaId);

            Remove(indexInput);
            Remove(idInput);
            Remove(nameInput);
            Remove(entityInput);
            indexInput = null;
            idInput = null;
            nameInput = null;
            entityInput = null;
            AddRefPort(meta);

            OnSourceChange?.Invoke();
        }

        private void AddRefPort(BGMetaEntity meta)
        {
            var sourceValue = Source;
            switch (sourceValue)
            {
                case BGCalcUnitSourceEnum.DB_Object:
                    entityInput = ValueInput(new BGCalcTypeCodeEntityRuntime(meta), "entity", EntityId);
                    break;
                case BGCalcUnitSourceEnum.Index:
                    indexInput = ValueInput(BGCalcTypeCodeRegistry.Int, "index", IndexId);
                    break;
                case BGCalcUnitSourceEnum.Id:
                    idInput = ValueInput(BGCalcTypeCodeRegistry.BGId, "id", IdId);
                    break;
                case BGCalcUnitSourceEnum.Name:
                    nameInput = ValueInput(BGCalcTypeCodeRegistry.String, "name", NameId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Remove(BGCalcValueInput port)
        {
            if (port == null) return;
            if (port.IsConnected) port.DisconnectAll();
            RemovePort(port);
        }

        protected BGEntity GetEntity(BGCalcFlowI flow)
        {
            var meta = Meta;
            if (meta == null) throw new Exception("Meta is not found! id=" + MetaId);

            BGEntity result;
            var sourceValue = Source;
            switch (sourceValue)
            {
                case BGCalcUnitSourceEnum.DB_Object:
                    result = flow.GetValue<BGEntity>(entityInput);
                    if (result == null) throw new Exception($"Entity is not set!");
                    break;
                case BGCalcUnitSourceEnum.Index:
                    var index = flow.GetValue<int>(indexInput);
                    result = meta.GetEntity(index);
                    if (result == null) throw new Exception($"Can not find an [{meta.Name}] entity using index={index}!");
                    break;
                case BGCalcUnitSourceEnum.Id:
                    var id = flow.GetValue<BGId>(idInput);
                    result = meta.GetEntity(id);
                    if (result == null) throw new Exception($"Can not find an [{meta.Name}] entity using id={id}!");
                    break;
                case BGCalcUnitSourceEnum.Name:
                    var name = flow.GetValue<string>(nameInput);
                    result = meta.GetEntity(name);
                    if (result == null) throw new Exception($"Can not find an [{meta.Name}] entity using name={name}!");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return result;
        }
    }
}