/*
<copyright file="BGCalcVarContainerBaseA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// abstract variables owner (container) for variables of type T
    /// </summary>
    public abstract class BGCalcVarContainerBaseA<T> where T : BGCalcVarA
    {
        protected readonly List<T> vars = new List<T>();
        private readonly BGCalcVarsOwnerBaseI owner;

        protected BGCalcVarContainerBaseA(BGCalcVarsOwnerBaseI owner) => this.owner = owner;

        /// <summary>
        /// get all variables
        /// </summary>
        public List<T> Variables => vars;

        /// <summary>
        /// variables count
        /// </summary>
        public int Count => vars.Count;

        /// <summary>
        /// add variables
        /// </summary>
        public void AddVar(T variable)
        {
            if (vars.Count >= byte.MaxValue) throw new Exception($"Can not add a variable: maximum number of variables={byte.MaxValue} is reached");
            vars.Add(variable);
            FireOnAnyChange();
        }

        /// <summary>
        /// get variable at provided index
        /// </summary>
        public T GetVar(int index) => vars[index];

        /// <summary>
        /// remove all variables and fire event
        /// </summary>
        public void ClearVars()
        {
            ClearVarsNoEvent();
            FireOnAnyChange();
        }

        /// <summary>
        /// remove all variables without firing event
        /// </summary>
        public void ClearVarsNoEvent() => vars.Clear();

        /// <summary>
        /// fire variables change event
        /// </summary>
        public void FireOnAnyChange() => owner.OnVarsChange();
        
        //================================================================================================
        //                                              equals
        //================================================================================================
        protected bool Equals(BGCalcVarContainerBaseA<T> other)
        {
            if (vars.Count != other.vars.Count) return false;
            for (var i = 0; i < vars.Count; i++)
            {
                var variable = vars[i];
                var variable2 = other.vars[i];
                if (!Equals(variable, variable2)) return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BGCalcVarContainerBaseA<T>)obj);
        }

        public override int GetHashCode()
        {
            return vars != null ? vars.GetHashCode() : 0;
        }

        public static bool IsEqual(BGCalcVarContainerBaseA<T> left, BGCalcVarContainerBaseA<T> right)
        {
            var leftIsEmpty = left == null || left.vars.Count == 0;
            var rightIsEmpty = right == null || right.vars.Count == 0;
            if (leftIsEmpty && rightIsEmpty) return true;
            if (leftIsEmpty || rightIsEmpty) return false;
            return left.Equals(right);
        }
    }
}