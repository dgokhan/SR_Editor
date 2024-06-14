/*
<copyright file="BGDnaMetaLocalization.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    public partial class BGDnaMetaLocalization : BGDnaMetaCreatable<BGMetaLocalization>
    {
        private readonly Type fieldType;

        public BGDnaMetaLocalization(BGDna dna, string dnaName, Type fieldType) : base(dna, dnaName)
        {
            this.fieldType = fieldType;
        }

        protected override BGMetaEntity New(BGRepo repo, string addon)
        {
            return new BGMetaLocalization(repo, DnaName, fieldType);
        }
    }
}