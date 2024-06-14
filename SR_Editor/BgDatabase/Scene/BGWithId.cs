/*
<copyright file="BGWithId.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// component for adding id to GameObject
    /// Original idea comes from this Unity project https://github.com/Unity-Technologies/guid-based-reference
    /// </summary>
    [AddComponentMenu("BansheeGz/Id")]
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public partial class BGWithId : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private string IdString;
        private BGId id;

        /// <summary>
        /// Used ID for this component
        /// </summary>
        public BGId Id
        {
            get => id;
            set
            {
                Remove(this);
                id = value;
                IdString = value.ToString();
                Add(this);
            }
        }

        //=============================== Methods
        /// <summary>
        /// Assign new ID for this component
        /// </summary>
        public void NewId() => Id = BGId.NewId;

        //=============================== Serialization
        /// <inheritdoc />
        public void OnBeforeSerialize()
        {
            IdString = id.ToString();
        }

        /// <inheritdoc />
        public void OnAfterDeserialize()
        {
            id = BGId.Parse(IdString);
        }

        //=============================== Unity Callbacks
        private void Awake()
        {
            Add(this);
        }

        private void OnDestroy()
        {
            Remove(this);
        }

        private void OnEnable()
        {
            //on reloading assembly Awake is not called in Editor  
            if (!Application.isEditor || Application.isPlaying) return;
            var list = GetAll(id);
            if (list != null && list.Any(t => t == this)) return;
            Add(this);
        }

        private void Reset()
        {
            Id = BGId.NewId;
        }

        //=============================== Static methods
        //static data container for fast components lookup 
        private static readonly Dictionary<BGId, List<BGWithId>> id2Component = new Dictionary<BGId, List<BGWithId>>();

        /// <summary>
        /// Retrieve the component by its ID
        /// </summary>
        public static BGWithId Get(BGId id)
        {
            var list = BGUtil.Get(id2Component, id);
            if (list == null || list.Count == 0) return null;
            return list[0];
        }

        /// <summary>
        /// Retrieve all the components with provided ID
        /// </summary>
        public static List<BGWithId> GetAll(BGId id)
        {
            var list = BGUtil.Get(id2Component, id);
            if (list == null || list.Count == 0) return null;
            return list;
        }

        /// <summary>
        /// Adds the components to the cache for faster retrieval
        /// </summary>
        private static void Add(BGWithId withId)
        {
            if (withId == null || withId.Id.IsEmpty) return;
            if (!id2Component.TryGetValue(withId.Id, out var list))
            {
                list = new List<BGWithId>();
                id2Component[withId.Id] = list;
            }

            list.Add(withId);
        }

        /// <summary>
        /// Removes the component from the cache 
        /// </summary>
        private static void Remove(BGWithId withId)
        {
            if (withId == null || withId.Id.IsEmpty) return;
            if (!id2Component.TryGetValue(withId.Id, out var list)) return;
            if (list == null) return;

            //do not replace with foreach or RemoveAll(predicate)- it's performance critical!
            for (var i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];
                if (item != withId) continue;
                list.RemoveAt(i);
            }

            if (list.Count == 0) id2Component.Remove(withId.Id);
        }
    }
}