using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankMockAPI.Entity;
using Microsoft.AspNetCore.Mvc;

namespace BankMockAPI.Controllers
{
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpPost("~/Payment"), Produces("application/json")]
        public ActionResult<Response> PostPayment(Transaction transaction)
        {
            // Process payment
            // ...

            // Return response
            Response response = new Response();
            response.TransactionId = Guid.NewGuid();
            return Ok(response);
        }

        [HttpPost("~/Refund"), Produces("application/json")]
        public ActionResult<Response> PostRefund(Transaction transaction)
        {
            // Process refund
            // ...

            // Return response
            Response response = new Response();
            response.TransactionId = Guid.NewGuid();
            return Ok(response);
        }
    }
}
