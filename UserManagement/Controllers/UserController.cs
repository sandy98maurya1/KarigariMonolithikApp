using Contract;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using Utility;
using Utility.LoggerService;
using UserManagement.Controllers.UserResponseMapper;
using Microsoft.AspNetCore.Authorization;

namespace UserManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUsers _userDomain;
        private readonly ILoggerManager _logger;
        public UserController(IUsers user, ILoggerManager logger)
        {
            _userDomain = user;
            _logger = logger;
        }

        [HttpGet, Route("/GetUsers")]
        [Authorize(Roles = "admin")]
        public IActionResult GetUsers()
        {
            ApiResponse<IList<Users>> responce = new ApiResponse<IList<Users>>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _userDomain.GetAllUser().GetUsersResponce();
            }
            catch (Exception ex)
            {
                responce = ex.CacheListExceptionResponse();
                _logger.LogInfo(ex.Message);
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);

        }

        [HttpGet, Route("/GetUsersById")]
        public IActionResult GetUsersById(int id)
        {
            ApiResponse<Users> responce = new ApiResponse<Users>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _userDomain.GetUserById(id).GetUserResponce();
            }
            catch (Exception ex)
            {
                _logger.LogInfo(ex.Message);
                responce = ex.CacheExceptionResponse();
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpGet, Route("/GetUsersByName")]
        public ActionResult GetUsersByName(string name)
        {
            ApiResponse<Users> responce = new ApiResponse<Users>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _userDomain.GetUserByName(name).GetUserResponce();
            }
            catch (Exception ex)
            {
                _logger.LogInfo(ex.Message);
                responce = ex.CacheExceptionResponse();
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpPost, Route("/AddUser")]
        public ActionResult AddUser(Users users)
        {
            ApiResponse<Users> responce = new ApiResponse<Users>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _userDomain.AddUser(users).CreateProfileResponse(users);
            }
            catch (Exception ex)
            {
                _logger.LogInfo(ex.Message);
                responce = ex.CacheExceptionResponse();
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpPost, Route("/UpdateUser")]
        public ActionResult UpdateUser(Users users, int id)
        {
            ApiResponse<Users> responce = new ApiResponse<Users>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _userDomain.UpdateUser(users, id).CreateProfileResponse(users);
            }
            catch (Exception ex)
            {
                _logger.LogInfo(ex.Message);
                responce = ex.CacheExceptionResponse();
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpPost, Route("/DeleteUser")]
        public ActionResult DeleteUser(int userId)
        {
            ApiResponse<Users> responce = new ApiResponse<Users>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _userDomain.DeleteUser(userId).DisableProfileResponse();
            }
            catch (Exception ex)
            {
                _logger.LogInfo(ex.Message);
                responce = ex.CacheExceptionResponse();
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpGet, Route("/GetStateDetails")]
        public ActionResult GetStateDetails(int countryId)
        {
            ApiResponse<IList<StateDetails>> responce = new ApiResponse<IList<StateDetails>>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _userDomain.GetStateDetails(countryId).GetCountryResponce();
            }
            catch (Exception ex)
            {
                _logger.LogInfo(ex.Message);
                responce = ex.CacheExceptionCountryResponse();
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpGet, Route("/GetDivisionDetails")]
        public ActionResult GetDivisionDetails(int stateId)
        {
            ApiResponse< IList < DivisionDetails> > responce = new ApiResponse<IList<DivisionDetails>>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _userDomain.GetDivisionDetails(stateId).GetDivisionResponce();
            }
            catch (Exception ex)
            {
                _logger.LogInfo(ex.Message);
                responce = ex.CacheExceptionDivisionResponse();
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpGet, Route("/GetTalukaDetails")]
        public ActionResult GetTalukaDetails(int divisionId)
        {
            ApiResponse< IList < TalukaDetails >> responce = new ApiResponse<IList<TalukaDetails>>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _userDomain.GetTalukaDetails(divisionId).GetTalukaResponce();
            }
            catch (Exception ex)
            {
                _logger.LogInfo(ex.Message);
                responce = ex.CacheExceptionTalukaDetailsResponse();
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpPost, Route("/ApplyForJob")]
        public ActionResult ApplyForJob(JobApply jobApply)
        {
            ApiResponse<JobApply> responce = new ApiResponse<JobApply>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _userDomain.ApplyForJob(jobApply).ApplyForJobResponse();
            }
            catch (Exception ex)
            {
                _logger.LogInfo(ex.Message);
                responce = ex.CacheExceptionApplyJobResponse();
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
