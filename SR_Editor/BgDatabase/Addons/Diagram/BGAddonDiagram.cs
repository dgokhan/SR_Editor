/*
<copyright file="BGAddonDiagram.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Diagram addon. Let you view your database structure as a graph.
    /// See  <a href="http://www.bansheegz.com/BGDatabase/Addons/Diagram/">this link</a> for more details.
    /// </summary>
    [AddonDescriptor(Name = "Diagram", ManagerType = "BansheeGz.BGDatabase.Editor.BGAddonManagerDiagram")]
    public class BGAddonDiagram : BGAddon
    {
        private readonly Dictionary<BGId, DiagramMetaData> metaId2Data = new Dictionary<BGId, DiagramMetaData>();
        private readonly List<DiagramMetaData> dataList = new List<DiagramMetaData>();
        private byte[] configArray;
        private int configVersion;
        private bool suppressEvents;

        //====================================== overrides
        public override BGAddon CloneTo(BGRepo repo)
        {
            var clone = new BGAddonDiagram
            {
                configArray = configArray,
                configVersion = configVersion
            };
            foreach (var metaData in dataList) clone.Add(metaData.metaId, metaData.X, metaData.Y);
            return clone;
        }

        //====================================== config
        /// <inheritdoc />
        public override string ConfigToString()
        {
            InitFromArray();
            var metas = new List<JsonConfigMeta>();
            foreach (var data in dataList)
            {
                metas.Add(new JsonConfigMeta
                {
                    metaId = data.metaId.ToString(),
                    X = data.X,
                    Y = data.Y,
                });
            }

            return JsonUtility.ToJson(new JsonConfig
            {
                metas = metas
            });
        }

        /// <inheritdoc />
        public override void ConfigFromString(string config)
        {
            configArray = null;
            Clear();
            var json = JsonUtility.FromJson<JsonConfig>(config);
            if (json?.metas != null)
            {
                foreach (var meta in json.metas)
                {
                    if (!BGId.TryParse(meta.metaId, out var id)) continue;
                    Add(id, meta.X, meta.Y);
                }
            }
        }

        [Serializable]
        private class JsonConfig
        {
            public List<JsonConfigMeta> metas = new List<JsonConfigMeta>();
        }

        [Serializable]
        private class JsonConfigMeta
        {
            public string metaId;
            public float X;
            public float Y;
        }

        public override byte[] ConfigToBytes()
        {
            InitFromArray();
            var arrayLength = 4 + (16 + 4 + 4) * dataList.Count;
            var writer = new BGBinaryWriter(4 + 4 + arrayLength);

            //version
            writer.AddInt(1);

            //data
            var writer2 = new BGBinaryWriter(arrayLength);
            writer2.AddArray(() =>
            {
                foreach (var data in dataList)
                {
                    writer2.AddId(data.metaId);
                    writer2.AddFloat(data.X);
                    writer2.AddFloat(data.Y);
                }
            }, dataList.Count);
            writer.AddByteArray(writer2.ToArray());


            return writer.ToArray();
        }

        public override void ConfigFromBytes(ArraySegment<byte> config)
        {
            Clear();
            var reader = new BGBinaryReader(config);
            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    //graph nodes are initialized on demand
                    configVersion = version;
                    configArray = BGUtil.ToArray(reader.ReadByteArray());
                    break;
                }
                default:
                {
                    throw new BGException("Unknown version: $", version);
                }
            }
        }

        private void InitFromArray()
        {
            if (configArray == null) return;
            try
            {
                suppressEvents = true;
                dataList.Clear();
                metaId2Data.Clear();
                switch (configVersion)
                {
                    case 1:
                    {
                        var reader = new BGBinaryReader(configArray);
                        reader.ReadArray(() =>
                        {
                            var id = reader.ReadId();
                            var x = reader.ReadFloat();
                            var y = reader.ReadFloat();
                            Add(id, x, y);
                        });
                        break;
                    }
                    default:
                    {
                        throw new BGException("Unknown version: $", configVersion);
                    }
                }

                configArray = null;
            }
            finally
            {
                suppressEvents = false;
            }
        }

        //====================================== methods
        private DiagramMetaData Add(BGId metaId, float x, float y)
        {
            var data = new DiagramMetaData(this, metaId, x, y);
            dataList.Add(data);
            metaId2Data[data.metaId] = data;
            return data;
        }

        public DiagramMetaData Get(BGId metaId)
        {
            InitFromArray();
            return BGUtil.Get(metaId2Data, metaId);
        }

        public DiagramMetaData Ensure(BGId metaId)
        {
            // InitFromArray is called in Get method
            var result = Get(metaId);
            if (result != null) return result;
            result = Add(metaId, 0, 0);
            return result;
        }

        public void Clear()
        {
            InitFromArray();
            dataList.Clear();
            metaId2Data.Clear();
        }

        //====================================== nested classes
        public class DiagramMetaData
        {
            public readonly BGAddonDiagram addon;
            public readonly BGId metaId;
            private float x;
            private float y;

            public float X
            {
                get => x;
                set
                {
                    if (x == value) return;
                    x = value;
                    if (!addon.suppressEvents) addon.FireChange();
                }
            }

            public float Y
            {
                get => y;
                set
                {
                    if (y == value) return;
                    y = value;
                    if (!addon.suppressEvents) addon.FireChange();
                }
            }

            public DiagramMetaData(BGAddonDiagram addon, BGId metaId)
            {
                this.addon = addon;
                this.metaId = metaId;
            }

            public DiagramMetaData(BGAddonDiagram addon, BGId metaId, float x, float y) : this(addon, metaId)
            {
                X = x;
                Y = y;
            }
        }
    }
}