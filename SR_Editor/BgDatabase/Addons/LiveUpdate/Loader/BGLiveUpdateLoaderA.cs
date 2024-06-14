/*
<copyright file="BGLiveUpdateLoaderA.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Abstract loader to use for LiveUpdate addon
    /// </summary>
    public abstract class BGLiveUpdateLoaderA
    {
        /// <summary>
        /// Method is called after all individual requests are complete (at the end of data loading)
        /// Can be used for delayed data loading if batching is used
        /// </summary>
        public abstract void Complete();
        /// <summary>
        /// Loads text data using parameters, provided by the context object
        /// </summary>
        public abstract void Load(LoadContext context, Action<LoadResultText> callback);
        /// <summary>
        /// Loads binary data using parameters, provided by the context object
        /// </summary>
        public abstract void Load(LoadContext context, Action<LoadResultBinary> callback);

        
        protected static void WriteToLocalFile(LoadContext context, Action<string> action)
        {
            var file = "";
            try
            {
                if (!string.IsNullOrEmpty(context.LocalFileName))
                {
                    file = Path.Combine(Application.persistentDataPath, context.LocalFileName);
                    action(file);
                    context.Log?.AddDetail("Loaded data is written to local file $", file);
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                context.Log?.AddWarning($"Can not write local file with remote data at path {file}");
            }
        }
        protected void LoadFromLocalFile(LoadContext context, ref Exception e, Action<string> action)
        {
            if (string.IsNullOrEmpty(context.LocalFileName)) return;

            var localFile = Path.Combine(Application.persistentDataPath, context.LocalFileName);
            try
            {
                if (File.Exists(localFile))
                {
                    action(localFile);
                    e = null;
                    context.Log?.AddDetail("Loading failed, but local fallback file found at path $", localFile);
                }
                else context.Log?.AddDetail($"Loading failed, local fallback file can not be found at path $", localFile);
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                context.Log?.AddWarning($"Loading failed and reading data from local fallback file at path $ also failed!", localFile);
            }
        }


        /// <summary>
        /// Abstract container for the loaded data
        /// </summary>
        public abstract class LoadResult<T>
        {
            public readonly string Error;
            public readonly T Result;

            public bool IsError => Error != null;

            public LoadResult(string error, T result)
            {
                Error = error;
                Result = result;
            }
        }

        /// <summary>
        /// Container for text result
        /// </summary>
        public class LoadResultText : LoadResult<string>
        {
            public LoadResultText(string error, string result) : base(error, result)
            {
            }
        }

        /// <summary>
        /// Container for binary result
        /// </summary>
        public class LoadResultBinary : LoadResult<byte[]>
        {
            public LoadResultBinary(string error, byte[] result) : base(error, result)
            {
            }
        }

        /// <summary>
        /// Data container for load request context
        /// </summary>
        public class LoadContext
        {
            /// <summary>
            /// Request URL
            /// </summary>
            public readonly string Url;
            /// <summary>
            /// Request HTTP Method
            /// </summary>
            public readonly BGLiveUpdateHttpMethodEnum Method;
            /// <summary>
            /// Request HTTP parameters
            /// </summary>
            public readonly List<Tuple<string, string>> httpParameters = new List<Tuple<string, string>>();
            /// <summary>
            /// Request HTTP headers
            /// </summary>
            public readonly List<Tuple<string, string>> httpHeaders = new List<Tuple<string, string>>();

            /// <summary>
            /// local file name to save last successfully loaded data
            /// </summary>
            public string LocalFileName;

            public readonly BGLiveUpdateLog Log;
            
            public LoadContext(string url, BGLiveUpdateLog log, BGLiveUpdateHttpMethodEnum method = BGLiveUpdateHttpMethodEnum.Default)
            {
                Url = url;
                Method = method;
                Log = log ?? new BGLiveUpdateLog(BGLiveUpdateLog.LogLevelEnum.Summary);
            }

            public LoadContext(string url, BGLiveUpdateLog log, BGLiveUpdateHttpMethodEnum method, List<Tuple<string, string>> httpParameters, List<Tuple<string, string>> httpHeaders)
            {
                Url = url;
                Method = method;
                Log = log ?? new BGLiveUpdateLog(BGLiveUpdateLog.LogLevelEnum.Summary);
                if (httpParameters != null) this.httpParameters.AddRange(httpParameters);
                if (httpHeaders != null) this.httpHeaders.AddRange(httpHeaders);
            }
        }
    }
}