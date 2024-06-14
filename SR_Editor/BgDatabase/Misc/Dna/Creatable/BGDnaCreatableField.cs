/*
<copyright file="BGDnaCreatableField.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Creatable field dna
    /// </summary>
    public partial class BGDnaCreatableField<T, TF> : BGDnaField<T>, BGDnaCreatable.CreatableI where TF : BGField<T>
    {
        public BGDnaCreatableField(BGDnaMeta metaDna, string dnaName) : base(metaDna, dnaName)
        {
        }
        //================================================================================================
        //                                              Create
        //================================================================================================

        /// <inheritdoc />
        public virtual void Create(BGRepo repo, string addon)
        {
            Field = New(MetaDna.Meta, addon);
            Field.Addon = addon;
        }

        //create a field from this DNA
        protected virtual BGField New(BGMetaEntity meta, string addon)
        {
            return BGUtil.Create<TF>(typeof(TF), false, meta, DnaName);
        }
    }
}