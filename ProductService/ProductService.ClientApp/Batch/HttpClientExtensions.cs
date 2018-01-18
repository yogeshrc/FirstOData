using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductService.ClientApp.Batch
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Sends a single HttpRequestMessage to a Web API/OData batch end point
        /// </summary>
        /// <param name="batchEndPoint">The URI accepting batch requests</param>
        /// <param name="requestsToBatch">Requests that need to be sent as batch</param>
        /// <returns></returns>
        public static async Task<IEnumerable<HttpResponseMessage>> SendBatchAsync(this HttpClient httpClient,
            Uri batchEndPoint,
            IEnumerable<HttpRequestMessage> requestsToBatch)
        {
            HttpRequestMessage httpBatchedRequest = CreateBatchedRequest(batchEndPoint, requestsToBatch);
            return await Send(httpClient, httpBatchedRequest);
        }

        private static HttpRequestMessage CreateBatchedRequest(Uri batchEndPoint, IEnumerable<HttpRequestMessage> requestsToBatch)
        {
            MultipartContent multipartContent = new MultipartContent("mixed", "batch_" + Guid.NewGuid().ToString());
            foreach (var request in requestsToBatch)
            {
                var httpMessageContent = new HttpMessageContent(request);
                httpMessageContent.Headers.Remove("Content-Type");
                httpMessageContent.Headers.Add("Content-Type", "application/http");
                httpMessageContent.Headers.Add("Content-Transfer-Encoding", "binary");

                multipartContent.Add(httpMessageContent);
            }

            return new HttpRequestMessage(HttpMethod.Post, batchEndPoint)
            {
                Content = multipartContent
            };
        }

        private static async Task<IEnumerable<HttpResponseMessage>> Send(HttpClient client, HttpRequestMessage batchRequest)
        {
            List<HttpResponseMessage> responses = new List<HttpResponseMessage>();

            HttpResponseMessage response = await client.SendAsync(batchRequest);
            MultipartMemoryStreamProvider responseContents = await response.Content.ReadAsMultipartAsync();

            foreach (HttpContent content in responseContents.Contents)
            {
                content.Headers.Remove("Content-Type");
                content.Headers.Add("Content-Type", "application/http; msgtype=response");

                responses.Add(await content.ReadAsHttpResponseMessageAsync());
            }
            return responses;
        }
    }
}
