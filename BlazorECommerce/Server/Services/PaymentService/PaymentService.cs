namespace BlazorECommerce.Server.Services.PaymentService;

using Stripe.Checkout;

public class PaymentService : IPaymentService
{
    private readonly IAuthService _authService;
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;

    public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService)
    {
        _cartService = cartService;
        _authService = authService;
        _orderService = orderService;
    }
    
    public Task<Session> CreateCheckoutSession()
    {
        throw new NotImplementedException();
    }
}