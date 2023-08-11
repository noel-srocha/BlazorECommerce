namespace BlazorECommerce.Server.Services.CartService;

using Shared.DTOs;

public class CartService : ICartService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthService _authService;

    public CartService(DataContext context, IAuthService authService)
    {
        _context = context;
        _authService = authService;
    }
    
    public async Task<ServiceResponse<bool>> AddToCart(CartItem cartItem)
    {
        cartItem.UserId = _authService.GetUserId();

        var sameItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId
                                       && ci.ProductTypeId == cartItem.ProductTypeId
                                       && ci.UserId == _authService.GetUserId());
        
        if (sameItem == null)
            _context.CartItems.Add(cartItem);
        else
            sameItem.Quantity += cartItem.Quantity;
        
        await _context.SaveChangesAsync();
        
        return new ServiceResponse<bool> { Data = true };
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
    
    public async Task<ServiceResponse<int>> GetCartItemsCount()
    {
        var count = (await _context.CartItems.Where(ci => ci.UserId == _authService.GetUserId()).ToListAsync()).Count;

        return new ServiceResponse<int> { Data = count };
    }
    
    public async Task<ServiceResponse<List<CartProductResponseDTO>>> GetDBCartProducts()
    {
        return await GetCartProducts(await _context.CartItems
            .Where(ci => ci.UserId == _authService.GetUserId())
            .ToListAsync()
        );
    }
    
    public async Task<ServiceResponse<bool>> RemoveItemFromCart(int productId, int productTypeId)
    {
        var dbCartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.ProductId == productId
                                       && ci.ProductTypeId == productTypeId
                                       && ci.UserId == _authService.GetUserId());
        
        if (dbCartItem == null)
            return new ServiceResponse<bool>
            {
                Data = false,
                Message = "Cart item does not exist.",
                Success = false,
            };
        
        _context.CartItems.Remove(dbCartItem);
        await _context.SaveChangesAsync();
        
        return new ServiceResponse<bool> { Data = true };
    }
    
    public async Task<ServiceResponse<List<CartProductResponseDTO>>> StoreCartItems(List<CartItem> cartItems)
    {
        cartItems.ForEach(cartItem => cartItem.UserId = _authService.GetUserId());
        _context.CartItems.AddRange(cartItems);
        await _context.SaveChangesAsync();

        return await GetDBCartProducts();
    }
    
    public async Task<ServiceResponse<bool>> UpdateQuantity(CartItem cartItem)
    {
        var dbCartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.ProductId == cartItem.ProductId
                                       && ci.ProductTypeId == cartItem.ProductTypeId
                                       && ci.UserId == cartItem.UserId);
        
        if (dbCartItem == null)
            return new ServiceResponse<bool>
            {
                Data = false,
                Message = "Cart item does not exist.",
                Success = false,
            };
        
        dbCartItem.Quantity = cartItem.Quantity;
        await _context.SaveChangesAsync();

        return new ServiceResponse<bool>
        {
            Data = true,
        };
    }
}