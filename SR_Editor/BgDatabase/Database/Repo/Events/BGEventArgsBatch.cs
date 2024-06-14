/*
<copyright file="BGEventArgsBatch.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;
using System.Text;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// batch event args. batch is fired then multiple changes are made
    /// </summary>
    public partial class BGEventArgsBatch : BGEventArgsA
    {
        private static readonly BGObjectPoolNTS<BGEventArgsBatch> pool = new BGObjectPoolNTS<BGEventArgsBatch>(() => new BGEventArgsBatch());
        protected override BGObjectPool Pool => pool;

        public BGRepo Repo { get; private set; }
        public bool StructureChange;
        public bool EverythingChanged;

        private readonly HashSet<BGId> metaWithAddedEntities = new HashSet<BGId>();
        private readonly HashSet<BGId> metaWithDeletedEntities = new HashSet<BGId>();
        private readonly HashSet<BGId> metaWithUpdatedEntities = new HashSet<BGId>();
        private readonly HashSet<BGId> metaWithEntitiesOrderChanged = new HashSet<BGId>();

        public bool IsEmpty => !StructureChange && !EverythingChanged
                                                && metaWithAddedEntities.Count == 0
                                                && metaWithDeletedEntities.Count == 0
                                                && metaWithUpdatedEntities.Count == 0
                                                && metaWithEntitiesOrderChanged.Count == 0;

        //to make sure instances are reused
        private BGEventArgsBatch()
        {
        }

        public static BGEventArgsBatch GetInstance(BGRepo repo)
        {
            var e = pool.Get();
            e.Clear();
            e.Repo = repo;
            return e;
        }

        /// <summary>
        /// mark meta as meta with added rows
        /// </summary>
        public void AddMetaWithAddedEntities(BGId metaId) => metaWithAddedEntities.Add(metaId);

        /// <summary>
        /// mark meta as meta with deleted rows
        /// </summary>
        public void AddMetaWithDeletedEntities(BGId metaId) => metaWithDeletedEntities.Add(metaId);

        /// <summary>
        /// mark meta as meta with updated rows
        /// </summary>
        public void AddMetaWithUpdatedEntities(BGId metaId) => metaWithUpdatedEntities.Add(metaId);

        /// <summary>
        /// mark meta as meta with  rows order changed
        /// </summary>
        public void AddMetaEntitiesOrderChanged(BGId metaId) => metaWithEntitiesOrderChanged.Add(metaId);

        /// <summary>
        /// Were rows added to the meta with provided ID
        /// </summary>
        public bool WasEntitiesAdded(BGId metaId) => EverythingChanged || metaWithAddedEntities.Contains(metaId);

        /// <summary>
        /// Were rows deleted from the meta with provided ID
        /// </summary>
        public bool WasEntitiesDeleted(BGId metaId) => EverythingChanged || metaWithDeletedEntities.Contains(metaId);

        /// <summary>
        /// Were rows updated in the meta with provided ID
        /// </summary>
        public bool WasEntitiesUpdated(BGId metaId) => EverythingChanged || metaWithUpdatedEntities.Contains(metaId);

        /// <summary>
        /// Were rows order updated in the meta with provided ID
        /// </summary>
        public bool WasEntitiesOrderChanged(BGId metaId) => EverythingChanged || metaWithEntitiesOrderChanged.Contains(metaId);

        /// <inheritdoc/>
        public override void Clear()
        {
            Repo = null;
            StructureChange = false;
            EverythingChanged = false;

            metaWithAddedEntities.Clear();
            metaWithDeletedEntities.Clear();
            metaWithUpdatedEntities.Clear();
            metaWithEntitiesOrderChanged.Clear();
        }
        public override string ToString()
        {
            return $"BGEventArgsBatch: StructureChange [{StructureChange}], EverythingChanged [{EverythingChanged}], " +
                   $"added meta Ids [{GetString(metaWithAddedEntities)}], deleted meta Ids [{GetString(metaWithDeletedEntities)}]" +
                   $"changed meta Ids [{GetString(metaWithUpdatedEntities)}]";
        }

        //hashset to string
        private static string GetString(HashSet<BGId> hashSet)
        {
            if (hashSet.Count == 0) return "None";
            var result = new StringBuilder();
            foreach (var id in hashSet)
            {
                if (result.Length != 0) result.Append('|');
                result.Append(id);
            }
            return result.ToString();
        }
    }
}