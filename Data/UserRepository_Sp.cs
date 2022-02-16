using Contract.DataContracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Data
{
    public class UserRepository_Sp : IUsersData
    {
        IConfiguration _configuration;
        public UserRepository_Sp(IConfiguration configuration)
        {
            _configuration=configuration;
        }
        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("ProductContext").Value;
            return connection;
        }
        public bool AddUser(Users user)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", user.Id);
                parameters.Add("@Password", user.Password);
                parameters.Add("@FirstName", user.FirstName);
                parameters.Add("@LastName", user.LastName);
                parameters.Add("@Contact", user.Contact);
                parameters.Add("@AlternetContact", user.AlternetContact);
                parameters.Add("@MaritalStatus", user.MaritalStatus);
                parameters.Add("@Gender", user.Gender);
                parameters.Add("@DateOfBirth", user.DateOfBirth);
                parameters.Add("@Address", user.Address);
                parameters.Add("@Role", user.Role);
                parameters.Add("@WorkerType", user.WorkerType);
                parameters.Add("@IsActive", user.IsActive);
                parameters.Add("@IsMobileVerified", user.IsMobileVerified);
                parameters.Add("@DeviceID", user.DeviceID);
                SqlMapper.Execute(baseRepository.connection, "AddUser", parameters);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool DeleteUser(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Id", userId);
            SqlMapper.Execute(baseRepository.connection, "DeleteUser", parameters);
            return true;
        }

        public IList<Users> GetAllUser()
        {
            IList<Users> customerList = SqlMapper.Query<Users>(baseRepository.connection, "GetAllUsers").ToList();
            return customerList;
        }

        public Users GetUserById(int userId)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ID", userId);
                return SqlMapper.Query<Users>((SqlConnection)baseRepository.connection, "GetUserById", parameters).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Users GetUserByName(string name)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ID", name);
                return SqlMapper.Query<Users>((SqlConnection)baseRepository.connection, "GetUserById", parameters).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateUser(Users user)
        {
            try
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", user.Id);
                parameters.Add("@Password", user.Password);
                parameters.Add("@FirstName", user.FirstName);
                parameters.Add("@LastName", user.LastName);
                parameters.Add("@Contact", user.Contact);
                parameters.Add("@AlternetContact", user.AlternetContact);
                parameters.Add("@MaritalStatus", user.MaritalStatus);
                parameters.Add("@Gender", user.Gender);
                parameters.Add("@DateOfBirth", user.DateOfBirth);
                parameters.Add("@Address", user.Address);
                parameters.Add("@Role", user.Role);
                parameters.Add("@WorkerType", user.WorkerType);
                parameters.Add("@IsActive", user.IsActive);
                parameters.Add("@IsMobileVerified", user.IsMobileVerified);
                parameters.Add("@DeviceID", user.DeviceID);
                SqlMapper.Execute(baseRepository.connection, "AddUser", parameters, ,, CommandType.StoredProcedure);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
