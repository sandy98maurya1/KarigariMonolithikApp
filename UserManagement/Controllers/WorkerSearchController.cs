using Contract;
using UserManagement.Controllers.ResponseMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using Utility;
using Utility.LoggerService;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerSearchController : ControllerBase
    {
        private IWorkerSearch _search;
        private readonly ILoggerManager _logger;
        public WorkerSearchController(IWorkerSearch search, ILoggerManager logger)
        {
            _search = search;
            _logger = logger;
        }

        [HttpGet, Route("/ByJobType")]
        public IActionResult GetWorkerByJobType(int jobType)
        {
            ApiListResponse<IEnumerable<WorkerSearch>> responce = new ApiListResponse<IEnumerable<WorkerSearch>>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _search.GetWorkerByJobType(jobType).SearchByJobTypeResponse();
            }
            catch (Exception ex)
            {
                responce = ex.CacheSearchListExceptionResponse();
                _logger.LogInfo(ex.Message);
            }
            _logger.LogInfo(responce.Message);
            return Ok(responce);

        }

        [HttpGet, Route("/ByLocation")]
        public IActionResult GetWorkerByLocation(int jobType, int location)
        {
            ApiListResponse<IEnumerable<WorkerSearch>> responce = new ApiListResponse<IEnumerable<WorkerSearch>>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = _search.GetWorkerByLocation(jobType, location).SearchByJobTypeResponse();
            }
            catch (Exception ex)
            {
                responce = ex.CacheSearchListExceptionResponse();
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
