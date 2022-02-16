using Models;


namespace Contract
{
    public interface IOTP
    {
        OTPResponse SaveOTP(OtpRequest model);
        OTPResponse ValidateOTP(OtpRequest model);

    }
}
