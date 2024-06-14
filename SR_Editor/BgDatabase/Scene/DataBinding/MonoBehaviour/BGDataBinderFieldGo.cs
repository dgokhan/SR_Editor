/*
<copyright file="BGDataBinderFieldGo.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Inject one single value from database to a component
    /// </summary>
    [AddComponentMenu("BansheeGz/BGDataBinderFieldGo")]
    public class BGDataBinderFieldGo : BGDataBinderSingleGoA<BGDBField>
    {
        //------------- Serializable
        [SerializeField] [HideInInspector] private string targetTypeString = typeof(string).FullName;
        [SerializeField] [HideInInspector] private string metaIdString;
        [SerializeField] [HideInInspector] private string entityIdString;
        [SerializeField] [HideInInspector] private string fieldIdString;
        [SerializeField] [HideInInspector] private short functionCode;
        [SerializeField] [HideInInspector] private string functionClass;

        //live update
        [SerializeField] [HideInInspector] private bool liveUpdate;


        //------------- Non Serializable
        [NonSerialized] private Type targetType;

        [NonSerialized] private bool listenersWasAdded;

        //for liveupdate 
        [NonSerialized] private bool dirty;

        /// <summary>
        /// Target field/property type
        /// </summary>
        public string TargetTypeString => targetTypeString;

        /// <summary>
        /// Source table ID as string
        /// </summary>
        public string MetaIdString => metaIdString;

        /// <summary>
        /// Source row ID as string
        /// </summary>
        public string EntityIdString => entityIdString;

        /// <summary>
        /// Source field ID as string
        /// </summary>
        public string FieldIdString
        {
            get => fieldIdString;
            set
            {
                fieldIdString = value;
                dirty = true;
            }
        }

        /// <summary>
        /// Source table ID
        /// </summary>
        public BGId MetaId
        {
            get
            {
                var meta = Meta;
                return meta?.Id ?? BGId.Empty;
            }
        }

        /// <summary>
        /// Source table 
        /// </summary>
        public BGMetaEntity Meta
        {
            get
            {
                InjectToDelegate();
                return BindDelegate.Meta;
            }
            set
            {
                if (value == null)
                {
                    metaIdString = null;
                    entityIdString = null;
                    fieldIdString = null;
                }
                else metaIdString = value.Id.ToString();

                InjectToDelegate();
                dirty = true;
            }
        }

        /// <summary>
        /// Source row ID
        /// </summary>
        public BGId EntityId
        {
            get
            {
                var entity = Entity;
                return entity?.Id ?? BGId.Empty;
            }
            set => Entity = BGRepo.I.GetEntity(value);
        }

        /// <summary>
        /// Source row
        /// </summary>
        public BGEntity Entity
        {
            get
            {
                InjectToDelegate();
                return BindDelegate.Entity;
            }
            set
            {
                var entity = Entity;
                if (value == null && entity == null) return;
                if (value != null && entity != null && entity.Id == value.Id) return;

                if (value == null)
                {
                    metaIdString = null;
                    entityIdString = null;
                }
                else
                {
                    metaIdString = value.Meta.Id.ToString();
                    entityIdString = value.Id.ToString();
                }

                InjectToDelegate();
                dirty = true;
            }
        }

        /// <summary>
        /// Source field ID
        /// </summary>
        private BGId FieldId
        {
            get
            {
                var field = Field;
                return field?.Id ?? BGId.Empty;
            }
        }

        /// <summary>
        /// Source field
        /// </summary>

        public BGField Field
        {
            get
            {
                InjectToDelegate();
                return BindDelegate.Field;
            }
            set
            {
                if (value == null) fieldIdString = null;
                else
                {
                    fieldIdString = value.Id.ToString();
                    metaIdString = value.MetaId.ToString();
                }

                functionCode = 0;
                functionClass = null;

                InjectToDelegate();
                dirty = true;
            }
        }

        /// <summary>
        /// Target field's path
        /// </summary>
        public BGDBField.FieldPath FieldPath
        {
            get
            {
                InjectToDelegate();
                return BindDelegate.BindSourceProvider.FieldPath;
            }
        }


        /// <summary>
        /// Should data be injected again if field value is changed
        /// </summary>
        public bool LiveUpdate
        {
            get => liveUpdate;
            set => liveUpdate = value;
        }

        /// <summary>
        /// Is using special custom field (like localization current locale= $locale)
        /// </summary>
        public bool IsUsingSpecialField => BindDelegate.IsUsingSpecialField;

        /// <summary>
        /// Target field/property type
        /// </summary>
        public override Type TargetType
        {
            get
            {
                if (string.IsNullOrEmpty(targetTypeString)) return null;
                if (targetType == null || !targetType.Name.Equals(targetTypeString)) targetType = BGUtil.GetType(targetTypeString);
                return targetType;
            }
            set
            {
                if (value == null)
                {
                    targetTypeString = null;
                    targetType = null;
                }
                else
                {
                    targetType = value;
                    targetTypeString = targetType.FullName;
                }
            }
        }

        //is live-update enabled
        protected bool IsLiveUpdateOn => liveUpdate && (Application.isPlaying || BGUtil.TestIsRunning) && BindDelegate.Error == null;

        /// <summary>
        /// function code
        /// </summary>
        public short FunctionCode
        {
            get => functionCode;
            set => functionCode = value;
        }

        /// <summary>
        /// function class
        /// </summary>
        public string FunctionClass
        {
            get => functionClass;
            set => functionClass = value;
        }


        //------------- Methods
        //inject data to implementation delegate
        protected override void InjectToDelegate()
        {
            base.InjectToDelegate();

            BindDelegate.MetaIdString = metaIdString;
            BindDelegate.FieldIdString = fieldIdString;
            BindDelegate.EntityIdString = entityIdString;
            BindDelegate.FunctionCode = functionCode;
            BindDelegate.FunctionClass = functionClass;
        }

        /*private void FieldIsChanged(object sender, BGEventArgsField e)
        {
            if (e.Entity.Id != EntityId) return;
            Bind();
        }*/

        //on database loaded listener handler
        private void OnLoad(bool loaded)
        {
            if (!loaded) return;
            Bind();
        }

        //on batch update listener handler
        private void OnBatch(object sender, BGEventArgsBatch e)
        {
            if (!e.WasEntitiesUpdated(MetaId)) return;
            Bind();
        }

        public override void Bind()
        {
            var wasBound = bindedOnce;
            base.Bind();
            if (wasBound && IsLiveUpdateOn && dirty)
            {
                dirty = false;
                BindDelegate.AddFieldsListeners(Bind);
            }
        }

        /// <inheritdoc/>
        protected override void OnDestroy()
        {
            if (!listenersWasAdded) return;
            BGRepo.OnLoad -= OnLoad;
            BGRepo.I.Events.OnBatchUpdate -= OnBatch;
            BindDelegate.RemoveFieldsListeners();
        }

        //add listeners if needed
        protected override void AddListeners()
        {
            if (!IsLiveUpdateOn || listenersWasAdded) return;
            listenersWasAdded = true;
            BGRepo.OnLoad += OnLoad;
            BGRepo.I.Events.OnBatchUpdate += OnBatch;
            BindDelegate.AddFieldsListeners(Bind);
        }

        public void AssignFunction(BGFBFuntion function)
        {
            if (function == null)
            {
                FunctionCode = 0;
                FunctionClass = null;
            }
            else
            {
                if (function is BGFBFuntionToString)
                {
                    FunctionCode = BGFBFuntionToString.Code;
                    FunctionClass = null;
                }
                else
                {
                    FunctionCode = BGFBFuntion.CustomCode;
                    FunctionClass = function.GetType().FullName;
                }
            }
        }
    }
}