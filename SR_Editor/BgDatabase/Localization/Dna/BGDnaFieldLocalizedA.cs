/*
<copyright file="BGDnaFieldLocalizedA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    public abstract partial class BGDnaFieldLocalizedA<T, TF> : BGDnaCreatableField<T, TF> where TF : BGField<T>
    {
        protected readonly BGDnaMetaLocalization LocaleDnaMeta;

        public BGDnaFieldLocalizedA(BGDnaMeta metaDna, string dnaName, BGDnaMetaLocalization localeDnaMeta) : base(metaDna, dnaName)
        {
            LocaleDnaMeta = localeDnaMeta;
        }
    }
}