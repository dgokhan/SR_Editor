using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    public partial class BGAddonPartition
    {
        /// <inheritdoc />
        public override byte[] ConfigToBytes()
        {
            // CheckIfValid();
            var writer = new BGBinaryWriter();

            //version
            writer.AddInt(2);

            writer.AddBool(disableTemporarily);

            //v2
            writer.AddBool(disableHorizontalTemporarily);
            writer.AddBool(disableVerticalTemporarily);

            return writer.ToArray();
        }


        /// <inheritdoc />
        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    disableTemporarily = reader.ReadBool();
                    break;
                }
                case 2:
                {
                    disableTemporarily = reader.ReadBool();
                    disableHorizontalTemporarily = reader.ReadBool();
                    disableVerticalTemporarily = reader.ReadBool();
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        /// <inheritdoc />
        public override string ConfigToString()
        {
            // CheckIfValid();
            var configToString = JsonUtility.ToJson(new Settings
            {
                DisableTemporarily = disableTemporarily,
                DisableHTemporarily = disableHorizontalTemporarily,
                DisableVTemporarily = disableVerticalTemporarily,
            });
            return configToString;
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            var fromJson = JsonUtility.FromJson<Settings>(config);
            disableTemporarily = fromJson.DisableTemporarily;
            disableHorizontalTemporarily = fromJson.DisableHTemporarily;
            disableVerticalTemporarily = fromJson.DisableVTemporarily;
        }

        [Serializable]
        private class Settings
        {
            public bool DisableTemporarily;
            public bool DisableHTemporarily;
            public bool DisableVTemporarily;
        }

        internal void CheckConfig()
        {
            if (EnabledVertical)
            {
                var structure = new BGPartitionVerticalStructure(Repo);
                var meta2Partition = new Dictionary<BGId, BGEntity>();
                structure.ForEachPartition(partition =>
                {
                    structure.ForEachMeta(partition, meta =>
                    {
                        if (meta2Partition.ContainsKey(meta.Id))
                        {
                            var oldPartition = BGUtil.Get(meta2Partition, meta.Id);
                            if(oldPartition.Equals(partition)) throw new Exception($"Invalid vertical partitioning config: meta {meta.Name} was included twice to partition: {oldPartition.Name}");
                            throw new Exception($"Invalid vertical partitioning config: meta {meta.Name} was included to two different partitions: {oldPartition.Name} and {partition.Name}");
                        }
                        meta2Partition.Add(meta.Id, partition);
                    });
                });
            }
        }
    }
}