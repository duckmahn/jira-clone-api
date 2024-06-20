using managementapp.Data;
using managementapp.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace managementapp
{

    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {//Thêm xóa sửa 
        private readonly DataContext _context;


        public UserController(DataContext context, ITokenService tokenService)
        {
            _context = context;
           
        }

        [HttpGet]
        public async Task<ActionResult<List<UserLogin>>> GetAllUser()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
       /* [Authorize]*/
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
            return Ok(await _context.Users.ToListAsync());
        }
        [HttpDelete("DeleteUser"),Authorize]
        public async Task<ActionResult<List<UserLogin>>> RemoveUser (int id )
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) {
                return NotFound();
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok(await _context.Users.ToListAsync());
        }


    }
}
