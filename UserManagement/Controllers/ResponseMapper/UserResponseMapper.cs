using Models;
using System;
using System.Collections.Generic;
using Utility;

namespace UserManagement.Controllers.UserResponseMapper
{
    public static class UserResponseMapper
    {
        public static ApiListResponse<IList<Users>> GetUsersResponce(this IList<Users> users)
        {
            return new ApiListResponse<IList<Users>>
            {
                Message = string.Format(ErrorMessages.Success),
                IsSuccess = true,
                Data = users,
                StatusCode = 200
            };
        }
        public static ApiListResponse<Users> GetUserResponce(this Users users)
        {
            return new ApiListResponse<Users>
            {
                Message = string.Format(ErrorMessages.Success),
                IsSuccess = true,
                Data = users,
                StatusCode = 200
            };
        }

        public static ApiListResponse<IList<StateDetails>> GetCountryResponce(this IList<StateDetails> country)
        {
            return new ApiListResponse<IList<StateDetails>>
            {
                Message = string.Format(ErrorMessages.Success),
                IsSuccess = true,
                Data = country,
                StatusCode = 200
            };
        }
        public static ApiResponse<IList<StateDetails>> CacheExceptionCountryResponse(this Exception ex)
        {
            return new ApiResponse<IList<StateDetails>>
            {
                IsSuccess = false,
                Data = null,
                Message = ex.Message
            };
        }

        public static ApiListResponse<IList<DivisionDetails>> GetDivisionResponce(this IList<DivisionDetails> division)
        {
            return new ApiListResponse<IList<DivisionDetails>>
            {
                Message = string.Format(ErrorMessages.Success),
                IsSuccess = true,
                Data = division,
                StatusCode = 200
            };
        }
        public static ApiResponse<IList<DivisionDetails>> CacheExceptionDivisionResponse(this Exception ex)
        {
            return new ApiResponse<IList<DivisionDetails>>
            {
                IsSuccess = false,
                Data = null,
                Message = ex.Message
            };
        }

        public static ApiListResponse<IList<TalukaDetails>> GetTalukaResponce(this IList<TalukaDetails> taluka)
        {
            return new ApiListResponse<IList<TalukaDetails>>
            {
                Message = string.Format(ErrorMessages.Success),
                IsSuccess = true,
                Data = taluka,
                StatusCode = 200
            };
        }
        public static ApiResponse<IList<TalukaDetails>> CacheExceptionTalukaDetailsResponse(this Exception ex)
        {
            return new ApiResponse<IList<TalukaDetails>>
            {
                IsSuccess = false,
                Data = null,
                Message = ex.Message
            };
        }

        public static ApiResponse<Users> CreateProfileResponse(this bool status, Users user)
        {
            if (status)
            {
                return new ApiResponse<Users>
                {
                    Message = string.Format(ErrorMessages.CreateSucess, user.FirstName + " " + user.LastName),
                    IsSuccess = status,
                    StatusCode = 200
                };
            }
            else
            {
                return new ApiResponse<Users>
                {
                    Message = string.Format(ErrorMessages.CreateFail, user.FirstName + " " + user.LastName),
                    IsSuccess = status,
                    StatusCode = 400
                };
            }
        }
        public static ApiResponse<JobApply> ApplyForJobResponse(this bool status)
        {
            if (status)
            {
                return new ApiResponse<JobApply>
                {
                    Message = string.Format(ErrorMessages.CreateSucess, "Job Applied"),
                    IsSuccess = status,
                    StatusCode = 200
                };
            }
            else
            {
                return new ApiResponse<JobApply>
                {
                    Message = string.Format(ErrorMessages.CreateFail, "Job Applied"),
                    IsSuccess = status,
                    StatusCode = 400
                };
            }
        }
        public static ApiResponse<JobApply> CacheExceptionApplyJobResponse(this Exception ex)
        {
            return new ApiResponse<JobApply>
            {
                IsSuccess = false,
                Data = null,
                Message = ex.Message
            };
        }

        public static ApiResponse<Users> DisableProfileResponse(this bool status)
        {
            if (status)
            {
                return new ApiResponse<Users>
                {
                    Message = string.Format(ErrorMessages.DeleteSucess, "Profile"),
                    IsSuccess = status,
                    StatusCode = 200
                };
            }
            else
            {
                return new ApiResponse<Users>
                {
                    Message = string.Format(ErrorMessages.DeleteFail, "Profile"),
                    IsSuccess = status,
                    StatusCode = 400
                };
            }
        }
        public static ApiResponse<Users> CacheExceptionResponse(this Exception ex)
        {
            return new ApiResponse<Users>
            {
                IsSuccess = false,
                Data = null,
                Message = ex.Message
            };
        }
        public static ApiResponse<IList<Users>> CacheListExceptionResponse(this Exception ex)
        {
            return new ApiListResponse<IList<Users>>
            {
                IsSuccess = false,
                Data = null,
                Message = ex.Message
            };
        }
    }
}
