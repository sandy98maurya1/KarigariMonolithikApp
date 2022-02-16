using Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using UserManagement.Controllers.ResponseMapper;
using Utility;
using Utility.LoggerService;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private IProfile _profileDomain;
        private readonly ILoggerManager _logger;
        public ProfileController(IProfile profile, ILoggerManager logger)
        {
            _profileDomain = profile;
            _logger = logger;
        }

        [HttpGet, Route("/GebProfileByUserId")]
        public IActionResult GetProfileByUserId(int userId)
        {
            ApiResponse<JobProfile> responce = new ApiResponse<JobProfile>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _profileDomain.GetProfileByUserId(userId).GetProfileResponce();
            }
            catch (Exception ex)
            {
                responce = ex.CacheJobProfileExceptionResponse();
                _logger.LogInfo(ex.Message);
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);

        }


        [HttpPost, Route("/CreateUserJobProfile")]
        public IActionResult CreateUserJobProfile(JobProfile profile)
        {
            ApiResponse<JobProfile> responce = new ApiResponse<JobProfile>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _profileDomain.CreateUserJobProfile(profile).GetProfileResponce();
            }
            catch (Exception ex)
            {
                responce = ex.CacheJobProfileExceptionResponse();
                _logger.LogInfo(ex.Message);
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);

        }

        [HttpPost, Route("/UpdateUserJobProfile")]
        public IActionResult UpdateUserJobProfile(JobProfile profile, int profilrId)
        {
            ApiResponse<JobProfile> responce = new ApiResponse<JobProfile>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _profileDomain.UpdateUserJobProfile(profile, profilrId).GetProfileResponce();
            }
            catch (Exception ex)
            {
                responce = ex.CacheJobProfileExceptionResponse();
                _logger.LogInfo(ex.Message);
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);

        }

        private ApiExposeResponse<Dictionary<string, string>> GetModelErrors()
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
