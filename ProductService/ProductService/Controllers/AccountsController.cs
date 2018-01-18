using ProductService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Routing;

namespace ProductService.Controllers
{
    public class AccountsController : ODataController
    {
        private static IList<Account> _accounts = null;

        public AccountsController()
        {
            if (_accounts == null) _accounts = InitAccounts();
        }

        //GET ~/Accounts(xxx)/PayinPIs
        [EnableQuery]
        public IHttpActionResult GetPayinPIs(int key)
        {
            var payinPIs = _accounts.Single(a => a.AccountID == key).PayinPIs;
            return Ok(payinPIs);
        }

        [EnableQuery]
        [ODataRoute("Accounts({accountId})/PayinPIs({paymentInstrumentId})")]
        public IHttpActionResult GetSinglePayinPI(int accountId, int paymentInstrumentId)
        {
            var payinPIs = _accounts.Single(a => a.AccountID == accountId).PayinPIs;
            var payinPI = payinPIs.Single(pi => pi.PaymentInstrumentID == paymentInstrumentId);
            return Ok(payinPI);
        }

        //GET ~/Accounts(xxx)
        public IHttpActionResult GetAccount(int key)
        {
            var account = _accounts.Single(a => a.AccountID == key);
            return Ok(account);
        }

        [EnableQuery]
        public IHttpActionResult Get()
        {
            return Ok(_accounts.AsQueryable());
        }

        private static IList<Account> InitAccounts()
        {
            var accounts = new List<Account>() {
                new Account() {
                    AccountID = 100, Name="Name100",
                    PayinPIs = new List<PaymentInstrument>() {
                        new PaymentInstrument() {
                            PaymentInstrumentID = 101,
                            FriendlyName = "101 first PI"
                        },
                        new PaymentInstrument() {
                            PaymentInstrumentID = 102,
                            FriendlyName = "102 second PI"
                        }
                    }
                }
            };
            return accounts;
        }
    }
}