/*
<copyright file="BGRepoDelta.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for difference between 2 repositories
    /// </summary>
    public class BGRepoDelta
    {
        //================================================================================================
        //                                              Fields
        //================================================================================================

        public int BinaryFormatVersion { get; set; }

        private readonly BGRepoDeltaAdded added = new BGRepoDeltaAdded();
        private readonly BGRepoDeltaUpdated updated = new BGRepoDeltaUpdated();
        private readonly BGRepoDeltaDeleted deleted = new BGRepoDeltaDeleted();

        //================================================================================================
        //                                              Constructors
        //================================================================================================
        public BGRepoDelta()
        {
        }

        public BGRepoDelta(byte[] data) => Load(data);

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <summary>
        /// Compare 2 databases and store the difference between them
        /// </summary>
        public static BGRepoDelta Create(BGRepo baseRepo, BGRepo targetRepo)
        {
            var delta = new BGRepoDelta();
            delta.added.Match(baseRepo, targetRepo);
            delta.updated.Match(baseRepo, targetRepo);
            delta.deleted.Match(baseRepo, targetRepo);
            return delta;
        }

        /// <summary>
        /// Apply the changes to the target repo
        /// </summary>
        public void ApplyTo(BGRepo repo, BGModdingRepoProtection repoProtection)
        {
            added.ApplyTo(repo, repoProtection);
            updated.ApplyTo(repo, repoProtection);
            deleted.ApplyTo(repo, repoProtection);
            //we do not have to fire any other event, cause this code is executed on repo loading
            repo.Events.FireAnyChange();
        }

        //================================================================================================
        //                                              Binary
        //================================================================================================
        //delta binary file format version
        public const int LastVersion = 1;
        //ID to use for determining if the binary data is valid delta file content
        private static readonly BGId UniqueId = new BGId(5702804340146962847, 3523676889787481529);

        //---------------------- Save
        /// <summary>
        /// Save difference data as binary array
        /// </summary>
        public byte[] Save()
        {
            var builder = new BGBinaryWriter();
            Save(builder);
            return builder.ToArray();
        }

        /// <summary>
        /// Save difference data as binary array
        /// </summary>
        public void Save(BGBinaryWriter builder) => new DeltaBinary().Save(this, builder);

        //---------------------- Load
        /// <summary>
        /// Load difference data from binary array
        /// </summary>
        public static BGRepoDelta LoadStatic(byte[] data)
        {
            var repoDelta = new BGRepoDelta();
            repoDelta.Load(data);
            return repoDelta;
        }

        /// <summary>
        /// Load difference data from binary array
        /// </summary>
        public void Load(byte[] data) => Load(new BGBinaryReader(data));

        /// <summary>
        /// Load difference data from binary array
        /// </summary>
        public void Load(BGBinaryReader reader) => new DeltaBinary().Load(this, reader);

        //---------------------- Implementation
        //binary delta file manager
        private class DeltaBinary
        {
            /// <summary>
            /// Save delta content to binary stream
            /// </summary>
            public void Save(BGRepoDelta delta, BGBinaryWriter builder)
            {
                builder.AddInt(LastVersion);
                builder.AddId(UniqueId);
                delta.added.ToBinary(builder);
                delta.updated.ToBinary(builder);
                delta.deleted.ToBinary(builder);
            }

            /// <summary>
            /// Load delta content from binary stream
            /// </summary>
            public void Load(BGRepoDelta delta, BGBinaryReader reader)
            {
                if (reader.Length < 4) return;
                var version = reader.ReadInt();
                delta.BinaryFormatVersion = version;

                if (reader.Length < 20) return;
                if (reader.ReadId() != UniqueId) throw new Exception("Provided binary array is not a valid BGDatabase delta content!");

                switch (version)
                {
                    case 1:
                    {
                        delta.added.FromBinary(reader);
                        delta.updated.FromBinary(reader);
                        delta.deleted.FromBinary(reader);
                        break;
                    }
                    default:
                    {
                        throw new BGException("Can not read repo delta from binary array: unsupported version $", version);
                    }
                }
            }
        }
    }
}