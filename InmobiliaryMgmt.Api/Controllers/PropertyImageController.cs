using InmobiliaryMgmt.Application.Services;
using InmobiliaryMgmt.Application.Interfaces;
using InmobiliaryMgmt.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InmobiliaryMgmt.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyImageController : ControllerBase
    {
        private readonly PropertyImageService _propertyImageService;
        private readonly CloudinaryService _cloudinaryService;

        public PropertyImageController(PropertyImageService propertyImageService,
                                       CloudinaryService cloudinaryService)
        {
            _propertyImageService = propertyImageService;
            _cloudinaryService = cloudinaryService;
        }

        // Obtener todas las imágenes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var images = await _propertyImageService.GetAllAsync();
            return Ok(images);
        }

        // Obtener una imagen por id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var image = await _propertyImageService.GetByIdAsync(id);
            if (image == null) return NotFound();
            return Ok(image);
        }

        // Subir una imagen y guardarla en la DB
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] int propertyId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No se proporcionó ningún archivo.");

            // Solo llamamos al servicio, que ya hace todo
            var createdImage = await _propertyImageService.CreateAsync(propertyId, file);
    
            return Ok(createdImage);
        }


        // Eliminar imagen
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _propertyImageService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
