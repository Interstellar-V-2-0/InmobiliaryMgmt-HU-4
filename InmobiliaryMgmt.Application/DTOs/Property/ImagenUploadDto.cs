using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace InmobiliaryMgmt.Application.DTOs.Property
{

    public class ImageUploadDto
    {
        [Required(ErrorMessage = "Debe proporcionar un archivo de imagen.")]
        public IFormFile File { get; set; } = default!;

        [Required(ErrorMessage = "Debe especificar el ID de la propiedad.")]
        public int PropertyId { get; set; }
    }
}