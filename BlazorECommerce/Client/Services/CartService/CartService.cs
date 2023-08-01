using Blazored.LocalStorage;
namespace BlazorECommerce.Client.Services.CartService;

using BlazorECommerce.Shared.DTOs;

public class CartService : ICartService
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _http;

    private readonly AuthenticationStateProvider _authStateProvider;

    public CartService(ILocalStorageService localStorage, HttpClient http, AuthenticationStateProvider authStateProvider)
    {
        _localStorage = localStorage;
        _http = http;
        _authStateProvider = authStateProvider;
    }
    
    public event Action? OnChange;
    
    public async Task AddToCart(CartItem cartItem)
    {
        if (await IdentityIsAuthenticated())
        {
            Console.WriteLine("User is authenticated");
        }
        else
        {
            Console.WriteLine("User is not authenticated");
        }
        
        var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

        var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId && x.ProductTypeId == cartItem.ProductTypeId);
        
        if (sameItem != null)
            sameItem.Quantity += cartItem.Quantity;
        else
            cart.Add(cartItem);
        
        OnChange?.Invoke();
        
        await _localStorage.SetItemAsync("cart", cart);
        
        OnChange!.Invoke();
    }

    public async Task<List<CartItem>> GetCartItems()
    {
        var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

        return cart;
    }
    public async Task<List<CartProductResponseDTO>> GetCartProducts()
    {
        var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();
        var response = await _http.PostAsJsonAsync("api/Cart/products", cartItems);
        
        var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponseDTO>>>();

        return cartProducts!.Data!;
    }
    public async Task RemoveProductFromCart(int productId, int productTypeId)
    {
        var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

        var cartItem = cart.Find(x => x.ProductId == productId && x.ProductTypeId == productTypeId);

        if (cartItem != null)
        {
            cart.Remove(cartItem);
            await _localStorage.SetItemAsync("cart", cart);
            OnChange!.Invoke();
        }
            
    }
    public async Task UpdateQuantity(CartProductResponseDTO product)
    {
        var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

        var cartItem = cart.Find(x => x.ProductId == product.ProductId && x.ProductTypeId == product.ProductTypeId);

        if (cartItem != null)
        {
            cartItem.Quantity = product.Quantity;
            await _localStorage.SetItemAsync("cart", cart);
        }
    }
    
    public async Task StoreCartItems(bool emptyLocalCart = false)
    {
        var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart") ?? new List<CartItem>();

        await _http.PostAsJsonAsync("api/Cart", localCart);

        if (emptyLocalCart)
            await _localStorage.RemoveItemAsync("cart");
    }
    
    public async Task GetCartItemsCount()
    {
        if (await IdentityIsAuthenticated())
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/Cart/count");
            var count = result.Data;
            
            await _localStorage.SetItemAsync("cartItemsCount", count);
        }
        else
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            await _localStorage.SetItemAsync("cartItemsCount", cart != null ? cart.Count : 0);
        }
        
        OnChange.Invoke();
    }

    private async Task<bool> IdentityIsAuthenticated()
    {
        return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
    }
}