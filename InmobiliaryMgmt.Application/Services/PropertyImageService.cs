using InmobiliaryMgmt.Application.Interfaces;
using InmobiliaryMgmt.Domain.Entities;
using InmobiliaryMgmt.Domain.Interfaces;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;


namespace InmobiliaryMgmt.Application.Services
{
    public class PropertyImageService : IPropertyImageService
    {
        private readonly IRepository<PropertyImage> _repository;
        private readonly CloudinaryService _cloudinaryService;

        public PropertyImageService(IRepository<PropertyImage> repository, CloudinaryService cloudinaryService)
        {
            _repository = repository;
            _cloudinaryService = cloudinaryService;
        }

        public Task<IEnumerable<PropertyImage>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<PropertyImage?> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        // Nuevo método: Crear imagen desde archivo
        public async Task<PropertyImage> CreateAsync(int propertyId, IFormFile file)
        {
            // Subir a Cloudinary
            ImageUploadResult uploadResult = await _cloudinaryService.UploadImageAsync(file);

            var propertyImage = new PropertyImage
            {
                PropertyId = propertyId,
                Url = uploadResult.SecureUrl.ToString(),
                PublicId = uploadResult.PublicId
            };

            return await _repository.CreateAsync(propertyImage);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            // Aquí podrías eliminar la imagen de Cloudinary si quieres
            // await _cloudinaryService.DeleteImageAsync(entity.PublicId);

            await _repository.DeleteAsync(entity);
            return true;
        }
    }
}