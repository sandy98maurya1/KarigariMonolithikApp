using Contract;
using Contract.DataContracts;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class JobPostDomain : IJobPost
    {
        private readonly IJobPostData _jobPostData;

        public JobPostDomain(IJobPostData jobPostData)
        {
            _jobPostData = jobPostData;
        }

        public JobPost CreateJobPost(JobPost post)
        {
            return _jobPostData.CreateJobPost(post); 
        }

        public JobPost GetJobPostById(int Id, int CompanyId)
        {
            return _jobPostData.GetJobPostById(Id, CompanyId);
        }

        public IEnumerable<JobPost> GetJobPosts(int CompanyId)
        {
            return _jobPostData.GetJobPosts(CompanyId);
        }

        public JobPost UpdateJobPost(JobPost post)
        {
            return _jobPostData.UpdateJobPost(post);
        }
    }
}
