namespace BlazorECommerce.Client.Services.ProductService;

using BlazorECommerce.Shared.DTOs;

public class ProductService : IProductService
{
    private readonly HttpClient _http;

    public ProductService(HttpClient http)
    {
        _http = http;
    }

    public event Action ProductsChanged;
    public List<Product> AdminProducts { get; set; }
    public List<Product> Products { get; set; } = new List<Product>();
    public string Message { get; set; } = "Loading Products...";
    public int CurrentPage { get; set; } = 1;
    public int PageCount { get; set; } = 0;
    public string LastSearchText { get; set; } = string.Empty;

    public async Task<Product> CreateProduct(Product product)
    {
        var result = await _http.PostAsJsonAsync("api/Product/admin", product);
        var newProduct = (await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;

        return newProduct;
    }
    
    public async Task DeleteProduct(Product product)
    {
        var result = await _http.DeleteAsync($"api/Product/admin/{product.Id}");
    }
    
    public async Task GetAdminProducts()
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/Product/admin");

        AdminProducts = result.Data;
        CurrentPage = 1;
        PageCount = 0;
        
        if (AdminProducts.Count == 0)
            Message = "No Products Found.";
    }
    
    public async Task GetProducts(string? categoryUrl = null)
    {
        var result = categoryUrl == null ?
            await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/featured")
            : await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/getproductsbycategory/{categoryUrl}");

        if (result is { Data: not null })
            Products = result.Data;

        CurrentPage = 1;
        PageCount = 0;
        
        if (Products.Count == 0)
            Message = "No Products Found";
        
        ProductsChanged.Invoke();
    }
    
    public async Task<ServiceResponse<Product>> GetProduct(int productId)
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/getproduct/{productId}");
        
        if (result is { Data: not null })
            return result;

        return new ServiceResponse<Product>();
    }
    
    public async Task<List<string>> GetProductSearchSuggestions(string searchText)
    {
        var result = await _http
            .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/Product/searchsuggestions/{searchText}");

        return result.Data;
    }
    
    public async Task SearchProducts(string searchText, int page)
    {
        LastSearchText = searchText;
        
        var result = await _http
            .GetFromJsonAsync<ServiceResponse<ProductSearchResultDTO>>($"api/Product/search/{searchText}/{page}");

        if (result is { Data: not null })
        {
            Products = result.Data.Products;
            CurrentPage = result.Data.CurrentPage;
            PageCount = result.Data.Pages;
        }

        if (Products.Count == 0)
            Message = "No Products Found";
        
        ProductsChanged.Invoke();
    }
    
    public async Task<Product> UpdateProduct(Product product)
    {
        var result = await _http.PutAsJsonAsync($"api/Product/admin/{product.Id}", product);
        
        return (await result.Content.ReadFromJsonAsync<ServiceResponse<Product>>()).Data;
    }

    
}