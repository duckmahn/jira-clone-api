//using Google.Cloud.Storage.V1;
//using Microsoft.AspNetCore.Mvc;
//using managementapp.Data;
//using managementapp.Data.Models;
//using Microsoft.EntityFrameworkCore;

//namespace managementapp.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserImageController : ControllerBase
//    {
//        private readonly DataContext _context;


//        public UserImageController(DataContext context)
//        {
//            _context = context;

//        }

//        [HttpPost]
//        public async Task<IActionResult> Upload(IFormFile file)
//        {
//            var token = Request.Headers["Authorization"].ToString();

//            int userid = int.Parse(_loginService.GetCurrentUserId(token));

//            var bucketName = "todo-list_bucket";
//            var storage = StorageClient.Create();
//            var filename = DateTime.UtcNow.ToString("yyyyMMddHHmmss") + file.FileName;
//            var typename = file.ContentType;
//            using var stream = file.OpenReadStream();
//            await storage.UploadObjectAsync(bucketName, filename, typename, stream);
//            //todo-list_bucket/userImage
//            var publicUrl = $"https://storage.cloud.google.com/{bucketName}/{filename}";

//            var image = new UserImageModel { Url = publicUrl, UserId = userid };

//            if (_context.UserImages.Any(e => e.UserId == userid))
//            {
//                _context.UserImages.Remove(_context.UserImages.First(e => e.UserId == userid));
//            }
//            _context.UserImages.Add(image);
//            await _context.SaveChangesAsync();

//            return Ok(new { Url = publicUrl });
//        }
//        [HttpGet]
//        public async Task<ActionResult<UserImageModel>> GetImage()
//        {
//            var token = Request.Headers["Authorization"].ToString();
//            int userid = int.Parse(_loginService.GetCurrentUserId(token));

//            var image = _userImageRepository.GetByUserId(userid);
//            //var image = await _context.UserImages.FindAsync(id);
//            string url = image.Url;
//            if (image == null)
//            {
//                return NotFound();
//            }

//            return Ok(new { Url = url });
//        }
//        private bool UserImageModelExists(int id)
//        {
//            return _context.UserImages.Any(e => e.Id == id);
//        }
//    }
//}
