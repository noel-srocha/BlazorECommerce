namespace BlazorECommerce.Client.Services.ProductService;

public class ProductService : IProductService
{
    private readonly HttpClient _http;

    public ProductService(HttpClient http)
    {
        _http = http;
    }

    public event Action ProductsChanged;
    public List<Product> Products { get; set; } = new List<Product>();
    public string Message { get; set; } = "Loading Products...";

    public async Task GetProducts(string? categoryUrl = null)
    {
        var result = categoryUrl == null ?
            await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product/featured")
            : await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/getproductsbycategory/{categoryUrl}");

        if (result is { Data: not null })
            Products = result.Data;
        
        ProductsChanged.Invoke();
    }
    
    public async Task<ServiceResponse<Product>> GetProduct(int productId)
    {
        var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/getproduct/{productId}");
        
        if (result is { Data: not null })
            return result;

        return new ServiceResponse<Product>();
    }
    
    public async Task SearchProducts(string searchText)
    {
        var result = await _http
            .GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/Product/search/{searchText}");
        
        if (result is { Data: not null })
            Products = result.Data;
        
        if (Products.Count == 0)
            Message = "No Products Found";
        
        ProductsChanged.Invoke();
    }
    
    public async Task<List<string>> GetProductSearchSuggestions(string searchText)
    {
        var result = await _http
            .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/Product/searchsuggestions/{searchText}");

        return result.Data;
    }
}