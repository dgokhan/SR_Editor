/*
<copyright file="BGDataBinderSingleGoA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract unit binder (injecting one single value) 
    /// </summary>
    public abstract class BGDataBinderSingleGoA<T> : BGDataBinderGoA where T : BGDBA, new()
    {
        //-------------------------- Parameters
        //serializable
        [SerializeField] [HideInInspector] private Component targetComponent;
        [SerializeField] [HideInInspector] private string targetFieldName;
        [SerializeField] [HideInInspector] private bool isTargetProperty;
        [SerializeField] [HideInInspector] private List<PathItem> path = new List<PathItem>();
        [SerializeField] [HideInInspector] private bool includePrivate;

        //non serializable
        [NonSerialized] protected readonly T BindDelegate = new T();

        /// <inheritdoc/>        
        public override string Error
        {
            get
            {
                InjectToDelegate();
                return BindDelegate.Error;
            }
        }

        /// <summary>
        /// Path to the target field
        /// </summary>
        public List<PathItem> Path
        {
            get => path;
            set => path = value;
        }

        /// <summary>
        /// target field/property as a string value
        /// </summary>
        public string TargetAsString
        {
            get
            {
                InjectToDelegate();
                return BindDelegate.TargetAsString;
            }
        }
        
        /// <summary>
        /// target field/property as C# member
        /// </summary>
        public MemberInfo TargetAsMember
        {
            get
            {
                InjectToDelegate();
                return BindDelegate.TargetAsMember;
            }
        }

        /// <summary>
        /// Target component
        /// </summary>
        public Component TargetComponent
        {
            get => targetComponent;
            set
            {
                targetComponent = value;
                InjectToDelegate();
            }
        }

        /// <summary>
        /// Target field/property name 
        /// </summary>
        public string TargetFieldName
        {
            get => targetFieldName;
            set
            {
                targetFieldName = value;
                InjectToDelegate();
            }
        }

        /// <summary>
        /// Is target member a field or a property
        /// </summary>
        public bool IsTargetProperty
        {
            get => isTargetProperty;
            set
            {
                isTargetProperty = value;
                InjectToDelegate();
            }
        }

        /// <summary>
        /// Should private fields/properties be included as possible targets
        /// </summary>
        public bool IncludePrivate
        {
            get => includePrivate;
            set => includePrivate = value;
        }

        /// <summary>
        /// Target field/property type
        /// </summary>
        public abstract Type TargetType { get; set; }

        /// <summary>
        /// Value to inject 
        /// </summary>
        public object ValueToBind => BindDelegate.ValueToBind;

        /// <summary>
        /// Does the binder support reverse binding
        /// </summary>
        public override bool SupportReverseBinding => BindDelegate.SupportReverseBinding;


        /// <inheritdoc/>
        protected override void FirstBind()
        {
            Bind();
            AddListeners();
        }

        /// <summary>
        /// Add listeners if needed
        /// </summary>
        protected abstract void AddListeners();

        /// <inheritdoc/>
        public override void Bind()
        {
            if (!bindedOnce)
            {
                bindedOnce = true;
                FirstBind();
            }
            else
            {
                InjectToDelegate();
                LogError(BindDelegate.Bind());
            }
            FireOnBind();
        }

        /// <inheritdoc/>
        public override void ReverseBind()
        {
            if (!BindDelegate.SupportReverseBinding) return;
            InjectToDelegate();
            BindDelegate.ReverseBind();
        }

        //inject settings into delegate implementation 
        protected virtual void InjectToDelegate()
        {
            BindDelegate.TargetComponent = TargetComponent;
            BindDelegate.TargetFieldName = TargetFieldName;
            BindDelegate.IsTargetProperty = IsTargetProperty;
            BindDelegate.Path = Path;
        }
    }
}