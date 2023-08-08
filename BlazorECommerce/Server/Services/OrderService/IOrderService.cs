namespace BlazorECommerce.Server.Services.OrderService;

using Shared.DTOs;

public interface IOrderService
{
    Task<ServiceResponse<bool>> PlaceOrder(int userId);
    Task<ServiceResponse<List<OrderOverviewResponseDTO>>> GetOrders();
    Task<ServiceResponse<OrderDetailsResponseDTO>> GetOrderDetails(int orderId);
}