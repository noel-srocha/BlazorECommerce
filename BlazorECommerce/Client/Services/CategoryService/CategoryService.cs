namespace BlazorECommerce.Client.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _http;

    public CategoryService(HttpClient http)
    {
        _http = http;
    }

    public List<Category> Categories { get; set; }

    public async Task GetCategories()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category/getcategories");

        if (response is { Data: not null })
            Categories = response.Data;
    }
}