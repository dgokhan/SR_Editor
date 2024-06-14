/*
<copyright file="BGDnaMeta.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Dna for meta (table)
    /// </summary>
    public partial class BGDnaMeta : BGDnaDescriptor
    {
        public readonly List<BGDnaField> Fields = new List<BGDnaField>();

        private BGMetaEntity meta;

        public BGMetaEntity Meta
        {
            get => meta;
            set => meta = value;
        }

        public T MetaAs<T>() where T : BGMetaEntity
        {
            return (T)meta;
        }

        private readonly BGDna dna;

        public BGDna Dna => dna;

        public T DnaAs<T>() where T : BGDna
        {
            return (T)dna;
        }

        //================================================================================================
        //                                              Constructor
        //================================================================================================

        protected BGDnaMeta(BGDna dna, string dnaName) : base(dnaName)
        {
            this.dna = dna;
            dna?.Add(this);

            //we can create fields with null dnaMeta to reduce number of lines
            var fieldType = typeof(BGDnaField);
            foreach (var field in GetType().GetFields())
            {
                if (!field.IsPublic) continue;
                if (!fieldType.IsAssignableFrom(field.FieldType)) continue;

                var fieldObject = (BGDnaField)field.GetValue(this);
                if (fieldObject == null || fieldObject.MetaDna != null) continue;

                fieldObject.MetaDna = this;
            }
        }

        //================================================================================================
        //                                              Helper methods
        //================================================================================================

        public BGEntity Get(BGId entityId)
        {
            return meta[entityId];
        }


        //================================================================================================
        //                                              Bind
        //================================================================================================

        /// <summary>
        /// Binds to specific database
        /// </summary>
        public virtual void Bind(BGRepo repo)
        {
            var meta = repo.GetMeta(DnaName);
            this.meta = meta ?? throw new BGException("Error while dna binding: Can not find meta with name ($)", DnaName);
            foreach (var field in Fields) field.Bind(meta);
        }
    }
}