using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vizsgaController.Dtos;
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
        [HttpGet("search_user")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUserNameBySearch([FromQuery] string name)
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
        [HttpGet("search_post")]
        public IActionResult GetPostBySearch([FromQuery] string title)
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
        [HttpDelete("delete_users")]
        public IActionResult DeleteUsers([FromQuery] int id)
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
        [HttpPut("modify_user")]
        public IActionResult ModifyUser(ModifyUserDTO userDto)
        {
            try
            {
                _model.ModifyUsers(userDto);
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
        [HttpPost("create_posts")]
        public IActionResult CreatePost([FromBody] PostDTO source)
        {
            try
            {
                _model.CreatePost(source);
                return Ok("Post created successfully");
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
        [HttpDelete("delete_posts")]
        public IActionResult DeletePosts([FromQuery] int id)
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
        [Authorize(Roles = "User")]
        [HttpDelete("delete_own_post")]
        public IActionResult DeleteOwnPost(DeleteOwnPostDTO deleteOwnpost)
        {
            try
            {
                _model.DeleteOwnPost(deleteOwnpost);
                return Ok("Your post has been deleted");
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
        [HttpPost("favourite_posts")]
        public IActionResult FavouritePosts(FavouritePostDTO dto)
        {
            try
            {
                _model.FavouritePost(dto);
                if (dto.addTo)
                {
                    return Ok("Post added to Favourites");
                }
                else
                {
                    return Ok("Post removed from Favourites");
                }
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
        
        [Authorize(Roles = "User")]
        [HttpPost("vote")]
        public IActionResult Upvote(VoteDTO dto)
        {
            try
            {
                _model.voteOnPost(dto);
                return Ok("Upvoted");
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
        
        [Authorize(Roles = "User")]
        [HttpPost("comment")]
        public IActionResult Comment([FromBody] CommentDTO source)
        {
            try
            {
                _model.CommentOnPost(source);
                return Ok("Comment posted");
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
        [HttpDelete("delete_comments")]
        public IActionResult DeleteSelectedComment([FromQuery] int id)
        {
            try
            {
                _model.DeleteComments(id);
                return Ok("Comment deleted successfully");
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
        [HttpPost("create_category")]
        public IActionResult CreateCat([FromBody] CategoryDTO source) //itt lehet [FromQuerry] kell?
        {
            try
            {
                _model.CreateCategory(source);
                return Ok("Category created successfully");
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
        [HttpDelete("delete_category")]
        public IActionResult DeleteSelectedCategory([FromQuery] int id)
        {
            try
            {
                _model.DeleteCategory(id);
                return Ok("Category deleted successfully");
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
        [HttpPost("create_report")]
        public IActionResult CreateRep([FromBody] ReportDTO source)
        {
            try
            {
                _model.CreateReport(source);
                return Ok("Report submitted successfully");
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
