using Contract;
using Contract.DataContracts;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class TokenUserDomain : ITokenUser
    {
        private readonly ITokenUserData tokenUserData;

        public TokenUserDomain(ITokenUserData _tokenUserData)
        {
            tokenUserData = _tokenUserData;
        }

         public TokenUser GetUser(string userName, string password)
        {
            return tokenUserData.GetUser(userName, password);
        }

        public TokenUser GetUserByUserName(string userName)
        {
            return tokenUserData.GetUserByUserName(userName);
        }


        public TokenUser GetUserById(int Id)
        {
            return tokenUserData.GetUserById(Id);
        }


        public TokenUser UpdateUser(TokenUser user)
        {
            return tokenUserData.UpdateUser(user);
        }
    }
}
