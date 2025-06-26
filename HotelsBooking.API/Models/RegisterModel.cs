namespace HotelsBooking.API.Models
{
    public record RegisterModel
        (
        string UserName,
        string Email,
        string Password,
        string Role
        );
}
