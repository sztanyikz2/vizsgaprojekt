using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using vizsgaController.Dtos;
using vizsgaController.Model;

namespace vizsgaController.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserModel _model;
        public UserController(UserModel model)
        {
            _model = model;
        }
        [HttpPost("/registration")]
        public ActionResult Registration(string username, string password)
        {
            try
            {
                _model.Registration(username, password);
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
                    return NotFound();
                }
                List<Claim> claims = new()
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
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

        [Authorize(Roles = "Admin")]
        [HttpPut("/rolemodify")]
        public ActionResult RoleModify(int userid)
        {
            try
            {
                _model.RoleModify(userid);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPut("/modifypassword")]
        public ActionResult ModyfiyPassword(string username, string password)
        {
            try
            {
                _model.ModyfiyPassword(username, password);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
