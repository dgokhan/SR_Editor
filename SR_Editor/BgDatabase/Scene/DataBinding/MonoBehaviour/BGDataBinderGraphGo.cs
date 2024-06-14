/*
<copyright file="BGDataBinderGraphGo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// The most advanced unit binder, which calculates the value to be injected using graph editor tool
    /// </summary>
    [AddComponentMenu("BansheeGz/BGDataBinderGraphGo")]
    public class BGDataBinderGraphGo : BGDataBinderSingleGoA<BGDBGraph>
    {
        //------------- Serializable
        [SerializeField] [HideInInspector] private byte typeCode = BGCalcTypeCodeString.Code;
        [SerializeField] private byte[] graphContent;

        //live update
        [SerializeField] [HideInInspector] private bool liveUpdate;

        /// <summary>
        /// serialized binary graph content 
        /// </summary>
        public byte[] GraphContent
        {
            get => graphContent;
            set => graphContent = value;
        }

        //------------- Non Serializable
        [NonSerialized] private bool listenersWasAdded;
        [NonSerialized] private bool graphIsInjected;

        /// <summary>
        /// Should data be injected again if field value is changed
        /// </summary>
        public bool LiveUpdate
        {
            get => liveUpdate;
            set => liveUpdate = value;
        }

        /// <inheritdoc/>
        public override string Error => null;

        /// <inheritdoc/>
        public override Type TargetType
        {
            get
            {
                switch (typeCode)
                {
                    case BGCalcTypeCodeBool.Code:
                        return typeof(bool);
                    case BGCalcTypeCodeString.Code:
                        return typeof(string);
                    case BGCalcTypeCodeInt.Code:
                        return typeof(int);
                    case BGCalcTypeCodeFloat.Code:
                        return typeof(float);
                    default:
                        return typeof(object);
                }
            }
            set
            {
                if (value == typeof(bool)) typeCode = BGCalcTypeCodeBool.Code;
                else if (value == typeof(string)) typeCode = BGCalcTypeCodeString.Code;
                else if (value == typeof(int)) typeCode = BGCalcTypeCodeInt.Code;
                else if (value == typeof(float)) typeCode = BGCalcTypeCodeFloat.Code;
                else typeCode = BGCalcTypeCodeObject.Code;
            }
        }

        /// <summary>
        /// graph type code
        /// </summary>
        public byte TypeCode
        {
            get => typeCode;
            set => typeCode = value;
        }

        /// <summary>
        /// graph to use for calculating value
        /// </summary>
        public BGCalcGraph Graph
        {
            get
            {
                InjectGraph();
                return BindDelegate.Graph;
            }
            set => BindDelegate.Graph = value;
        }

        //is live-update enabled
        protected bool IsLiveUpdateOn => liveUpdate && (Application.isPlaying || BGUtil.TestIsRunning) && BindDelegate.Error == null;

        /*
        protected override void FirstBind()
        {
            base.FirstBind();
        }

        private void OnDisable()
        {
            // ResetGraph();
        }
        */

        /*private void ResetGraph()
        {
            //ensure the graph is reloaded
            BindDelegate.Graph = null;
            graphIsInjected = false;
        }*/

        //------------- Methods
        /// <inheritdoc/>
        protected override void InjectToDelegate()
        {
            base.InjectToDelegate();
            BindDelegate.SetContext(gameObject);
            InjectGraph();
        }

        //database load listener handler
        private void OnLoad(bool loaded)
        {
            if (!loaded) return;
            Bind();
        }

        //on batch update listener handler
        private void OnBatch(object sender, BGEventArgsBatch e)
        {
            Bind();
        }

        //inject the graph into delegate implementation
        private void InjectGraph()
        {
            BindDelegate.LiveUpdate = LiveUpdate;
            var graph = BindDelegate.Graph;
            if (graphIsInjected && graph != null) return;
            graphIsInjected = true;
            if (graph == null)
            {
                if (graphContent != null && graphContent.Length > 0)
                {
                    graph = BGCalcGraph.ExistingGraph();
                    graph.FromBytes(new ArraySegment<byte>(graphContent));
                }
                else graph = BGCalcGraph.NewGraph(typeCode == 0 ? BGCalcTypeCodeRegistry.String : BGCalcTypeCodeRegistry.Get(typeCode));
            }

            BindDelegate.Graph = graph;
        }

        /// <inheritdoc/>
        protected override void OnDestroy()
        {
            if (!listenersWasAdded) return;
            BGRepo.OnLoad -= OnLoad;
            BGRepo.I.Events.OnBatchUpdate -= OnBatch;
            BindDelegate.RemoveFieldsListeners();
        }

        /// <inheritdoc/>
        protected override void AddListeners()
        {
            if (!IsLiveUpdateOn || listenersWasAdded) return;
            listenersWasAdded = true;
            BGRepo.OnLoad += OnLoad;
            BGRepo.I.Events.OnBatchUpdate += OnBatch;
            BindDelegate.AddFieldsListeners(Bind);
        }
    }
}