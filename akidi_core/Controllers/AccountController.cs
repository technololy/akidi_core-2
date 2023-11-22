using BackEndServices.Models;
using BackEndServices.Services;
using BackEndServices.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace BackEndServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        AccountService accountService = new AccountService();
        

        [HttpPost]
        [Route("api/add-account")]
        public AccountResponse Register(Account account)
        {

            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToArray();
                return new AccountResponse() { code = 1010, accountInfos = null, message = "Invalid Data" };
            }

            Utils.Utils.SaveLog("AccountController", "api/add-account", JsonConvert.SerializeObject(account));
            ReturnMessage resp = accountService.addNewAccountForCustomer(account);

            AccountResponse accountResponse = new AccountResponse();

            if (resp.code == System.Net.HttpStatusCode.OK)
            {
                accountResponse = new AccountResponse() { code = 1000, message = "Success", accountInfos = resp };
                return accountResponse;
            }

            accountResponse = new AccountResponse() { code = 1010, message = "Creation failed", accountInfos = resp };
            return accountResponse;
        }


        [HttpPost]
        [Route("api/get-account-per-userId")]
        public AccountResponse getCustomerAccount(AccountFilter accountFilter)
        {
            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToArray();
                return new AccountResponse() { code = 1010, accountInfos = null, message = "Invalid Data" };
            }

            Utils.Utils.SaveLog("AccountController", "api/get-account-per-userId", JsonConvert.SerializeObject(accountFilter));
            ReturnMessage message = accountService.getUserAccount(accountFilter);


            AccountResponse accountResponse = new AccountResponse() { };
            if (message.code == HttpStatusCode.OK)
            {
                accountResponse = new AccountResponse() { code = 1000, message = message.message, accountInfos = message.returnObject };
                return accountResponse;
            }

            accountResponse = new AccountResponse() { code = 1010, message = "Error", accountInfos = null };
            return accountResponse;
        }




        //[HttpPost]
        //[Route("api/account-by-bank")]
        //public AccountResponse getAccountByBank(string userId, string bankCode, string accountType)
        //{
        //    Utils.Utils.SaveLog("AccountController", "/get-account", JsonConvert.SerializeObject(accountFilter));

        //    ReturnMessage message = accountService.GetAccountByBank(userId, bankCode, accountType);

        //    if (message.code == HttpStatusCode.OK)
        //    {
        //        return Ok(message);
        //    }
        //    AccountResponse accountResponse = new AccountResponse();
        //    accountResponse = new AccountResponse() { code = 1010, message = "Creation failed", accountInfos = null };
        //    return accountResponse;

        //    return ResponseMessage(Request.CreateResponse(message.code, message.message));
        //}



        //[HttpPost]
        //[Route("api/account-by-bank")]
        //public AccountResponse getAccountByBank(AccountFilter accountFilter)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var errorList = (from item in ModelState.Values
        //                         from error in item.Errors
        //                         select error.ErrorMessage).ToArray();
        //        return new AccountResponse() { code = 1010, accountInfos = null, message = "Invalid Data" };
        //    }

        //    Utils.Utils.SaveLog("AccountController", "/account-by-bank", JsonConvert.SerializeObject(accountFilter));
        //    ReturnMessage message = accountService.GetUserAccount(accountFilter);

        //    if (message.code == HttpStatusCode.OK)
        //    {
        //        return Ok(message);
        //    }

        //    return ResponseMessage(Request.CreateResponse(message.code, message.message));
        //}


        //[HttpPost]
        //[Route("api/add-account")]
        //public IHttpActionResult addNewAccount(Account account)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var errorList = (from item in ModelState.Values
        //                         from error in item.Errors
        //                         select error.ErrorMessage).ToArray();
        //        return StatusCode(HttpStatusCode.BadRequest);
        //    }
        //    Utils.Utils.SaveLog("AccountController", "/add-account", JsonConvert.SerializeObject(account));
        //    ReturnMessage message = accountService.addNewAccountForCustomer(account);

        //    if (message.code == HttpStatusCode.OK)
        //    {
        //        return Ok(message);
        //    }

        //    return ResponseMessage(Request.CreateResponse(message.code, message.message));
        //}



        //[HttpPost]
        //[Route("api/get-user-name-account")]
        //public IHttpActionResult getUserNameByAccount(UserAccountFiler userAccountFiler)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var errorList = (from item in ModelState.Values
        //                         from error in item.Errors
        //                         select error.ErrorMessage).ToArray();
        //        return StatusCode(HttpStatusCode.BadRequest);
        //    }
        //    Utils.Utils.SaveLog("AccountController", "/add-account", JsonConvert.SerializeObject(userAccountFiler));
        //    ReturnMessage message = accountService.GetUserNameByAccount(userAccountFiler.userId);

        //    if (message.code == HttpStatusCode.OK)
        //    {
        //        return Ok(message);
        //    }

        //    return ResponseMessage(Request.CreateResponse(message.code, message.message));
        //}







    }
}
