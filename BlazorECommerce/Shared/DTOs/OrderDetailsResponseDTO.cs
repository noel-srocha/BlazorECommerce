namespace BlazorECommerce.Shared.DTOs;

public class OrderDetailsResponseDTO
{
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public List<OrderDetailsProductResponseDTO> Products { get; set; }
}