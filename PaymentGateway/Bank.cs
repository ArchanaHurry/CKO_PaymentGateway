using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PaymentGateway.Entities;
using Newtonsoft.Json;

namespace PaymentGateway
{

    public class Bank
    {
        static HttpClient client = new HttpClient();

        public Bank ()
        {
        }

        public async Task<HttpResponseMessage> SendPayment(BankTransaction transaction)
        {
            string requestUri;

            // Serialize the JSON payment content into a string
            var requestContent = await Task.Run(() => JsonConvert.SerializeObject(transaction));

            // Wrap the payment content in a StringContent which can then be used by the HttpClient class
            var httpContent = new StringContent(requestContent, Encoding.UTF8, "application/json");

            // TODO: Method to determine bank api endpoint to route the transaction
            requestUri = "https://localhost:44316/Payment";

            // Send payment to bank
            var response = await client.PostAsync(requestUri, httpContent);
            return response;
        }

        public async Task<HttpResponseMessage> SendRefund(BankTransaction transaction)
        {
            string requestUri;

            // Serialize the JSON payment content into a string
            var requestContent = await Task.Run(() => JsonConvert.SerializeObject(transaction));

            // Wrap the payment content in a StringContent which can then be used by the HttpClient class
            var httpContent = new StringContent(requestContent, Encoding.UTF8, "application/json");

            // TODO: Method to determine bank api endpoint to route the transaction
            requestUri = "https://localhost:44316/Refund";

            // Send refund to bank
            var response = await client.PostAsync(requestUri, httpContent);
            return response;
        }
    }

}
