using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using vizsgaController.Dtos;
using vizsgaController.Model;

namespace vizsgaController.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginModel _model;
        public LoginController(LoginModel model)
        {
            _model = model;
        }
        [HttpPost("/registration")]
        public ActionResult Registration(string username, string password, bool IsAdmin)
        {
            try
            {
                string role = IsAdmin ? "Admin" : "User";
                _model.Registration(username, password, role);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("/login")]
        public async Task<ActionResult> LogIn(string username, string password)
        {
            try
            {
                var user = _model.ValidateUser(username, password);
                if (user == null)
                {
                    return null;
                }
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.userID.ToString()),
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim(ClaimTypes.Role, user.Role )
                };
                var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(id);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Ok(new { role = user.Role });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("/logout")]
        public async Task<ActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

       
    }
}
