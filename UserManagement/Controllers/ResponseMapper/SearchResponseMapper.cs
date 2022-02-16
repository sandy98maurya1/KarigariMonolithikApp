using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility;

namespace UserManagement.Controllers.ResponseMapper
{
    public static class SearchResponseMapper
    {
        public static ApiListResponse<IEnumerable<Search>> SearchByJobTypeResponse(this IEnumerable<Search> searchResult)
        {
            if (searchResult != null)
            {
                return new ApiListResponse<IEnumerable<Search>>
                {
                    Message = string.Format(ErrorMessages.Success),
                    IsSuccess = true,
                    Data = searchResult,
                    StatusCode = 200
                };
            }
            else
            {
                return new ApiListResponse<IEnumerable<Search>>
                {
                    Message = string.Format(ErrorMessages.NoRecordFound),
                    IsSuccess = false,
                    Data= null,
                    StatusCode = 400
                };
            }
        }

        public static ApiListResponse<IEnumerable<Search>> CacheListExceptionResponse(this Exception ex)
        {
            return new ApiListResponse<IEnumerable<Search>>
            {
                IsSuccess = false,
                Data = null,
                Message = ex.Message
            };
        }
    }
}
