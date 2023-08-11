namespace BlazorECommerce.Server.Services.OrderService;

using Shared.DTOs;

public interface IOrderService
{
    Task<ServiceResponse<bool>> PlaceOrder();
    Task<ServiceResponse<List<OrderOverviewResponseDTO>>> GetOrders();
    Task<ServiceResponse<OrderDetailsResponseDTO>> GetOrderDetails(int orderId);
}