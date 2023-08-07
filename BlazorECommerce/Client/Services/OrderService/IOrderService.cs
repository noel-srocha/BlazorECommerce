namespace BlazorECommerce.Client.Services.OrderService;

using BlazorECommerce.Shared.DTOs;

public interface IOrderService
{
    Task<List<OrderOverviewResponseDTO>> GetOrders();
    Task<OrderDetailsResponseDTO> GetOrderDetails(int orderId);
    Task<string> PlaceOrder();
}