using Google.Cloud.Storage.V1;
using managementapp.Data;
using managementapp.Data.Models;
using managementapp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;



namespace managementapp
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IDataService _dataService;
        private readonly IBlobService _blobService;

        public UserController(DataContext context, IDataService dataService, IBlobService blobService)
        {
            _context = context;
            _dataService = dataService;
            _blobService = blobService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Users>>> GetAllUser()
        {
            var users = await _context.Users.ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUserById(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("userAvatar")]
        public async Task<ActionResult<Users>> GetUserAvatar()
        {
            var userId = _dataService.GetTokenData().Id;
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            string avatar = user.Avatar;

            if (string.IsNullOrEmpty(user.Avatar))
            {
                return NotFound("User avatar not set.");
            }

            return Ok(avatar);
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

        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            string userId = _dataService.GetTokenData().Id.ToString();
            string containerName = "photo"; // Ensure this container exists in your Azure Blob Storage
            var publicUrl = await _blobService.SaveFile(containerName, file);
            // Assuming you have a method to update the user's image URL in your user management system
            var user = await _context.Users.FindAsync(new Guid(userId));
            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.Avatar = publicUrl;
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { Url = publicUrl });
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
        public async Task<ActionResult<List<Users>>> RemoveUser (Guid id )
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
