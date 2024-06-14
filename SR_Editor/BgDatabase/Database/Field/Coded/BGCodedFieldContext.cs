/*
<copyright file="BGCodedFieldContext.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// context object to be used by programmable field to calculate the value
    /// </summary>
    public class BGCodedFieldContext : IDisposable
    {
        private static readonly BGObjectPool<BGCodedFieldContext> cellsPool = new BGObjectPool<BGCodedFieldContext>(() => new BGCodedFieldContext());

        private BGField field;
        private BGEntity entity;

        public BGField Field
        {
            get => field;
            set => field = value;
        }

        public BGEntity Entity
        {
            get => entity;
            set => entity = value;
        }

        private BGCodedFieldContext()
        {
        }

        public static BGCodedFieldContext Get() => cellsPool.Get();
        public void Dispose()
        {
            field = null;
            entity = null;
            cellsPool.Return(this);
        }
    }
}