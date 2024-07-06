using managementapp.Data.Models;

namespace managementapp.Service.Implements
{
    public class TokenSingletonService
    {

        private Token _tokenData;
        public Token TokenData
        {
            get => _tokenData;
            set => _tokenData = value;
        }

        private static TokenSingletonService _instance;
        public static TokenSingletonService Instance
            => _instance ?? (_instance = new TokenSingletonService());

        private TokenSingletonService()
        {
        }   
    }
}
