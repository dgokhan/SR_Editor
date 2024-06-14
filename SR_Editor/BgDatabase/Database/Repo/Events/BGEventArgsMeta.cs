/*
<copyright file="BGEventArgsMeta.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// table event args. 
    /// </summary>
    public partial class BGEventArgsMeta : BGEventArgsA
    {
        private static readonly BGObjectPoolNTS<BGEventArgsMeta> pool = new BGObjectPoolNTS<BGEventArgsMeta>(() => new BGEventArgsMeta());
        protected override BGObjectPool Pool => pool;

        public enum OperationEnum
        {
            Update,
            Add,
            Delete
        }

        /// <summary>
        /// database table
        /// </summary>
        public BGMetaEntity Meta { get; private set; }
        
        /// <summary>
        /// database view
        /// </summary>
        public BGMetaView View { get; private set; }
        
        /// <summary>
        /// operation performed
        /// </summary>
        public OperationEnum Operation { get; private set; }

        //to make sure instances are reused
        private BGEventArgsMeta()
        {
        }

        public static BGEventArgsMeta GetInstance(OperationEnum operation, BGMetaEntity meta)
        {
            var e = pool.Get();
            e.Meta = meta;
            e.View = null;
            e.Operation = operation;
            return e;
        }
        public static BGEventArgsMeta GetInstance(OperationEnum operation, BGMetaView view)
        {
            var e = pool.Get();
            e.Meta = null;
            e.View = view;
            e.Operation = operation;
            return e;
        }

        /// <inheritdoc/>
        public override void Clear()
        {
            Meta = null;
            View = null;
            Operation = OperationEnum.Update;
        }

        public override string ToString() => $"BGEventArgsMeta: meta [{Meta}], view [{View}], operation [{Operation}]";
    }
}