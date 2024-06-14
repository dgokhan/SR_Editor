/*
<copyright file="BGDnaCreatable.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Creatable dna has all information, required for dna data to be created for particular database.
    /// </summary>
    public partial class BGDnaCreatable : BGDna, BGDnaCreatable.CreatableI
    {
        //================================================================================================
        //                                              Create
        //================================================================================================

        /// <inheritdoc />
        public void Create(BGRepo repo, string addon)
        {
            repo.Transaction(() =>
            {
                //create
                foreach (var metaDna in Metas) ((CreatableI)metaDna).Create(repo, addon);

                foreach (var metaDna in Metas)
                foreach (var fieldDna in metaDna.Fields)
                    ((CreatableI)fieldDna).Create(repo, addon);
            });
        }

        //================================================================================================
        //                                              Delete
        //================================================================================================
        /// <summary>
        /// Removes tables/fields, described by this dna from particular database 
        /// </summary>
        public virtual void Delete(BGRepo repo)
        {
            repo.Transaction(() =>
            {
                foreach (var dnaMeta in Metas)
                {
                    var meta = repo.GetMeta(dnaMeta.DnaName);
                    meta?.Delete();
                }
            });
        }

        /// <summary>
        /// Creatable dna
        /// </summary>
        public interface CreatableI
        {
            /// <summary>
            /// Create tables/fields, described by this dna in particular database 
            /// </summary>
            void Create(BGRepo repo, string addon);
        }
    }
}