namespace HotelsBooking.BLL.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool ValidatePassword(string password, string hashedPassword);
    }
}