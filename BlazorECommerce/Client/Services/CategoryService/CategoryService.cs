﻿namespace BlazorECommerce.Client.Services.CategoryService;

public class CategoryService : ICategoryService
{
    private readonly HttpClient _http;

    public CategoryService(HttpClient http)
    {
        _http = http;
    }

    public event Action? OnChange;
    
    public List<Category> AdminCategories { get; set; }
    public List<Category> Categories { get; set; }
    
    public Category CreateNewCategory()
    {
        var newCategory = new Category { IsNew = true, Editing = true };
        
        AdminCategories.Add(newCategory);
        
        OnChange.Invoke();

        return newCategory;
    }
    
    public async Task AddCategory(Category category)
    {
        var response = await _http.PostAsJsonAsync("api/Category/admin", category);
        
        AdminCategories = (await response.Content
            .ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;

        await GetCategories();
        
        OnChange.Invoke();
    }
    
    public async Task DeleteCategory(int id)
    {
        var response = await _http.DeleteAsync($"api/Category/admin/{id}");
        
        AdminCategories = (await response.Content
            .ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;

        await GetCategories();
        
        OnChange.Invoke();
    }
    
    public async Task GetAdminCategories()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category/admin");

        if (response is { Data: not null })
            AdminCategories = response.Data;
    }

    public async Task GetCategories()
    {
        var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("api/Category/getcategories");

        if (response is { Data: not null })
            Categories = response.Data;
    }
    public async Task UpdateCategory(Category category)
    {
        var response = await _http.PutAsJsonAsync("api/Category/admin", category);
        
        AdminCategories = (await response.Content
            .ReadFromJsonAsync<ServiceResponse<List<Category>>>()).Data;

        await GetCategories();
        
        OnChange.Invoke();
    }
}