using Microsoft.Extensions.Options;
using Stripe;
using System.Threading.Tasks;

public class StripePaymentService
{
    private readonly StripeSettings _stripeSettings;

    public StripePaymentService(IOptions<StripeSettings> stripeSettings)
    {
        _stripeSettings = stripeSettings.Value;
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
    }

    public async Task<string> CreateChargeAsync(decimal amount, string currency, string source, string description)
    {
        var options = new ChargeCreateOptions
        {
            Amount = (long)(amount * 100), // Amount in cents
            Currency = currency,
            Source = source,
            Description = description
        };

        var service = new ChargeService();
        var charge = await service.CreateAsync(options);

        return charge.Id;
    }
}
