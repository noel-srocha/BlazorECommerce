using Blazored.LocalStorage;
namespace BlazorECommerce.Client.Services.CartService;

using BlazorECommerce.Shared.DTOs;

public class CartService : ICartService
{
    private readonly IAuthService _authService;
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;

    public CartService(ILocalStorageService localStorage, HttpClient http, IAuthService authService)
    {
        _localStorage = localStorage;
        _http = http;
        _authService = authService;
    }
    
    public event Action? OnChange;
    
    public async Task AddToCart(CartItem cartItem)
    {
        if (await _authService.IsUserAuthenticated())
            await _http.PostAsJsonAsync("api/Cart/add", cartItem);
        else
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

            var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId && x.ProductTypeId == cartItem.ProductTypeId);
        
            if (sameItem != null)
                sameItem.Quantity += cartItem.Quantity;
            else
                cart.Add(cartItem);

            await _localStorage.SetItemAsync("cart", cart);
        }

        await GetCartItemsCount();
    }
    
    public async Task GetCartItemsCount()
    {
        if (await _authService.IsUserAuthenticated())
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/Cart/count");
            var count = result!.Data;
            
            await _localStorage.SetItemAsync("cartItemsCount", count);
        }
        else
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            await _localStorage.SetItemAsync("cartItemsCount", cart?.Count ?? 0);
        }
        
        OnChange!.Invoke();
    }
    
    public async Task<List<CartProductResponseDTO>> GetCartProducts()
    {
        if (await _authService.IsUserAuthenticated())
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<CartProductResponseDTO>>>("api/Cart/");
            return response!.Data!;
        }
        else
        {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

            var response = await _http.PostAsJsonAsync("api/Cart/products", cartItems);
        
            var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponseDTO>>>();

            return cartProducts!.Data!;   
        }
    }
    
    public async Task RemoveProductFromCart(int productId, int productTypeId)
    {
        if (await _authService.IsUserAuthenticated())
            await _http.DeleteAsync($"api/Cart/{productId}/{productTypeId}");
        else
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

            var cartItem = cart.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeId);

            if (cartItem != null)
            {
                cart.Remove(cartItem);
                await _localStorage.SetItemAsync("cart", cart);
                await GetCartItemsCount();
            }
        }
    }
    
    public async Task StoreCartItems(bool emptyLocalCart = false)
    {
        var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

        await _http.PostAsJsonAsync("api/Cart", localCart);

        if (emptyLocalCart)
            await _localStorage.RemoveItemAsync("cart");
    }
    
    public async Task UpdateQuantity(CartProductResponseDTO product)
    {
        if (await _authService.IsUserAuthenticated())
        {
            var request = new CartItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity
            };
            
            await _http.PostAsJsonAsync("api/Cart/update-quantity", request);
        }
        else
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

            var cartItem = cart.Find(x => x.ProductId == product.ProductId && x.ProductTypeId == product.ProductTypeId);

            if (cartItem != null)
            {
                cartItem.Quantity = product.Quantity;
                await _localStorage.SetItemAsync("cart", cart);
            }
        }
    }
}