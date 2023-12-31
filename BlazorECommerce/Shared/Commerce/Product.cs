﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorECommerce.Shared;

using System.ComponentModel.DataAnnotations;

public class Product
{
  public int Id { get; set; }
  [Required]
  public string Title { get; set; }
  public string Description { get; set; } = string.Empty;
  public string ImgUrl { get; set; }
  public Category? Category { get; set; }
  public int CategoryId { get; set; }
  public bool Featured { get; set; } = false;
  public List<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
  public bool Visible { get; set; } = true;
  public bool Deleted { get; set; } = false;
  [NotMapped]
  public bool Editing { get; set; } = false;
  [NotMapped]
  public bool IsNew { get; set; } = false;
}