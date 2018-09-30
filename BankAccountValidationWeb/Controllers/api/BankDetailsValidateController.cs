using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ModuleChecksLibrary;

namespace BankAccountValidationWeb.Controllers.api
{
    public class BankDetailsValidateController : ApiController
    {
        private IModulusCheck modulusCheck;

        public BankDetailsValidateController()
        {
            modulusCheck = new ModulusCheck();
        }

        public BankDetailsValidateController(IModulusCheck modulusCheck)
        {
            this.modulusCheck = modulusCheck;
        }

        // GET api/<controller>
        public ModulusCheckValidationResult Get(string sortCode, string accountNo)
        {
            ModulusCheckValidationResult result = modulusCheck.ModulusCheckValidation(sortCode, accountNo);

            return result;
        }
    }
}