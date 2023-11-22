using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndServices.Models
{
    public class AuthenticateResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public dynamic customerInfos { get; set; }
    }

    public class BasicResponse 
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public class AccountResponse : BasicResponse
    {
        public dynamic accountInfos { get; set; }
    }

    public class TransactionResponse : BasicResponse
    {
        public dynamic transactionInfos { get; set; }
    }
}