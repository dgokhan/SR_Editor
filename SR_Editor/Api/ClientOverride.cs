using RoyaleSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoyaleSupport
{
    public partial class RoyaleSupportClient
    {
        public virtual async Task PrepareRequestAsync(HttpClient client, HttpRequestMessage message, StringBuilder urlBuilder, CancellationToken cancellationToken)
        {
            if(!message.Headers.Contains("authorization"))
                message.Headers.Add("authorization", "Bearer " + SR_Editor.EditorApplication.EditorApplication.AccessToken);

        }
        public virtual async Task PrepareRequestAsync(HttpClient client, HttpRequestMessage message, string url, CancellationToken cancellationToken)
        {
            if (!message.Headers.Contains("authorization"))
                message.Headers.Add("authorization", "Bearer " + SR_Editor.EditorApplication.EditorApplication.AccessToken);
        }

        public virtual async Task ProcessResponseAsync(System.Net.Http.HttpClient client_, System.Net.Http.HttpResponseMessage response_, CancellationToken cancellationToken)
        {
            if (response_.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                response_.StatusCode = System.Net.HttpStatusCode.OK;
            }
        }

        public RoyaleSupportClient() : this(
            "http://51.68.178.17:4545"
            //"https://localhost:44380"
            , new HttpClient())
        {

        }

    }
}
