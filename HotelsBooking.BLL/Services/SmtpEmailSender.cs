
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace HotelsBooking.BLL.Services
{
    public class SmtpEmailSender
    {
        private readonly IConfiguration _configuration;

        public SmtpEmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailWithPdfAsync(
            string toEmail,
            string subject,
            string message,
            string pdfName,
            byte[] pdfAttachment,
            CancellationToken ct = default)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_configuration["Email:Name"], _configuration["Email:From"]));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;
            var builder = new BodyBuilder
            {
                HtmlBody = message
            };
            builder.Attachments.Add(pdfName, pdfAttachment, new ContentType("application", "pdf"));

            emailMessage.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["Email:Smtp"], int.Parse(_configuration["Email:Port"]), false, ct);
            await smtp.AuthenticateAsync(_configuration["Email:Username"], _configuration["Email:Password"], ct);
            await smtp.SendAsync(emailMessage, ct);
            await smtp.DisconnectAsync(true, ct); 
        }

    }
}
