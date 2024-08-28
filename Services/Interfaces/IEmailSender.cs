using Review_Web_App.Models.DTOs;

namespace Review_Web_App.Services.Interfaces
{
    public interface IEmailSender
    {
        bool SendEmail(EmailDto mailRequest);
    }
}
