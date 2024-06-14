/*
<copyright file="BGLiveUpdateUrl.cs" company="BansheeGz">
    Copyright (c) 2018-2023 All Rights Reserved
</copyright>
*/


using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace BansheeGz.BGDatabase
{
    /// <summary>
    /// Data container for one single HTTP request 
    /// </summary>
    [Serializable]
    public class BGLiveUpdateUrl
    {
        [SerializeField] private string url;
        [SerializeField] private BGLiveUpdateUrlTypeEnum urlType;
        [SerializeField] private string metaId;

        //added in BGLiveUpdateUrls v.2
        [SerializeField] private BGLiveUpdateHttpMethodEnum httpMethod;
        [SerializeField] private List<HttpParameter> httpParameters;
        [SerializeField] private List<HttpHeader> httpHeaders;


        //reference to the parent container
        [NonSerialized] private BGLiveUpdateUrls urls;

        /// <summary>
        /// Reference to the parent container
        /// </summary>
        public BGLiveUpdateUrls Urls
        {
            get => urls;
            internal set => urls = value;
        }

        /// <summary>
        /// HTTP URL to used for HTTP request
        /// </summary>
        public string URL
        {
            get => url;
            set
            {
                if (url == value) return;
                url = value;
                FireEvent();
            }
        }

        /// <summary>
        /// data format for response data
        /// </summary>
        public BGLiveUpdateUrlTypeEnum URLType
        {
            get => urlType;
            set
            {
                if (urlType == value) return;
                urlType = value;
                FireEvent();
            }
        }

        /// <summary>
        /// Table ID
        /// </summary>
        public string MetaId
        {
            get => metaId;
            set
            {
                if (metaId == value) return;
                metaId = value;
                FireEvent();
            }
        }

        /// <summary>
        /// HTTP method to use for request
        /// </summary>
        public BGLiveUpdateHttpMethodEnum HttpMethod
        {
            get => httpMethod;
            set
            {
                if (httpMethod == value) return;
                httpMethod = value;
                FireEvent();
            }
        }

        /// <summary>
        /// HTTP parameters for HTTP request
        /// </summary>
        public List<HttpParameter> HttpParameters => httpParameters;

        /// <summary>
        /// HTTP headers for HTTP request
        /// </summary>
        public List<HttpHeader> HttpHeaders => httpHeaders;

        /// <summary>
        /// Convert HTTP parameters to the list of Tuple(string,string) = (name,value)
        /// </summary>
        public List<Tuple<string, string>> HttpParametersAsTuples
        {
            get
            {
                if (httpParameters == null || httpParameters.Count == 0) return null;
                var result = new List<Tuple<string, string>>();
                foreach (var postParameter in httpParameters) result.Add(new Tuple<string, string>(postParameter.Name, postParameter.FinalValue));
                return result;
            }
        }

        /// <summary>
        /// Convert HTTP headers to the list of Tuple(string,string) = (name,value)
        /// </summary>
        public List<Tuple<string, string>> HttpHeadersAsTuples
        {
            get
            {
                if (httpHeaders == null || httpHeaders.Count == 0) return null;
                var result = new List<Tuple<string, string>>();
                foreach (var httpHeader in httpHeaders) result.Add(new Tuple<string, string>(httpHeader.Name, httpHeader.FinalValue));
                return result;
            }
        }

        public BGLiveUpdateUrl()
        {
        }

        public BGLiveUpdateUrl(BGLiveUpdateUrls urls, string url, BGLiveUpdateUrlTypeEnum urlType, string metaId)
        {
            this.urls = urls;
            this.url = url;
            this.urlType = urlType;
            this.metaId = metaId;
        }

        //================================================================================================
        //                                              Methods
        //================================================================================================
        //fired than some attributes of request is changed
        private void FireEvent()
        {
            urls?.FireEvent();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var builder = new StringBuilder(url ?? "[No URL]");
            if (httpParameters != null && httpParameters.Count > 0)
            {
                //this code does not apply proper encoding and can be used only inside ToString method
                builder.Append('?');
                foreach (var httpParameter in httpParameters) builder.Append(httpParameter.Name + '=' + httpParameter.FinalValue);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Clone current object to the new parent container 
        /// </summary>
        public BGLiveUpdateUrl CloneTo(BGLiveUpdateUrls urls)
        {
            var clone = new BGLiveUpdateUrl(urls, url, urlType, metaId)
            {
                httpMethod = httpMethod
            };
            if (httpParameters != null && httpParameters.Count > 0)
            {
                foreach (var httpParameter in httpParameters) httpParameter.CloneTo(clone);
            }
            if (httpHeaders != null && httpHeaders.Count > 0)
            {
                foreach (var httpHeader in httpHeaders) httpHeader.CloneTo(clone);
            }
            return clone;
        }
        //================================================================================================
        //                                              Http parameters
        //================================================================================================
        /// <summary>
        /// Adds HTTP parameter
        /// </summary>
        public HttpParameter AddHttpParameter(string name, string value)
        {
            httpParameters = httpParameters ?? new List<HttpParameter>();
            var httpParameter = new HttpParameter(name, value);
            httpParameters.Add(httpParameter);
            FireEvent();
            return httpParameter;
        }

        /// <summary>
        /// Removes HTTP parameter
        /// </summary>
        public void RemoveHttpParameter(HttpParameter parameter)
        {
            if (httpParameters?.Remove(parameter) ?? false) FireEvent();
        }

        //================================================================================================
        //                                              Http headers
        //================================================================================================
        /// <summary>
        /// Adds HTTP header
        /// </summary>
        public HttpHeader AddHttpHeader(string name, string value)
        {
            httpHeaders = httpHeaders ?? new List<HttpHeader>();
            var httpHeader = new HttpHeader(name, value);
            httpHeaders.Add(httpHeader);
            FireEvent();
            return httpHeader;
        }

        /// <summary>
        /// Removes HTTP header
        /// </summary>
        public void RemoveHttpHeader(HttpHeader httpHeader)
        {
            if (httpHeaders?.Remove(httpHeader) ?? false) FireEvent();
        }

        //================================================================================================
        //                                              Nested classes
        //================================================================================================
        /// <summary>
        /// Data container for parameter/header with graph support. Graph is used to calculate parameter/header value
        /// </summary>
        [Serializable]
        public class ParameterWithGraph : ISerializationCallbackReceiver
        {
            /// <summary>
            /// Parameter/header name
            /// </summary>
            public string Name;
            /// <summary>
            /// Parameter/header value
            /// </summary>
            public string Value;
            /// <summary>
            /// Graph, if used for value calculation, serialized as string
            /// </summary>
            public string GraphAsString;
            [NonSerialized] public BGCalcGraph Graph;
            
            public ParameterWithGraph(string name, string value)
            {
                Name = name;
                Value = value;
            }
            /// <summary>
            /// Final parameter/header value. If graph is used, it's called to get calculated value
            /// </summary>
            public string FinalValue
            {
                get
                {
                    if (Graph == null) return Value;
                    var context = BGCalcFlowContext.Get();
                    try
                    {
                        context.Graph = Graph;
                        context.GraphType = BGCalcGraphTypeEnum.LiveUpdateHttpParameterValue;
                        return Graph.Execute<string>(context);
                    }
                    finally
                    {
                        BGCalcFlowContext.Return(context);
                    }
                }
            }

            /// <inheritdoc />
            public void OnBeforeSerialize()
            {
                GraphAsString = Graph?.ToJsonString();
            }

            /// <inheritdoc />
            public void OnAfterDeserialize()
            {
                if (string.IsNullOrEmpty(GraphAsString)) Graph = null;
                else
                {
                    Graph = BGCalcGraph.ExistingGraph();
                    Graph.FromJsonString(GraphAsString);
                }
            }

        }
        /// <summary>
        /// Data container for HTTP parameter
        /// </summary>
        [Serializable]
        public class HttpParameter : ParameterWithGraph
        {

            public HttpParameter(string name, string value) : base(name, value)
            {
            }
  
            public void CloneTo(BGLiveUpdateUrl url)
            {
                var parameter = url.AddHttpParameter(Name, Value);
                //we do not clone graph for performance reason, cause we know that reassigning the same graph will not result in any problem
                parameter.Graph = Graph;
            }
        }

        /// <summary>
        /// Data container for HTTP header
        /// </summary>
        [Serializable]
        public class HttpHeader : ParameterWithGraph
        {
            public HttpHeader(string name, string value): base(name, value)
            {
            }
            public HttpHeader CloneTo(BGLiveUpdateUrl url)
            {
                var httpHeader = url.AddHttpHeader(Name, Value);
                //we do not clone graph for performance reason, cause we know that reassigning the same graph will not result in any problem
                httpHeader.Graph = Graph;
                return httpHeader;
            }
        }
    }
}