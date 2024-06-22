using managementapp.Data.Models;
using managementapp.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace managementapp.Service.Implements
{
    public class DataService : IDataService
    {
        public Token DeToken(string token)
        {
            token = token.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var tokenData = new Token
            {
                Id = Guid.Parse(jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value),
                Email = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "Email")?.Value,
                Username = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "Username")?.Value,
            };

            return tokenData;
        }

        public string GetUserId(string token)
        {
            throw new NotImplementedException();
        }
    }
}
