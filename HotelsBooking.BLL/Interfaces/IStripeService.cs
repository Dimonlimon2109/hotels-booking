using HotelsBooking.BLL.Models;
using Stripe;
using Stripe.Checkout;

namespace HotelsBooking.BLL.Interfaces
{
    public interface IStripeService
    {
        Event ConstructWebhookEvent(string json, string stripeSignature);
        Task<Session> CreateBookingCheckoutSessionAsync(string userEmail, decimal amount, string bookingId, CancellationToken ct = default);
        Task<CompletedPaymentInfoModel> HandleBookingWebhook(string json, string stripeSignature);
        Task RefundPaymentAsync(string paymentIntentId, CancellationToken ct = default);
    }
}