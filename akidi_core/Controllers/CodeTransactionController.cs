using BackEndServices.Models;
using BackEndServices.Services;
using BackEndServices.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace BackEndServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CodeTransactionController : ControllerBase
    {
        CodeTransactionService codeTransactionService = new CodeTransactionService();
        

        [HttpPost]
        [Route("api/generate-transaction-code")]
        public TransactionResponse generateTransCode([FromBody] GenerateCodeRequest transferRequestParam)
        {
            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToArray();
                return new TransactionResponse() { code = 1010, transactionInfos = null, message = "Invalid Data" };
            }

            Utils.Utils.SaveLog("CodeTransactionController", "api/generate-transaction-code", JsonConvert.SerializeObject(transferRequestParam));
            transferResponse response = TransferFundService.generateCode(transferRequestParam);
            var responseStatusCode = (response.code == 500) ? HttpStatusCode.InternalServerError : HttpStatusCode.OK;

            return new TransactionResponse() { code = 1000, message = response.message, transactionInfos = transferRequestParam };
            
        }



        [HttpPost]
        [Route("api/cancel-transaction-code")]
        public TransactionResponse cancelTransCode([FromBody] payCancelCodeRequest transferRequestParam)
        {

            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToArray();
                return new TransactionResponse() { code = 1010, transactionInfos = null, message = "Invalid Data" };
            }
            Utils.Utils.SaveLog("CodeTransactionController", "api/cancel-transaction-code", JsonConvert.SerializeObject(transferRequestParam));

            transferResponse response = TransferFundService.cancelRequestCode(transferRequestParam);
            var responseStatusCode = (response.code == 500) ? HttpStatusCode.InternalServerError : HttpStatusCode.OK;
            return new TransactionResponse() { code = 1000, message = response.message, transactionInfos = transferRequestParam };
        }


        //[HttpPost]
        //[Route("api/pay-code")]
        //public TransactionResponse payCode([FromBody] payCodeRequest payCodeRequest)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        var errorList = (from item in ModelState.Values
        //                         from error in item.Errors
        //                         select error.ErrorMessage).ToArray();
        //        return new TransactionResponse() { code = 1010, transactionInfos = null, message = "Invalid Data" };
        //    }
        //    Utils.Utils.SaveLog("CodeTransactionController", "/payCode", JsonConvert.SerializeObject(payCodeRequest));

        //    transferResponse response = TransferFundService.payRequestCode(payCodeRequest);
        //    var responseStatusCode = (response.code == 500) ? HttpStatusCode.InternalServerError : HttpStatusCode.OK;
        //    return new TransactionResponse() { code = 1000, message = response.message, transactionInfos = transferRequestParam };
        //}


        [HttpPost]
        [Route("api/send-money")]
        public TransactionResponse sendMoney([FromBody] TransferPayload transferPayload)
        {

            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToArray();
                return new TransactionResponse() { code = 1010, transactionInfos = null, message = "Invalid Data" };
            }
            Utils.Utils.SaveLog("CodeTransactionController", "/sendMoney", JsonConvert.SerializeObject(transferPayload));

            transferResponse response = TransferFundService.transferFund(transferPayload);
            var responseStatusCode = (response.code == 500) ? HttpStatusCode.InternalServerError : HttpStatusCode.OK;
            return new TransactionResponse() { code = 1000, message = response.message, transactionInfos = transferPayload };
        }
    }
}
