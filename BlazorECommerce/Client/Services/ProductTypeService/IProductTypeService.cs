namespace BlazorECommerce.Client.Services.ProductTypeService;

public interface IProductTypeService
{
    event Action OnChange;
    
    List<ProductType> ProductTypes { get; set; }
    
    Task AddProductType(ProductType productType);
    Task GetProductTypes();
    Task UpdateProductType(ProductType productType);
    
    ProductType CreateNewProductType();
}