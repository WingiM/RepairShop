namespace RepairShop.Services.Impl;

public class UserService : IUserService
{
    private readonly IApplicationContext _context;

    public UserService(IApplicationContext context)
    {
        _context = context;
    }

    public User? GetUser(int id)
    {
        return _context.Users.FirstOrDefault(x => x.Id == id);
    }
}
