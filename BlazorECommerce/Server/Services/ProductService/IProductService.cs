namespace BlazorECommerce.Server.Services.ProductService;

using Shared.DTOs;

public interface IProductService
{
	Task<ServiceResponse<List<Product>>> GetProducts();
	Task<ServiceResponse<Product>> GetProduct(int productId);
	Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl);
	Task<ServiceResponse<ProductSearchResultDTO>> SearchProducts(string searchText, int page);
	Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText);
	Task<ServiceResponse<List<Product>>> GetFeaturedProducts();
}