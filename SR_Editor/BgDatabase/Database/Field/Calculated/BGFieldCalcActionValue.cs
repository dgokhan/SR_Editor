/*
<copyright file="BGFieldCalcActionValue.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Value for action field
    /// </summary>
    public struct BGFieldCalcActionValue
    {
        private readonly Action action;

        public Action Action => action;

        public bool IsEmpty => action == null;

        public BGFieldCalcActionValue(Action action) => this.action = action;

        public static implicit operator Action(BGFieldCalcActionValue value) => value.action;

        public override string ToString() => action == null ? "No action" : "Action";

        /// <summary>
        /// run the action
        /// </summary>
        public void Invoke() => action();
    }
}