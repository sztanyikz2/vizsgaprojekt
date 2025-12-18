using Microsoft.AspNetCore.Mvc;
using vizsgaController.Model;

namespace vizsgaController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController:ControllerBase
    {
        private readonly IImageModel _model;
        public ImageController(IImageModel model)
        {
            _model = model;
        }

        [HttpPost("/Upload")]
        public async Task<ActionResult> Upload([FromForm] int userId, [FromForm] IFormFile file)
        {
            try
            {
                int id = await _model.SaveImageAsync(userId, file);
                return Ok(new { imageId = id });
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet("/Image/{id}")]
        public IActionResult GetImage(int id)
        {
            try
            {
                var img = _model.GetImage(id);
                ///return File(img.Content, img.ContentType);
                return null;
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
