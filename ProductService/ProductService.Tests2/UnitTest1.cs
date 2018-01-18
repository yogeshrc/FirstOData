using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProductService.Tests2
{
    [TestClass]
    public class UnitTest1
    {
        private readonly Uri ServiceRoot = new Uri("http://localhost:54077/");

        [TestMethod]
        public void TestMethod1()
        {
            var container = new Default.Container(serviceRoot: ServiceRoot);
        }
    }
}
