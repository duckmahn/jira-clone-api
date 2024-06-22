using managementapp.Data;
using managementapp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace managementapp
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        public UserController(DataContext context, ITokenService tokenService)
        {
            _context = context;
           
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetAllUser()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
        [HttpPost("addUser")]
        public async Task<ActionResult<List<Users>>> AddUser(Users user)
        {
            var newUser = new Users
            {
                Id = Guid.NewGuid(),
                Username = user.Username,
                Email = user.Email,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Avatar = user.Avatar,
                Phone = user.Phone,
                Password = user.Password
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok( _context.Users.FindAsync(newUser.Id));
        }
        [HttpPut("UpdateUser"),Authorize]
        public async Task<ActionResult<List<Users>>> UpdatedUser(DTOupdate updatedUser)
        {
            
            var user = await _context.Users.FindAsync(updatedUser.Id);
            if(user == null)
            {
                return NotFound();
            }
          
            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.Firstname = updatedUser.Firstname;
            user.Lastname = updatedUser.Lastname;

            await _context.SaveChangesAsync();
            return Ok(await _context.Users.FindAsync(user.Id));
        }
        [HttpDelete("DeleteUser"),Authorize]
        public async Task<ActionResult<List<Users>>> RemoveUser (int id )
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
