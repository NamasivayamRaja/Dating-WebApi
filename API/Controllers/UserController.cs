using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [Authorize]
    public class UserController(IUserRepository<AppUser> repository, IMapper mapper, IPhotoService photoService
        ) : BaseController
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers()
        {
            var users = await repository.GetMemberAll();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberDTO>> GetUserById(int id)
        {
            var user = await repository.GetByIdAsync(id);

            if (user == null) 
            {
                return NotFound();
            }

            var userReturn = mapper.Map<MemberDTO>(user);

            return Ok(userReturn);
        }

        [HttpGet("userName/{userName}")] // api/users/raja
        public async Task<ActionResult<MemberDTO>> GetUserByName(string userName)
        {
            var user = await repository.GetMemberByUserNameAsync(userName);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberEditDTO memberUpdateDTO)
        {
            var userName = User.GetUserName();
            
            var user = await repository.GetByUserNameAsync(userName);
            
            if (user == null) return BadRequest("User profile not found");

            mapper.Map(memberUpdateDTO, user);

            if (await repository.SaveAllChangesAsync()) return NoContent();

            return BadRequest("Update Request failed");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult> AddPhotos(IFormFile file)
        {
            var userName = User.GetUserName();

            var user = await repository.GetByUserNameAsync(userName);

            if (user == null) return BadRequest("User profile not found");

            var result = await photoService.AddPhotoAsync(file);

            if(result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                IsMain = false,
                PublicId = result.PublicId,
            };

            user.Photos.Add(photo);

            if (await repository.SaveAllChangesAsync())
                return CreatedAtAction(nameof(GetUserByName), new { userName = userName }, mapper.Map<PhotoDTO>(photo));

            return BadRequest("Photo upload failed");
        }

        [HttpPut("set-main-photo/{photoId:int}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await repository.GetByUserNameAsync(User.GetUserName());

            if (user is null) return BadRequest("User not found");

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo == null || photo.IsMain) return BadRequest("Photo cannot be set as Main");

            var currentMainPhoto = user.Photos.FirstOrDefault(p => p.IsMain);

            if (currentMainPhoto is not null) currentMainPhoto.IsMain = false;

            photo.IsMain = true;

            if (await repository.SaveAllChangesAsync()) return NoContent();

            return BadRequest("Problem with setting the main photo");
        }

        [HttpDelete("delete-photo/{photoId:int}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await repository.GetByUserNameAsync(User.GetUserName());

            if (user is null) return BadRequest("Could not found the user!");

            var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo is null || photo.IsMain || photo.PublicId is null) return BadRequest("Photo is not found");

            var result = photoService.DeletePhotoAsync(photo.PublicId);

            user.Photos.Remove(photo);

            if(await repository.SaveAllChangesAsync()) { return NoContent(); }

            return BadRequest("Problem occurred while deleting!!");
        }
    }
}