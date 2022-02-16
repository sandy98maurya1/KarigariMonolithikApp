using Contract.DataContracts;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Linq;

namespace Data
{
    public class JobPostRepository : IJobPostData
    {
        public object MapResults { get; private set; }

        IConfiguration configuration;
        internal string connection { get; set; }
        public JobPostRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
            connection = ApplicationDbContext.GetConnection(configuration);
        }

        public JobPost CreateJobPost(JobPost jobPost)
        {
            JobPost jobPostResult = new JobPost();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {

                        int profileId = dbConnection.Query<int>(@"INSERT INTO Company_Job_Post( Duration,JobAvailableDate,JobTypeID,LocationID,IsAccomodation,NoOfPositions,CompanyId)VALUES( @Duration,@JobAvailableDate,@JobTypeID,@LocationID,@IsAccomodation,@NoOfPositions,@CompanyId); SELECT CAST(SCOPE_IDENTITY() as INT);",
                        new { @Duration = jobPost.Duration, @JobAvailableDate = jobPost.JobAvailableDate, @JobTypeID = jobPost.JobTypeID, @LocationID = jobPost.LocationID, @IsAccomodation = jobPost.IsAccomodation, @NoOfPositions =jobPost.NoOfPositions, @CompanyId = jobPost.CompanyId }, transaction: transaction).FirstOrDefault();

                        transaction.Commit();
                        jobPostResult = jobPost;
                        jobPostResult.Id = profileId;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return jobPost;
            }
        }

        public JobPost GetJobPostById(int Id, int CompanyId)
        {
            JobPost products = new JobPost();

            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT Duration,CONVERT(datetime,JobAvailableDate,103) as JobAvailableDate,JobTypeID,LocationID,IsAccomodation,NoOfPositions,CompanyId FROM Company_Job_Post where Id = @Id And CompanyId = @CompanyId";
                    products = (JobPost)dbConnection.Query<JobPost>(query, new {@Id = Id, @CompanyId = CompanyId }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return products;
            }
        }

        public IEnumerable<JobPost> GetJobPosts(int CompanyId)
        {
            IEnumerable<JobPost> products;

            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT Duration,CONVERT(datetime,AvailableDate,103) as AvailableDate,JobTypeID,LocationID,IsAccomodation,NoOfPositions,CompanyId FROM Company_Job_Poste where CompanyId = @CompanyId";
                    products = (IEnumerable<JobPost>)dbConnection.Query<IEnumerable<JobPost>>(query, new { @CompanyId = CompanyId }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return products;
            }
        }

        public JobPost UpdateJobPost(JobPost jobPost)
        {
            JobPost jobPostResult = new JobPost();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {

                        dbConnection.Query<int>(@"Update Company_Job_Post set Duration=@Duration,JobAvailableDate=@JobAvailableDate,JobTypeID=@JobTypeID,LocationID=@LocationID, IsAccomodation = @IsAccomodation, @NoOfPositions = NoOfPositions Where CompanyId = @CompanyId ",
                        new { @Duration = jobPost.Duration, @JobAvailableDate = jobPost.JobAvailableDate, @JobTypeID = jobPost.JobTypeID, @LocationID = jobPost.LocationID, @IsAccomodation = jobPost.IsAccomodation, @NoOfPositions = jobPost.NoOfPositions, @CompanyId = jobPost.CompanyId }, transaction: transaction);
                        transaction.Commit();
                        jobPostResult = jobPost;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return jobPostResult;
            }
        }

    }
}
