/*
<copyright file="BGRepoBinary.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Default binary reader/writer
    /// </summary>
    public partial class BGRepoBinary : BGRepo.RepoReaderI, BGRepo.RepoWriterI
    {
        public const int LastVersion = 8;

        /// <inheritdoc/>
        public BGRepo Read(byte[] dataBytes)
        {
            if (dataBytes == null || dataBytes.Length < 5) return new BGRepo();
            return GetReader(BGFieldInt.ValueFromBytes(new ArraySegment<byte>(dataBytes, 0, BGFieldInt.SizeOfTheValue))).Read(dataBytes);
        }

        /// <inheritdoc/>
        public byte[] Write(BGRepo repo) => GetWriter(LastVersion).Write(repo);

        private BGRepo.RepoReaderI GetReader(int version)
        {
            switch (version)
            {
                case 1:
                    return new BGRepoBinaryV1();
                case 2:
                    return new BGRepoBinaryV2();
                case 3:
                    return new BGRepoBinaryV3();
                case 4:
                    return new BGRepoBinaryV4();
                case 5:
                    return new BGRepoBinaryV5();
                case 6:
                    return new BGRepoBinaryV6();
                case 7:
                    return new BGRepoBinaryV7();
                case 8:
                    return new BGRepoBinaryV8();
                default:
                    throw new BGException("Invalid DB version: can not find a reader for database version $", version);
            }
        }

        private BGRepo.RepoWriterI GetWriter(int version) => GetReader(version) as BGRepo.RepoWriterI;
    }
}