using managementapp.Data.Models;

namespace managementapp.Service.Interfaces;

public interface IDataService
{
    Token DeToken(string token);
    string GetUserId(string token);

}
