using Models;
using System.Collections.Generic;

namespace Contract.DataContracts
{
    public interface ITokenUserData
    {
        public TokenUser GetUser(string userName, string password);
        public TokenUser GetUserByUserName(string userName);
        public TokenUser GetUserById(int Id);
        public TokenUser UpdateUser(TokenUser user);
        
    }
}
