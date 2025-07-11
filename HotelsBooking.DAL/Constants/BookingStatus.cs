
namespace HotelsBooking.DAL.Constants
{
    public enum BookingStatus
    {
        Pending,        // Ожидает подтверждения (создано, но не подтверждено)
        Confirmed,      // Подтверждено (например, после оплаты)
        Cancelled,      // Отменено пользователем или администратором
        CheckedIn,      // Гость заселился
        CheckedOut,     // Гость выселился
        NoShow,         // Гость не прибыл (без отмены)
        FailedPayment   // Не удалось провести оплату
    }
}
