namespace BlazorECommerce.Server.Services.AddressService;

public class AddressService : IAddressService
{
    private readonly IAuthService _authService;
    private readonly DataContext _context;

    public AddressService(DataContext context, IAuthService authService)
    {
        _authService = authService;
        _context = context;
    }
    
    
    public async Task<ServiceResponse<Address>> AddOrUpdateAddress(Address address)
    {
        var response = new ServiceResponse<Address>();
        var dbAddress = (await GetAddress()).Data;
        
        // TODO: Refactor with another method for updating address
        
        if (dbAddress == null)
        {
            address.UserId = _authService.GetUserId();
            _context.Addresses.Add(address);
            response.Data = address;
        }
        else
        {
            dbAddress.FirstName = address.FirstName;
            dbAddress.LastName = address.LastName;
            dbAddress.Street = address.Street;
            dbAddress.City = address.City;
            dbAddress.State = address.State;
            dbAddress.ZipCode = address.ZipCode;
            dbAddress.Country = address.Country;

            response.Data = dbAddress;
        }

        await _context.SaveChangesAsync();

        return response;
    }
    
    public async Task<ServiceResponse<Address>> GetAddress()
    {
        var userId = _authService.GetUserId();
        var address = await _context.Addresses.FirstOrDefaultAsync(a => a.UserId == userId);
        
        return new ServiceResponse<Address> { Data = address };
    }
}