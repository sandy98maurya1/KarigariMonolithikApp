using Contract.DataContracts;
using Dapper;
using Dapper.Mapper;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Data
{
    public class ProfileRepository : IProfileData
    {
        public object MapResults { get; private set; }

        IConfiguration configuration;
        internal string connection { get; set; }
        public ProfileRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
            connection = ApplicationDbContext.GetConnection(configuration);
        }
        public JobProfile GetProfileByUserId(int userId)
        {

            JobProfile products = new JobProfile();

            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT Duration,CONVERT(datetime,AvailableDate,103) as AvailableDate,JobTypeID,LocationID FROM Worker_Job_Profile where UserID = @UserId";
                    products =(JobProfile)dbConnection.Query< JobProfile>(query,new { @UserId = userId }).FirstOrDefault();
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

        public JobProfile CreateUserJobProfile(JobProfile profile)
        {
            
            JobProfile jobProfile = new JobProfile();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {

                        int profileId = dbConnection.Query<int>(@"INSERT INTO Worker_Job_Profile( Duration,AvailableDate,JobTypeID,LocationID,UserID)VALUES( @Duration,@AvailableDate,@JobTypeID,@LocationID,@UserID); SELECT CAST(SCOPE_IDENTITY() as INT);",
                        new { @Duration=profile.Duration, @AvailableDate= profile.AvailableDate, @JobTypeID= profile.JobTypeID, @LocationID= profile.LocationID, @UserID= profile.UserID }, transaction: transaction).FirstOrDefault();
                        
                        transaction.Commit();
                        jobProfile = profile;
                        jobProfile.Id = profileId;
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

                return jobProfile;
            }
        }      

        public JobProfile UpdateUserJobProfile(JobProfile profile, int profilrId)
        {
            JobProfile jobProfile = new JobProfile();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {

                        dbConnection.Query<int>(@"Update Worker_Job_Profile set Duration=@Duration,AvailableDate=@AvailableDate,JobTypeID=@JobTypeID,LocationID=@LocationID where UserID=@UserID ",
                        new { @Duration = profile.Duration, @AvailableDate = profile.AvailableDate, @JobTypeID = profile.JobTypeID, @LocationID = profile.LocationID, @UserID = profile.UserID }, transaction: transaction);
                        transaction.Commit();
                        transaction.Commit();
                        jobProfile = profile;                       
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

                return jobProfile;
            }
        }
    }
}
