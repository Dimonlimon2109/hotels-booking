namespace HotelsBooking.BLL.Interfaces
{
    public interface ISmtpEmailSender
    {
        Task SendEmailWithPdfAsync(string toEmail, string subject, string message, string pdfName, byte[] pdfAttachment, CancellationToken ct = default);
    }
}