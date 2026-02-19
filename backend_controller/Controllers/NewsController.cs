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
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Hiba történt");
            }
        }
        [HttpGet("search_post")]
        public async Task<ActionResult> GetPostBySearch([FromQuery] string title)
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
            catch (ArgumentException ex)
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
        public async Task<ActionResult> DeleteUsers([FromQuery] int id)
        {
            try
            {
                await _model.DeleteUsers(id);
                return Ok("User managed successfully");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
        [HttpPut("modify_user")]
        public async Task<ActionResult> ModifyUser(ModifyUserDTO userDto)
        {
            try
            {
                await _model.ModifyUsers(userDto);
                return Ok("User modified successfully");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Hiba történt");
            }
        }
        [HttpPost("create_posts")]
        public async Task<ActionResult> CreatePost([FromBody] PostDTO source)
        {
            try
            {
                await _model.CreatePost(source);
                return Ok("Post created successfully");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
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
        public async Task<ActionResult> DeletePosts([FromQuery] int id)
        {
            try
            {
                await _model.DeletePost(id);
                return Ok("Post deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
        [HttpDelete("delete_own_post")]
        public async Task<ActionResult> DeleteOwnPost([FromBody] DeleteOwnPostDTO deleteOwnpost)
        {
            try
            {
                await _model.DeleteOwnPost(deleteOwnpost);
                return Ok("Your post has been deleted");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
        [HttpPost("favourite_posts")]
        public async Task<ActionResult> FavouritePosts([FromBody] FavouritePostDTO dto)
        {
            try
            {
                await _model.FavouritePost(dto);
                return Ok("Post favourited/unfavourited successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception e)
            {
                return BadRequest("Hiba történt");
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("vote")]
        public async Task<ActionResult> Vote([FromBody] VoteDTO dto)
        {
            try
            {
                await _model.voteOnPost(dto);
                return Ok("Voted/Unvoted");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
        public async Task<ActionResult> Comment([FromBody] CommentDTO source)
        {
            try
            {
                await _model.CommentOnPost(source);
                return Ok("Comment posted");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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
        public async Task<ActionResult> DeleteSelectedComment([FromQuery] int id)
        {
            try
            {
                await _model.DeleteComments(id);
                return Ok("Comment deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
        [HttpPost("create_category")]
        public async Task<ActionResult> CreateCat([FromBody] CategoryDTO source)
        {
            try
            {
                await _model.CreateCategory(source);
                return Ok("Category created successfully");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
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
        [HttpDelete("delete_category")]
        public async Task<ActionResult> DeleteSelectedCategory([FromQuery] int id)
        {
            try
            {
                await _model.DeleteCategory(id);
                return Ok("Category deleted successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
        [HttpPost("create_report")]
        public async Task<ActionResult> CreateRep([FromBody] ReportDTO source)
        {
            try
            {
                await _model.CreateReport(source);
                return Ok("Report submitted successfully");
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
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
