using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using project.Data;
using project.Models;
using project.Models.DTO;
using project.Service.LoginService;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace project.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class UserController : ControllerBase
    {//Thêm xóa sửa 
        private readonly ITokenService _tokenService;
        private readonly Datacontext _context;
        

        public UserController(Datacontext context,ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<List<User>>> Login(DTOLogin request)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);
            if (user == null)
            {
                return Unauthorized();
            }
            if (request.Password != user.Password)
            {
                return NotFound("Sai password ");
            }

            var token = _tokenService.CreateToken(user);
           

            return Ok(token);
        }
        [HttpPost("Register")]
        public async Task<ActionResult<List<User>>> AddUser(UserDTO userDTO)

        {


            var user = new User
            {
                Username = userDTO.Username,
                Firstname = userDTO.Firstname,
                Lastname = userDTO.Lastname,
                Email = userDTO.Email,
                Password = userDTO.Password,

            };
            _context.Users.Add(user);

            await _context.SaveChangesAsync();
            var token = _tokenService.CreateToken(user);
            return Ok(token);
        }
        
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }
       /* [Authorize]*/
        [HttpPut("UpdateUser"),Authorize]
        public async Task<ActionResult<List<User>>> UpdatedUser(DTOupdate updatedUser )
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
            user.Password = updatedUser.Password;

            await _context.SaveChangesAsync();
            return Ok(await _context.Users.ToListAsync());
        }
        [HttpDelete("DeleteUser"),Authorize]
        public async Task<ActionResult<List<User>>> RemoveUser (int id )
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
//enable-migration
//add-migration
//update