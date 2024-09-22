namespace SampleProjectactual.Models
{
    public class PaymentViewModel
    {
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public decimal TotalAmount { get; set; }
        public string CardToken { get; set; }
        public string StripePublishableKey { get; set; }
    }
}
