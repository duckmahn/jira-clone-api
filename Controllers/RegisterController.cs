using managementapp.Data;
using managementapp.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace managementapp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly DataContext _context;


        public LoginController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }
      
        [HttpPost("Login")]
        public async Task<ActionResult<List<UserLogin>>> Login(DTOLogin request)
        {
            var user = Authentication(request);            
            var token = _tokenService.CreateToken(user);
            return Ok(token);
                    
        }
        [HttpPost("Register")]
        public async Task<ActionResult<List<UserLogin>>> AddUser(UserDTO userDTO)
        {
            var existingUser = await _context.UserLogins.SingleOrDefaultAsync(u => u.Email == userDTO.Email);

            if (existingUser != null)
            {
                return BadRequest("User with this email already exists");
            }
            var user = new Users
            {
                Username = userDTO.Username,
                Firstname = userDTO.Firstname,
                Lastname = userDTO.Lastname,
                Email = userDTO.Email,


            };
            var userLogin = new UserLogin
            {
                Email = userDTO.Email,
                Password = userDTO.Password,
            };
            _context.UserLogins.Add(userLogin);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            //var token = _tokenService.CreateToken(user);
            return Ok(user);
        }
        public UserLogin Authentication(DTOLogin request)
        {
            var user = _context.UserLogins.SingleOrDefault(u => u.Email == request.Email && u.Password == request.Password);
            if (user == null)
            {
                return null;
            }
            else
            {
                return user;
            }
        }
    }
}
