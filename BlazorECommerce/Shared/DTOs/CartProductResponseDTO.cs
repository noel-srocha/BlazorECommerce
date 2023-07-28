﻿namespace BlazorECommerce.Shared.DTOs;

public class CartProductResponseDTO
{
    public int ProductId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int ProductTypeId { get; set; }
    public string ProductType { get; set; } = string.Empty;
    public string ImgUrl { get; set; }
    public decimal Price { get; set; }
}