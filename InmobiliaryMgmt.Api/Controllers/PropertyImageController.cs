using InmobiliaryMgmt.Application.DTOs.Property; 
using InmobiliaryMgmt.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace InmobiliaryMgmt.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PropertyImageController : ControllerBase
    {
        private readonly IPropertyImageService _propertyImageService;
       
        public PropertyImageController(IPropertyImageService propertyImageService)
        {
            _propertyImageService = propertyImageService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var images = await _propertyImageService.GetAllAsync();
            return Ok(images);
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var image = await _propertyImageService.GetByIdAsync(id);
            if (image == null) return NotFound();
            return Ok(image); 
        }
        
        [HttpPost("upload")]
 
        public async Task<IActionResult> Upload([FromForm] ImageUploadDto dto) 
        {
            if (dto.File == null || dto.File.Length == 0)
 
                return BadRequest("No se proporcionó ningún archivo o el archivo está vacío.");
            
            try
            {

                var createdImage = await _propertyImageService.CreateAsync(dto.PropertyId, dto.File);
                return CreatedAtAction(nameof(GetById), new { id = createdImage.Id }, createdImage);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = $"Error al subir la imagen: {ex.Message}" });
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            
            var deleted = await _propertyImageService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}