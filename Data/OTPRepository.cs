﻿using Contract.DataContracts;
using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Data
{
    public class OTPRepository : IOTPData
    {
        IConfiguration configuration;
        internal string connectionString { get; set; }
        public OTPRepository(IConfiguration _configuration)
        {
            configuration = _configuration;
            connectionString = ApplicationDbContext.GetConnection(configuration);
        }

        public OTPResponse GetOtpByContact(string contact, string Otp)
        {
            OTPResponse response = new OTPResponse();
            using (var cnn = new SqlConnection(connectionString))
            {
                cnn.Open();

                string sql = " Select Contact, OtpCreationDate, OTP, DeviceId from OTP Where Contact = @Contact AND otp = @OTP";
                response = cnn.Query<OTPResponse>(sql, new { @Contact = contact, @OTP = Otp }).FirstOrDefault();

            }

            return response;
        }

        public OTPResponse SaveOTP(OtpRequest model)
        {
            OTPResponse response = new OTPResponse();

            using (var cnn = new SqlConnection(connectionString))
            {
                cnn.Open();
                
                string sql = "Insert INTO OTP(Contact, OtpCreationDate, OTP) values (@Contact, @OtpCreationDate, @OTP, @DeviceId) ;";

                var count = cnn.Execute(sql, new { @Contact = model.ContactNo, @OtpCreationDate = DateTime.Now , @OTP = model.OTP , @DeviceId = model.DeviceId });

                sql = " Select Contact, OtpCreationDate, OTP, DeviceId from OTP Where Contact = @Contact AND otp = @OTP";
                response = cnn.Query<OTPResponse>(sql, new { @Contact = model.ContactNo, @OTP = model.OTP }).FirstOrDefault();

            }

            return response;
        }

        public OTPResponse UpdateOTP(OtpRequest model)
        {
            throw new NotImplementedException();
        }
    }
}
