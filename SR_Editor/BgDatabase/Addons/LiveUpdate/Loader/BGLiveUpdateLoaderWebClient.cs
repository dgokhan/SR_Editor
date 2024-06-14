/*
<copyright file="BGLiveUpdateLoaderWebClient.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Synchronous loader, using WebClient
    /// </summary>
    public class BGLiveUpdateLoaderWebClient : BGLiveUpdateLoaderA
    {
        private readonly MyWebClient client;

        public BGLiveUpdateLoaderWebClient(int timeOut) => client = new MyWebClient { Timeout = timeOut };

        /// <summary>
        /// Load result as binary array 
        /// </summary>
        public void LoadBinary(LoadContext context, Action<LoadResultBinary> callback) => Load(context, callback);

        /// <summary>
        /// Load result as string 
        /// </summary>
        public void LoadText(LoadContext context, Action<LoadResultText> callback) => Load(context, callback);

        /// <inheritdoc />
        public override void Load(LoadContext context, Action<LoadResultText> callback)
        {
            string result = null;
            Exception ex = null;
            try
            {
                switch (context.Method)
                {
                    case BGLiveUpdateHttpMethodEnum.Default:
                        result = client.DownloadString(context.Url);
                        break;
                    case BGLiveUpdateHttpMethodEnum.Get:
                    case BGLiveUpdateHttpMethodEnum.Post:
                        result = LoadString(context, context.Method);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(context.Method));
                }

                WriteToLocalFile(context, file => File.WriteAllText(file, result));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                LoadFromLocalFile(context, ref e, file => result = File.ReadAllText(file));
                ex = e;
            }
            finally
            {
                callback(ex == null ? new LoadResultText(null, result) : new LoadResultText(ex.Message ?? "unknown error " + ex.GetType().FullName, null));
            }
        }

        /// <inheritdoc />
        public override void Load(LoadContext context, Action<LoadResultBinary> callback)
        {
            byte[] result = null;
            Exception ex = null;
            try
            {
                switch (context.Method)
                {
                    case BGLiveUpdateHttpMethodEnum.Default:
                        result = client.DownloadData(context.Url);
                        break;
                    case BGLiveUpdateHttpMethodEnum.Get:
                    case BGLiveUpdateHttpMethodEnum.Post:
                        result = LoadByteArray(context, context.Method);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(context.Method));
                }

                WriteToLocalFile(context, file => File.WriteAllBytes(file, result));
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                LoadFromLocalFile(context, ref e, file => result = File.ReadAllBytes(file));
                ex = e;
            }
            finally
            {
                callback(ex == null ? new LoadResultBinary(null, result) : new LoadResultBinary(ex.Message ?? "unknown error " + ex.GetType().FullName, null));
            }
        }

        //load result as string
        private string LoadString(LoadContext context, BGLiveUpdateHttpMethodEnum method)
        {
            var response = LoadByteArray(context, method);
            var loadString = Encoding.UTF8.GetString(response);
            return loadString;
        }

        //load result as byte array
        private byte[] LoadByteArray(LoadContext context, BGLiveUpdateHttpMethodEnum method)
        {
            if (context.httpHeaders != null && context.httpHeaders.Count > 0)
            {
                foreach (var httpHeader in context.httpHeaders) client.Headers[httpHeader.Item1] = httpHeader.Item2;
            }

            byte[] response;
            switch (method)
            {
                case BGLiveUpdateHttpMethodEnum.Get:
                    client.QueryString.Clear();
                    if (context.httpParameters != null && context.httpParameters.Count > 0)
                    {
                        foreach (var httpParameter in context.httpParameters) client.QueryString.Add(httpParameter.Item1, httpParameter.Item2);
                    }

                    response = client.DownloadData(context.Url);
                    client.QueryString.Clear();
                    break;
                case BGLiveUpdateHttpMethodEnum.Post:
                {
                    NameValueCollection parameters = null;
                    if (context.httpParameters != null && context.httpParameters.Count > 0)
                    {
                        parameters = new NameValueCollection();
                        foreach (var httpParameter in context.httpParameters) parameters[httpParameter.Item1] = httpParameter.Item2;
                    }

                    response = client.UploadValues(context.Url, "POST", parameters);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(method), method, $"Unsupported HTTP method {method}");
            }

            return response;
        }

        /// <inheritdoc />
        public override void Complete() => client?.Dispose();

        //standard webclient with timeout support
        public class MyWebClient : WebClient
        {
            public int Timeout { private get; set; }

            protected override WebRequest GetWebRequest(Uri uri)
            {
                var request = base.GetWebRequest(uri);
                if (request == null) return null;
                request.Timeout = Timeout;
                ((HttpWebRequest)request).ReadWriteTimeout = Timeout;
                return request;
            }
        }
    }
}