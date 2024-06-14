/*
<copyright file="BGLiveUpdateLoaderUnityWebRequest.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
#if !BG_SA
using Object = UnityEngine.Object;
#endif

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Asynchronous loader for WebGl, using UnityWebRequest
    /// </summary>
    public class BGLiveUpdateLoaderUnityWebRequest : BGLiveUpdateLoaderA
    {
        private static GameObject go;

        private readonly Action done;
        private readonly List<LoadContext> textRequests = new List<LoadContext>();
        private readonly List<Action<LoadResultText>> textRequestsCallbacks = new List<Action<LoadResultText>>();
        private readonly List<LoadContext> binaryRequests = new List<LoadContext>();
        private readonly List<Action<LoadResultBinary>> binaryRequestsCallbacks = new List<Action<LoadResultBinary>>();
        private readonly int timeout;

        private int count;

        /// <summary>
        /// GameObject, used for running coroutine
        /// </summary>
        private static GameObject Go
        {
            get
            {
                if (go != null) return go;
                go = new GameObject("BGDatabaseLiveUpdateLoader");
                Object.DontDestroyOnLoad(go);
                return go;
            }
        }

        public BGLiveUpdateLoaderUnityWebRequest(int timeout, Action done)
        {
            this.timeout = timeout;
            this.done = done;
        }

        /// <inheritdoc />
        public override void Load(LoadContext context, Action<LoadResultText> callback)
        {
            textRequests.Add(context);
            textRequestsCallbacks.Add(callback);
        }

        /// <inheritdoc />
        public override void Load(LoadContext context, Action<LoadResultBinary> callback)
        {
            binaryRequests.Add(context);
            binaryRequestsCallbacks.Add(callback);
        }

        /// <inheritdoc />
        public override void Complete()
        {
            if (textRequests.Count == 0 && binaryRequests.Count == 0) return;
//            if (!Application.isPlaying) return;

            count = textRequests.Count + binaryRequests.Count;
            var go = Go;
            for (var i = 0; i < textRequests.Count; i++)
            {
                var request = textRequests[i];
                var requestCallback = textRequestsCallbacks[i];
                var loader = go.AddComponent<AsyncLoader>();
                loader.Init(request, timeout, exception =>
                {
                    try
                    {
                        //try local file fallback
                        var localData = "";
                        LoadFromLocalFile(request, ref exception, file => localData = File.ReadAllText(file));

                        requestCallback(exception != null
                            ? new LoadResultText(GetMessage(exception), null)
                            : new LoadResultText(null, localData)
                        );
                    }
                    finally
                    {
                        Done();
                    }
                }, handler =>
                {
                    try
                    {
                        requestCallback(new LoadResultText(null, handler.text));
                        WriteToLocalFile(request, file => File.WriteAllText(file, handler.text));
                    }
                    finally
                    {
                        Done();
                    }
                });
            }

            for (var i = 0; i < binaryRequests.Count; i++)
            {
                var request = binaryRequests[i];
                var requestCallback = binaryRequestsCallbacks[i];
                var loader = go.AddComponent<AsyncLoader>();
                loader.Init(request, timeout, exception =>
                {
                    try
                    {
                        //try local file fallback
                        byte[] localData = null;
                        LoadFromLocalFile(request, ref exception, file => localData = File.ReadAllBytes(file));

                        requestCallback(exception != null
                            ? new LoadResultBinary(GetMessage(exception), null)
                            : new LoadResultBinary(null, localData)
                        );
                    }
                    finally
                    {
                        Done();
                    }
                }, handler =>
                {
                    try
                    {
                        requestCallback(new LoadResultBinary(null, handler.data));
                        WriteToLocalFile(request, file => File.WriteAllBytes(file, handler.data));
                    }
                    finally
                    {
                        Done();
                    }
                });
            }
        }

        //get error message from exception object
        private static string GetMessage(Exception e)
        {
            if (e == null) return "unknown error";
            if (e.Message == null) return "unknown error: " + e.GetType().FullName;
            return e.Message;
        }

        //completion callback, called for each request
        private void Done()
        {
            count--;
            if (count != 0) return;

            if (go != null) Object.Destroy(go);
            done();
        }

        /// <summary>
        /// This is asynchronous loader Unity component, handling single async request using coroutines 
        /// </summary>
        public class AsyncLoader : MonoBehaviour
        {
            private LoadContext context;
            private int timeout;
            private Action<Exception> errorAction;
            private Action<DownloadHandler> successAction;

            public void Init(LoadContext context, int timeout, Action<Exception> errorAction, Action<DownloadHandler> successAction)
            {
                this.context = context;
                this.timeout = timeout;
                this.errorAction = errorAction;
                this.successAction = successAction;
            }

            private void Start()
            {
                StartCoroutine(Load());
            }

            private IEnumerator Load()
            {
                Exception ex = null;
                UnityWebRequest webRequest = null;
                try
                {
                    switch (context.Method)
                    {
                        case BGLiveUpdateHttpMethodEnum.Default:
                            webRequest = UnityWebRequest.Get(context.Url);
                            break;
                        case BGLiveUpdateHttpMethodEnum.Get:
                            var queryString = "";
                            if (context.httpParameters.Count > 0)
                            {
                                queryString += "?";
                                foreach (var httpParameter in context.httpParameters)
                                {
                                    if (queryString.Length > 1) queryString += "&";
                                    queryString += httpParameter.Item1 + "=" + UnityWebRequest.EscapeURL(httpParameter.Item2);
                                }
                            }

                            webRequest = UnityWebRequest.Get(context.Url + queryString);
                            break;
                        case BGLiveUpdateHttpMethodEnum.Post:
                            var form = new WWWForm();
                            foreach (var httpParameter in context.httpParameters) form.AddField(httpParameter.Item1, httpParameter.Item2);
                            webRequest = UnityWebRequest.Post(context.Url, form);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    //http headers
                    foreach (var httpHeader in context.httpHeaders) webRequest.SetRequestHeader(httpHeader.Item1, httpHeader.Item2);

                    webRequest.timeout = timeout;
                }
                catch (Exception e)
                {
                    ex = e;
                }

                if (ex == null)
                {
                    //send HTTP request
                    yield return webRequest.SendWebRequest();

                    //handle possible error
                    try
                    {
                        if (webRequest.isNetworkError || webRequest.isHttpError)
                            throw new Exception("Error while loading: " + (!string.IsNullOrEmpty(webRequest.error) ? webRequest.error : "unknown error") + ", response code:" +
                                                webRequest.responseCode);
                    }
                    catch (Exception e)
                    {
                        ex = e;
                    }
                }

                //call final callback (error or success)
                if (ex != null)
                {
                    Debug.LogException(ex);
                    errorAction(ex);
                }
                else successAction(webRequest.downloadHandler);
            }
        }
    }
}