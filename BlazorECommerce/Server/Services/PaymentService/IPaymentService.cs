namespace BlazorECommerce.Server.Services.PaymentService;

using Stripe.Checkout;

public interface IPaymentService
{
    Task<Session> CreateCheckoutSession();
    Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request);
}