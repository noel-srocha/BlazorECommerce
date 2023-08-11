namespace BlazorECommerce.Client.Services.CartService;

using BlazorECommerce.Shared.DTOs;

public interface ICartService
{
    event Action OnChange;
    
    Task AddToCart(CartItem cartItem);
    Task GetCartItemsCount();
    Task<List<CartProductResponseDTO>> GetCartProducts();
    Task RemoveProductFromCart(int productId, int productTypeId);
    Task StoreCartItems(bool emptyLocalCart);
    Task UpdateQuantity(CartProductResponseDTO product);
}