using iSIMS.MicroService.ApiClient.RestClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProductService.ClientApp
{
    public class BatchRestClient: StructuredRestClient
    {
        private Uri _batchUri;
        public BatchRestClient(Uri batchUri)
        {
            _batchUri = batchUri;
        }

        /// <summary>
        /// Sends a single HttpRequestMessage to a Web API/OData batch end point
        /// </summary>
        /// <param name="batchEndPoint">The URI accepting batch requests</param>
        /// <param name="requestsToBatch">Requests that need to be sent as batch</param>
        /// <returns></returns>
        public async Task<IEnumerable<HttpResponseMessage>> SendBatchAsync(IEnumerable<HttpRequestMessage> requestsToBatch)
        {

            HttpRequestMessage httpBatchedRequest = CreateBatchedRequest(requestsToBatch);
            return await Send(httpBatchedRequest);
        }

        private HttpRequestMessage CreateBatchedRequest(IEnumerable<HttpRequestMessage> requestsToBatch)
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

            return new HttpRequestMessage(HttpMethod.Post, _batchUri)
            {
                Content = multipartContent
            };
        }

        private async Task<IEnumerable<HttpResponseMessage>> Send(HttpRequestMessage batchRequest)
        {
            List<HttpResponseMessage> responses = new List<HttpResponseMessage>();

            HttpResponseMessage response = await HttpClient.SendAsync(batchRequest);
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
