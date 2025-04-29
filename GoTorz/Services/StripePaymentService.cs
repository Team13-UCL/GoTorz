using Stripe;
using Stripe.Checkout;
using Microsoft.Extensions.Configuration;

public class StripePaymentService
{

    private readonly IConfiguration _configuration;

    public StripePaymentService(IConfiguration configuration)
    {
        _configuration = configuration;
        StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
    }

    // Metode til at oprette en Stripe Checkout-session
    // metoden tager et beløb og et pakkenavn som input og returnerer en URL til checkout-sessionen
    public async Task<string> CreateCheckoutSessionAsync(decimal amount, string packageName)
    {
        var options = new SessionCreateOptions
        {
            //Angiver betalingsmetoderne, der er tilladt i checkout-sessionen ("card" i dette tilfæde)
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "eur",
                        UnitAmount = (long)(amount * 100), // Stripe kræver beløb i "cents", derfor * 100

                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = packageName,
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            //URL Stripe skal sende brugeren til, hvis betaling lykkes
            SuccessUrl = "https://yourdomain.com/success",

            //URL Stripe skal sende brugeren til, hvis betaling mislykkes
            CancelUrl = "https://yourdomain.com/cancel",
        };

        // Opretter en ny SessionService som håndterer kommunikationen med Stripe API
        var service = new SessionService();

        // Kalder Stripe API'et asynkront for at oprette en session
        Session session = await service.CreateAsync(options);

        return session.Url; // Returnér URL til Checkout
    }
}


