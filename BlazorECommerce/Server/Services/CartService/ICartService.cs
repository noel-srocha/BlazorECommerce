namespace BlazorECommerce.Server.Services.CartService;

using Shared.DTOs;

public interface ICartService
{
    Task<ServiceResponse<List<CartProductResponseDTO>>> GetCartProducts(List<CartItem> cartItems);
}