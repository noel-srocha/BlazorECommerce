namespace BlazorECommerce.Client.Services.CategoryService;

public interface ICategoryService
{
    event Action OnChange;
    
    List<Category> AdminCategories { get; set; }
    List<Category> Categories { get; set; }

    Category CreateNewCategory();
    
    Task AddCategory(Category category);
    Task DeleteCategory(int id);
    Task GetAdminCategories();
    Task GetCategories();
    Task UpdateCategory(Category category);
}