using BackEndServices.Models;
using BackEndServices.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using static BackEndServices.Models.MobileMoney;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BackEndServices.Controllers
{


    [Route("[controller]")]
    public class MomoController : ControllerBase
    {
        private readonly MomoService mobileMoneyService = new MomoService();
        [HttpPost]
        [Route("api/momo/transfer")]
        public TransactionResponse MomoTransferPayOut(MobileMoneyPayload payload)
        {

            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToArray();
                return new TransactionResponse() { code = 1010, transactionInfos = null, message = "Invalid Data" };
            }

            Utils.Utils.SaveLog("MomoController", "api/momo/transfer", JsonConvert.SerializeObject(payload));
            var momoPaymentResponse = mobileMoneyService.MobileMoneyPayOut(payload);
            return new TransactionResponse() { code = 1000, message = momoPaymentResponse.message, transactionInfos = momoPaymentResponse.returnObject };
            
        }


        [HttpPost]
        [Route("api/momo/payment")]
        public TransactionResponse MomoPayment(MomoPaymentPayload payload)
        {

            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToArray();
                return new TransactionResponse() { code = 1010, transactionInfos = null, message = "Invalid Data" };
            }

            Utils.Utils.SaveLog("MomoController", "api/momo/payment", JsonConvert.SerializeObject(payload));
            var momoPaymentResponse = mobileMoneyService.StartPaymentProcess(payload);
            return new TransactionResponse() { code = 1000, message = momoPaymentResponse.message, transactionInfos = momoPaymentResponse.returnObject };
        }




        [HttpGet]
        [Route("api/momo/payment")]
        public TransactionResponse FetchMomoPayment()
        {

            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToArray();
                return new TransactionResponse() { code = 1010, transactionInfos = null, message = "Invalid Data" };
            }

            Utils.Utils.SaveLog("MomoController", "api/momo/payment","");
            var momoPaymentResponse = mobileMoneyService.GetMobileMoneyPayment();
            return new TransactionResponse() { code = 1000, message = momoPaymentResponse.message, transactionInfos = momoPaymentResponse.returnObject };
        }





        [HttpGet]
        [Route("api/momo/{trans_id}")]
        public TransactionResponse MomoTransferByTransactionId(string trans_id)
        {
            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToArray();
                return new TransactionResponse() { code = 1010, transactionInfos = null, message = "Invalid Data" };
            }

            Utils.Utils.SaveLog("MomoController", "api/momo/{trans_id}", JsonConvert.SerializeObject(trans_id));
            var momoTransactionByIdResponce = mobileMoneyService.GetMobileMoneyTransactionById(trans_id);

            return new TransactionResponse() { code = 1000, message = momoTransactionByIdResponce.message, transactionInfos = momoTransactionByIdResponce.returnObject };

        }



        [HttpGet]
        [Route("api/momo")]
        public TransactionResponse MomoTransferByTransactions()
        {
            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToArray();
                return new TransactionResponse() { code = 1010, transactionInfos = null, message = "Invalid Data" };
            }
            Utils.Utils.SaveLog("MomoController", "api/momo","");
            var momoTransactionByIdResponce = mobileMoneyService.GetMobileMoneyTransactions();

            return new TransactionResponse() { code = 1000, message = momoTransactionByIdResponce.message, transactionInfos = momoTransactionByIdResponce.returnObject };

        }
    }
}