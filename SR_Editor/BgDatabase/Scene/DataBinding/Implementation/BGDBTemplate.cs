/*
<copyright file="BGDBTemplate.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// implementation class for the template binder 
    /// </summary>
    [Serializable]
    public class BGDBTemplate : BGDBA
    {
        //reusable text processor
        private static readonly BGDBTextProcessor TextProcessor = new BGDBTextProcessor();

        //--------serialized
        //template
        [SerializeField] private string template;

        //--------non serialized
        private BGDBTextBinderRoot binder;


        /// <summary>
        /// Template to use for calculating final value 
        /// </summary>
        public string Template
        {
            get => template;
            set => template = value;
        }

        /// <summary>
        /// template root processor
        /// </summary>
        public BGDBTextBinderRoot Binder => binder;

        /// <inheritdoc/>
        public override bool SupportReverseBinding => false;

        /// <inheritdoc/>
        public override Type TargetType => typeof(string);

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
            if (binder == null || !string.Equals(binder.Template, template)) binder = TextProcessor.Process(template);

            if (binder.Error != null)
            {
                error = binder.Error;
                return null;
            }

            var result = binder.Bind();
            if (binder.Error != null) error = binder.Error;
            return result;
        }

        /// <inheritdoc/>
        public override int AddFieldsListeners(Action action)
        {
            RemoveFieldsListeners();
            var fields = Binder.Fields;
            fields.ForEach(info =>
            {
                eventHandlers.Add(new FieldEventHandler(info.MetaId, info.FieldId, info.EntityId, action));
            });
            return eventHandlers.Count;
        }
    }
}