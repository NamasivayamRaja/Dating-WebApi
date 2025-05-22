using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UserController : BaseController
    {
        private readonly DataContext _dataContext;

        public UserController(DataContext dataContext) 
        {
            _dataContext = dataContext;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _dataContext.Users.ToListAsync();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var user = await _dataContext.Users.FindAsync(id);

            if (user == null) 
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
