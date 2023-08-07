namespace BlazorECommerce.Server.Services.OrderService;

using Shared.DTOs;

public class OrderService : IOrderService
{
    private readonly DataContext _context;
    private readonly ICartService _cartService;

    private readonly IAuthService _authService;

    public OrderService(DataContext context, ICartService cartService, IAuthService authService)
    {
        _context = context;
        _cartService = cartService;
        _authService = authService;
    }
    
    public async Task<ServiceResponse<List<OrderOverviewResponseDTO>>> GetOrders()
    {
        var response = new ServiceResponse<List<OrderOverviewResponseDTO>>();
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Where(o => o.UserId == _authService.GetUserId())
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        var orderResponse = new List<OrderOverviewResponseDTO>();
        orders.ForEach(o => orderResponse.Add(new OrderOverviewResponseDTO
        {
            Id = o.Id,
            OrderDate = o.OrderDate,
            TotalPrice = o.TotalPrice,
            ProductName = o.OrderItems.Count > 1 ?
                o.OrderItems.First().Product.Title + " and " + (o.OrderItems.Count - 1) + " more..." :
                o.OrderItems.First().Product.Title,
            ProductImgUrl = o.OrderItems.First().Product.ImgUrl,
        }));

        response.Data = orderResponse;
        
        return response;
    }
    public async Task<ServiceResponse<OrderDetailsResponseDTO>> GetOrderDetails(int orderId)
    {
        var response = new ServiceResponse<OrderDetailsResponseDTO>();
        
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.ProductType)
            .Where(o => o.UserId == _authService.GetUserId() && o.Id == orderId)
            .OrderByDescending(o => o.OrderDate)
            .FirstOrDefaultAsync();

        if (order == null)
        {
            response.Success = false;
            response.Message = "Order not found.";
            return response;
        }

        var orderDetailsResponse = new OrderDetailsResponseDTO
        {
            OrderDate = order.OrderDate,
            TotalPrice = order.TotalPrice,
            Products = new List<OrderDetailsProductResponseDTO>(),
        };

        order.OrderItems.ForEach(oi => orderDetailsResponse.Products.Add(new OrderDetailsProductResponseDTO
        {
           ProductId = oi.ProductId,
           ImgUrl = oi.Product.ImgUrl,
           ProductType = oi.ProductType.Name,
           Quantity = oi.Quantity,
           Title = oi.Product.Title,
           TotalPrice = oi.TotalPrice
        }));

        response.Data = orderDetailsResponse;
        return response;
    }

    public async Task<ServiceResponse<bool>> PlaceOrder()
    {
        var products = (await _cartService.GetDBCartProducts()).Data;
        decimal totalPrice = 0;
        products!.ForEach(product => totalPrice += product.Price * product.Quantity);
        
        var orderItems = new List<OrderItem>();
        products.ForEach(product => orderItems.Add(new OrderItem
        {
            ProductId = product.ProductId,
            Quantity = product.Quantity,
            ProductTypeId = product.ProductTypeId,
            TotalPrice = product.Price * product.Quantity,
        }));

        var order = new Order
        {
            UserId = _authService.GetUserId(),
            OrderDate = DateTime.Now,
            TotalPrice = totalPrice,
            OrderItems = orderItems,
        };
        
        _context.Orders.Add(order);

        _context.CartItems.RemoveRange(_context.CartItems
            .Where(ci => ci.UserId == _authService.GetUserId()));
        
        await _context.SaveChangesAsync();
        
        return new ServiceResponse<bool> { Data = true };

    }
}