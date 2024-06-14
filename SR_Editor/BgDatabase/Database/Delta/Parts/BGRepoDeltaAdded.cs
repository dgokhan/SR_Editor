/*
<copyright file="BGRepoDeltaAdded.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


namespace BansheeGz.BGDatabase
{
    //wrapper class for added members
    internal class BGRepoDeltaAdded
    {
        private readonly BGRepo added = new BGRepo();

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <summary>
        /// Find any added members and store them in internal state
        /// </summary>
        public void Match(BGRepo repo, BGRepo targetRepo)
        {
            added.Clear();

            BGRepoDeltaUtils.ForEachMatchingMeta(repo, targetRepo, (meta, targetMeta) =>
            {
                if (targetMeta.CountEntities == 0) return;

                var addedIds = BGRepoDeltaUtils.Except(targetMeta, meta);
                if (addedIds.Count == 0) return;

                var myMeta = Create(meta, targetMeta);

                var newEntities = new BGEntity[addedIds.Count];
                for (var i = 0; i < addedIds.Count; i++) newEntities[i] = myMeta.NewEntity(addedIds[i]);

                BGRepoDeltaUtils.ForEachMatchingField(myMeta, targetMeta, (field, targetField) =>
                {
                    for (var i = 0; i < addedIds.Count; i++)
                    {
                        var myEntity = newEntities[i];
                        var targetEntity = targetMeta.GetEntity(addedIds[i]);
                        //can it be ?? probably not
                        if (targetEntity == null) continue;

                        field.CopyValue(targetField, targetEntity.Id, targetEntity.Index, myEntity.Id);
                    }
                });
            });
        }

        private BGMetaEntity Create(BGMetaEntity meta, BGMetaEntity targetMeta)
        {
            var myMeta = BGRepoDeltaUtils.CreateMeta(added, meta);

            BGRepoDeltaUtils.ForEachMatchingField(meta, targetMeta, (field, targetField) =>
            {
                var myField = BGRepoDeltaUtils.CreateField(myMeta, field);
            });
            return myMeta;
        }

        /// <summary>
        /// Apply the changes to the repo
        /// </summary>
        public void ApplyTo(BGRepo repo, BGModdingRepoProtection repoProtection)
        {
            BGRepoDeltaUtils.ForEachMatchingMeta(added, repo, (meta, targetMeta) =>
            {
                if (repoProtection != null && repoProtection.IsAddDisabled(meta.Id)) return;

                var countEntities = meta.CountEntities;
                if (countEntities == 0) return;


                var newEntities = new BGEntity[countEntities];
                for (var i = 0; i < countEntities; i++)
                {
                    var newEntityId = meta.GetEntity(i).Id;
                    var newEntity = targetMeta.GetEntity(newEntityId) ?? targetMeta.NewEntity(newEntityId);
                    newEntities[i] = newEntity;
                }

                BGRepoDeltaUtils.ForEachMatchingField(meta, targetMeta, (field, targetField) =>
                {
                    for (var i = 0; i < countEntities; i++)
                    {
                        var fromEntity = meta.GetEntity(i);
                        var toEntity = newEntities[i];
                        targetField.CopyValue(field, fromEntity.Id, fromEntity.Index, toEntity.Id);
                    }
                });
            });
        }

        //================================================================================================
        //                                              Binary
        //================================================================================================
        private const int LastVersion = 1;

        public void ToBinary(BGBinaryWriter builder)
        {
            builder.AddInt(LastVersion);
            builder.AddByteArray(added.Save());
        }

        public void FromBinary(BGBinaryReader reader)
        {
            added.Clear();
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    added.Load(BGRepoDeltaUtils.ToArray(reader.ReadByteArray()));
                    break;
                }
                default:
                {
                    throw new BGException("Can not read repo delta deleted from binary array: unsupported version $", version);
                }
            }
        }
    }
}