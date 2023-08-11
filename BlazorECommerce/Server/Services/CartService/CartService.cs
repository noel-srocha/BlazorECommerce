namespace BlazorECommerce.Server.Services.CartService;

using Shared.DTOs;

public class CartService : ICartService
{
    private readonly DataContext _context;

    public CartService(DataContext context)
    {
        _context = context;
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
}