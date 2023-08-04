namespace BlazorECommerce.Client.Services.OrderService;

using Microsoft.AspNetCore.Components;

public class OrderService : IOrderService
{
    private readonly HttpClient _http;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly NavigationManager _navigationManager;

    public OrderService(HttpClient http, AuthenticationStateProvider authStateProvider, NavigationManager navigationManager)
    {
        _http = http;
        _authStateProvider = authStateProvider;
        _navigationManager = navigationManager;
    }
    
    public async Task PlaceOrder()
    {
        if (await IdentityIsAuthenticated())
            await _http.PostAsync("api/order", null);
        else
            _navigationManager.NavigateTo("login");
    }
    
    private async Task<bool> IdentityIsAuthenticated()
    {
        return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity!.IsAuthenticated;
    }
}