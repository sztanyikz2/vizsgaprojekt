using Microsoft.AspNetCore.Mvc;
using vizsgaController.Model;

namespace vizsgaController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ImageModel _model;
        public ImageController(ImageModel model)
        {
            _model = model;
        }

        [HttpPost("Upload")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Upload([FromQuery] int userId, IFormFile file)
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

        [HttpGet("Image/{id}")]
        public ActionResult GetImage(int id)
        {
            try
            {
                var img = _model.GetImage(id);
                return null;
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}