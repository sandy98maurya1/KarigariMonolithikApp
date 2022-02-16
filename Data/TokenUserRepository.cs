using Contract.DataContracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Data
{
    public class TokenUserRepository : ITokenUserData
    {
        IConfiguration configuration;
        internal string connectionString { get; set; }
        public TokenUserRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = ApplicationDbContext.GetConnection(configuration);
        }
        public TokenUser GetUser(string userName, string password)
        {

            TokenUser user = new TokenUser();
            IConfiguration configuration;

            using (var dbConnection = new SqlConnection(connectionString))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT Id,UserName, Password, Role, IsActive, RefreshToken, 
                                    DeviceID from Users Where UserName = @UserName and Password=@Password ";
                    user = dbConnection.Query<TokenUser>(query, new { @UserName = userName, @Password = password }).FirstOrDefault();

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
        

        public TokenUser GetUserById(int Id)
        {
            TokenUser user = new TokenUser();

            using (var dbConnection = new SqlConnection(connectionString))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT Id,UserName, Password, Role, IsActive, RefreshToken, 
                                    DeviceID from Users Where UserName = @UserName ";
                    user = dbConnection.Query<TokenUser>(query, new { @Id = Id}).FirstOrDefault();

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

        public TokenUser GetUserByUserName(string userName)
        {
            TokenUser user = new TokenUser();

            using (var dbConnection = new SqlConnection(connectionString))
            {
                try
                {
                    dbConnection.Open();
                    var query = @"SELECT Id,UserName, Password, Role, IsActive, RefreshToken, 
                                    DeviceID from Users Where UserName = @UserName ";
                    user = dbConnection.Query<TokenUser>(query, new { @UserName = userName }).FirstOrDefault();

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

        public TokenUser UpdateUser(TokenUser user)
        {
            throw new NotImplementedException();
        }
    }
}
