using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankAccountValidationWeb.Controllers.api
{
    public class BankDetailsValidateController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get(string sortCode, string accountNo)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }
    }
}