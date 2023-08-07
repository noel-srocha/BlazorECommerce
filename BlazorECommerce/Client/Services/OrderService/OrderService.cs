namespace BlazorECommerce.Client.Services.OrderService;

using BlazorECommerce.Shared.DTOs;
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

    public async Task<List<OrderOverviewResponseDTO>> GetOrders()
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<List<OrderOverviewResponseDTO>>>("api/Order");

        return result.Data;
    }
    public async Task<OrderDetailsResponseDTO> GetOrderDetails(int orderId)
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<OrderDetailsResponseDTO>>($"api/Order/{orderId}");

        return result.Data;
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