using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MiddlewareAuth;
using MiddlewareAuth.Models;

namespace BackEndServices.CustomMiddleware
{
    public class MerchantValidationMiddleware
	{
        private readonly RequestDelegate next;
        private readonly IMerchantValidation _merchantValidation;

        public MerchantValidationMiddleware(RequestDelegate next, IMerchantValidation merchantValidation)
		{
            this.next = next;
            this._merchantValidation = merchantValidation;
        }

        public async Task Invoke(HttpContext context)
        {
            // Get the client IP address from the request
            string clientIpAddress = context.Connection.RemoteIpAddress.ToString();
            var endpoint = context.GetEndpoint();
            if (endpoint == null)
            {
                //means its prob startup
                await next(context);
            }
            else
            {
                // Get the client auth from the request
                string authParameter = context.Request.Headers.Authorization.ToString();

                var merchant = _merchantValidation.IsUserAuthorized(authParameter, clientIpAddress, endpoint.DisplayName);
                if (string.IsNullOrEmpty(merchant))
                {
                    await EndRequest(context);
                }
                else
                {
                    var serviceActivatedChecking = _merchantValidation.IsServiceActivatedForMerchantNew(merchant);
                    if (serviceActivatedChecking.code != 201)
                    {
                        await EndAllTransferRequest(context, serviceActivatedChecking);
                    }
                    else
                    {
                        string requestBody;
                        if (endpoint.DisplayName.Equals("transfer", StringComparison.OrdinalIgnoreCase))
                        {
                            // Read the request body
                            using StreamReader reader = new(context.Request.Body, Encoding.UTF8);
                            requestBody = await reader.ReadToEndAsync();
                            var transferParam = JsonSerializer.Deserialize<transferRequestToBank>(requestBody);
                            var response = _merchantValidation.ValidateTransfer(transferParam, merchant);
                            if (!response.status)
                            {
                                await EndAllTransferRequest(context, response.response);
                            }
                            else
                            {
                                // Continue to the next middleware in the pipeline
                                await next(context);
                            }

                        }
                    }
                }
            }

        }

        private static async Task EndRequest(HttpContext context)
        {
            // Return an unauthorized / forbidden response if the check is not valid
            var response = Activator.CreateInstance<transferResponse>();
            response.message = "UNAUTHORIZED";
            response.code = 401;
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task EndAllTransferRequest<T>(HttpContext context, T response)
        {
            // Return an unauthorized / forbidden response if the check is not valid
            await context.Response.WriteAsJsonAsync<T>(response);
        }
    }
}

