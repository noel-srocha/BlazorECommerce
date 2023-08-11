namespace BlazorECommerce.Shared.DTOs;

public class OrderDetailsProductResponseDTO
{
    public int ProductId { get; set; }
    public string Title { get; set; }
    public string ProductType { get; set; }
    public string ImgUrl { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}