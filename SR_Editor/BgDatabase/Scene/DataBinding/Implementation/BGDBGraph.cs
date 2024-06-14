/*
<copyright file="BGDBGraph.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// implementation class for the graph binder
    /// </summary>
    [Serializable]
    public class BGDBGraph : BGDBA
    {
        //-------------------- serializable
        //field
        [SerializeField] private string graphContent;
        [SerializeField] private byte typeCode = BGCalcTypeCodeString.Code;

        //-------------------- Non serializable
        private BGCalcGraph graph;
        private BGCalcFlowEvents events;
        private bool addBatchListeners;

        /// <inheritdoc/>
        public override bool SupportReverseBinding => false;

        /// <summary>
        /// graph result type code
        /// </summary>
        public byte TypeCode
        {
            get
            {
                if (typeCode == 0) typeCode = BGCalcTypeCodeRegistry.String.TypeCode;
                return typeCode;
            }
            set => typeCode = value == 0 ? BGCalcTypeCodeRegistry.String.TypeCode : value;
        }

        /// <summary>
        /// initialized graph to use
        /// </summary>
        public BGCalcGraph Graph
        {
            get => graph;
            set => graph = value;
        }

        /// <summary>
        /// serialized graph content 
        /// </summary>
        public string GraphContent
        {
            get => graphContent;
            set => graphContent = value;
        }

        /// <summary>
        /// owner GamaObject 
        /// </summary>
        public GameObject Owner { get; private set; }

        /// <inheritdoc/>
        public override Type TargetType => typeCode == 0 ? typeof(string) : BGCalcTypeCodeRegistry.Get(typeCode).Type;

        /// <inheritdoc/>
        public override object ValueToBind
        {
            get
            {
                error = null;
                EnsureTarget();
                if (error != null) return null;

                return GetValue();
            }
        }

        public override object GetValue()
        {
            RemoveFieldsListeners();

            var myGraph = Graph ?? EnsureGraph();
            try
            {
                //EVENTS
                if (LiveUpdate && (Application.isPlaying || BGUtil.TestIsRunning))
                {
                    //we need events
                    if (events == null) events = new BGCalcFlowEvents(Bind);
                    events.AddBatchListeners = addBatchListeners;
                }
                else events = null;


                var context = BGCalcFlowContext.Get();
                try
                {
                    context.CurrentGameObject = Owner;
                    context.Events = events;
                    context.GraphType = BGCalcGraphTypeEnum.GraphBinder;
                    var flow = myGraph.Launch(context);
                    events?.AddListeners();
                    return flow.Result;
                }
                finally
                {
                    BGCalcFlowContext.Return(context);
                }
            }
            catch (Exception e)
            {
                // Debug.LogException(e);
                var unitData = e.Data[BGCalcFlow.UnitExceptionKey] as string;
                error = "Graph throws exception" + (unitData == null ? ":" : " at [" + unitData + "] unit: ") + e.Message;
                return null;
            }
        }

        /// <summary>
        /// make sure the graph is initialized 
        /// </summary>
        public BGCalcGraph EnsureGraph()
        {
            if (graph != null) return graph;
            if (string.IsNullOrEmpty(graphContent))
            {
                graph = BGCalcGraph.NewGraph(BGCalcTypeCodeRegistry.Get(TypeCode));
                return graph;
            }

            graph = BGCalcGraph.ExistingGraph();
            graph.FromBytes(new ArraySegment<byte>(Convert.FromBase64String(graphContent)));
            return graph;
        }


        public BGDBGraph()
        {
        }


        /// <inheritdoc/>
        public override string ReverseBind()
        {
            //not supported
            return null;
        }

        /// <inheritdoc/>
        public override int AddFieldsListeners(Action action)
        {
            return 0;
        }

        /// <inheritdoc/>
        public override void RemoveFieldsListeners()
        {
            events?.Clear();
        }

        /// <summary>
        /// set the owner GameObject
        /// </summary>
        public void SetContext(GameObject owner) => Owner = owner;

        /// <summary>
        /// set the owner GameObject
        /// </summary>
        public void SetContext(GameObject owner, bool addBatchListeners)
        {
            SetContext(owner);
            this.addBatchListeners = addBatchListeners;
        }
    }
}