using Contract.DataContracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class WorkerSearchRepository : IWorkerSearchData
    {
        public object MapResults { get; private set; }

        IConfiguration configuration;
        internal string connection { get; set; }
        public WorkerSearchRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
            connection = ApplicationDbContext.GetConnection(configuration);
        }

        public IEnumerable<WorkerSearch> GetWorkerByJobType(int JobType)
        {
            IEnumerable<WorkerSearch> result;

            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT w.Duration
                                 , j.JobName as JobType
                                 , l.Name as Location 
                                 , w.AvailableDate 
                                 , u.FirstName + ' ' + u.LastName as UserName 
                                 , u.Contact
                                FROM Worker_Job_Profile w 
                                INNER JOIN JobType j 
                                ON w.JobTypeID = j.Id
                                INNER JOIN Location_State l
                                ON w.LocationID = l.Id
                                INNER JOIN Users u 
                                ON w.UserID = u.Id
                                WHERE w.JobTypeID = @JobType"; 
                    result = (IEnumerable<WorkerSearch>)dbConnection.Query<IEnumerable<WorkerSearch>>(query, new { @JobType = JobType }).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return result;
            }
        }

        public IEnumerable<WorkerSearch> GetWorkerByLocation(int JobType, int Location)
        {
            IEnumerable<WorkerSearch> result;

            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT w.Duration, j.JobName as JobType, l.Name as Location, w.AvailableDate, u.FirstName + ' ' + u.LastName as UserName, u.Contact
                                FROM Worker_Job_Profile w
                                INNER JOIN JobType j
                                ON w.JobTypeID = j.Id
                                INNER JOIN Location_State l
                                ON w.LocationID = l.Id
                                INNER JOIN Users u
                                ON w.UserID = u.Id
                                WHERE JobTypeID = @JobType AND LocationId = @Location";
                    result = (IEnumerable<WorkerSearch>)dbConnection.Query<IEnumerable<WorkerSearch>>(query, new { @JobType = JobType, @Location = Location }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return result;
            }
        }

    }
}
