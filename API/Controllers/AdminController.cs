using API.Entities;
using API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController(UserManager<AppUser> userManager) : BaseController
    {
        [HttpGet]
        [Authorize(Policy = "RequiredAdminRole")]
        [Route("users-with-roles")]
        public async Task<ActionResult> GetUsersWithRole()
        {
            var users = await userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpPost("edit-roles/{userName}")]
        [Authorize(Policy = "RequiredAdminRole")]
        public async Task<ActionResult> EditRoles(string userName, [FromQuery] string roles)
        {
            if(!roles.IsNotNull()) { return BadRequest("Atleast one role should be selected!"); }
            
            var selectedRoles = roles.Split(",").ToArray();

            var user = await userManager.FindByNameAsync(userName);

            if (user == null) { return BadRequest("User not found!"); }

            var userRoles = await userManager.GetRolesAsync(user);

            var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded)
            {
                return BadRequest("Failed to add to roles");
            }

            result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if (!result.Succeeded)
            {
                return BadRequest("Failed to remove from roles");
            }

            return Ok(await userManager.GetRolesAsync(user));
        }

        [HttpGet]
        [Authorize(Policy = "ModeratePhotoRole")]
        [Route("photos-to-moderate")]
        public async Task<ActionResult> ModeratePhoto()
        {
            return Ok("This is the admin and Moderator route for users with role.");
        }
    }
}
