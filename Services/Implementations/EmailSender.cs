
using MailKit.Net.Smtp;
using MimeKit;
using Review_Web_App.Models.DTOs;
using Review_Web_App.Services.Interfaces;

namespace Review_Web_App.Services.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 465;
        string username = "ayoolalawal00@gmail.com";
        string password = "hvbu mvpq ptml zltk";
        string senderEmail = "ayooolalawal00@gmail.com";
        public bool SendEmail(EmailDto mailRequest)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Review Web App", senderEmail));
            message.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            message.Subject = mailRequest.Subject;

            var body = new TextPart("html")
            {
                Text = mailRequest.HtmlContent,
            };
            message.Body = body;

            SmtpClient client = new SmtpClient();
            try
            {
                client.Connect(smtpServer, smtpPort, true);
                client.Authenticate(username, password);
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send email.", ex);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
