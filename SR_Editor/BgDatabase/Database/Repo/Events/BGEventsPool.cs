/*
<copyright file="BGEventsPool.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Pooled event args provider
    /// </summary>
    public static partial class BGEventsPool
    {
        /*
        private static readonly Dictionary<Type, Stack<BGEventArgsA>> Type2EventStack = new Dictionary<Type, Stack<BGEventArgsA>>();

        /// <summary>
        /// get pooled event args
        /// </summary>
        public static T Pop<T>() where T : BGEventArgsA
        {
            var type = typeof(T);

            var stack = EnsureStack(type);
            return stack.Count > 0 ? (T)stack.Pop() : BGUtil.Create<T>(type, true);
        }

        /// <summary>
        /// return  event args to the pool
        /// </summary>
        public static void Reuse(BGEventArgsA args)
        {
            if (args == null) return;
            var type = args.GetType();

            var stack = EnsureStack(type);
            stack.Push(args);
            args.Clear();
        }

        private static Stack<BGEventArgsA> EnsureStack(Type type)
        {
            if (Type2EventStack.TryGetValue(type, out var stack)) return stack;

            stack = new Stack<BGEventArgsA>();
            Type2EventStack.Add(type, stack);
            return stack;
        }
    */
    }
}