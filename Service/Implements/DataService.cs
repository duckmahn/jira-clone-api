using managementapp.Data.Models;
using managementapp.Service.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace managementapp.Service.Implements
{
    public class DataService : IDataService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DataService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
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
            TokenSingletonService.Instance.TokenData = tokenData;
            return tokenData;
        }

        public Token GetTokenData()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            token = token.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);

            var tokenData = new Token
            {
                Id = Guid.Parse(jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value),
                Email = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "Email")?.Value,
                Username = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "Username")?.Value,
            };
            TokenSingletonService.Instance.TokenData = tokenData;
            return tokenData;
        }

        public string GetUserId(string token)
        {
            throw new NotImplementedException();
        }
    }
}
