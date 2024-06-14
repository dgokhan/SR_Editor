/*
<copyright file="BGDataBinderTemplateGo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Inject value, derived from a text template from database to a component
    /// Can be replaced with graph binder, which has user GUI
    /// </summary>
    [AddComponentMenu("BansheeGz/BGDataBinderTemplateGo")]
    public class BGDataBinderTemplateGo : BGDataBinderSingleGoA<BGDBTemplate>
    {
        //---------serialized

        [SerializeField] [HideInInspector] private string template;

        //live update
        [SerializeField] [HideInInspector] private bool liveUpdate;

        //---------not serialized
        private bool listenersWasAdded;

        //---------properties
        /// <summary>
        /// template to use for calculating final value
        /// </summary>
        public string Template
        {
            get => template;
            set
            {
                if (string.Equals(template, value)) return;
                template = value;
                InjectToDelegate();
            }
        }

        /// <summary>
        /// Is live update is on
        /// </summary>
        public bool LiveUpdate
        {
            get => liveUpdate;
            set => liveUpdate = value;
        }

        /// <summary>
        /// Delegate binder implementation
        /// </summary>
        public BGDBTextBinderRoot Binder => BindDelegate.Binder;


        /// <inheritdoc/>
        public override Type TargetType
        {
            get => typeof(string);
            set => throw new Exception("Target Type can not be changed for BGDataBinderTemplateGo component- it's always string type");
        }

        //is live-update on 
        protected bool IsLiveUpdateOn => liveUpdate && (Application.isPlaying || BGUtil.TestIsRunning) && BindDelegate.Error == null;

        //---------methods
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

        /// <inheritdoc/>
        protected override void InjectToDelegate()
        {
            base.InjectToDelegate();
            BindDelegate.Template = Template;
        }

        // on database load event handler 
        private void OnLoad(bool loaded)
        {
            if (!loaded) return;
            Bind();
        }

        // on batch update event handler
        private void OnBatch(object sender, BGEventArgsBatch e)
        {
            var needToUpdate = false;
            var fields = BindDelegate.Binder.Fields;
            foreach (var field in fields)
                if (e.WasEntitiesUpdated(field.MetaId))
                {
                    needToUpdate = true;
                    break;
                }

            if (!needToUpdate) return;
            Bind();
        }
    }
}