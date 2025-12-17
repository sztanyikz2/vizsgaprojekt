using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vizsgaController.Model;

namespace vizsgaController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {

        private readonly INewsModel _model;
        public NewsController(INewsModel model)
        {
            _model = model;
        }
        [HttpGet("/searchuser")]
        public IActionResult GetUserNameBySearch([FromQuery] string name)
        {
            try
            {
                var users = _model.GetUserNamesBySearch(name);
                return Ok(users);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Hiba történt");
            }
        }
        [HttpGet("searchpost")]
        public IActionResult GetPostBySeatch([FromQuery] string title)
        {
            try
            {
                var posts = _model.GetPostsBySearch(title);
                return Ok(posts);
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Hiba történt");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("deletepost")]
        public IActionResult DeletePost([FromQuery] int id)
        {
            try
            {
                _model.DeletePost(id);
                return Ok("Post deleted successfully");
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Hiba történt");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("moderatecomments")]
        public IActionResult ModerateComments([FromQuery] int id)
        {
            try
            {
                _model.ModerateComments(id);
                return Ok("Comment moderated successfully");
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Hiba történt");
            }
        }
        [Authorize(Roles = "User")]
        [HttpDelete("deleteownpost")]
        public IActionResult DeleteOwnPost([FromQuery] int postid, [FromQuery] int userid)
        {
            try
            {
                _model.DeleteOwnPost(postid, userid);
                return Ok("Saját poszt sikeresen törölve");
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Hiba történt");
            }
        }
        [HttpPost("favouriteposts")]
        public IActionResult FavouritePosts([FromQuery] int postID, [FromQuery] int userID)
        {
            try
            {
                _model.FavouritePost(postID, userID);
                return Ok("Poszt sikeresen hozzáadva a kedvencekhez");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Hiba történt");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("manageusers")]
        public IActionResult ManageUsers([FromQuery] int id)
        {
            try
            {
                _model.DeleteUsers(id);
                return Ok("User managed successfully");
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Hiba történt");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("modifyusers")]
        public IActionResult ModifyUsers([FromQuery] int id, [FromQuery] string name)
        {
            try
            {
                _model.ModifyUsers(id, name);
                return Ok("User modified successfully");
            }
            catch (InvalidDataException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Hiba történt");
            }
        }

    }

}
