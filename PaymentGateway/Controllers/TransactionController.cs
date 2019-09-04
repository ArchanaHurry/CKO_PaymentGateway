using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Validation;
using PaymentGateway.Entities;
using PaymentGateway.Models;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using PaymentGateway.Types;
using Newtonsoft.Json.Linq;
using PaymentGateway.Utilities;

namespace PaymentGateway.Controllers
{
    [Authorize(AuthenticationSchemes = OpenIddictValidationDefaults.AuthenticationScheme)]
    [ApiController]
    public class TransactionController : Controller
    {
        private readonly TransactionDbContext _TransactionContext;
        private readonly MerchantUsernameDbContext _MerchantUsernameContext;

        public TransactionController(TransactionDbContext Transaction_Context, MerchantUsernameDbContext MerchantUsernameContext)
        {
            _TransactionContext = Transaction_Context;
            _MerchantUsernameContext = MerchantUsernameContext;
        }

        [HttpPost("~/Payment"), Produces("application/json")]
        public async Task<IActionResult> PostPayment([FromBody] Transaction transaction)
        {
            // Determine merchant from username
            var merchantUsername = _MerchantUsernameContext.MerchantUsername.Where(s => s.Username == User.Identity.Name.ToString()).FirstOrDefault();
            if (merchantUsername == null)
            {
                return Unauthorized();
            }
            else
            {
                // Forward payment to bank
                var BankTransaction = new BankTransaction
                {
                    PayeeBankAccount = transaction.MerchantBankAccount,
                    PayerCardNumber  = transaction.CardNumber,
                    ExpMonth         = transaction.ExpMonth,
                    ExpYear          = transaction.ExpYear,
                    CVV              = transaction.CVV,
                    Amount           = transaction.Amount,
                    Currency         = transaction.Currency
                };

                Bank bank = new Bank();
                HttpResponseMessage response = await bank.SendPayment(BankTransaction);

                // Retrieve bank transaction id
                var content = await response.Content.ReadAsStringAsync();
                var jsonContent = JObject.Parse(content);
                string BankTransactionId;

                try
                {
                    BankTransactionId = jsonContent["transactionId"].ToString();
                }
                catch (NullReferenceException e)
                {
                    BankTransactionId = "";
                }

                // Saves transaction in database
                var NewTransaction = new Transaction()
                {
                    Merchant            = merchantUsername.Merchant.ToString(),
                    MerchantBankAccount = transaction.MerchantBankAccount,
                    CardNumber          = transaction.CardNumber,
                    ExpMonth            = transaction.ExpMonth,
                    ExpYear             = transaction.ExpYear,
                    CVV                 = transaction.CVV,
                    Amount              = transaction.Amount,
                    Currency            = transaction.Currency,
                    TransactionType     = "Payment",
                    BankTransactionId   = BankTransactionId,
                    BankResponseStatus  = response.StatusCode.ToString(),
                    DateTime            = DateTime.UtcNow,
                    Status              = (response.StatusCode == HttpStatusCode.OK ? TransactionStatus.Processed : TransactionStatus.Failed)
                };

                _TransactionContext.Transaction.Add(NewTransaction);
                await _TransactionContext.SaveChangesAsync();

                // Retrieve the transaction ID
                string CurrentTransactionId = _TransactionContext.Transaction.Where(s => s.Merchant        == NewTransaction.Merchant &&
                                                                                         s.CardNumber      == NewTransaction.CardNumber &&
                                                                                         s.Amount          == NewTransaction.Amount &&
                                                                                         s.TransactionType == NewTransaction.TransactionType &&
                                                                                         s.Status          == NewTransaction.Status &&
                                                                                         s.DateTime        == NewTransaction.DateTime
                                                                                    ).FirstOrDefault().Id.ToString();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Ok("Payment Processed. " + "Transaction ID: " + CurrentTransactionId);
                }
                else
                {
                    return  BadRequest("Payment Failed. " + "Transaction ID: " + CurrentTransactionId);
                }
            }
        }

