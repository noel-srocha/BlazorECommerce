namespace BlazorECommerce.Server.Services.CartService;

using Shared.DTOs;
using System.Security.Claims;

public class CartService : ICartService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CartService(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ServiceResponse<List<CartProductResponseDTO>>> GetCartProducts(List<CartItem> cartItems)
    {
        var result = new ServiceResponse<List<CartProductResponseDTO>>
        {
            Data = new List<CartProductResponseDTO>(),
        };

        foreach (var cartItem in cartItems)
        {
            var product = await _context.Products
                .Where(p => p.Id == cartItem.ProductId)
                .FirstOrDefaultAsync();
            
            if (product == null)
                continue;

            var variant = await _context.ProductVariants
                .Where(v => v.ProductId == cartItem.ProductId && v.ProductTypeId == cartItem.ProductTypeId)
                .Include(v => v.ProductType)
                .FirstOrDefaultAsync();
            
            if (variant == null)
                continue;

            var cartProduct = new CartProductResponseDTO
            {
                ProductId = product.Id,
                Title = product.Title,
                ImgUrl = product.ImgUrl,
                Price = variant.Price,
                ProductType = variant.ProductType.Name,
                ProductTypeId = variant.ProductTypeId,
                Quantity = cartItem.Quantity,
            };
            
            result.Data.Add(cartProduct);

        }
        
        return result;
    }
    
    public async Task<ServiceResponse<List<CartProductResponseDTO>>> StoreCartItems(List<CartItem> cartItems)
    {
        cartItems.ForEach(cartItem => cartItem.UserId = GetUserId());
        _context.CartItems.AddRange(cartItems);
        await _context.SaveChangesAsync();

        return await GetCartProducts(await _context.CartItems.Where(ci => ci.UserId == GetUserId()).ToListAsync());
    }
    
    public async Task<ServiceResponse<int>> GetCartItemsCount()
    {
        var count = (await _context.CartItems.Where(ci => ci.UserId == GetUserId()).ToListAsync()).Count;

        return new ServiceResponse<int> { Data = count };
    }

    private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
}