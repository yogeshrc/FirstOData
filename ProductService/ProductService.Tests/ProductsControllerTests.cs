using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Default;
using Microsoft.OData.Client;
using ProductService.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;
using iSIMS.MicroService.ApiClient.RestClient;
using iSIMS.MicroService.ApiClient.DataObjects.Response;

namespace ProductService.Tests
{
    [TestClass]
    public class ProductsControllerTests
    {
        private const string ServiceRootUrl = "http://localhost:54077/odata/";

        private Container _odataContainer;

        [TestInitialize]
        public void ProductsServiceInitialize()
        {
            _odataContainer = new Container(new Uri(ServiceRootUrl));
        }

        [TestMethod]
        public void ListProductsTest()
        {
            foreach (var product in _odataContainer.Products)
            {
                string message = string.Format("{0}: {1}\t{2}", product.Id, product.Name, product.Price);
                Trace.WriteLine(message);
            }
        }

        [TestMethod]
        public void TestBatchUsingODataClientLib()
        {
            var productsQuery = _odataContainer.Products;
            var suppliersQuery = _odataContainer.Suppliers;

            var batchResponse = _odataContainer.ExecuteBatch(productsQuery, suppliersQuery);
            foreach (var item in batchResponse)
            {
                if (item is QueryOperationResponse<Product> products)
                    foreach (var product in products)
                        Trace.WriteLine(string.Format("{0}: {1}\t{2}",
                            product.Id, product.Name, product.Price));

                if (item is QueryOperationResponse<Supplier> suppliers)
                    foreach (var supplier in suppliers)
                        Trace.WriteLine(string.Format("{0}: {1}", 
                            supplier.Id, supplier.Name));
            }
        }

        [TestMethod]
        public async Task TestBatchUsingHttpClient()
        {
            var batchEndPoint = new Uri(ServiceRootUrl + "$batch");
            var requestsToBatch = new HttpRequestMessage[]{
                                        new HttpRequestMessage(HttpMethod.Get, ServiceRootUrl + "Products"),
                                        new HttpRequestMessage(HttpMethod.Get, ServiceRootUrl + "Suppliers")

                };

            IEnumerable<HttpResponseMessage> responses;
            using (HttpClient httpClient = new HttpClient())
            {
                responses = await httpClient.SendBatchAsync(batchEndPoint, requestsToBatch);
            }

            //review each response
            int responseId = 1;
            foreach (var response in responses)
            {
                Trace.WriteLine("Response >> " + responseId);
                var jsonText = await response.Content.ReadAsStringAsync();
                Trace.WriteLine(jsonText);
                responseId++;

            }
        }

        [TestMethod]
        public async Task TestBatchUsingHttpClientAndTypedResponse()
        {
            var batchEndPoint = new Uri(ServiceRootUrl + "$batch");
            var requestsToBatch = new HttpRequestMessage[]{
                                        new HttpRequestMessage(HttpMethod.Get, ServiceRootUrl + "Products"),
                                        new HttpRequestMessage(HttpMethod.Get, ServiceRootUrl + "Suppliers")

                };
            IList<Type> responseTypes = new Type[] {
                typeof(StructuredRestResponse<IEnumerable<Product>>),
                typeof(StructuredRestResponse<IEnumerable<Supplier>>)
            };
            
            IEnumerable<HttpResponseMessage> responses;
            using (HttpClient httpClient = new HttpClient())
                responses = await httpClient.SendBatchAsync(batchEndPoint, requestsToBatch);

            //review each response
            int responseId = 1;
            foreach (var response in responses)
            {
                
                Trace.WriteLine("Response >> " + responseId);
                var structuredRestResponse = await response.Content.ReadAsAsync(responseTypes[responseId - 1]);
                Trace.WriteLine(structuredRestResponse);
                responseId++;
            }
        }

        [TestMethod]
        public async Task TestBatchUsingStructureRestClient()
        {
            using (var client = new StructuredRestClient())
            {
                Uri url = new Uri(@"http://localhost:54077/odata/Products");
                StructuredRestResponse<Product> product = await client.Get<Product>(url);
            }
        }

        [TestMethod]
        public async Task TestBatchUsingStructureRestClient2()
        {
            using (var client = new StructuredRestClient())
            {
                Uri url = new Uri(@"http://localhost:54077/odata/Products");
                StructuredRestResponse<Product> product = await client.Get<Product>(url);
            }
        }

    }
}
