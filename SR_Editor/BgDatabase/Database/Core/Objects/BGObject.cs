/*
<copyright file="BGObject.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Abstract parent for database objects (table/fields/rows)
    /// </summary>
    public abstract partial class BGObject : BGObjectI
    {
        public event Action<BGObject> OnUnload;

        //do not make readonly (why?????)
        private BGId id;

        protected bool deleted;

        public BGId Id => id;

        /// <summary>
        /// Is this object was deleted? 
        /// </summary>
        public bool IsDeleted => deleted;

        protected BGObject(BGId id)
        {
            this.id = id;
        }

        //========================================= Equality members
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Id == ((BGObject)obj).Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return "[id:" + id + "]";
        }

        //========================================= Methods
        /// <summary>
        /// Delete this object
        /// </summary>
        public virtual void Delete() => deleted = true;

        protected internal virtual void Unload()
        {
            // if (deleted) return;
            deleted = true;
            try
            {
                OnUnload?.Invoke(this);
            }
            catch (Exception e)
            {
            }
        }
    }
}