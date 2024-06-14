/*
<copyright file="BGDataBinderGoA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract data binder for all the binders
    /// </summary>
    public abstract class BGDataBinderGoA : MonoBehaviour
    {
        /// <summary>
        /// called after the binding 
        /// </summary>
        public event Action OnBind;


        /// <summary>
        /// When binding should take place, Manual-meaning binder is executed manually by calling the Bind method 
        /// </summary>
        public enum SourceMethodEnum
        {
            Start,
            Awake,
            Manual
        }

        //-------------------------- Parameters

        [SerializeField] [HideInInspector] private bool ignoreErrors;

        /// <summary>
        /// Ignore possible errors, meaning no error messages are printed to the Unity console
        /// </summary>
        public bool IgnoreErrors
        {
            get => ignoreErrors;
            set => ignoreErrors = value;
        }

        //target

        // Unity method  
        [SerializeField] [HideInInspector] private SourceMethodEnum sourceMethod = SourceMethodEnum.Start;

        //if the binder was executed at least once
        [NonSerialized] protected bool bindedOnce;

        /// <summary>
        /// When binding should be executed
        /// </summary>
        public SourceMethodEnum SourceMethod
        {
            get => sourceMethod;
            set => sourceMethod = value;
        }

        /// <summary>
        /// Is there any error in binder settings?
        /// </summary>
        public abstract string Error { get; }

        /// <summary>
        /// Does this binder support reverse binding? Reverse binding means the the value from Unity component is injected to the database
        /// </summary>
        public virtual bool SupportReverseBinding => true;

        //========================================== Unity callbacks
        private void Awake()
        {
            if (sourceMethod != SourceMethodEnum.Awake) return;
            bindedOnce = true;
            FirstBind();
        }

        private void Start()
        {
            if (sourceMethod != SourceMethodEnum.Start) return;
            bindedOnce = true;
            FirstBind();
        }

        /// <summary>
        /// On destroy Unity callback
        /// </summary>
        protected abstract void OnDestroy();

        //========================================== Methods
        /// <summary>
        /// Binding for the first time, can be used for attaching listeners
        /// </summary>
        protected abstract void FirstBind();

        /// <summary>
        /// Bind the values. The value(s) from database is injected to Unity component(s)
        /// </summary>
        public abstract void Bind();

        /// <summary>
        /// Reverse binding means  the value from Unity component is injected to the database
        /// </summary>
        public abstract void ReverseBind();

        //utility method for printing the error to the console
        protected void LogError(string error) => LogError(this, error);

        /// <summary>
        /// utility method for printing the error to the console for provided binder
        /// </summary>
        public static void LogError(BGDataBinderGoA binder, string error)
        {
            if (string.IsNullOrEmpty(error)) return;
            if (binder == null)
            {
                if (!string.IsNullOrEmpty(error)) Debug.LogError("BGDatabase.UnknownBinder error [" + error + "]");
                return;
            }

            if (binder.IgnoreErrors) return;

            Debug.LogError("BGDatabase." + binder.GetType().Name + " binder error at [" + binder.Path + "] GameObject: [" + error +
                           "]. You can disable this message by 1) fixing the error or 2) toggling 'ignoreErrors' toggle on at target dataBinder");
        }

        //path to the Unity GameObject
        private string Path
        {
            get
            {
                var obj = gameObject;
                var path = "/" + obj.name;
                while (obj.transform.parent != null)
                {
                    obj = obj.transform.parent.gameObject;
                    path = "/" + obj.name + path;
                }

                return path;
            }
        }

        protected void FireOnBind() => OnBind?.Invoke();


        //this method is used to prevent certain properties/fields from code stripping (Is it really working?)
        private void Unused_DoNotCallIt_PreventIosStripping()
        {
            GetComponent<SpriteRenderer>().sprite = null;
            GetComponent<Material>().mainTexture = null;
            GetComponent<MeshRenderer>().sharedMaterial = null;
            GetComponent<MeshRenderer>().sharedMaterial.mainTexture = null;
            // GetComponent<Text>().text = null;
            GetComponent<TextMesh>().text = null;
            // GetComponent<Image>().sprite = null;
            GetComponent<AudioSource>().clip = null;
        }

        /// <summary>
        /// The serializable pointer to the target field/property
        /// </summary>
        [Serializable]
        public class PathItem
        {
            public string Field;
            public bool IsProperty;

            //===================================   equality members
            protected bool Equals(PathItem other) => string.Equals(Field, other.Field) && IsProperty == other.IsProperty;

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((PathItem)obj);
            }

            public override int GetHashCode()
            {
                unchecked { return ((Field != null ? Field.GetHashCode() : 0) * 397) ^ IsProperty.GetHashCode(); }
            }

            public override string ToString() => Field;
        }
    }
}