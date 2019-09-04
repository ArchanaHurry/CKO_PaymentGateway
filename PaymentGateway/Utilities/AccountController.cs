using System.Threading.Tasks;
using PaymentGateway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// Attention:
// This class is used to register new user in the Auth Server
// It is used to facilitate testing and must not be implemented in production environments
// Endpoint: "~/Account/Register"
// Body:
//{
//	"Email": "xxx",
//	"Password": "xxx"
//}

namespace PaymentGateway.Utilities
{
    // https://github.com/openiddict/openiddict-samples/blob/dev/samples/PasswordFlow/AuthorizationServer/Controllers/AccountController.cs
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthorizationDbContext _applicationDbContext;

        public AccountController(
            UserManager<IdentityUser> userManager,
            AuthorizationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        [HttpPost("~/Account/Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterUserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    return StatusCode(StatusCodes.Status409Conflict);
                }

                user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return Ok();
                }
                AddErrors(result);
            }

            // If we got this far, something failed.
            return BadRequest(ModelState);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        #endregion
    }
}