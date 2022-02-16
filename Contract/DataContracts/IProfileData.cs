using Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contract.DataContracts
{
    public interface IProfileData
    {
        JobProfile GetProfileByUserId(int userId);
        JobProfile CreateUserJobProfile(JobProfile profile);
        JobProfile UpdateUserJobProfile(JobProfile profile, int profilrId);
    }
}
