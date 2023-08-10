namespace BlazorECommerce.Client.Services.ProductService; 

public interface IProductService
{
    event Action ProductsChanged;
    List<Product> AdminProducts { get; set; }
    List<Product> Products { get; set; }
    string Message { get; set; }
    int CurrentPage { get; set; }
    int PageCount { get; set; }
    string LastSearchText { get; set; }
    
    Task<Product> CreateProduct(Product product);
    Task DeleteProduct(Product product);
    Task GetAdminProducts();
    Task GetProducts(string? categoryUrl = null);
    Task<ServiceResponse<Product>> GetProduct(int productId);
    Task<List<string>> GetProductSearchSuggestions(string searchText);
    Task SearchProducts(string searchText, int page);
    Task<Product> UpdateProduct(Product product);
}