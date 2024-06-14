/*
<copyright file="BGMetaTypeCodeFactory.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Registry for mapping table code to table type
    /// PROPOSED CHANGE: This class should be auto-generated with code generator somehow
    /// </summary>
    public class BGMetaTypeCodeFactory : BGMetaTypeCodeFactory.BGMetaTypeCodeFactoryI
    {
        public static readonly BGMetaTypeCodeFactory Instance = new BGMetaTypeCodeFactory();

        private BGMetaTypeCodeFactory()
        {
        }

        /// <summary>
        /// Create database table using provided type code and parameters
        /// </summary>
        public BGMetaEntity Create(BGRepo repo, ushort typeCode, BGId id, string name, ArraySegment<byte> config, bool system, string addon, bool nameUnique, bool singleton, bool nameEmpty)
        {
            var meta = Create(repo, typeCode, id, name);
            if (meta == null) throw new Exception($"Can not create a meta: unsupported meta type code={typeCode}!");

            meta.System = system;
            meta.UniqueName = nameUnique;
            meta.Singleton = singleton;
            meta.EmptyName = nameEmpty;
            meta.Addon = addon;

            meta.ConfigFromBytes(config);

            return meta;
        }

        /// <inheritdoc/>
        public BGMetaEntity Create(BGRepo repo, ushort typeCode, BGId id, string name)
        {
            BGMetaEntity meta;
            switch (typeCode)
            {
                case BGMetaRow.CodeType:
                {
                    meta = new BGMetaRow(repo, id, name);
                    break;
                }
                case BGMetaNested.CodeType:
                {
                    meta = new BGMetaNested(repo, id, name);
                    break;
                }
                default:
                {
                    meta = BGLocalizationUglyHacks.LocalizationMetaFactory?.Create(repo, typeCode, id, name);
                    break;
                }
            }

            return meta;
        }

        /// <summary>
        /// Custom interface, providing additional table type code to table type mapping
        /// </summary>
        public interface BGMetaTypeCodeFactoryI
        {
            /// <summary>
            /// Create database table, using provided parameters
            /// </summary>
            BGMetaEntity Create(BGRepo repo, ushort typeCode, BGId id, string name);
        }
    }
}