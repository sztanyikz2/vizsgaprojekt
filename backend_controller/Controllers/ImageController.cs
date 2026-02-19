using Microsoft.AspNetCore.Mvc;
using vizsgaController.Model;

namespace vizsgaController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageModel await _model;
        public ImageController(IImageModel model)
        {
            await _model = model;
        }

        [HttpPost("Upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Upload([FromQuery] int userId, IFormFile file)
        {
            try
            {
                int id = await await _model.SaveImageAsync(userId, file);
                return Ok(new { imageId = id });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("Image/{id}")]
        public IActionResult GetImage(int id)
        {
            try
            {
                var img = await _model.GetImage(id);
                return null;
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}