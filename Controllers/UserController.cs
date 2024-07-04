using Google.Cloud.Storage.V1;
using managementapp.Data;
using managementapp.Data.Models;
using managementapp.Service.Interfaces;
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
        private readonly IDataService _dataService;

        public UserController(DataContext context, IDataService dataService)
        {
            _context = context;
            _dataService = dataService;
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
        [HttpPost("UploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            string userId = _dataService.GetTokenData().Id.ToString();

            var bucketName = "jira-clone";
            var storage = StorageClient.Create();
            var filename = $"{DateTime.UtcNow:yyyyMMddHHmmss}_{file.FileName}";
            var contentType = file.ContentType;

            using (var stream = file.OpenReadStream())
            {
                await storage.UploadObjectAsync(bucketName, filename, contentType, stream);
            }

            var publicUrl = $"https://storage.googleapis.com/{bucketName}/{filename}";

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
