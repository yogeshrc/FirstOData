using System;
using System.Threading.Tasks;

namespace ProductService.ClientApp
{
    using iSIMS.MicroService.ApiClient.RestClient;
    using iSIMS.MicroService.ApiClient.DataObjects.Response;
    using ProductService.Models;
    using System.Collections.Generic;
    using DinnerMoneyService.ApiClient.OData;
    using System.Net.Http;
    using ProductService.ClientApp.Batch;

    class Program
    {
        static void Main(string[] args)
        {
            //TestBatchUsingStructuredRestClient();
        }

        private static void TestBatchUsingStructuredRestClient()
        {
            Uri uri = new Uri(@"http://localhost:54077/odata");

            using (var client = new BatchedRestClient(uri))
            {
                Uri productsGetUri = new Uri(@"http://localhost:54077/odata/Products");
                client.AddStructuredGet<ODataResponse<IEnumerable<Product>>>(productsGetUri);
            }
        }

        private static void TestSingleUsingStructuredRestClient()
        {
            using (var client = new StructuredRestClient())
            {
                Uri url = new Uri(@"http://localhost:54077/odata/Products");
                StructuredRestResponse<ODataResponse<IEnumerable<Product>>> product = client.Get<ODataResponse<IEnumerable<Product>>>(url).Result;
            }
        }

    }

    class StructuredBatchClient: StructuredRestClient
    {
        public void SendBatchRequest()
        {
            base.HttpClient.SendBatchAsync
        }
    }
}
