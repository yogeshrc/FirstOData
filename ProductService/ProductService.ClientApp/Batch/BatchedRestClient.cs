using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace ProductService.ClientApp.Batch
{
    using iSIMS.MicroService.ApiClient.DataObjects.Response.Error;
    using iSIMS.MicroService.ClientSecurity;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    public class BatchedRestClient : IDisposable
    {
        private HttpClient _client;
        private List<HttpRequestMessage> _requests;
        private List<HttpRequestItem> _requestItems;

        protected Uri BaseAddress { get; private set; }
        protected string BearerToken { get; private set; }
        protected HttpClient HttpClient { get; private set; }

        public BatchedRestClient(Uri baseAddress, string bearerToken = null)
        {
            BaseAddress = baseAddress;
            BearerToken = bearerToken;
            SetupHttpClientSecurity();
            _requests = new List<HttpRequestMessage>();
            _requestItems = new List<HttpRequestItem>();
        }

        public void Dispose()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }

        private void SetupHttpClientSecurity()
        {
            HttpClient = new HttpClient();

            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            HttpClient.BaseAddress = BaseAddress;

            ////FOR DINNER MONEY DEBUGGING, DISABLE SECURITY
            //if (!string.IsNullOrWhiteSpace(BearerToken))
            //{
            //    HttpClient.SetUpAuthorization(BearerToken);
            //}
            //else
            //{
            //    HttpClient.SetUpAuthorization();
            //}
        }

        public virtual void AddStructuredGet<T>(Uri getUri) where T: new()
        {
            //add to internal request list
            _requests.Add(new HttpRequestMessage(HttpMethod.Get, getUri));

            HttpRequestItem<T> requestItem = new HttpRequestItem<T>(getUri, HttpMethod.Get);
            _requestItems.Add(requestItem);
        }

        public async void Execute()
        {
            var batchEndPoint = new Uri(BaseAddress, "$batch");
            var requestsToBatch = _requestItems.Select(t => t.Request);

            IEnumerable<HttpResponseMessage> responses;
            using (HttpClient httpClient = new HttpClient())
            {
                responses = await httpClient.SendBatchAsync(batchEndPoint, requestsToBatch);
            }

            //var result = BatchResponsesToStructuredRestResponses(responses);
            //await BatchResponsesToStructuredRestResponses(responses);
        }

        //private async Task<IEnumerable<StructuredRestResponse>> BatchResponsesToStructuredRestResponses(IEnumerable<HttpResponseMessage> responses)
        //{
        //    for (int i = 0; i < responses.Count(); i++)
        //    {
        //        var successResult = Activator.CreateInstance(_requestItems[i].ResponseType);
        //        ErrorResponse errorResult = null;

        //        var response = responses.ElementAt(i);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            successResult = await response.Content.ReadAsAsync(_requestItems[i].ResponseType).ConfigureAwait(false);
        //        }
        //        else
        //        {
        //            errorResult = await GetError(response).ConfigureAwait(false);
        //        }
        //        var structuredRestResponse = new StructuredRestResponse(_requestItems[i].ResponseType, response.StatusCode, errorResult, successResult);
        //    }
        //}

        private async Task<ErrorResponse> GetError(HttpResponseMessage response)
        {
            ErrorResponse result;

            var errorResponseFromContent = new Func<HttpContent, Task<ErrorResponse>>(async (content) =>
            {
                var text = await content.ReadAsStringAsync().ConfigureAwait(false);
                var errorResult = new ErrorResponse
                {
                    Code = ErrorCodeEnum.UnspecifiedError,
                    Message = text
                };
                return errorResult;
            });

            if (response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var errorResponseObject = await response.Content.ReadAsAsync<ErrorResponse>().ConfigureAwait(false) ?? await errorResponseFromContent(response.Content);
                result = errorResponseObject;
            }
            else
            {
                result = await errorResponseFromContent(response.Content);
            }
            return result;
        }
    }

    public abstract class HttpRequestItem
    {
        public HttpRequestMessage Request { get; protected set; }
        public Type ResponseType { get; protected set; }
    }
    public class HttpRequestItem<TResponse>: HttpRequestItem where TResponse: new()
    {
        public HttpRequestItem(Uri uri, HttpMethod method)
        {
            Request = new HttpRequestMessage(method, uri);
            ResponseType = typeof(TResponse);
        }
    }

    public class StructuredRestResponse
    {
        public Type ResponseType { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public ErrorResponse ErrorResult { get; set; }
        public object Result { get; set; }

        public StructuredRestResponse(Type responseType, HttpStatusCode statusCode, ErrorResponse errorResult, object result)
        {
            ResponseType = responseType;
            StatusCode = statusCode;
            ErrorResult = errorResult;
            Result = result;
        }

        //public T ConvertType<T>() where T: new()
        //{
        //    if (ResponseType.Equals(typeof(T)))
        //    {
        //        Convert<T>
        //    }
        //}
    }
}
