namespace BlazorECommerce.Server.Services.ProductService;

using Shared.DTOs;

public interface IProductService
{
	Task<ServiceResponse<List<Product>>> GetFeaturedProducts();
	Task<ServiceResponse<Product>> GetProduct(int productId);
	Task<ServiceResponse<List<Product>>> GetProducts();
	Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl);
	Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText);
	Task<ServiceResponse<ProductSearchResultDTO>> SearchProducts(string searchText, int page);
}