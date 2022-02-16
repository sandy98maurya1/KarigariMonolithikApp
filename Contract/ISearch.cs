using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contract
{
    public interface ISearch
    {
        IEnumerable<Search> GetCompanyByJobType(int JobType);
        IEnumerable<Search> GetCompanyByLocation(int JobType, int Location,int WorkerType);
    }
}