        [HttpPost("~/Refund"), Produces("application/json")]
        public async Task<IActionResult> PostRefund([FromBody] Transaction transaction)
        {
            // Determine merchant from username
            var merchantUsername = _MerchantUsernameContext.MerchantUsername.Where(s => s.Username == User.Identity.Name.ToString()).FirstOrDefault();
            if (merchantUsername == null)
            {
                return Unauthorized();
            }
            else
            {
                // Forward refund to bank
                var BankTransaction = new BankTransaction
                {
                    PayeeBankAccount = transaction.MerchantBankAccount,
                    PayerCardNumber  = transaction.CardNumber,
                    ExpMonth         = transaction.ExpMonth,
                    ExpYear          = transaction.ExpYear,
                    CVV              = transaction.CVV,
                    Amount           = transaction.Amount,
                    Currency         = transaction.Currency
                };

                Bank bank = new Bank();
                HttpResponseMessage response = await bank.SendRefund(BankTransaction);

                // Retrieve bank transaction id
                var content = await response.Content.ReadAsStringAsync();
                var jsonContent = JObject.Parse(content);
                string BankTransactionId;

                try
                {
                    BankTransactionId = jsonContent["transactionId"].ToString();
                }
                catch (NullReferenceException e)
                {
                    BankTransactionId = "";
                }

                // Saves transaction in database
                DateTime transactionTime = DateTime.UtcNow;

                var NewTransaction = new Transaction()
                {
                    Merchant            = merchantUsername.Merchant.ToString(),
                    MerchantBankAccount = transaction.MerchantBankAccount,
                    CardNumber          = transaction.CardNumber,
                    ExpMonth            = transaction.ExpMonth,
                    ExpYear             = transaction.ExpYear,
                    CVV                 = transaction.CVV,
                    Amount              = transaction.Amount,
                    Currency            = transaction.Currency,
                    TransactionType     = "Refund",
                    BankTransactionId   = BankTransactionId,
                    BankResponseStatus  = response.StatusCode.ToString(),
                    DateTime            = transactionTime,
                    Status              = (response.StatusCode == HttpStatusCode.OK ? TransactionStatus.Processed : TransactionStatus.Failed)
                };

                _TransactionContext.Transaction.Add(NewTransaction);
                await _TransactionContext.SaveChangesAsync();

                // Retrieve the transaction ID
                string CurrentTransactionId = _TransactionContext.Transaction.Where(s => s.Merchant        == NewTransaction.Merchant &&
                                                                                         s.CardNumber      == NewTransaction.CardNumber &&
                                                                                         s.Amount          == NewTransaction.Amount &&
                                                                                         s.TransactionType == NewTransaction.TransactionType &&
                                                                                         s.Status          == NewTransaction.Status &&
                                                                                         s.DateTime        == NewTransaction.DateTime
                                                                                    ).FirstOrDefault().Id.ToString();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Ok("Refund Processed. " + "Transaction ID: " + CurrentTransactionId);
                }
                else
                {
                    return BadRequest("Refund Failed. " + "Transaction ID: " + CurrentTransactionId);
                }
            }
        }

        // ~/GetTransaction?id=
        [HttpGet("~/GetTransaction")]
        public async Task<ActionResult<string>> GetTransactionById(Guid id)
        {
            // Determine merchant from username
            var merchantUsername = _MerchantUsernameContext.MerchantUsername.Where(s => s.Username == User.Identity.Name.ToString()).FirstOrDefault();
            if (merchantUsername == null)
            {
                return Unauthorized();
            }
            else
            {
                // Search the transaction in the database
                var transaction = await _TransactionContext.Transaction.FindAsync(id);

                if (transaction == null)
                {
                    return NotFound();
                }

                // Allow merchant to view their own transactions only
                if (transaction.Merchant != merchantUsername.Merchant)
                {
                    return Unauthorized();
                }


                // Generare response
                Response response = new Response()
                {
                    TransactionId       = transaction.Id.ToString(),
                    BankTransactionId   = transaction.BankTransactionId.ToString(),
                    MerchantBankAccount = transaction.MerchantBankAccount.ToString(),
                    CardNumber          = transaction.CardNumber.ToString(),
                    ExpMonth            = transaction.ExpMonth,
                    ExpYear             = transaction.ExpYear,
                    CVV                 = transaction.CVV,
                    Amount              = transaction.Amount,
                    Currency            = transaction.Currency,
                    TransactionType     = transaction.TransactionType,
                    DateTime            = transaction.DateTime.ToString("dd/MM/yyyy HH:mm:ss"),
                    Status              = Enum.GetName(typeof(TransactionStatus),transaction.Status),
                };

                // Mask the Card number
                CardNumMasker maskedNum = new CardNumMasker();
                response.MerchantBankAccount = maskedNum.MaskNumber(response.MerchantBankAccount);
                response.CardNumber = maskedNum.MaskNumber(response.CardNumber);

                // Send response
                return Ok(response);

            }
        }
    }
}