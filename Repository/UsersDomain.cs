using Contract;
using Contract.DataContracts;
using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class UsersDomain : IUsers
    {
        private readonly IUsersData _Users;
        public UsersDomain(IUsersData users) => _Users = users;
        public bool AddUser(Users user) => _Users.AddUser(user);

        public bool ApplyForJob(JobApply jobApply) => _Users.ApplyForJob(jobApply);

        public bool DeleteUser(int userId) => _Users.DisableUser(userId);

        public IList<Users> GetAllUser() => _Users.GetAllUser();

        public IList<DivisionDetails> GetDivisionDetails(int stateId) => _Users.GetDivisionDetails(stateId);

        public IList<StateDetails> GetStateDetails(int countryId) => _Users.GetStateDetails(countryId);

        public IList<TalukaDetails> GetTalukaDetails(int divisionId) => _Users.GetTalukaDetails(divisionId);

        public Users GetUserById(int userId) => _Users.GetUserById(userId);

        public Users GetUserByName(string name) => _Users.GetUserByName(name);

        public bool UpdateUser(Users user, int id) => _Users.UpdateUser(user, id);
    }
}
