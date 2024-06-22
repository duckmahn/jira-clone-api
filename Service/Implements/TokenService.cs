using managementapp.Data;
using managementapp.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace managementapp
{
    public class TokenService : ITokenService
    {

        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        

        public TokenService(IConfiguration configuration, DataContext context)
        {
            _context = context;
            _configuration = configuration;
        }

        public string CreateToken(UserLogin user)
        {
            var users = _context.Users.SingleOrDefault(u => u.Email == user.Email);
            List<Claim> claims = new List<Claim>
        {

            new Claim("Id",user.Id.ToString()),         
            new Claim("Email",user.Email),
            new Claim("Username",users.Firstname + " "+users.Lastname),
          
        };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }


    }
}
