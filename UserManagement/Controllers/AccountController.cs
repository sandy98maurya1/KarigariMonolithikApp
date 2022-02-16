using Contract;
using eFlowerPortal.ResponseMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.LoggerService;

namespace TokenGenerator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenUser user;
        private readonly AppSettings appSettings;
        private readonly ILoggerManager loggerManager;
        public AccountController(ITokenUser _user, AppSettings _appSettings, ILoggerManager _loggerManager)
        {
            user = _user;
            appSettings = _appSettings;
            loggerManager = _loggerManager;
            
        }

        [HttpPost, Route("LogOn")]
        [AllowAnonymous]
        public async Task<IActionResult> TokenValue([FromBody] TokenRequest tokenRequest)
        {

            TokenResponse tokenResponse = new TokenResponse();
            ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors(tokenRequest);
            dynamic userObj = null;

            if (modelErrors.Error.Count() > 0)
            {
                loggerManager.LogInfo(ErrorMessages.UserOrPasswordEmpty);
                return Ok(AccountResponseMapper.ModelErrorTokenResponse());
            }

            userObj = ValidateSystemUser(tokenRequest);
            if (userObj is null)
            {
                loggerManager.LogInfo(ErrorMessages.LoginFail);
                tokenResponse = AccountResponseMapper.SystemUserNotExist();
                return Ok(tokenResponse);
            }

            tokenResponse = GenerateToken(userObj);
            if (string.IsNullOrEmpty(tokenResponse.Error))
            {
                tokenResponse.IsSuccess = true;
                tokenResponse.UserName = userObj.UserName;
                tokenResponse.Token_type = Constants.TokenType;
            }

            return Ok(tokenResponse);

        }

        private TokenResponse GenerateToken(dynamic userObj)
        {
            TokenResponse tokenResponse = new TokenResponse();

            var token = GetAccessToken(userObj);

            tokenResponse.Access_token = token;
            tokenResponse.AccessRights = userObj.Role;
            tokenResponse.Expires = DateTime.Now;
            tokenResponse.TimeOutMins = (int)Utility.Enums.AppSettings.ScreenTimeoutMins;

            return tokenResponse;
        }

        private string GetAccessToken(dynamic userObj)
        {
            var key = Encoding.UTF8.GetBytes(appSettings.Secret);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, appSettings.Audiance.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim(Constants.ClaimTypeId,userObj.Id.ToString()),
                new Claim(Constants.ClaimTypeFullName, userObj.UserName),
                new Claim(ClaimTypes.Role, userObj.Role),
            };

            var token = new JwtSecurityToken(
                    appSettings.Issuer,
                    appSettings.Issuer,
                    claims,
                    null,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
              );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private TokenUser ValidateSystemUser(TokenRequest request)
        {
            TokenUser userObj = user.GetUserByUserName(request.Username);
            //request.Password = SHA256Cryptography.GetHashString(request.Password);

            if (userObj != null && (request.Username.Equals(userObj.UserName) && request.Password.Equals(userObj.Password)))
                return userObj;

            return userObj;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Secret)),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        [HttpPost, Route("GetRefreshToken")]
        //[Authorize(Roles = Roles.VENDOR + "," + Roles.ADMIN + "," + Roles.USER)]
        public async Task<IActionResult> Refresh(RefreshTokenRequest refreshTokenRequest)
        {
            TokenResponse tokenResponse = new TokenResponse();
            var principal = GetPrincipalFromExpiredToken(refreshTokenRequest.token);
            var username = principal.Identity.Name; //this is mapped to the Name claim by default
            var email = principal.Claims.FirstOrDefault(c => c.Type == "Email")?.Value;

            var userData = user.GetUserByUserName(username);
            if (userData is null || userData.RefreshToken != refreshTokenRequest.refreshToken) return BadRequest();

            var newJwtToken = GetAccessToken(userData);
            var newRefreshToken = Utility.CommonUitilityFunctions.GenerateRefreshToken();

            userData.RefreshToken = newRefreshToken;
           // user.UpdateUser(userData);

            tokenResponse.Access_token = newJwtToken;
            tokenResponse.RefreshToken = refreshTokenRequest.refreshToken;

            return Ok(tokenResponse);
        }

        private ApiExposeResponse<Dictionary<string, string>> GetModelErrors<T>(T Model)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>();
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    string errordetails = string.Empty;
                    foreach (var error in state.Value.Errors)
                    {
                        errordetails = errordetails + error.ErrorMessage;
                    }

                    errors.Add(state.Key.Contains(".") ? state.Key.Split('.')[1] : state.Key, errordetails);
                }
            }

            return new ApiExposeResponse<Dictionary<string, string>>
            {
                IsSuccess = false,
                Message = ErrorMessages.InValidInputMsg,
                Error = errors
            };
        }

    }
}
