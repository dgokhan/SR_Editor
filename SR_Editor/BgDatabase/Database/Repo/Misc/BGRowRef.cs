/*
<copyright file="BGRowRef.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Row reference (used by multi-threaded addon)
    /// </summary>
    public class BGRowRef
    {
        private readonly BGId metaId;
        private readonly BGId entityId;

        public BGId MetaId => metaId;

        public BGId EntityId => entityId;

        public BGRowRef(BGId metaId, BGId entityId)
        {
            this.metaId = metaId;
            this.entityId = entityId;
        }

        public BGRowRef(string jsonString)
        {
            var fromJson = JsonUtility.FromJson<JsonConfig>(jsonString);
            BGId.TryParse(fromJson.MetaId, out metaId);
            BGId.TryParse(fromJson.EntityId, out entityId);
        }

        public BGRowRef(BGEntity entity)
        {
            metaId = entity.MetaId;
            entityId = entity.Id;
        }

        public BGRowRef(ArraySegment<byte> segment)
        {
            metaId = new BGId(segment.Array, segment.Offset);
            entityId = new BGId(segment.Array, segment.Offset + 16);
        }
        public BGRowRef(byte[] array, int offset)
        {
            metaId = new BGId(array, offset);
            entityId = new BGId(array, offset + 16);
        }

        public BGEntity GetEntity(BGRepo repo)
        {
            var meta = repo.GetMeta(metaId);
            return meta?.GetEntity(entityId);
        }

        //================================= serialization
        [Serializable]
        private class JsonConfig
        {
            public string MetaId;
            public string EntityId;
        }

        public string ToJson()
        {
            return JsonUtility.ToJson(new JsonConfig
            {
                MetaId = metaId.ToString(),
                EntityId = entityId.ToString()
            });
        }

        public string ToString(BGRepo repo)
        {
            if (repo == null) return ToString();
            var e = GetEntity(repo);
            if (e == null) return "[not found]";
            return e.MetaName + '.' + e.Name;
        }

        public byte[] ToBytes()
        {
            var result = new byte[32];
            metaId.ToByteArray(result, 0);
            entityId.ToByteArray(result, 16);
            return result;
        }

        //================================= Equality members
        protected bool Equals(BGRowRef other)
        {
            return metaId.Equals(other.metaId) && entityId.Equals(other.entityId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BGRowRef)obj);
        }

        public override int GetHashCode()
        {
            unchecked { return (metaId.GetHashCode() * 397) ^ entityId.GetHashCode(); }
        }

        public static bool operator ==(BGRowRef left, BGRowRef right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BGRowRef left, BGRowRef right)
        {
            return !Equals(left, right);
        }

        //================================= Misc
        public bool IsMatch(BGEntity entity)
        {
            if (entity == null) return metaId.IsEmpty && entityId.IsEmpty;
            return entity.MetaId == metaId && entity.Id == entityId;
        }

        public override string ToString()
        {
            return metaId.ToString() + '.' + entityId;
        }
    }
}