using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public interface IWorkerSearch
    {
        IEnumerable<WorkerSearch> GetWorkerByJobType(int JobType);
        IEnumerable<WorkerSearch> GetWorkerByLocation(int JobType, int Location);
    }
}
