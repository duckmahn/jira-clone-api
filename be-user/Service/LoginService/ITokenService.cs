using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using project.Data;
using project.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace project.Service.LoginService
{
    public interface ITokenService
    {

        string CreateToken(User user);

        /* private readonly IConfiguration configuration;
         public ITokenService( IConfiguration configuration)
         {

             this.configuration = configuration;
         }
         public string CreateToken(User user)
         {

             List<Claim> claims = new List<Claim>
             {
                 new Claim(ClaimTypes.Name, user.Username),

             };

             var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                 configuration.GetSection("AppSettings:Token").Value));

             var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

             var token = new JwtSecurityToken(
                 claims: claims,
                 expires: DateTime.Now.AddDays(1),
                 signingCredentials: creds);

             var jwt = new JwtSecurityTokenHandler().WriteToken(token);
             return jwt;

         }*/
    }
}
