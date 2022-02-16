using Contract;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class SearchDomain : ISearch
    {
        private readonly ISearchData _searchData;
        public SearchDomain(ISearchData searchData)
        {
            _searchData = searchData;
        }

        public IEnumerable<Search> GetCompanyByJobType(int JobType)
        {
            return _searchData.GetCompanyByJobType(JobType);
        }

        public IEnumerable<Search> GetCompanyByLocation(int JobType, int Location, int WorkerType)
        {
            return _searchData.GetCompanyByLocation(JobType,Location, WorkerType);
        }
    }
}
