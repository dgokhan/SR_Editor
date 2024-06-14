/*
<copyright file="BGLiveUpdateUrls.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for all the URLs, used for updating tables data
    /// </summary>
    [Serializable]
    public partial class BGLiveUpdateUrls : BGConfigurableBinaryI
    {
        //the list of URLs, which will be used for updating tables data
        public List<BGLiveUpdateUrl> urls;

        private BGAddonLiveUpdate addon;

        public BGAddonLiveUpdate Addon
        {
            get => addon;
            set
            {
                addon = value;
                //reassign Urls field references to "this" for each URL
                if (urls != null)
                    foreach (var url in urls)
                        url.Urls = this;
            }
        }

        public BGLiveUpdateUrls()
        {
        }

        public BGLiveUpdateUrls(BGAddonLiveUpdate addon)
        {
            this.addon = addon;
        }

        //================================================================================================
        //                                              Config
        //================================================================================================

        /// <inheritdoc />
        public byte[] ConfigToBytes()
        {
            var writer = new BGBinaryWriter(1024);

            //version
            writer.AddInt(2);

            //urls
            writer.AddArray(() =>
            {
                foreach (var url in urls)
                {
                    writer.AddString(url.URL);
                    writer.AddInt((int)url.URLType);
                    writer.AddString(url.MetaId);

                    //added in v.2
                    writer.AddByte( (byte) url.HttpMethod);
                    WriteParameters(writer, url.HttpParameters);
                    WriteParameters(writer, url.HttpHeaders);
                }
            }, urls?.Count ?? 0);

            return writer.ToArray();
        }

        /// <inheritdoc />
        public void ConfigFromBytes(ArraySegment<byte> config)
        {
            if (urls != null) urls.Clear();
            else urls = new List<BGLiveUpdateUrl>();

            var reader = new BGBinaryReader(config);

            var version = reader.ReadInt();
            switch (version)
            {
                case 1:
                {
                    reader.ReadArray(() =>
                    {
                        urls.Add(new BGLiveUpdateUrl(this, reader.ReadString(), (BGLiveUpdateUrlTypeEnum)reader.ReadInt(), reader.ReadString()));
                    });
                    break;
                }
                case 2:
                {
                    reader.ReadArray(() =>
                    {
                        var liveUpdateUrl = new BGLiveUpdateUrl(this, reader.ReadString(), (BGLiveUpdateUrlTypeEnum)reader.ReadInt(), reader.ReadString());
                        urls.Add(liveUpdateUrl);
                        
                        //added in v.2
                        liveUpdateUrl.HttpMethod = (BGLiveUpdateHttpMethodEnum) reader.ReadByte();
                        ReadParameters(reader, (key, value) => liveUpdateUrl.AddHttpParameter(key, value));
                        ReadParameters(reader, (key, value) => liveUpdateUrl.AddHttpHeader(key, value));
                    });
                    break;
                }
                default:
                {
                    throw new ArgumentException("wrong version=" + version);
                }
            }
        }

        //write HTTP parameters/headers attributes to binary writer
        private static void WriteParameters<T>(BGBinaryWriter writer, List<T> parameters) where T: BGLiveUpdateUrl.ParameterWithGraph
        {
            writer.AddArray(() =>
            {
                foreach (var postParameter in parameters)
                {
                    writer.AddString(postParameter.Name);
                    writer.AddString(postParameter.Value);
                    writer.AddBool(postParameter.Graph != null);
                    if (postParameter.Graph != null) writer.AddByteArray(postParameter.Graph.ToBytes());
                }
            }, parameters?.Count ?? 0);

        }
        
        //read HTTP parameters/headers attributes from binary reader
        private static void ReadParameters(BGBinaryReader reader, Func<string, string, BGLiveUpdateUrl.ParameterWithGraph> factory)
        {
            reader.ReadArray(() =>
            {
                var httpParameter = factory(reader.ReadString(), reader.ReadString());
                var hasGraph = reader.ReadBool();
                if (hasGraph)
                {
                    httpParameter.Graph = BGCalcGraph.ExistingGraph();
                    httpParameter.Graph.FromBytes(reader.ReadByteArray());
                }
            });
        }

        //================================================================================================
        //                                              Methods
        //================================================================================================
        /// <summary>
        /// Removes an URL with specified index from the list of URLs 
        /// </summary>
        public void DeleteUrl(int index)
        {
            if (urls == null || index >= urls.Count || index < 0) throw new Exception("Can not delete at specified index " + index + ", number of URLs is  " + (urls?.Count ?? 0));
            urls.RemoveAt(index);
            FireEvent();
        }

        /// <summary>
        /// Adds an URL to the list of URLs 
        /// </summary>
        public BGLiveUpdateUrl AddUrl()
        {
            urls = urls ?? new List<BGLiveUpdateUrl>();
            var result = new BGLiveUpdateUrl();
            urls.Add(result);
            return result;
        }

        //fires addon's changed event
        internal void FireEvent()
        {
            addon?.FireChange();
        }

        /// <summary>
        /// Clones this URLs object to another addon 
        /// </summary>
        public BGLiveUpdateUrls CloneTo(BGAddonLiveUpdate addon)
        {
            var clone = new BGLiveUpdateUrls(addon);

            if (urls == null || urls.Count <= 0) return clone;

            clone.urls = new List<BGLiveUpdateUrl>(urls.Count);
            for (var i = 0; i < urls.Count; i++)
            {
                var url = urls[i];
                clone.urls.Add(url.CloneTo(clone));
            }

            return clone;
        }
    }
}