/*
<copyright file="BGDnaMetaRow.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Creatable row meta DNA
    /// </summary>
    public partial class BGDnaMetaRow : BGDnaMetaCreatable<BGMetaRow>
    {
        public BGDnaMetaRow(BGDna dna, string dnaName) : base(dna, dnaName)
        {
        }
    }
}