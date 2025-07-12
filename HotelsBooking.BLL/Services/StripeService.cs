
using HotelsBooking.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace HotelsBooking.BLL.Services
{
    public class StripeService : IStripeService
    {
        private readonly IConfiguration _configuration;

        public StripeService(IConfiguration configuration)
        {
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<Session> CreateBookingCheckoutSessionAsync(
            string userEmail,
            decimal amount,
            string bookingId,
            CancellationToken ct = default)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmountDecimal = amount * 100,
                        Currency = "byn",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Бронирование #{bookingId}"
                        }
                    },
                    Quantity = 1
                }
            },
                Mode = "payment",
                CustomerEmail = userEmail,
                Metadata = new Dictionary<string, string>
            {
                { "bookingId", bookingId }
            },
                SuccessUrl = "https://yourdomain.com/payment/success?bookingId=" + bookingId,
                CancelUrl = "https://yourdomain.com/payment/cancel"
            };

            var service = new SessionService();
            return await service.CreateAsync(options, requestOptions: null, ct);
        }

        public int HandleBookingWebhook(string json, string stripeSignature)
        {
            try
            {
                var stripeEvent = ConstructWebhookEvent(json, stripeSignature);

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Session;
                    if (int.TryParse(session?.Metadata?["bookingId"], out var result))
                    {
                        return result;
                    }

                    throw new InvalidOperationException("bookingId не найден или невалидный");
                }

                throw new NotSupportedException("Событие не обрабатывается");
            }
            catch (StripeException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }


        public Event ConstructWebhookEvent(string json, string stripeSignature)
        {
            var secret = _configuration["Stripe:WebhookSecret"];
            return EventUtility.ConstructEvent(json, stripeSignature, secret);
        }
    }
}
