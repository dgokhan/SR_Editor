/*
<copyright file="BGDnaFieldNested.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Nested field DNA
    /// </summary>
    public partial class BGDnaFieldNested : BGDnaCreatableField<List<BGEntity>, BGFieldNested>
    {
        public readonly BGDnaMetaNested NestedDnaMeta;
        public bool AutoCreated;

        public BGDnaFieldNested(BGDnaMeta metaDna, string dnaName) : base(metaDna, dnaName)
        {
            NestedDnaMeta = new BGDnaMetaNested(dnaName, metaDna);
        }

        protected override BGField New(BGMetaEntity meta, string addon)
        {
            var nestedField = AutoCreated ? (BGFieldNested)meta.GetField(DnaName) : new BGFieldNested(meta, DnaName) { Addon = addon };

            NestedDnaMeta.Meta = nestedField.NestedMeta;
            NestedDnaMeta.Meta.Addon = addon;
            ((BGMetaNested)NestedDnaMeta.Meta).OwnerRelation.Addon = addon;

            foreach (var field in NestedDnaMeta.Fields) ((BGDnaCreatable.CreatableI)field).Create(null, addon);

            return nestedField;
        }

        /// <inheritdoc />
        public override void Bind(BGMetaEntity meta)
        {
            base.Bind(meta);
            NestedDnaMeta.Bind(meta.Repo);
        }
    }
}