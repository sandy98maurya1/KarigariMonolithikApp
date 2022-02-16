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
    public class JobSearchController : ControllerBase
    {
        private ISearch _search;
        private readonly ILoggerManager _logger;
        public JobSearchController(ISearch search, ILoggerManager logger)
        {
            _search = search;
            _logger = logger;
        }

        [HttpGet, Route("/SearchByJobType")]
        public IActionResult GetCompanyByJobType(int jobType)
        {
            ApiListResponse<IEnumerable<Search>> responce = new ApiListResponse<IEnumerable<Search>>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _search.GetCompanyByJobType(jobType).SearchByJobTypeResponse();
            }
            catch (Exception ex)
            {
                responce = ex.CacheListExceptionResponse();
                _logger.LogInfo(ex.Message);
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);

        }

        [HttpGet, Route("/SearchByLocation")]
        public IActionResult GetCompanyByLocation(int jobType, int location, int workerType )
        {
            ApiListResponse<IEnumerable<Search>> responce = new ApiListResponse<IEnumerable<Search>>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _search.GetCompanyByLocation(jobType, location, workerType).SearchByJobTypeResponse();
            }
            catch (Exception ex)
            {
                responce = ex.CacheListExceptionResponse();
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
