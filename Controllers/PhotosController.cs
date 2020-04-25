using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.DTO.Photo;
using DatingApp.API.Helper;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/user/{user_id}/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDataRepository _dataRepository;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinarySettings;
        private readonly Cloudinary _cloudinary;

        public PhotosController(IDataRepository dataRepository, IMapper mapper, IOptions<CloudinarySettings> cloudinarySettings)
        {
            this._dataRepository = dataRepository;
            this._mapper = mapper;
            this._cloudinarySettings = cloudinarySettings;

            Account account = new Account(
                cloudinarySettings.Value.CloudName,
                cloudinarySettings.Value.ApiKey,
                cloudinarySettings.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(account);
        }


        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _dataRepository.GetPhoto(id);

            var photo = _mapper.Map<PhotoCreateDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int user_id, [FromForm]PhotoCreateDto photoCreateDto)
        {

            if (user_id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _dataRepository.GetUser(user_id);

            var file = photoCreateDto.file;

            var upload_result = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var file_upload_param = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    upload_result = _cloudinary.Upload(file_upload_param);
                }
            }

            photoCreateDto.url = upload_result.Uri.ToString();
            photoCreateDto.public_photo_id = upload_result.PublicId;

            var photo = _mapper.Map<Photo>(photoCreateDto);

            if (!userFromRepo.Photos.Any(u => u.is_main))
                photo.is_main = true;

            userFromRepo.Photos.Add(photo);

            if (await _dataRepository.SaveAll())
            {
                var photo_to_return = _mapper.Map<PhotoCreateDto>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photo_to_return);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpPost("{photo_id}/setMainPhoto")]
        public async Task<IActionResult> SetMainPhoto(int user_id, int photo_id)
        {
            if (user_id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _dataRepository.GetUser(user_id);

            if (!userFromRepo.Photos.Any(x => x.Id == photo_id))
                return Unauthorized();

            var photoFromRepo = await _dataRepository.GetPhoto(photo_id);
            if (photoFromRepo.is_main)
                return BadRequest("This photo is already the main photo.");

            var currentMainPhoto = await _dataRepository.GetMainPhoto(user_id);
            currentMainPhoto.is_main = false;

            photoFromRepo.is_main = true;

            if (await _dataRepository.SaveAll())
                return NoContent();

            return BadRequest("Error while set main photo.");
        }

        [HttpDelete("{photo_id}")]
        public async Task<IActionResult> DeleteUserPhoto(int user_id, int photo_id)
        {    
            if (user_id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var userFromRepo = await _dataRepository.GetUser(user_id);

            if (!userFromRepo.Photos.Any(x => x.Id == photo_id))
                return Unauthorized();

            var photoFromRepo = await _dataRepository.GetPhoto(photo_id);
            if (photoFromRepo.is_main)
                return BadRequest("You cannot delete your main photo.");


            if (photoFromRepo.public_photo_id != null)
            {
                var deleteParam = new DeletionParams(photoFromRepo.public_photo_id);
                var deleteResult = _cloudinary.Destroy(deleteParam);

                if (deleteResult.Result == "ok")
                    _dataRepository.Delete(photoFromRepo);
            }

            if (photoFromRepo.public_photo_id == null)
                _dataRepository.Delete(photoFromRepo);

            if (await _dataRepository.SaveAll())
                return Ok();

            return BadRequest("Something wrong when photo delete");
        }

    }
}