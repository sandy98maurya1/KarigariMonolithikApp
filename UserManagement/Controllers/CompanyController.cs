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
    public class CompanyController : ControllerBase
    {
        private readonly ICompany company;
        private readonly ILoggerManager logger;

        public CompanyController(ICompany _company, ILoggerManager _logger)
        {
            company = _company;
            logger = _logger;
        }

        [HttpPost, Route("/CreateCompany")]
        public IActionResult CreateCompany(Company model)
        {
            ApiResponse<bool> responce = new ApiResponse<bool>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = company.CreateCompany(model).CreateCompanyResponse();

            }
            catch (System.Exception ex)
            {
                responce = CompanyResponseMapper.CacheExceptionResponse(ex);
                logger.LogInfo(ex.Message); ;
            }

            logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpPost, Route("/UpdateCompany")]
        public IActionResult UpdateCompany(Company model, int Id)
        {
            ApiResponse<bool> responce = new ApiResponse<bool>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = company.UpdateCompany(model, Id).UpdateCompanyResponse();

            }
            catch (System.Exception ex)
            {
                responce = CompanyResponseMapper.CacheExceptionResponse(ex);
                logger.LogInfo(ex.Message); ;
            }

            logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpDelete, Route("/DeleteCompany")]
        public IActionResult DeleteCompany(int CompanyId)
        {
            ApiResponse<bool> responce = new ApiResponse<bool>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                var response = company.DeleteCompany(CompanyId).DeleteCompanyResponse();

            }
            catch (System.Exception ex)
            {
                responce = CompanyResponseMapper.CacheExceptionResponse(ex);
                logger.LogInfo(ex.Message); ;
            }

            logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpPost, Route("/DisableCompany")]
        public IActionResult DisableCompany(int CompanyId)
        {
            ApiResponse<bool> responce = new ApiResponse<bool>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = company.DisableCompany(CompanyId).DisableCompanyResponse();

            }
            catch (System.Exception ex)
            {
                responce = CompanyResponseMapper.CacheExceptionResponse(ex);
                logger.LogInfo(ex.Message); ;
            }

            logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpGet, Route("/GetCompanyById")]
        public IActionResult GetCompanyById(int Id)
        {
            ApiResponse<Company> responce = new ApiResponse<Company>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = company.GetCompanyById(Id).GetCompanyByIdResponse();

            }
            catch (System.Exception ex)
            {
                responce = CompanyResponseMapper.CacheCompanyExceptionResponse(ex);
                logger.LogInfo(ex.Message); ;
            }

            logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpGet, Route("/GetCompanyByName")]
        public IActionResult GetCompanyByName(string Name)
        {
            ApiResponse<Company> responce = new ApiResponse<Company>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = company.GetCompanyByName(Name).GetCompanyByNameResponse();

            }
            catch (System.Exception ex)
            {
                responce = CompanyResponseMapper.CacheCompanyExceptionResponse(ex);
                logger.LogInfo(ex.Message); ;
            }

            logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpGet, Route("/GetCompanies")]
        public IActionResult GetCompanies()
        {
            ApiListResponse<IEnumerable<Company>> responce = new ApiListResponse<IEnumerable<Company>>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = company.GetCompanies().GetCompaniesResponse();

            }
            catch (System.Exception ex)
            {
                responce = CompanyResponseMapper.CacheListCompanyExceptionResponse(ex);
                logger.LogInfo(ex.Message); ;
            }

            logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpGet, Route("/GetStateDetail")]
        public IActionResult GetStateDetails(int countryId)
        {
            ApiResponse<IList<StateDetails>> responce = new ApiResponse<IList<StateDetails>>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = company.GetStateDetails(countryId).GetCountryResponce();
            }
            catch (Exception ex)
            {
                logger.LogInfo(ex.Message);
                responce = ex.CacheExceptionCountryResponse();
            }
            logger.LogInfo(responce.Message);
            return Ok(responce);
        }

        [HttpGet, Route("/GetDivisionDetail")]
        public IActionResult GetDivisionDetails(int sataeId)
        {
            ApiResponse<IList<DivisionDetails>> responce = new ApiResponse<IList<DivisionDetails>>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = company.GetDivisionDetails(sataeId).GetDivisionResponce();
            }
            catch (Exception ex)
            {
                logger.LogInfo(ex.Message);
                responce = ex.CacheExceptionDivisionResponse();
            }
            logger.LogInfo(responce.Message);
            return Ok(responce);
        }


        [HttpGet, Route("/GetTalukas")]
        public ActionResult GetTalukaDetails(int divisionId)
        {
            ApiResponse<IList<TalukaDetails>> responce = new ApiResponse<IList<TalukaDetails>>();
            try
            {
                ApiExposeResponse<Dictionary<string, string>> modelErrors = GetModelErrors();
                responce = company.GetTalukaDetails(divisionId).GetTalukaResponce();
            }
            catch (Exception ex)
            {
                logger.LogInfo(ex.Message);
                responce = ex.CacheExceptionTalukaDetailsResponse();
            }
            logger.LogInfo(responce.Message);
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
