using Contract.DataContracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Data
{
    public class UserRepository : IUsersData
    {
        IConfiguration configuration;
        internal string connection { get; set; }
        public UserRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
            connection = ApplicationDbContext.GetConnection(configuration);
        }
        public IList<Users> GetAllUser()
        {
            List<Users> products = new List<Users>();

            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT U.Id,U.FirstName,U.LastName,U.Contact,U.AlternetContact,U.MaritalStatus,U.Gender,U.DateOfBirth,U.Role,U.WorkerType,U.IsActive," +
                        "U.RefreshToken,U.Password,U.IsMobileVerified,U.DeviceID,A.Id as UserId, A.Village,A.Taluka,A.City,A.State,A.Country,A.Zip FROM Users U, " +
                        "Address A  where U.ID = A.UserId";
                    products = dbConnection.Query<Users, Address, Users>(query, MapResults, splitOn: "UserId").ToList();

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
        public Users GetUserById(int userId)
        {
            Users user = new Users();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();

                    var query = @"SELECT U.Id,U.FirstName,U.LastName,U.Contact,U.AlternetContact,U.MaritalStatus,U.Gender,U.DateOfBirth,U.Role,
                                    U.WorkerType,U.IsActive,U.RefreshToken,U.Password,U.IsMobileVerified,U.DeviceID,
                                    (
                                        SELECT a.village + ', ' + ta.Name + ', ' + d.Name + ', ' + s.Name + ', ' + 'India, Zip: ' + a.Zip  FROM Address A 
                                        inner join Address_State s on a.State = Convert(nvarchar(max),s.Id )
                                        inner join Address_Division d on a.City = Convert(nvarchar(max),d.Id )
                                        inner join Address_Districts_Taluka ta on a.Taluka = Convert(nvarchar(max),ta.Id) 
                                        where a.UserId = @id
                                    ) as FinalAddress,
                                    A.Id as UserId, A.Village,A.Taluka,A.City,A.State,A.Country,A.Zip 
                                    FROM Users U,Address A  where U.ID = A.UserId and U.ID = @id";

                    user = (Users)dbConnection.Query<Users, Address, Users>(query, MapResults, new { @id = userId }, splitOn: "UserId").SingleOrDefault();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return user;
            }
        }
        public Users GetUserByName(string name)
        {
            Users user = new Users();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT U.Id,U.FirstName,U.LastName,U.Contact,U.AlternetContact,U.MaritalStatus,U.Gender,U.DateOfBirth,U.Role,U.WorkerType,U.IsActive," +
                        "U.RefreshToken,U.Password,U.IsMobileVerified,U.DeviceID,A.Id as UserId, A.Village,A.Taluka,A.City,A.State,A.Country,A.Zip FROM Users U, " +
                        "Address A  where U.ID = A.UserId and U.FirstName = @name";

                    user = (Users)dbConnection.Query<Users, Address, Users>(query, MapResults, new { @name = name }, splitOn: "UserId");

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return user;
            }
        }
        public bool AddUser(Users user)
        {
            int count = 0;
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {

                        var userId = dbConnection.Query<int>(@"INSERT INTO Users(FirstName,LastName,Contact,AlternetContact,MaritalStatus,Gender,Role,WorkerType,IsActive,RefreshToken,Password,IsMobileVerified,DeviceID,Enabled) VALUES (@FirstName,@LastName,@Contact,@AlternetContact,@MaritalStatus,@Gender,@Role,@WorkerType,@IsActive,@RefreshToken,@Password,@IsMobileVerified,@DeviceID,@Enabled); SELECT CAST(SCOPE_IDENTITY() as INT);",
                        new { @FirstName = user.FirstName, @LastName = user.LastName, @Contact = user.Contact, @AlternetContact = user.AlternetContact, @MaritalStatus = user.MaritalStatus, @Gender = user.Gender, @Role = user.Role, @WorkerType = user.WorkerType, @IsActive = user.IsActive, @RefreshToken = user.RefreshToken, @Password = user.Password, @IsMobileVerified = user.IsMobileVerified, @DeviceID = user.DeviceID, @Enabled = user.Enabled }, transaction: transaction).FirstOrDefault();
                        if (user.Address != null && userId != 0)
                        {
                            count = dbConnection.Execute(@"INSERT INTO Address(UserId,Village, Taluka, City, State, Country, Zip) 
                            VALUES(@UserId,@Village, @Taluka, @City, @State, @Country, @Zip);",
                            new { @UserId = userId, @Village = user.Address.Village, @Taluka = user.Address.Taluka, @City = user.Address.City, @State = user.Address.State, @Country = user.Address.Country, @Zip = user.Address.Zip }, transaction: transaction);
                        }
                        transaction.Commit();
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

                return count == 1 ? true : false;
            }
        }
        public bool UpdateUser(Users user, int id)
        {
            int count = 0;
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();

                    using (var transaction = dbConnection.BeginTransaction())
                    {

                        var userId = dbConnection.Query<int>(@"UPDATE Users Set FirstName=@FirstName,LastName=@LastName,Contact=@Contact,AlternetContact=@AlternetContact,MaritalStatus=@MaritalStatus,Gender=@Gender,Role=@Role,WorkerType=@WorkerType,IsActive=@IsActive,RefreshToken=@RefreshToken,Password=@Password,IsMobileVerified=@IsMobileVerified,DeviceID=@DeviceID,Enabled=@Enabled where Id=@userId",
                        new { @FirstName = user.FirstName, @LastName = user.LastName, @Contact = user.Contact, @AlternetContact = user.AlternetContact, @MaritalStatus = user.MaritalStatus, @Gender = user.Gender, @Role = user.Role, @WorkerType = user.WorkerType, @IsActive = user.IsActive, @RefreshToken = user.RefreshToken, @Password = user.Password, @IsMobileVerified = user.IsMobileVerified, @DeviceID = user.DeviceID, @Enabled = user.Enabled, @userId = id }, transaction: transaction).FirstOrDefault();
                        if (user.Address != null && userId != 0)
                        {
                            count = dbConnection.Execute(@"Update Address set Village=@Village, Taluka=@Taluka, City=@City, State=@State, Country=@Country, Zip=@Zip where UserId=@UserId",
                            new { @UserId = id, @Village = user.Address.Village, @Taluka = user.Address.Taluka, @City = user.Address.City, @State = user.Address.State, @Country = user.Address.Country, @Zip = user.Address.Zip }, transaction: transaction);
                        }
                        transaction.Commit();
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

                return count == 1 ? true : false;
            }
        }
        public bool DisableUser(int userId)
        {
            var count = 0;

            using (var con = new SqlConnection(connection))
            {
                try
                {
                    con.Open();
                    var query = "update Users set Enabled=0 WHERE Id =@Id";
                    count = con.Execute(query, new { @Id = userId });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

                return count == 1 ? true : false;
            }
        }

        public IList<StateDetails> GetStateDetails(int countryId)
        {
            IList<StateDetails> state = new List<StateDetails>();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT Id,Name FROM [dbo].[Address_State]";

                    state = (List<StateDetails>)dbConnection.Query<StateDetails>(query);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return state;
            }
        }


        public IList<DivisionDetails> GetDivisionDetails(int stateId)
        {
            IList<DivisionDetails> divisions = new List<DivisionDetails>();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT Id,Name, State_Id as StateId FROM [dbo].[Address_Division] where State_Id= @Id";

                    divisions = (List<DivisionDetails>)dbConnection.Query<DivisionDetails>(query, new { @Id = stateId });

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return divisions;
            }
        }


        public IList<TalukaDetails> GetTalukaDetails(int divisionId)
        {
            IList<TalukaDetails> talukaDetails = new List<TalukaDetails>();
            using (var dbConnection = new SqlConnection(connection))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT Id,Name,  Division_ID as DivisionId FROM [dbo].[Address_Districts_Taluka] where Division_ID= @Id";

                    talukaDetails = (IList<TalukaDetails>)dbConnection.Query<TalukaDetails>(query, new { @Id = divisionId });

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbConnection.Close();
                }

                return talukaDetails;
            }
        }

        public bool ApplyForJob(JobApply jobApply)
        {
            var count = 0;

            using (var con = new SqlConnection(connection))
            {
                try
                {
                    con.Open();
                    var query = "INSERT INTO UserAndCompanyMapping(UserId, CompanyId, JobAppliedFor_JobId, AppliedDate) VALUES (@UserId, @CompanyId, @JobAppliedFor_JobId, @AppliedDate)";
                    count = con.Execute(query, new { @UserId = jobApply.UserId, @CompanyId = jobApply.CompanyId, @JobAppliedFor_JobId = jobApply.JobAppliedFor_JobId, @DateFrom = jobApply.AppliedDate });
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }

                return count == 1 ? true : false;
            }
        }

        private Users MapResults(Users user, Address address)
        {
            user.Address = address;
            return user;
        }
    }
}
