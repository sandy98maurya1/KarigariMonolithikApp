using Models;
using System;
using System.Collections.Generic;
using Utility;

namespace UserManagement.Controllers.ResponseMapper
{
    public static class ProfileResponseMapper
    {
        public static ApiListResponse<JobProfile> GetProfileResponce(this JobProfile profile)
        {
            return new ApiListResponse<JobProfile>
            {
                Message = string.Format(ErrorMessages.Success),
                IsSuccess = true,
                Data = profile,
                StatusCode = 200
            };
        }
       
        public static ApiResponse<JobProfile> ProfileResponse(this bool status, JobProfile profile)
        {
            if (status)
            {
                return new ApiResponse<JobProfile>
                {
                    Message = string.Format(ErrorMessages.CreateSucess, profile.Id + " " + profile.JobTypeID),
                    IsSuccess = status,
                    StatusCode = 200
                };
            }
            else
            {
                return new ApiResponse<JobProfile>
                {
                    Message = string.Format(ErrorMessages.CreateFail, profile.Id + " " + profile.JobTypeID),
                    IsSuccess = status,
                    StatusCode = 400
                };
            }
        }
      
        public static ApiResponse<JobProfile> CacheJobProfileExceptionResponse(this Exception ex)
        {
            return new ApiResponse<JobProfile>
            {
                IsSuccess = false,
                Data = null,
                Message = ex.Message
            };
        }
    }
}
