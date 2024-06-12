using managementapp.Data.Models;

namespace managementapp
{
    public interface ITokenService
    {

        string CreateToken(UserLogin user);


    }
}
