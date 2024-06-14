/*
<copyright file="BGDnaDescriptor.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Basic class describing some dna object
    /// </summary>
    public partial class BGDnaDescriptor
    {
        public readonly string DnaName;

        protected BGDnaDescriptor(string dnaName)
        {
            if (string.IsNullOrEmpty(dnaName)) throw new BGException("Name can not be null");
            DnaName = dnaName;
        }

        public override string ToString()
        {
            return DnaName;
        }
    }
}