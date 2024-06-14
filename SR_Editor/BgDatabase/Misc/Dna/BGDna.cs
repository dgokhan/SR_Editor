/*
<copyright file="BGDna.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// THIS IS NOT CURRENTLY USED
    /// Dna relates to one of code generation methods.
    /// This class represent Dna for set of metas
    /// </summary>
    public partial class BGDna
    {
        protected readonly List<BGDnaMeta> Metas = new List<BGDnaMeta>();


        public bool IsObsolete
        {
            get
            {
                var metasCount = Metas.Count;
                for (var i = 0; i < metasCount; i++)
                {
                    var metaDna = Metas[i];
                    if (metaDna.Meta == null || metaDna.Meta.IsDeleted) return true;
                }

                return false;
            }
        }

        //================================================================================================
        //                                              Add
        //================================================================================================

        /// <summary>
        /// Add meta dna
        /// </summary>
        public virtual void Add(BGDnaMeta meta)
        {
            Metas.Add(meta);
        }

        //================================================================================================
        //                                              Bind
        //================================================================================================

        /// <summary>
        /// bind all meta dna to some particular database
        /// </summary>
        public void Bind(BGRepo repo)
        {
            foreach (var metaDna in Metas) metaDna.Bind(repo);
        }

        //================================================================================================
        //                                              Base class
        //================================================================================================
    }
}