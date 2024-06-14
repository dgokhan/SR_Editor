/*
<copyright file="BGFieldCalcVarRef.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Reference to the variable from default field graph
    /// Used for overriding variables values
    /// </summary>
    public class BGFieldCalcVarRef : BGObjectI
    {
        private readonly VarRefContainer container;
        private BGId id;
        private object value;

        /// <inheritdoc/>
        public BGId Id => id;

        /// <summary>
        /// variable value
        /// </summary>
        public object Value
        {
            get => value;
            set
            {
                if (Equals(this.value, value)) return;
                this.value = value;
                Container.FireAnyChange();
            }
        }

        /// <summary>
        /// reference to the parent variables container
        /// </summary>
        public VarRefContainer Container => container;

        private BGFieldCalcVarRef(VarRefContainer container)
        {
            this.container = container;
            container.Add(this);
        }

        private BGFieldCalcVarRef(VarRefContainer container, BGId id)
        {
            this.container = container;
            this.id = id;
            container.Add(this);
        }

        /// <summary>
        /// Clone this variable to provided container
        /// </summary>
        public BGFieldCalcVarRef CloneTo(VarRefContainer newContainer)
        {
            return new BGFieldCalcVarRef(newContainer, Id)
            {
                Value = Value
            };
        }

        /// <summary>
        /// new container factory method
        /// </summary>
        public static VarRefContainer NewContainer() => new VarRefContainer();

        //================Equals
        protected bool Equals(BGFieldCalcVarRef other) => id.Equals(other.id) && Equals(value, other.value);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BGFieldCalcVarRef)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (id.GetHashCode() * 397) ^ (value != null ? value.GetHashCode() : 0); }
        }

        public static bool operator ==(BGFieldCalcVarRef left, BGFieldCalcVarRef right) => Equals(left, right);

        public static bool operator !=(BGFieldCalcVarRef left, BGFieldCalcVarRef right) => !Equals(left, right);

        //================CONTAINER

        /// <summary>
        /// Container with variables overrides
        /// </summary>
        public class VarRefContainer
        {
            private readonly List<BGFieldCalcVarRef> vars = new List<BGFieldCalcVarRef>();

            public int Count => vars.Count;

            /// <summary>
            /// get variable value at specified index
            /// </summary>
            public BGFieldCalcVarRef this[int i] => vars[i];

            internal VarRefContainer()
            {
            }

            /// <summary>
            /// remove variable at specified index
            /// </summary>
            public void RemoveAt(int i)
            {
                vars.RemoveAt(i);
                FireAnyChange();
            }

            /// <summary>
            /// add variable 
            /// </summary>
            public void Add(BGFieldCalcVarRef varRef)
            {
                vars.Add(varRef);
                FireAnyChange();
            }

            /// <summary>
            /// iterate over all variables
            /// </summary>
            public void ForEach(Action<BGFieldCalcVarRef> action)
            {
                for (var i = 0; i < vars.Count; i++) action(vars[i]);
            }

            /// <summary>
            /// new variable with specified ID and value
            /// </summary>
            public BGFieldCalcVarRef NewVar(BGId id, object value)
            {
                return new BGFieldCalcVarRef(this, id)
                {
                    Value = value
                };
            }

            /// <summary>
            /// new variable with specified ID 
            /// </summary>
            public BGFieldCalcVarRef NewVar(BGId id) => new BGFieldCalcVarRef(this, id);

            /// <summary>
            /// fire any change event
            /// </summary>
            public void FireAnyChange() => OnAnyChange?.Invoke();

            /// <summary>
            /// any change event
            /// </summary>
            public event Action OnAnyChange;

            //================Equals
            protected bool Equals(VarRefContainer other)
            {
                if (vars.Count != other.vars.Count) return false;
                for (var i = 0; i < vars.Count; i++)
                {
                    var varRef = vars[i];
                    var otherVarRef = other.vars[i];
                    if (!Equals(varRef, otherVarRef)) return false;
                }

                return true;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((VarRefContainer)obj);
            }

            public override int GetHashCode()
            {
                return vars != null ? vars.GetHashCode() : 0;
            }
        }
    }
}