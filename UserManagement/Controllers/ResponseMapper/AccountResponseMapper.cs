using Models;
using System;
using Utility;

namespace eFlowerPortal.ResponseMapper
{
    public static class AccountResponseMapper
    {
        public static TokenResponse SystemUserNotExist()
        {
            return new TokenResponse()
            {
                IsSuccess = false,
                Error = ErrorMessages.LoginFail,
                Error_description = ErrorMessages.LoginFail,
            };
        }

        public static TokenResponse ModelErrorTokenResponse()
        {
            return new TokenResponse()
            {
                IsSuccess = false,
                Error = ErrorMessages.UserOrPasswordEmpty,
                Error_description = ErrorMessages.UserOrPasswordEmpty,
            };
        }
    }
}
