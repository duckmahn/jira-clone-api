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
       
         
    }
}
