﻿namespace BlazorECommerce.Server.Services.CartService;

using Shared.DTOs;

public interface ICartService
{
    Task<ServiceResponse<List<CartProductResponseDTO>>> GetCartProducts(List<CartItem> cartItems);
    Task<ServiceResponse<List<CartProductResponseDTO>>> StoreCartItems(List<CartItem> cartItems);
    Task<ServiceResponse<int>> GetCartItemsCount();
    Task<ServiceResponse<List<CartProductResponseDTO>>> GetDBCartProducts();
    Task<ServiceResponse<bool>> AddToCart(CartItem cartItem);
    Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem);
    Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId);
}