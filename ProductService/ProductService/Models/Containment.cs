using System.Collections.Generic;
using System.Web.OData.Builder;

namespace ProductService.Models
{
    public class Account
    {
        public int AccountID { get; set; }
        public string Name { get; set; }
        [AutoExpand]
        public IList<PaymentInstrument> PayinPIs { get; set; }
    }

    public class PaymentInstrument
    {
        public int PaymentInstrumentID { get; set; }
        public string FriendlyName { get; set; }
    }
}