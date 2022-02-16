using Contract;
using Contract.DataContracts;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;
using Utility.Enums;

namespace Domain
{
    public class OTPDomain : IOTP
    {
        private readonly IOTPData otp;

        public OTPDomain(IOTPData _otp)
        {
            otp= _otp;
        }

        public OTPResponse SaveOTP(OtpRequest model)
        {
            return otp.SaveOTP(model);
        }

        public OTPResponse ValidateOTP(OtpRequest request)
        {
            OTPResponse otpResult = null;

            if (!string.IsNullOrEmpty(request.OTP))
            {
                otpResult = otp.GetOtpByContact(request.ContactNo, request.OTP);
                if ((DateTime.Now > otpResult.OtpCreationDate && DateTime.Now < otpResult.OtpCreationDate.AddMinutes((int)Utility.Enums.AppSettings.OTPValidMins)) == false)
                {
                    return new OTPResponse
                    {
                        Contact = request.ContactNo,
                        IsOtpVerified = false,
                        ErrorMessage = String.Format(ErrorMessages.OTPExpired),
                    };
                }
            }
            else
            {
                return new OTPResponse
                {
                    Contact = request.ContactNo,
                    IsOtpVerified = false,
                    ErrorMessage = String.Format(ErrorMessages.RequiredData,"OTP"),
                };
            }

            return otpResult;
        }
    }
}
