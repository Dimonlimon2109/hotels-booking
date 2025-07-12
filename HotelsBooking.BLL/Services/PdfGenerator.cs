
using HotelsBooking.BLL.Interfaces;
using HotelsBooking.DAL.Entities;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace HotelsBooking.BLL.Services
{
    public class PdfGenerator : IPdfGenerator
    {
        private readonly IRootPath _rootPath;
        public PdfGenerator(IRootPath rootPath)
        {
            _rootPath = rootPath;
        }
        public byte[] GenerateBookingConfirmation(Booking booking)
        {
            using var ms = new MemoryStream();
            using var writer = new PdfWriter(ms);
            using var pdf = new PdfDocument(writer);
            var document = new Document(pdf);

            var fontPath = Path.Combine(_rootPath.RootPath, "fonts", "arial.ttf");
            var font = PdfFontFactory.CreateFont(fontPath, PdfEncodings.IDENTITY_H);
            document.SetFont(font);

            var titleParagraph = new Paragraph($"Подтверждение брони #{booking.Id}")
                .SetFontSize(18);
            document.Add(titleParagraph);
            document.Add(new Paragraph($"Имя пользователя: {booking.User?.UserName}"));
            document.Add(new Paragraph($"Email пользователя: {booking.User?.Email}"));
            document.Add(new Paragraph($"Отель: {booking.Room?.Hotel?.Name}"));
            document.Add(new Paragraph($"Номер: {booking.Room?.Type}"));
            document.Add(new Paragraph($"Даты: {booking.CheckInDate:dd.MM.yyyy} – {booking.CheckOutDate:dd.MM.yyyy}"));
            document.Add(new Paragraph($"Гостей: {booking.Adults + booking.Children} (взрослых: {booking.Adults}, детей: {booking.Children})"));
            document.Add(new Paragraph($"Сумма: {booking.TotalPrice} BYN"));
            document.Add(new Paragraph($"Статус: {booking.Status}"));
            document.Add(new Paragraph($"Дата генерации: {DateTime.UtcNow:dd.MM.yyyy HH:mm} UTC"));

            document.Close();
            return ms.ToArray();
        }
    }
}
