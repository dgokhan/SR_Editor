/*
<copyright file="BGDataBinderBatchGo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Compound binder, containing of lists of the following unit binders- field/template/graph binders 
    /// </summary>
    [AddComponentMenu("BansheeGz/BGDataBinderBatchGo")]
    public class BGDataBinderBatchGo : BGDataBinderGoA
    {
        private static readonly StringBuilder error = new StringBuilder();

        //serialized
        [SerializeField] [HideInInspector] private bool showResults;
        [SerializeField] [HideInInspector] private List<BGDBField> fieldBinders;
        [SerializeField] [HideInInspector] private List<BGDBTemplate> templateBinders;
        [SerializeField] [HideInInspector] private List<BGDBGraph> graphBinders;
        private int listenersCount;

        //non serialized

        public List<BGDBField> FieldBinders => fieldBinders;

        public List<BGDBTemplate> TemplateBinders => templateBinders;

        public List<BGDBGraph> GraphBinders => graphBinders;

        public bool ShowResults => showResults;

        public override string Error
        {
            get
            {
                error.Length = 0;
                var count = 0;
                if (fieldBinders != null)
                    foreach (var binder in fieldBinders)
                    {
                        var binderError = binder.Error;
                        if (!string.IsNullOrEmpty(binderError))
                        {
                            if (count != 0) error.Append(Environment.NewLine);
                            error.Append(++count).Append(") ").Append(binderError);
                        }
                    }

                if (templateBinders != null)
                    foreach (var binder in templateBinders)
                    {
                        var binderError = binder.Error;
                        if (!string.IsNullOrEmpty(binderError))
                        {
                            if (count != 0) error.Append(Environment.NewLine);
                            error.Append(++count).Append(") ").Append(binderError);
                        }
                    }

                //graph binders do not return error

                return error.Length == 0 ? null : error.ToString();
            }
        }

        public override void Bind()
        {
            if (!bindedOnce)
            {
                bindedOnce = true;
                FirstBind();
            }
            else
            {
                if (fieldBinders != null)
                    foreach (var binder in fieldBinders)
                        LogError(binder.Bind());

                if (templateBinders != null)
                    foreach (var binder in templateBinders)
                        LogError(binder.Bind());
                if (graphBinders != null)
                    foreach (var binder in graphBinders)
                        LogError(binder.Bind());
            }
            FireOnBind();
        }

        public override void ReverseBind()
        {
            if (fieldBinders != null)
                foreach (var binder in fieldBinders)
                    binder.ReverseBind();
            //template & graph binders do not support binding
        }

        protected override void FirstBind()
        {
            var isRunning = Application.isPlaying || BGUtil.TestIsRunning;
            listenersCount = 0;
            if (fieldBinders != null)
                foreach (var binder in fieldBinders)
                {
                    var binderError = binder.Bind();
                    LogError(binderError);
                    if (binder.LiveUpdate && binderError == null && isRunning) listenersCount += binder.AddFieldsListeners(() => binder.Bind());
                }

            if (templateBinders != null)
                foreach (var binder in templateBinders)
                {
                    var binderError = binder.Bind();
                    LogError(binderError);
                    if (binder.LiveUpdate && binderError == null && isRunning) listenersCount += binder.AddFieldsListeners(() => binder.Bind());
                }

            if (graphBinders != null)
                foreach (var binder in graphBinders)
                {
                    binder.SetContext(gameObject, listenersCount == 0);
                    var binderError = binder.Bind();
                    LogError(binderError);
                    if (binder.LiveUpdate && binderError == null && isRunning) listenersCount += binder.AddFieldsListeners(() => binder.Bind());
                }

            if (listenersCount > 0)
            {
                BGRepo.OnLoad += OnLoad;
                BGRepo.I.Events.OnBatchUpdate += OnBatch;
            }
        }

        private void OnLoad(bool loaded)
        {
            if (!loaded) return;
            Bind();
        }

        private void OnBatch(object sender, BGEventArgsBatch e)
        {
            // if (listenersCount == 0) return;
            Bind();

            /*
            foreach (var binder in fieldBinders)
            {
                if (!binder.LiveUpdate) continue;
                // Maybe it makes sense to check  if meta id is changed (!e.WasEntitiesUpdated(monitor.MetaId)) continue;
                LogError(binder.Bind());
            }
            foreach (var binder in templateBinders)
            {
                if (!binder.LiveUpdate) continue;
                LogError(binder.Bind());
            }
        */
        }


        protected override void OnDestroy()
        {
            if (listenersCount == 0 || !Application.isPlaying && !BGUtil.TestIsRunning) return;

            BGRepo.OnLoad -= OnLoad;
            BGRepo.I.Events.OnBatchUpdate -= OnBatch;

            if (fieldBinders != null)
                foreach (var binder in fieldBinders)
                {
                    if (!binder.LiveUpdate) continue;
                    binder.RemoveFieldsListeners();
                }

            if (templateBinders != null)
                foreach (var binder in templateBinders)
                {
                    if (!binder.LiveUpdate) continue;
                    binder.RemoveFieldsListeners();
                }

            if (graphBinders != null)
                foreach (var binder in graphBinders)
                {
                    if (!binder.LiveUpdate) continue;
                    binder.RemoveFieldsListeners();
                }

            listenersCount = 0;
        }

        public List<BGDBField> EnsureFieldBinders() => fieldBinders ?? (fieldBinders = new List<BGDBField>());
        public List<BGDBTemplate> EnsureTemplateBinders() => templateBinders ?? (templateBinders = new List<BGDBTemplate>());
        public List<BGDBGraph> EnsureGraphBinders() => graphBinders ?? (graphBinders = new List<BGDBGraph>());

    }
}