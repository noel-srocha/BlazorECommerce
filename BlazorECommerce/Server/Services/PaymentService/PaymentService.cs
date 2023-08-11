namespace BlazorECommerce.Server.Services.PaymentService;

using Stripe;
using Stripe.Checkout;

public class PaymentService : IPaymentService
{
    private readonly IAuthService _authService;
    private readonly ICartService _cartService;
    private readonly IOrderService _orderService;

    private const string SECRET_WEBHOOK = "whsec_c37a746ea8cbae978fcf1434f79e19e28f394d2b6997276251ec6c9743bccbb4";

    public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService)
    {
        StripeConfiguration.ApiKey = "sk_test_51NcUOWIPkCzemRo3A4ylipfiK4OmiS924YPP72PA53bql0aOoSA6q1a2K3J1jwjUYV4BuFXzUZXYNlPNXHUqFTbg00dOOwYUOT";
        
        _cartService = cartService;
        _authService = authService;
        _orderService = orderService;
    }
    
    public async Task<Session> CreateCheckoutSession()
    {
        var products = (await _cartService.GetDBCartProducts()).Data;
        var lineItems = new List<SessionLineItemOptions>();
        
        products.ForEach(p => lineItems.Add(new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmountDecimal = p.Price * 100,
                Currency = "usd",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = p.Title,
                    Images = new List<string> { p.ImgUrl },
                }
            },
            Quantity = p.Quantity,
        }));

        var options = new SessionCreateOptions
        {
            CustomerEmail = _authService.GetUserEmail(),
            ShippingAddressCollection = new SessionShippingAddressCollectionOptions
            {
                AllowedCountries = new List<string> { "US" },
            },
            PaymentMethodTypes = new List<string> { "card" },
            LineItems = lineItems,
            Mode = "payment",
            SuccessUrl = "https://localhost:5044/order-success",
            CancelUrl = "https://localhost:5044/cart",
        };

        var service = new SessionService();
        Session session = service.Create(options);

        return session;
    }
    public async Task<ServiceResponse<bool>> FulfillOrder(HttpRequest request)
    {
        var json = await new StreamReader(request.Body).ReadToEndAsync();
        
        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json, 
                request.Headers["Stripe-Signature"], 
                SECRET_WEBHOOK
            );

            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
                var session = stripeEvent.Data.Object as Session;
                var user = await _authService.GetUserByEmail(session.CustomerEmail);
                await _orderService.PlaceOrder(user.Id);
            }

            return new ServiceResponse<bool> { Data = true };
        }
        catch (StripeException e)
        {
            return new ServiceResponse<bool>
            {
                Data = false,
                Message = e.Message,
                Success = false,
            };
        }
    }
}