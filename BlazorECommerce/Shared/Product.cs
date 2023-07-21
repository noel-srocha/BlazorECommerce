using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorECommerce.Shared;

public class Product
{
  public int Id { get; set; }
  public string Title { get; set; }
  public string Description { get; set; } = string.Empty;
  public string ImgUrl { get; set; }
  [Column(TypeName = "decimal(18, 2)")]
  public decimal Price { get; set; }
}