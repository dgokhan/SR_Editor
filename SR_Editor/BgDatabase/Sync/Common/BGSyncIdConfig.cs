/*
<copyright file="BGSyncIdConfig.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Serializable id configuration
    /// Id configuration defines, how excel/GoogleSheet rows are mapped to database entities 
    /// </summary>
    [Serializable]
    public class BGSyncIdConfig : ISerializationCallbackReceiver, BGConfigurableBinaryI
    {
        //individual table configurations
        [SerializeField] private List<BGSyncIdConfigMeta> metaConfigs = new List<BGSyncIdConfigMeta>();

        //individual table configurations, indexed by meta ID
        private Dictionary<BGId, BGSyncIdConfigMeta> metaId2Config = new Dictionary<BGId, BGSyncIdConfigMeta>();

        /// <summary>
        /// All possible ID options
        /// </summary>
        public enum IdConfigEnum : byte
        {
            IdColumn,
            NoId,
            Index,
            Field
        }

        /// <summary>
        /// number of table configurations
        /// </summary>
        public int CountMetas => metaId2Config.Count;

        //===================================================================================================================
        //                                    Serialization
        //===================================================================================================================
        /// <inheritdoc/>
        public void OnBeforeSerialize()
        {
            metaConfigs.Clear();
            var ok = BGRepo.Ok;
            foreach (var pair in metaId2Config)
            {
                if (ok && !BGRepo.I.HasMeta(pair.Key)) continue;
                metaConfigs.Add(pair.Value);
            }
        }

        /// <inheritdoc/>
        public void OnAfterDeserialize()
        {
            metaId2Config.Clear();
            foreach (var metaConfig in metaConfigs)
            {
                if (!BGId.TryParse(metaConfig.metaIdStr, out var metaId)) continue;
                metaId2Config[metaId] = metaConfig;
            }
        }

        //===================================================================================================================
        //                                    Methods
        //===================================================================================================================

        /// <summary>
        /// returns the first config error 
        /// </summary>
        public string GetError(BGRepo repo)
        {
            foreach (var metaConfig in metaId2Config)
            {
                var configMeta = metaConfig.Value;
                if (configMeta.configType != IdConfigEnum.Field) continue;
                var metaId = configMeta.MetaId;
                if (metaId == BGId.Empty) continue;
                var meta = repo.GetMeta(metaId);
                if (meta == null) continue;
                var fieldId = configMeta.FieldId;
                if (fieldId.IsEmpty) return "Field is not set for " + meta.Name;
                if (!meta.HasField(fieldId)) return "Can not find a field with ID=" + fieldId + " in meta" + meta.Name;
            }

            return null;
        }

        /// <summary>
        /// get table settings 
        /// </summary>
        public BGSyncIdConfigMeta GetMetaConfig(BGId metaId)
        {
            if (!metaId2Config.TryGetValue(metaId, out var result)) return null;
            return result;
        }

        /// <summary>
        /// ensure table settings is added
        /// </summary>
        public BGSyncIdConfigMeta EnsureMetaConfig(BGId metaId)
        {
            var result = GetMetaConfig(metaId);
            if (result == null) result = AddMetaConfig(metaId);
            return result;
        }

        /// <summary>
        /// Add table settings
        /// </summary>
        private BGSyncIdConfigMeta AddMetaConfig(BGId metaId)
        {
            var result = new BGSyncIdConfigMeta { metaIdStr = metaId.ToString() };
            metaId2Config[metaId] = result;
            return result;
        }

        /// <summary>
        /// Remove table settings
        /// </summary>
        public void RemoveMetaSetting(BGId metaId) => metaId2Config.Remove(metaId);

        /// <summary>
        /// Iterate over all tables configs
        /// </summary>
        public void ForEach(Action<BGId, BGSyncIdConfigMeta> action)
        {
            foreach (var pair in metaId2Config) action(pair.Key, pair.Value);
        }

        /// <summary>
        /// Can a given field be used as ID
        /// </summary>
        public static bool IsSupported(BGField field) => field is BGFieldString || field is BGFieldInt;

        //===================================================================================================================
        //                                    Binary serialization
        //===================================================================================================================
        /// <inheritdoc/>
        public byte[] ConfigToBytes()
        {
            OnBeforeSerialize();

            var writer = new BGBinaryWriter();

            //version
            writer.AddInt(1);

            writer.AddArray(() =>
            {
                foreach (var metaMap in metaConfigs)
                {
                    writer.AddString(metaMap.metaIdStr);
                    writer.AddString(metaMap.fieldIdStr);
                    writer.AddInt((int)metaMap.configType);
                }
            }, metaConfigs.Count);

            return writer.ToArray();
        }

        /// <inheritdoc/>
        public void ConfigFromBytes(ArraySegment<byte> config)
        {
            if (config.Count < 8) return;

            metaConfigs.Clear();
            metaId2Config.Clear();

            var reader = new BGBinaryReader(config);

            var version = reader.ReadInt();

            switch (version)
            {
                case 1:
                {
                    reader.ReadArray(() =>
                    {
                        var metaId = reader.ReadString();
                        var fieldId = reader.ReadString();
                        var configType = (IdConfigEnum)(byte)reader.ReadInt();
                        metaConfigs.Add(new BGSyncIdConfigMeta { configType = configType, fieldIdStr = fieldId, metaIdStr = metaId });
                    });
                    break;
                }
                default:
                {
                    throw new Exception("Unsupported version " + version);
                }
            }

            OnAfterDeserialize();
        }

        /// <summary>
        /// serializable data container for single table ID config
        /// </summary>
        [Serializable]
        public class BGSyncIdConfigMeta : BGObjectI
        {
            public string metaIdStr;
            public IdConfigEnum configType;
            public string fieldIdStr;

            /// <summary>
            /// Table ID
            /// </summary>
            public BGId MetaId => BGId.TryParse(metaIdStr, out var result) ? result : BGId.Empty;

            /// <summary>
            /// Table ID
            /// </summary>
            public BGId Id => MetaId;

            /// <summary>
            /// Field ID
            /// </summary>
            public BGId FieldId => BGId.TryParse(fieldIdStr, out var result) ? result : BGId.Empty;
        }
    }
}