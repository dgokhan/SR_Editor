/*
<copyright file="BGFieldCalcA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// base abstract class for calculated field
    /// </summary>
    public abstract class BGFieldCalcA<T> : BGFieldDictionaryBasedA<T, BGFieldCalcValue>, BGFieldCalcI
    {
        //================================================================================================
        //                                              Fields
        //================================================================================================
        private BGCalcGraph graph;

        /// <inheritdoc/>
        public BGCalcGraph Graph
        {
            get => graph;
            set
            {
                if (graph == value) return;
                SetGraphNoEvent(value);
                FireMetaChanged();
            }
        }

        private void SetGraphNoEvent(BGCalcGraph value)
        {
            if (graph != null) graph.OnAnyChange -= FireMetaChanged;
            graph = value;
            if (graph != null) graph.OnAnyChange += FireMetaChanged;
        }

        private void FireMetaChanged()
        {
            Meta.Repo.Events.MetaWasChanged(Meta);
        }


        /// <inheritdoc/>
        public override bool ReadOnly => true;

        /// <inheritdoc/>
        public override bool CustomStringFormatSupported => false;

        /// <inheritdoc/>
        public override bool StoredValueIsTheSameAsValueType => false;

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGFieldCalcA(BGMetaEntity meta, string name) : base(meta, name)
        {
        }

        //for existing fields
        protected BGFieldCalcA(BGMetaEntity meta, BGId id, string name) : base(meta, id, name)
        {
        }


        //================================================================================================
        //                                              Config serialization
        //================================================================================================
        /// <inheritdoc/>
        public override string ConfigToString()
        {
            if (Graph == null) return null;
            var config = new JsonConfig
            {
                Graph = new BGCalcSaverString(graph).Save()
            };
            return JsonUtility.ToJson(config);
        }

        /// <inheritdoc/>
        public override void ConfigFromString(string config)
        {
            if (string.IsNullOrEmpty(config)) return;
            var configObj = JsonUtility.FromJson<JsonConfig>(config);
            if (configObj == null || string.IsNullOrEmpty(configObj.Graph)) return;
            SetGraphNoEvent(BGCalcGraph.ExistingGraph());
            Graph.FromJsonString(configObj.Graph);
        }

        /// <inheritdoc/>
        public override byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(Graph == null ? 8 : 1024);

            //version
            writer.AddInt(1);

            //has default graph?
            writer.AddBool(Graph != null);

            //serialize graph
            if (Graph != null) writer.AddByteArray(Graph.ToBytes());

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    var hasGraph = reader.ReadBool();
                    if (hasGraph)
                    {
                        SetGraphNoEvent(BGCalcGraph.ExistingGraph());
                        Graph.FromBytes(reader.ReadByteArray());
                    }

                    break;
                }
                default:
                    throw new BGException("Unsupported version: $", version);
            }
        }

        [Serializable]
        private class JsonConfig
        {
            public string Graph;
        }

        //================================================================================================
        //                                              PREVENT SET
        //================================================================================================
        /// <inheritdoc/>
        public override T this[BGId entityId]
        {
            set
            {
                //field is readonly!
            }
        }

        /// <inheritdoc/>
        public override T this[int index]
        {
            set
            {
                //field is readonly!
            }
        }

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <inheritdoc/>
        public abstract BGCalcTypeCode ResultCode { get; }

        /// <inheritdoc/>
        //set never called - so this method never called 
        protected override BGFieldCalcValue Convert(BGEntity entity, T value) => throw new NotImplementedException();

        /// <inheritdoc/>
        protected override T Convert(BGEntity entity, BGFieldCalcValue value)
        {
            //launch value graph
            if (value?.Graph != null) return Run(value.Graph, this,entity);

            //launch field graph
            if (Graph != null) return Run(Graph, this, entity, value);

            return default;
        }

        //execute provided graph abd return calculation result
        private static T Run(BGCalcGraph graph, BGField field, BGEntity entity, BGCalcVarsProvider varsOverrides = null)
        {
            var context = BGCalcFlowContext.Get();
            try
            {
                var result = Run(context, graph, field, entity, varsOverrides);
                if (result == null) return default;
                return (T) result;
            }
            finally
            {
                BGCalcFlowContext.Return(context);
            }
        }
        //execute provided graph abd return calculation result
        public static object Run( BGCalcFlowContext context, BGCalcGraph graph, BGField field, BGEntity entity, BGCalcVarsProvider varsOverrides = null)
        {
            if (context.ContainsCell(field, entity)) throw new Exception($"Recursive call is detected- field {field.FullName} for row {entity.FullName} is called more than once!");
            context.Graph = graph;
            context.CurrentEntity = entity;
            context.VarsOverrides = varsOverrides;
            context.GraphType = BGCalcGraphTypeEnum.CalculatedField;
            context.AddCell(field, entity);
            return graph.Execute(context);
            
        }

        //================================================================================================
        //                                              Serialization
        //================================================================================================

        protected override byte[] ValueToBytes(BGFieldCalcValue value) => value?.ToBytes(Graph);

        protected override BGFieldCalcValue ValueFromBytes(int entityIndex, ArraySegment<byte> segment)
        {
            if (segment.Count == 0) return null;
            var result = new BGFieldCalcValue(this, Meta.GetEntity(entityIndex));
            result.FromBytes(segment, Graph);
            return result;
        }

        protected override string ValueToString(BGFieldCalcValue value) => value?.ToJsonString(Graph);

        protected override BGFieldCalcValue ValueFromString(int entityIndex, string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            var result = new BGFieldCalcValue(this, Meta.GetEntity(entityIndex));
            result.FromJsonString(value, Graph);
            return result;
        }
    }
}